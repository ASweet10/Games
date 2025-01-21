using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
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


    [Header("Parameters")]
    [SerializeField, Range(1, 5)] float walkSpeed = 2f;
    [SerializeField, Range(5, 20)] float sprintSpeed = 10f;
    [SerializeField] float attackRange = 3f;
    [SerializeField] float hearingRange = 15f;
    bool canAttack = true;
    // use to determine point last saw player
    // player goes around corner, last saw them at this corner, investigate area around last seen place if lost sight
    // killer loses sight of player due to bush / trees / etc., killer checks last seen area
    Transform playerLastSeenPosition;

    public enum State{ idle, patrolling, lookAroundAtWaypoint, chasingPlayer, attacking, searchingBushes, investigatingSound };
    public State state = State.patrolling;

    void Start () {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
            case State.lookAroundAtWaypoint:
                HandleLookAroundAtWaypoint();
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

        if(Vector3.Distance(tf.position, playerTF.position) <= 20f) {
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

        if (Vector3.Distance(tf.position, waypoints[currentWaypoint].position) > 0.01f) {
            
            Vector3 waypointPos = waypoints[currentWaypoint].position - tf.position;
            //tf.rotation = Quaternion.LookRotation(waypointPos);
            waypointPos = waypointPos.normalized * walkSpeed;

            //Vector3 currentMovement = waypoints[currentWaypoint].position - tf.position;
            //currentMovement = currentMovement.normalized * walkSpeed;

            if(!controller.isGrounded){
                waypointPos.y -= 9.8f * Time.deltaTime; // Apply gravity
                if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                    waypointPos.y = 0;
                }
            }

            anim.SetBool("idle", false);
            anim.SetBool("lookingAtWaypoint", false);
            anim.SetBool("chasing", false);
            anim.SetBool("patrolling", true);

            controller.Move(waypointPos * Time.deltaTime);

            HandleKillerWalkSFX();
        } else {
            currentWaypoint ++;
            state = State.lookAroundAtWaypoint;
        }
    }
    
    void HandleChaseTarget() {
        if(Vector3.Distance(tf.position, playerTF.position) <= 20f) {
            tf.LookAt(playerTF);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

            anim.SetBool("idle", false);
            anim.SetBool("patrolling", false);
            anim.SetBool("attacking", false);
            anim.SetBool("chasing", true);

            Vector3 currentMovement = playerTF.position - tf.position;
            currentMovement = currentMovement.normalized * sprintSpeed;
   
            if(!controller.isGrounded){
                currentMovement.y -= 9.8f; // Apply gravity
                if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                    currentMovement.y = 0;
                }
            }
            controller.Move(currentMovement * Time.deltaTime);

            if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
                state = State.attacking;
            }
        } else {
            state = State.patrolling;
        }
    }

    void HandleAttack() {
        if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
            tf.LookAt(playerTF);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

            anim.SetBool("chasing", false);
            anim.SetBool("attacking", true);
            //StartCoroutine(AttemptAttack());
            /*
            tf.LookAt(playerTF);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

            if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
                fpController.TakeDamage("Stab");
            }
            */
        } else {
            Debug.Log("resume chase");
            state = State.chasingPlayer;
        }
    }

    IEnumerator AttemptAttack() {
        //canAttack = false;
        //Vector3 direction = (playerTF.position - tf.position).normalized;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //tf.rotation = Quaternion.Slerp(tf.rotation, rotation, Time.deltaTime * 60);
        tf.LookAt(playerTF);
        tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(1f);
        if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
            //fpController.TakeDamage("Stab");
        }
        //canAttack = true;
    }
    void HandleLookAroundAtWaypoint() {
        if(fovScript.canSeePlayer) {
            state = State.chasingPlayer;
        }
        anim.SetBool("patrolling", false);
        anim.SetBool("lookingAtWaypoint", true);

        StartCoroutine(ResumePatrollingAfterDelay());
    }
    IEnumerator ResumePatrollingAfterDelay() {
        yield return new WaitForSeconds(2f);
        state = State.patrolling;
    }
    void HandleSearch() {
        anim.SetBool("chasing", false);
        anim.SetBool("idle", false);
        anim.ResetTrigger("Stab");
        anim.SetBool("searching", true);
    }

    void HandleInvestigateSound() {
        // if killer hears something while patrolling or searching
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