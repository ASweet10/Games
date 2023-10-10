using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemyInFOV : Node 
{
    private Transform checkedUnit;
    private Animator animator;
    private AudioSource audioSource;
    private static int enemyLayerMask = 1 << 6;
    
    public CheckEnemyInFOV(Transform transform)
    {
        checkedUnit = transform;
        animator = transform.GetComponent<Animator>();
        audioSource = transform.GetComponent<AudioSource>();
    }

    public override NodeState Evaluate()
    {
        //If target is found in data...
        object target = GetData("target");

        //If no target found in data...
        if (target == null)
        {
            //FOV logic, change to 2-part cone.
            // small cone can see player
            // large cone can see lights / will be alerted / go to investigate / "!!"
            Collider[] colliders = Physics.OverlapSphere(
                checkedUnit.position, GuardBehaviorTree.fovRange, enemyLayerMask
            );

            //If player found..
            if(colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                animator.SetBool("Walking", true);
                audioSource.Play();
                state = NodeState.SUCCESS;
                return state;
            }

            //Else, node fails
            state = NodeState.FAILURE;
            return state;
        }
        //Node returns success
        state = NodeState.SUCCESS;
        return state;
    }
}