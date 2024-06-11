using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunter : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
    [SerializeField] Transform[] waypoints;
    Transform tf;
    [SerializeField] float turnSpeed = 60f;
    [SerializeField] float rotationSpeed = 60f;
    bool atWaypoint = false;
    int currentWP = 0;

    [Header("Movement")]
    [SerializeField, Range(1, 5)] float walkSpeed = 1f;
    [SerializeField, Range(6, 10)] float sprintSpeed = 10f;
    float gravityValue = 9.8f;
    float verticalSpeed;
    Vector3 currentMovement;
    Rigidbody rb;

    [Header("Audio")]
    AudioSource footstepAudioSource;

    enum State{ idle, walkingToWaypoint, talking };
    [SerializeField] State state = State.idle;

    void Start () {
        anim = gameObject.GetComponent<Animator>();
        tf = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
        state = State.walkingToWaypoint;
    }

    void Update() {
        HandleAIBehavior();
    }
    void HandleAIBehavior() {
        switch (state) {
            case State.idle:
                HandleIdle();
                break;
            case State.talking:
                HandleTalking();
                break;
            case State.walkingToWaypoint:
                HandleWaypointNavigation();
                break;
        }
    }

    void HandleIdle() {
        anim.SetBool("isIdle", true);
    }
    void HandleTalking() {
        //coroutine
        // play random talk animation, wait few seconds, play another
        // currently plays talk1 once
        anim.SetBool("isTalking", true);
    }

    void HandleWaypointNavigation() {
        if (currentWP != waypoints.Length) {
            if (!atWaypoint) {
                if(!CanRotateTowardsWaypoint(waypoints[currentWP])) {
                    GoToNextWaypoint(waypoints[currentWP]);
                }
            }
        } else {
            state = State.idle;
        }
    }
    void GoToNextWaypoint(Transform wp) {
        atWaypoint = false;
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);

        if(Vector3.Distance(tf.position, wp.position) <= 1f){
            atWaypoint = true;
            currentWP ++;
            if(currentWP >= waypoints.Length) {
                state = State.idle;
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", true);
            } else {
                GoToNextWaypoint(waypoints[currentWP]);
            }

        } else {
            Vector3 direction = wp.position - tf.position;
            direction.Scale(wp.position + direction.normalized * walkSpeed * Time.deltaTime);
            rb.AddForce(direction);
        }
    }

    public bool CanRotateTowardsWaypoint(Transform wp) {
        Vector3 nextWPVector = wp.position - tf.position;

        if(nextWPVector != Vector3.zero){
            Quaternion targetRotation = Quaternion.LookRotation(nextWPVector);
            if (targetRotation != transform.rotation) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed);
                return true; // Rotated, true
            } else {
                return false; // Facing that direction
            }
        } else {
            return false;
        }
    }

    public void RotateAndStartTalking() {
        Vector3 targetPosition = playerTF.position - tf.position;
        targetPosition.y = 0;
        var rotation = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Slerp(tf.rotation, rotation, Time.deltaTime * rotationSpeed);
        
        state = State.talking;
    }
}