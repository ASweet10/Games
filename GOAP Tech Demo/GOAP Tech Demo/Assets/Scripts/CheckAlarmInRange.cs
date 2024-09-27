using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckAlarmInRange : Node
{
    Transform transform;
    Transform[] alarmPositions;
    Animator anim;
    Transform closestAlarm;
    float oldDistance = 9999f;
    public CheckAlarmInRange(Transform tf, Transform[] alarms){
        transform = tf;
        alarmPositions = alarms;
        anim = tf.GetComponent<Animator>();
    }


    public override NodeState Evaluate() {
        foreach (Transform alarm in alarmPositions) {
            float dist = Vector3.Distance(transform.position, alarm.position);
            if (dist < oldDistance) {
                closestAlarm = alarm;
                oldDistance = dist;
            }
        }
        Transform target = closestAlarm;

        if(Vector3.Distance(transform.position, target.position) <= GuardBT.useAlarmRange){
            anim.SetBool("RingingBell", true);
            anim.SetBool("Walking", false);

            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}