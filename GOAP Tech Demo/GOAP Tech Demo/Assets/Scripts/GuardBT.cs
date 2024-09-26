using System.Collections;
using System.Collections.Generic;

using BehaviorTree;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;   

    public static float speed = 4f;
    public static float innerFOVRange = 9f;
    public static float outerFOVRange = 18f;
    public static float attackRange = 3f;
    public static int fleeHealthValue = 3;
    public static float fleeSpeed = 5f;

    public enum GuardType {Aggressive, RunAtLowHP, RunForAlarm}
    public GuardType guardType;

    protected override Node SetupTree(){
        switch(guardType){
            case GuardType.Aggressive:
                Node root = new Selector(new List<Node>{
                new Sequence(new List<Node>{ // Last checked; If player in attack range, attack
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node>{ // Next checked; If player seen in FOV cone, go to them
                    new CheckEnemyInFOVCones(transform),
                    new TaskGoToTarget(transform)
                }),
                new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
                });
                return root;
            case GuardType.RunAtLowHP:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckGuardHP(transform),
                    new TaskRunAway(transform)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCones(transform),
                    new TaskGoToTarget(transform)
                }),
                new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
                });
                return root;
            case GuardType.RunForAlarm:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCones(transform),
                    new TaskGoToTarget(transform)
                }),
                new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
                });
                return root;
            default:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCones(transform),
                    new TaskGoToTarget(transform)
                }),
                new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
                });
                return root;
        }
    }
}
