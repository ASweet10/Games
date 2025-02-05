using UnityEngine;
using BehaviorTree;

public class CheckEnemyInFOVCone : Node
{
    Transform transform;
    static int targetMask = 1 << 8;
    LayerMask obstacleMask;
    Animator anim;
    float angle = 80f;
    public CheckEnemyInFOVCone(Transform tf){
        transform = tf;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object targetRef = GetData("target");
        if(targetRef == null){
            Collider[] colliders = Physics.OverlapSphere(transform.position, GuardBT.FOVRange, targetMask);
    
            if(colliders.Length > 0){
                Transform target = colliders[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {

                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask)){
                        parent.parent.SetData("target", colliders[0].transform);
                        anim.SetBool("Walking", false);
                        anim.SetBool("Idle", false);
                        anim.SetBool("Running", true);
                        //anim.Play("DrawSword");

                        state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}