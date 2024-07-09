using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKiller : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
    [SerializeField] FirstPersonController fpController;
    Transform tf;
    FieldOfView fovScript;
    CharacterController controller;

    [Header("Movement")]
    [SerializeField, Range(1, 5)] float walkSpeed = 2f;
    [SerializeField, Range(6, 10)] float sprintSpeed = 10f;

    [Header("Ranges")]
    [SerializeField] float chaseRange = 10f; // If player within range & visible, chase
    [SerializeField] float attackRange = 4f; // If player within range, attack

    enum State{ idle, chasingPlayer, attacking, searchingBushes, investigatingNoise };
    [SerializeField] State state = State.idle;
    void Start () {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        fpController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        fovScript = gameObject.GetComponent<FieldOfView>();
        anim = gameObject.GetComponent<Animator>();
        tf = gameObject.GetComponent<Transform>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update() {
        HandleAIBehavior();
    }

    void HandleAIBehavior() {
        switch (state) {
            case State.idle:
                HandleIdle();
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
    void HandleChaseTarget() {
        if(Vector3.Distance(tf.position, playerTF.position) <= chaseRange) {
            tf.LookAt(playerTF);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

            Vector3 currentMovement = playerTF.position - tf.position;
            currentMovement = currentMovement.normalized * walkSpeed;
   
            if(!controller.isGrounded){
                Debug.Log("airborne");
                currentMovement.y -= 9.8f * Time.deltaTime; // Apply gravity
                if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                    currentMovement.y = 0;
                }
            }
            anim.SetBool("chasing", true);
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
        if(Vector3.Distance(tf.position, playerTF.position) <= 4) {
            //Vector3 direction = (playerTF.position - tf.position).normalized;
            //Quaternion rotation = Quaternion.LookRotation(direction);
            //tf.rotation = Quaternion.Slerp(tf.rotation, rotation, Time.deltaTime * 60);
            tf.LookAt(playerTF);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

            anim.SetBool("searching", false);
            anim.SetBool("chasing", false);
            anim.SetBool("idle", true);
            anim.SetTrigger("Stab");
            if(Vector3.Distance(tf.position, playerTF.position) <= 3) {
                fpController.TakeDamage();
            }
        } else {
            //if player dead, stop attacking / go idle?
            state = State.chasingPlayer;
        }
    }

    void HandleSearch() {
        anim.SetBool("chasing", false);
        anim.SetBool("idle", false);
        anim.SetBool("searching", true);
    }
}