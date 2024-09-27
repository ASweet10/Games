using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEditor.UIElements;

public class TaskRunForAlarm : Node
{
    [SerializeField] Transform[] alarmPositions;
    Transform transform;
    Animator anim;
    float oldDistance = 9999;
    Transform closestAlarm;
    public TaskRunForAlarm(Transform tf, Transform[] alarmArray){
        transform = tf;
        alarmPositions = alarmArray;
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

        if (Vector3.Distance(transform.position, target.position) > 1f){
            transform.position = Vector3.MoveTowards(transform.position, closestAlarm.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(target);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}