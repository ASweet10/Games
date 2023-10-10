using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class Patrol : Node 
{
    private Transform patrolUnit;
    private Animator animator;
    private Transform[] _waypoints;

    private int currentWaypoint = 0;
    private float waitTime = 1f;
    private float waitCounter = 0f;
    private bool waiting = false;

    public Patrol(Transform transform, Transform[] waypoints)
    {
        patrolUnit = transform;
        _waypoints = waypoints;
        animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if(waiting)
        {
            waitCounter += Time.deltaTime;
            if(waitCounter >= waitTime)
            {
                waiting = false;
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            Transform waypoint = _waypoints[currentWaypoint];
            if(Vector3.Distance(patrolUnit.position, waypoint.position) < 0.01f)
            {
                patrolUnit.position = waypoint.position;
                waitCounter = 0f;
                waiting = true;
                animator.SetBool("Walking", false);

                currentWaypoint = (currentWaypoint + 1) % _waypoints.Length;
            }
            else
            {
                patrolUnit.position = Vector3.MoveTowards(
                    patrolUnit.position, waypoint.position, GuardBehaviorTree.speed * Time.deltaTime);
                
                
                /*Vector3 gravityVector = new Vector3(0, -9.8f, 0);
                patrolUnit.position = gravityVector;
                */
                
                patrolUnit.LookAt(waypoint.position);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}