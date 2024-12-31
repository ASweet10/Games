using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    [SerializeField] Transform[] waypoints;
    CharacterController controller;
    Animator anim;
    Transform transform;
    int currentWP = 0;
    [SerializeField] float waitTime = 3f;
    float waitCounter = 0f;
    bool waiting = false;

    public TaskPatrol(Transform tf, Transform[] wpArray){
        transform = tf;
        waypoints = wpArray;
        controller = tf.GetComponent<CharacterController>();
        anim = tf.GetComponent<Animator>();
    }


    public override NodeState Evaluate() {
        if(waiting) {
            waitCounter += Time.deltaTime;
            if(waitCounter >= waitTime){
                waiting = false;
                anim.SetBool("Looking", false);
                anim.SetBool("Walking", true);
            }
        }
        else{
            Transform wp = waypoints[currentWP];

            if(Vector3.Distance(transform.position, wp.position) < 1f) {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;
                anim.SetBool("Walking", false);
                anim.SetBool("Looking", true);

                if(currentWP == waypoints.Length - 1) {
                    currentWP = 0;
                } else {
                    currentWP++;
                }
            } else { // Move to point
                Vector3 targetPosition = new Vector3(waypoints[currentWP].position.x, transform.position.y, waypoints[currentWP].position.z);

                transform.LookAt(targetPosition);
                Quaternion rot = transform.localRotation;
                rot.y -= 90;
                transform.localRotation = rot;

                Vector3 moveDirection = Vector3.MoveTowards(transform.position, waypoints[currentWP].position, GuardBT.walkSpeed * Time.deltaTime);
                if(moveDirection.y > 0) {
                    moveDirection.y += GuardBT.gravity * Time.deltaTime;
                }
                transform.position = moveDirection;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}