using UnityEngine;
using BehaviorTree;

public class CheckReactionToAlarm : Node
{
    [SerializeField] Transform alarmPosition;
    Transform transform;

    public CheckReactionToAlarm(Transform tf, Transform alarm){
        transform = tf;
        alarmPosition = alarm;
    }

    public override NodeState Evaluate() {
        if(GuardBT.alarmRungRecently) {
            if(Vector3.Distance(transform.position, alarmPosition.position) < 10) {
                state = NodeState.SUCCESS;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}