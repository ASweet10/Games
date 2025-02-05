using UnityEngine;
using BehaviorTree;

public class CheckAlarmInRange : Node
{
    Transform transform;
    Transform alarmPosition;

    public CheckAlarmInRange(Transform tf, Transform alarm){
        transform = tf;
        alarmPosition = alarm;
    }

    public override NodeState Evaluate() {
        if (GuardBT.canUseAlarm) {
            if(Vector3.Distance(transform.position, alarmPosition.position) <= GuardBT.useAlarmRange){
                state = NodeState.SUCCESS;
                return state;
            }
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}