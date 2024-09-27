using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskAttack : Node
{
    Transform lastTarget;
    PlayerController playerController;
    Animator anim;
    float attackTime = 1f;
    float attackCounter = 0f;

    public TaskAttack(Transform tf) {
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");
        if(target != lastTarget) {
            playerController = target.GetComponent<PlayerController>();
            lastTarget = target;
        }

        attackCounter += Time.deltaTime;
        if(attackCounter >= attackTime) {
            bool playerDead = playerController.TakeSwordHit();

            if(playerDead){
                ClearData("target");
                //Guard recognizes player is dead; celebration / idle / animation before going back to patrolling?
                anim.SetBool("Attacking", false);
                anim.SetBool("Walking", true);
            } else {
                attackCounter = 0f;
            }

        }

        state = NodeState.RUNNING;
        return state;
    }
}