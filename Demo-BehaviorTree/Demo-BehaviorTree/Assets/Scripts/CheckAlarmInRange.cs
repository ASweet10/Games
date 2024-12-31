using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckAlarmInRange : Node
{
    Transform transform;
    Transform alarmPosition;
    Animator anim;

    public CheckAlarmInRange(Transform tf, Transform alarm){
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
        */
        if (GuardBT.canUseAlarm) {
            if(Vector3.Distance(transform.position, alarmPosition.position) <= GuardBT.useAlarmRange){
                anim.SetBool("RingingBell", true);
                anim.SetBool("Walking", false);

                GuardBT.canUseAlarm = false;

                state = NodeState.SUCCESS;
                return state;
            }
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}