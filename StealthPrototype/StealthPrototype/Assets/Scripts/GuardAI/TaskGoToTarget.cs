using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskGoToTarget : Node
{
    Transform transform;
    static int targetMask = 1 << 8;
    Animator anim;
    public TaskGoToTarget(Transform tf){
        transform = tf;
        anim = tf.GetComponent<Animator>();
    }


    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (Vector3.Distance(transform.position, target.position) > 0.01f){
            transform.position = Vector3.MoveTowards(transform.position, target.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(target);
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
