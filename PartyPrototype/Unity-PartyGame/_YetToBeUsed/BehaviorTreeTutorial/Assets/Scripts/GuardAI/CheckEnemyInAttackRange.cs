using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInAttackRange : Node 
{
    private Transform checkedUnit;
    private Animator animator;
    
    public CheckEnemyInAttackRange(Transform transform)
    {
        checkedUnit = transform;
        animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        //If target is found in data...
        object t = GetData("target");

        //If no target found in data...
        if (t == null)
        {
            //Node fails
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if(Vector3.Distance(checkedUnit.position, target.position) <= GuardBehaviorTree.attackRange)
        {
            animator.SetBool("Attacking", true);
            animator.SetBool("Walking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}