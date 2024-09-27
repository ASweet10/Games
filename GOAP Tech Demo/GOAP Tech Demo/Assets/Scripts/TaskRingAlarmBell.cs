using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskRingAlarmBell : Node
{
    [SerializeField] Transform[] alarmPositions;
    Transform transform;
    Animator anim;
    AudioSource bellAudioSource;
    Transform closestAlarm;
    float oldDistance = 9999f;
    public TaskRingAlarmBell(Transform tf, Transform[] alarms){
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
        bellAudioSource = target.GetComponent<AudioSource>();

        if (Vector3.Distance(transform.position, target.position) < GuardBT.useAlarmRange) {
            Debug.Log(Vector3.Distance(transform.position, target.position));
            transform.LookAt(target);
            anim.SetBool("RingingBell", true);
            anim.SetBool("Walking", false);
            bellAudioSource.Play();
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}