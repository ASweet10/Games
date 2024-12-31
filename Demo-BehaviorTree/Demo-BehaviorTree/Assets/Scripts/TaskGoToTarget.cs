using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskGoToTarget : Node
{
    Transform transform;
    //static int targetMask = 1 << 8;
    Animator anim;
    public TaskGoToTarget(Transform tf){
        transform = tf;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");
        if(Vector3.Distance(transform.position, target.position) > 10f) {
            state = NodeState.FAILURE;
            return state;
        }
        if (Vector3.Distance(transform.position, target.position) > 2f){
            Vector3 moveDirection = Vector3.MoveTowards(transform.position, target.position, GuardBT.runSpeed * Time.deltaTime);
            if(moveDirection.y > 0) {
                moveDirection.y += GuardBT.gravity * Time.deltaTime;
            }
            transform.position = moveDirection;

            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            /*
            Quaternion rot = transform.localRotation;
            rot.y -= 90;
            transform.localRotation = rot;
            */
            anim.SetBool("Walking", true);
            anim.SetBool("Idle", false);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}