using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;

using BehaviorTree;
using UnityEngine.EventSystems;

public class TaskRunForAlarm : Node
{
    [SerializeField] Transform alarmPosition;
    Transform transform;
    Animator anim;

    public TaskRunForAlarm(Transform tf, Transform alarm){
        transform = tf;
        alarmPosition = alarm;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        Vector3 targetPosition = new Vector3(alarmPosition.position.x, transform.position.y, alarmPosition.position.z);
        if (Vector3.Distance(transform.position, alarmPosition.position) > 2f){
            Vector3 moveDirection = Vector3.MoveTowards(transform.position, alarmPosition.position, GuardBT.runSpeed * Time.deltaTime);
            if(moveDirection.y > 0) {
                moveDirection.y += GuardBT.gravity * Time.deltaTime;
            }
            transform.position = moveDirection;

            transform.LookAt(targetPosition);
            anim.SetBool("Running", true);
        }
        state = NodeState.RUNNING;
        return state;
    }
}