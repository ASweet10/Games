using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
    [SerializeField] FirstPersonController fpController;
    Transform tf;
    FieldOfView fovScript;
    CharacterController controller;
    
    [Header("Footsteps")]
    TerrainTexDetector terrainTexDetector;
    AudioSource footstepAudioSource;
    [SerializeField] AudioClip[] grassClips;
    [SerializeField] AudioClip[] dirtClips;
    float baseStepSpeed = 0.5f;
    float sprintStepMultiplier = 0.6f;
    float footstepTimer = 0;
    float GetCurrentOffset => state == State.chasingPlayer ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    public int terrainDataIndex;

    [Header ("Patrol")]
    [SerializeField] Transform[] waypoints;
    int currentWaypoint = 0;

    [Header("Movement")]
    [SerializeField, Range(1, 5)] float walkSpeed = 2f;
    [SerializeField, Range(6, 10)] float sprintSpeed = 10f;

    [Header("Ranges")]
    [SerializeField] float chaseRange = 20f;
    [SerializeField] float attackRange = 3f;
    [SerializeField] float hearingRange = 15f;
    bool canAttack = true;
    // use to determine point last saw player
    // player goes around corner, last saw them at this corner, investigate area around last seen place if lost sight
    // killer loses sight of player due to bush / trees / etc., killer checks last seen area
    Transform playerLastSeenPosition;


    
    public enum State{ idle, patrolling, chasingPlayer, attacking, searchingBushes, investigatingSound };
    public State state = State.patrolling;

    void Start () {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        fpController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        fovScript = gameObject.GetComponent<FieldOfView>();
        anim = gameObject.GetComponent<Animator>();
        tf = gameObject.GetComponent<Transform>();
        terrainTexDetector = gameObject.GetComponent<TerrainTexDetector>();
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
        currentWaypoint = 0;
    }

    void Update() {
        terrainDataIndex = terrainTexDetector.GetActiveTerrainTextureIdx(tf.position);
        HandleAIBehavior();
    }

    void HandleAIBehavior() {
        switch (state) {
            case State.idle:
                HandleIdle();
                break;
            case State.patrolling:
                HandlePatrol();
                break;
            case State.chasingPlayer:
                HandleChaseTarget();
                break;
            case State.attacking:
                HandleAttack();
                break;
            case State.searchingBushes:
                HandleSearch();
                break;
            case State.investigatingSound:
                HandleInvestigateSound();
                break;
            default:
                break;
        }
    }

    void HandleIdle() {
        anim.SetBool("searching", false);
        anim.SetBool("chasing", false);
        anim.SetBool("idle", true);

        if(Vector3.Distance(tf.position, playerTF.position) <= chaseRange) {
            if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
                state = State.attacking;
            } else {
                if(fovScript.canSeePlayer) {
                    state = State.chasingPlayer;
                }
            }
        }
    }

    void HandlePatrol() {
        if(fovScript.canSeePlayer) {
            state = State.chasingPlayer;
        }
        // if killer hears sound, investigate

        //Otherwise, continue patrol behavior
        if (Vector3.Distance(tf.position, waypoints[currentWaypoint].position) > 0.01f) {

            Vector3 currentMovement = waypoints[currentWaypoint].position - tf.position;
            currentMovement = currentMovement.normalized * walkSpeed;

            if(!controller.isGrounded){
                currentMovement.y -= 9.8f * Time.deltaTime; // Apply gravity
                if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                    currentMovement.y = 0;
                }
            }

            anim.SetBool("idle", false);
            anim.SetBool("chasing", true);
            controller.Move(currentMovement * Time.deltaTime);

            HandleKillerWalkSFX();
        } else {
            currentWaypoint ++;
        }
    }

    void HandleChaseTarget() {
        if(Vector3.Distance(tf.position, playerTF.position) <= chaseRange) {
            tf.LookAt(playerTF);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);
            
            anim.ResetTrigger("Stab");
            anim.SetBool("searching", false);
            anim.SetBool("idle", false);
            anim.SetBool("chasing", true);

            Vector3 currentMovement = playerTF.position - tf.position;
            currentMovement = currentMovement.normalized * walkSpeed;
   
            if(!controller.isGrounded){
                //Debug.Log("airborne");
                currentMovement.y -= 9.8f * Time.deltaTime; // Apply gravity
                if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                    currentMovement.y = 0;
                }
            }

            controller.Move(currentMovement * Time.deltaTime);

            if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
                state = State.attacking;
                Debug.Log("attack");
            }
        } else {
            state = State.idle;
        }
    }
    void HandleAttack() {
        if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
            if(canAttack) {
                StartCoroutine(AttemptAttack());
            }
        } else {
            //if player dead, stop attacking / go idle?
            anim.ResetTrigger("Stab");
            anim.SetBool("chasing", true);
            state = State.chasingPlayer;
        }
    }

    IEnumerator AttemptAttack() {
        canAttack = false;
        //Vector3 direction = (playerTF.position - tf.position).normalized;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //tf.rotation = Quaternion.Slerp(tf.rotation, rotation, Time.deltaTime * 60);
        tf.LookAt(playerTF);
        tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

        anim.SetBool("chasing", false);
        anim.SetTrigger("Stab");
        yield return new WaitForSeconds(1f);
        if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
            fpController.TakeDamage("Stab");
        }
        canAttack = true;
    }

    void HandleSearch() {
        anim.SetBool("chasing", false);
        anim.SetBool("idle", false);
        anim.ResetTrigger("Stab");
        anim.SetBool("searching", true);
    }

    void HandleInvestigateSound() {

    }

    void HandleKillerWalkSFX() {
        if(!controller.isGrounded) {
            footstepAudioSource.Stop();
            return;
        }

        footstepTimer -= Time.deltaTime; // Play one footstep per second? what is this

        if(footstepTimer <= 0) {
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 4)) {
                    switch(terrainDataIndex) {
                        case 1:
                            footstepAudioSource.PlayOneShot(dirtClips[Random.Range(0, dirtClips.Length - 1)]);
                            break;
                        case 5:
                            footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                            break;
                        default:
                            break;
                    }
            }

            footstepTimer = GetCurrentOffset;
        }

    }
}