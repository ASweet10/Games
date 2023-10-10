using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskAttack : Node
{
    Transform lastTarget;
    PlayerManager playerManager;
    Animator anim;
    float attackTime = 1f;
    float attackCounter = 0f;

    public TaskAttack(Transform tf){
        //anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if(target != lastTarget){
            playerManager = target.GetComponent<PlayerManager>();
            lastTarget = target;
        }

        attackCounter += Time.deltaTime;
        if(attackCounter >= attackTime){
            bool playerDead = playerManager.TakeSwordHit();

            //Guard recognizes player is dead
            // Could do a celebration / idle / animation before going back to patrolling
            if(playerDead){
                ClearData("target");
                //anim.SetBool("Attacking", false);
                //anim.SetBool("Walking", true);
            } else {
                attackCounter = 0f;
            }

        }

        state = NodeState.RUNNING;
        return state;
    }
}