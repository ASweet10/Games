using System.Collections;
using System.Collections.Generic;
using BehaviorTree;

// GuardBT placed on game object that uses logic
// AI can see world state, tracks can_see_player, player_in_range, etc.
// Define goals: Condition and desired world state, i.e. player dead
// Define actions: Preconditions (required state) and effects (resulting state due to action)
// Every frame: Select goal with highest priority that also has satisfiable conditions
// Planner runs through all possible actions, applies effects, determines if this path results in desired state
// Take best action and execute; Rinse repeat
public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.Transform[] alarmPositions;
    public static UnityEngine.Transform chosenAlarm = null;
    public static float speed = 4f;
    public static float innerFOVRange = 9f;
    public static float outerFOVRange = 18f;
    public static float attackRange = 3f;
    public static float useAlarmRange = 2f;
    public static int fleeHealthValue = 3;
    public static float fleeSpeed = 5f;

    public enum GuardType {Aggressive, RunAtLowHP, RunForAlarm}
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
            case GuardType.RunForAlarm:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckAlarmInRange(transform, alarmPositions),
                    new TaskRingAlarmBell(transform, alarmPositions)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCones(transform),
                    new TaskRunForAlarm(transform, alarmPositions)
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