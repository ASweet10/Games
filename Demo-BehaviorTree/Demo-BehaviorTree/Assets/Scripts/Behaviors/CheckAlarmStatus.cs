using BehaviorTree;

public class CheckAlarmStatus: Node
{
    public CheckAlarmStatus(){
    }

    public override NodeState Evaluate() {
        if (GuardBT.canUseAlarm) {
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}