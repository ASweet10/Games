using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKiller : MonoBehaviour
{
    [SerializeField] Animator anim;
    FieldOfView fovScript;
    [SerializeField] Transform playerTF;
    Transform tf;

    [Header("Movement")]
    CharacterController controller;
    [SerializeField, Range(3, 5)] float walkSpeed = 5f;
    [SerializeField, Range(6, 10)] float sprintSpeed = 10f;
    float runMultiplier;
    float gravityValue = 9.8f;
    float verticalSpeed;
    Vector2 currentInput;
    Vector3 currentMovement;

    [Header("Ranges")]
    [SerializeField] float chaseRange = 10f; // If player within range & visible, chase
    [SerializeField] float attackRange = 3f; // If player within range, attack

    enum State{ idle, chasingPlayer, attacking, searchingBushes, investigatingNoise };
    [SerializeField] State state = State.idle;

    void Start () {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        fovScript = gameObject.GetComponent<FieldOfView>();
        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
        tf = gameObject.GetComponent<Transform>();
    }

    void Update() {
        HandleAIBehavior();
    }

    void HandleAIBehavior() {
        switch (state) {
            case State.idle:
                HandleIdle();
                if(CheckIfPlayerInRange(playerTF, chaseRange)) {
                    if(fovScript.canSeePlayer) {
                        state = State.chasingPlayer;
                    }
                }
                break;
            case State.chasingPlayer:
                if(Vector3.Distance(tf.position, playerTF.position) < chaseRange) {
                    HandleChaseTarget();
                } else {

                }
                break;
            case State.attacking:
                if(Vector3.Distance(tf.position, playerTF.position) <= attackRange) {
                    HandleAttack();
                } else {
                    //if player dead, stop attacking / go idle?
                    state = State.chasingPlayer;
                }
                break;
            default:
                break;
        }
    }

    void HandleIdle() {
        anim.SetBool("isIdle", true);
    }
    void HandleChaseTarget() {
        if(Vector3.Distance(tf.position, playerTF.position) < chaseRange) { // If in range of player...
            state = State.chasingPlayer;
        }
    }
    void HandleAttack() {
        Debug.Log("attacking");
        anim.SetTrigger("Stab");
    }

    bool CheckIfPlayerInRange(Transform character, float range) {
        if(Vector3.Distance(transform.position, character.position) < chaseRange) {
            return true;
        } else {
            return false;
        }
    }

    public void FollowPath(Transform[] pathNodes){
        for(int i = 0; i <= pathNodes.Length - 1; i++) {
            //tf.Rotate(new Vector3(0, 0, 0));
        }
    }

}