using UnityEngine;
using BehaviorTree;

public class TaskAttack : Node
{
    Transform transform;
    Animator anim;

    public TaskAttack(Transform tf) {
        transform = tf;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");
        Vector3 targetPosition = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
        
        if(targetRotation != transform.rotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
        }

        if(Vector3.Distance(targetPosition, transform.position) < 2) {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", true);

        }
        state = NodeState.RUNNING;
        return state;
    }
}