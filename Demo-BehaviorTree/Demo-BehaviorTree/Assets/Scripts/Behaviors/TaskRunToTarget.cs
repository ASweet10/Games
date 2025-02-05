using UnityEngine;
using BehaviorTree;

public class TaskRunToTarget : Node
{
    Transform transform;
    public TaskRunToTarget(Transform tf){
        transform = tf;
    }

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");
        /*
        if(Vector3.Distance(transform.position, target.position) > 10f) {
            state = NodeState.FAILURE;
            return state;
        }
        */

        if (Vector3.Distance(transform.position, target.position) > 0.5f){
            /*
            Vector3 targetPosition = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            if(targetRotation != transform.rotation) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
            }

            Vector3 moveDirection = Vector3.MoveTowards(transform.position, targetPosition, GuardBT.runSpeed * Time.deltaTime);
            if(moveDirection.y > 0) {
                moveDirection.y += GuardBT.gravity * Time.deltaTime;
            }
            transform.position = moveDirection;
            */

            transform.position = Vector3.MoveTowards(transform.position, target.position, GuardBT.runSpeed * Time.deltaTime);
            transform.LookAt(target.position);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}