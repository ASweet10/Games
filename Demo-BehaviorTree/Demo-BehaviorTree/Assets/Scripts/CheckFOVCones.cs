using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInFOVCones : Node
{
    Transform transform;
    static int targetMask = 1 << 8;
    Animator anim;
    public CheckEnemyInFOVCones(Transform tf){
        transform = tf;
        anim = tf.GetComponent<Animator>();
    }


    public override NodeState Evaluate()
    {
        object targetRef = GetData("target");
        if(targetRef == null){
            Collider[] colliders = Physics.OverlapSphere(transform.position, GuardBT.innerFOVRange, targetMask);
    
            if(colliders.Length > 0){
                parent.parent.SetData("target", colliders[0].transform);
                anim.SetBool("Walking", true);
                state = NodeState.SUCCESS;
                return state;                
            }

            state = NodeState.FAILURE;
            return state;
        }


        
        state = NodeState.SUCCESS;
        return state;
    }
}
