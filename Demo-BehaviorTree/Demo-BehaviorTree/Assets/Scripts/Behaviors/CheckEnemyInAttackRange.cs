using UnityEngine;
using BehaviorTree;

public class CheckEnemyInAttackRange : Node
{
    Transform transform;
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
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", true);
            state = NodeState.SUCCESS;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}