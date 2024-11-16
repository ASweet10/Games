using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;

using BehaviorTree;

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
        /*
        foreach (Transform alarm in alarmPositions) {
            float dist = Vector3.Distance(transform.position, alarm.position);
            if (dist < oldDistance) {
                closestAlarm = alarm;
                oldDistance = dist;
            }
        }
        Transform target = closestAlarm;

        if (Vector3.Distance(transform.position, target.position) > 1f){
            transform.position = Vector3.MoveTowards(transform.position, closestAlarm.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(target);
        }
        */
        if (Vector3.Distance(transform.position, alarmPosition.position) > 1f){
            transform.position = Vector3.MoveTowards(transform.position, alarmPosition.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(alarmPosition);
        }
        state = NodeState.RUNNING;
        return state;
    }
}