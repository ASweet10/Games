using UnityEngine;
using BehaviorTree;

public class CheckGuardHP : Node
{
    GuardManager guardManager;
    public CheckGuardHP(Transform tf) {
        guardManager = tf.GetComponent<GuardManager>();
    }

    public override NodeState Evaluate() {
        //Guard is healthy enough, node fails
        if(guardManager.ReturnCurrentHP() > GuardBT.fleeHealth){
            state = NodeState.FAILURE;
            return state;
        } else{
            state = NodeState.SUCCESS;
            return state;
        }
    }
}