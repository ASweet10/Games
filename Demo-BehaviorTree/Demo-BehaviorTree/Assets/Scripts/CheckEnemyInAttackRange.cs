using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    Transform transform;
    //static int targetMask = 1 << 8;
    Animator anim;
    public CheckEnemyInAttackRange(Transform tf) {
        transform = tf;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        object targetRef = GetData("target");
        if(targetRef == null) {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)targetRef;

        if(Vector3.Distance(transform.position, target.position) <= GuardBT.attackRange){
            anim.SetBool("Attacking", true);
            anim.SetBool("Walking", false);
                        
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}