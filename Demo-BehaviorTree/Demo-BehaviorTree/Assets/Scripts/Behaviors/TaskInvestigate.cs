using UnityEngine;
using BehaviorTree;

public class TaskInvestigate : Node
{
    Transform transform;
    Vector3 searchPosition;
    Animator anim;
    int searchTimer = 0;

    public TaskInvestigate(Transform tf, Vector3 searchPos) {
        transform = tf;
        searchPosition = searchPos;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        Quaternion targetRotation = Quaternion.LookRotation(searchPosition);
        
        if(targetRotation != transform.rotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
        }

        if(Vector3.Distance(searchPosition, transform.position) > 1) {
            anim.SetBool("Running", false);
            anim.SetBool("Walking", true);
            state = NodeState.RUNNING;
            return state;
        } else {
            anim.SetBool("Walking", false);
            anim.SetBool("Investigating", true);
        }

        state = NodeState.FAILURE;
        return state;
    }
}