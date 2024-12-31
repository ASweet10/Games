using System.Collections;
using System.Collections.Generic;

using BehaviorTree;
using UnityEngine;

// GuardBT: place on enemy that uses logic; AI can see state, track player, etc.
// Goals: Condition and desired world state, i.e. player dead
// Actions: Preconditions (required state) and effects (resulting state due to action)
// Every frame: Select highest-priority goal with satisfiable conditions
// Planner runs through all possible actions, determines if path results in desired state
// Take best action and execute; Rinse repeat

public class GuardBT : BehaviorTree.Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.Transform alarmPosition;
    public AudioSource bellAudio;
    //public static UnityEngine.Transform chosenAlarm = null;
    public static float gravity = -9.8f;
    public static float walkSpeed = 5f;
    public static float runSpeed = 10f;
    public static float innerFOVRange = 9f;
    public static float outerFOVRange = 18f;
    public static float attackRange = 3f;
    public static float useAlarmRange = 2f;
    public static int fleeHealth = 3;
    public static bool canUseAlarm = true;

    public enum GuardType {Aggressive, RunForAlarm, Flanker}
    public GuardType guardType;

    protected override Node SetupTree() {
        switch(guardType) {
            case GuardType.Aggressive:
                Node root = new Selector(new List<Node> {
                new Sequence(new List<Node> { // Last checked; If player in attack range, attack
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
            case GuardType.RunForAlarm:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckAlarmInRange(transform, alarmPosition),
                    new TaskRingAlarmBell(transform, alarmPosition, bellAudio)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCones(transform),
                    new TaskRunForAlarm(transform, alarmPosition)
                }),
                new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
                });
                return root;
            case GuardType.Flanker:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node> {
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node> {
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
/*
case GuardType.RunAtLowHP:
    root = new Selector(new List<Node>{
    new Sequence(new List<Node> {
        new CheckGuardHP(transform),
        new TaskRunAway(transform)
    }),
    new Sequence(new List<Node> {
        new CheckEnemyInAttackRange(transform),
        new TaskAttack(transform)
    }),
    new Sequence(new List<Node> {
        new CheckEnemyInFOVCones(transform),
        new TaskGoToTarget(transform)
    }),
    new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
    });
    return root;
*/