using System.Collections.Generic;
using BehaviorTree;

public class GuardBehaviorTree : Tree 
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 5f;
    public static float fovRange = 6f;
    public static float attackRange = 3f;

    protected override Node SetupTree() 
    { 
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInFOV(transform),
                new GoToTarget(transform),
            }),
            new Patrol(transform, waypoints)
        });

        return root;
    }

}