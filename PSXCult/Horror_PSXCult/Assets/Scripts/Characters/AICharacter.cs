using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AICharacter : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
    [SerializeField] Transform killerTF;
    [SerializeField] Transform[] waypoints;
    Transform tf;
    [SerializeField] float turnSpeed = 60f;
    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float aiRange = 15f;
    bool playerInRange = false;
    bool killerInRange = false;
    bool injured = false;
    bool atPathNode = false;
    int currentWP = 0;

    enum State{ idle, walking, walkingToWaypoint, talking, running, hiding, injured };
    [SerializeField] State state = State.idle;

    void Start () {
        anim = gameObject.GetComponent<Animator>();
        tf = gameObject.GetComponent<Transform>();
        FollowPath(waypoints);
    }

    void Update() {
        HandleAIBehavior();
        if(!injured) {
            if(playerInRange) {
                
            }
        }
    }
    void HandleAIBehavior() {
        switch (state) {
            case State.idle:
                HandleIdle();
                break;
            case State.talking:
                HandleTalking();
                break;
            case State.walking:
                if(!CanRotateTowardsWaypoint(waypoints[currentWP])) {
                    GoToNextNode(waypoints[currentWP]);
                }
                break;
            case State.hiding:
                HandleHideOnBush();
                break;
            default:
                break;
        }
    }

    void HandleIdle() {
        anim.SetBool("isIdle", true);
    }
    void HandleTalking() {
        //LookAtPlayer();
        anim.SetBool("isTalking", true);
        if(!playerInRange) {
            //handle idle?
            // or handle follow player?
            // or handle hide if running from killer?
        }
    }

    void HandleWaypointNavigation() {
        if(!CanRotateTowardsWaypoint(waypoints[currentWP])) {
            GoToNextNode(waypoints[currentWP]);
        }
        //if player not in range...
        if(!playerInRange) {
            //handle idle?
            // or handle follow player?
            // or handle hide if running from killer?
        }
    }
    void HandleHideOnBush() {
        // find nearest bush you can hide in
        // If killer not within range, hide there
        // If killer within range, run away
    }

    bool CheckIfCharacterInRange(Transform character) {
        if(Vector3.Distance(transform.position, character.position) < aiRange) {
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

    void GoToNextNode(Transform node) {
        if(!atPathNode) {
            atPathNode = false;
            // Rotate towards wp
            //tf.Rotate(new Vector3(0, node.position.y, 0));


            //Move towards wp
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
            if(Vector3.Distance(node.position, tf.position) <= 1f){
                atPathNode = true;
            }
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

    void LookAtPlayer() {
        Vector3 targetPosition = playerTF.position - tf.position;
        targetPosition.y = 0;
        var rotation = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Slerp(tf.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}