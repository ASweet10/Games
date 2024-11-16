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
    [SerializeField] float waitTime = 2f;
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
                anim.SetBool("Walking", true);
            }
        }
        else{
            Transform wp = waypoints[currentWP];
            if(Vector3.Distance(transform.position, wp.position) < 0.01f) {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;
                anim.SetBool("Walking", false);

            if(currentWP == waypoints.Length - 1) {
                currentWP = 0;
            } else {
                currentWP++;
            }
        }
        else{
            /*
            if(!controller.isGrounded){
                verticalSpeed -= gravityValue * Time.deltaTime;
                currentMovement.y = verticalSpeed;
            }
            */
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWP].position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(waypoints[currentWP].position);
        }
        }

        state = NodeState.RUNNING;
        return state;
    }
}