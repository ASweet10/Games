using UnityEngine;
using BehaviorTree;

public class TaskRingAlarmBell : Node
{
    [SerializeField] Transform alarmPosition;
    Transform transform;
    Animator anim;
    GuardManager guardManager;
    
    public TaskRingAlarmBell(Transform tf, Transform alarm){
        transform = tf;
        alarmPosition = alarm;
        anim = tf.GetComponent<Animator>();
        guardManager = tf.GetComponent<GuardManager>();
    }

    public override NodeState Evaluate() {
        Debug.Log("ringing bell");
        if (Vector3.Distance(transform.position, alarmPosition.position) < GuardBT.useAlarmRange) {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Searching", false);
            anim.SetBool("RingingBell", true);

            guardManager.RingBell();
            GuardBT.canUseAlarm = false; // Disable alarm, all guards now search and fight
        }
        state = NodeState.SUCCESS;
        return state;
    }
}

/*
foreach (Transform alarm in alarmPositions) {
    float dist = Vector3.Distance(transform.position, alarm.position);
    if (dist < oldDistance) {
        closestAlarm = alarm;
        oldDistance = dist;
    }
}
Transform target = closestAlarm;
if (Vector3.Distance(transform.position, target.position) < GuardBT.useAlarmRange) {}
*/