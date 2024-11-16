using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskRingAlarmBell : Node
{
    [SerializeField] Transform alarmPosition;
    Transform transform;
    Animator anim;
    AudioSource bellAudioSource;
    
    public TaskRingAlarmBell(Transform tf, Transform alarm){
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
        bellAudioSource = target.GetComponent<AudioSource>();

        if (Vector3.Distance(transform.position, target.position) < GuardBT.useAlarmRange) {
            transform.LookAt(target);
            anim.SetBool("Walking", false);
            anim.SetBool("RingingBell", true);
            bellAudioSource.Play();
        }
        */
        if (Vector3.Distance(transform.position, alarmPosition.position) < GuardBT.useAlarmRange) {
            transform.LookAt(alarmPosition);
            anim.SetBool("Walking", false);
            anim.SetBool("RingingBell", true);
            bellAudioSource.Play();
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}