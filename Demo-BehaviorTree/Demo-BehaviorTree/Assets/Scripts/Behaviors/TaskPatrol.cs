using UnityEngine;
using BehaviorTree;

public class TaskPatrol : Node
{
    [SerializeField] Transform[] waypoints;
    Animator anim;
    Transform transform;
    int currentWP = 0;
    [SerializeField] float waitTime = 3f;
    float waitCounter = 0f;
    bool waiting = false;

    public TaskPatrol(Transform tf, Transform[] wpArray){
        transform = tf;
        waypoints = wpArray;
        anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate() {
        if(waiting) {
            HandleWaitAtWaypoint();
        }
        else {
            HandlePatrol();
        }
        state = NodeState.RUNNING;
        return state;
    }

    void HandlePatrol() {
        Transform wp = waypoints[currentWP];

        if(Vector3.Distance(transform.position, wp.position) > 1f) {
            /*
            Vector3 targetPosition = wp.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            if(targetRotation != transform.rotation) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);
                transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }

            Vector3 moveDirection = Vector3.MoveTowards(transform.position, wp.position, GuardBT.walkSpeed * Time.deltaTime);
            if(moveDirection.y > 0) {
                moveDirection.y += GuardBT.gravity * Time.deltaTime;
            }
            transform.position = moveDirection;
            */
            transform.position = Vector3.MoveTowards(transform.position, wp.position, GuardBT.walkSpeed * Time.deltaTime);
            transform.LookAt(wp.position);
        } else { // Wait at wp
            transform.position = wp.position;
            waitCounter = 0f;
            waiting = true;
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Idle", true);

            if(currentWP == waypoints.Length - 1) {
                currentWP = 0;
            } else {
                currentWP++;
            }
        }
    }

    void HandleWaitAtWaypoint() {
        waitCounter += Time.deltaTime;
        if(waitCounter >= waitTime){
            waiting = false;
            anim.SetBool("Idle", false);
            anim.SetBool("Running", false);
            anim.SetBool("Walking", true);
        }
    }
}