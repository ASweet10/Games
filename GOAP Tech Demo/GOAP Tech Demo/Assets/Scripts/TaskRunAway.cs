using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskRunAway : Node
{
    Transform transform;
    Animator anim;
    public TaskRunAway(Transform tf){
        transform = tf;
        //anim = tf.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWP].position, GuardBT.speed * Time.deltaTime);
        //transform.LookAt(waypoints[currentWP].position);

        state = NodeState.RUNNING;
        return state;
    }
}