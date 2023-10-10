using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class CheckGuardHP : Node
{
    Transform transform;
    GuardManager guardManager;
    public CheckGuardHP(Transform tf){
        transform = tf;
        guardManager = tf.GetComponent<GuardManager>();
    }


    public override NodeState Evaluate()
    {
        //Guard is healthy enough, node fails
        if(guardManager.ReturnCurrentHP() >= GuardBT.fleeHealthValue){
            state = NodeState.FAILURE;
            return state;
        } else{
            state = NodeState.SUCCESS;
            return state;
        }
    }
}
