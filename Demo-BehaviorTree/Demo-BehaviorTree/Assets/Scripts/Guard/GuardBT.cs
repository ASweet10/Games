using System.Collections.Generic;
using BehaviorTree;

// Place GuardBT on guard that uses logic
// Goals: Condition and desired world state, (player dead)
// Actions: Preconditions (required state) and effects (resulting state due to action)
// Every frame: Select highest-priority goal with satisfiable conditions
// Planner runs through all possible actions, determines if path results in desired state
// Take best action and execute; Rinse repeat

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.Transform alarmPosition;
    //public UnityEngine.Transform leftLegParent;
    //public UnityEngine.Transform handParent;
    //public UnityEngine.Transform sword;
    public static float gravity = -9.8f;
    public static float walkSpeed = 5f;
    public static float runSpeed = 10f;
    public static float FOVRange = 9f;
    public static float attackRange = 3f;
    public static float useAlarmRange = 2f;
    public static int fleeHealth = 3;
    public static bool canUseAlarm = true;
    public static bool alarmRungRecently = false;
    public static bool swordDrawn = false;

    public enum GuardType {Aggressive, RunForAlarm, Flanker}
    public GuardType guardType;

    protected override Node SetupTree() {
        switch(guardType) {
            case GuardType.Aggressive:
                Node root = new Selector(new List<Node> {
                new Sequence(new List<Node> { 
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node>{ 
                    new CheckEnemyInFOVCone(transform),
                    new TaskRunToTarget(transform) // Go to player if seen in FOV cone
                }),
                /*
                new Sequence(new List<Node> {
                    new CheckReactionToAlarm(transform, alarmPosition),
                    new TaskRunToTarget(alarmPosition)
                }),
                */
                new TaskPatrol(transform, waypoints), //Default behavior, lowest priority
                });
                return root;
            case GuardType.RunForAlarm:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node> {
                    //new CheckTargetStatus(transform),
                    new CheckEnemyInFOVCone(transform),
                    new TaskRunToTarget(transform)
                }),
                new Sequence(new List<Node>{
                    new CheckAlarmInRange(transform, alarmPosition),
                    new TaskRingAlarmBell(transform, alarmPosition)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCone(transform),
                    new CheckAlarmStatus(),
                    new TaskRunForAlarm(transform, alarmPosition)
                }),
                new TaskPatrol(transform, waypoints),
                });
                return root;
            case GuardType.Flanker:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node> {
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node> {
                    new CheckEnemyInFOVCone(transform),
                    new TaskRunToTarget(transform)
                }),
                new TaskPatrol(transform, waypoints),
                });
                return root;
            default:
                root = new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new CheckEnemyInAttackRange(transform),
                    new TaskAttack(transform)
                }),
                new Sequence(new List<Node>{
                    new CheckEnemyInFOVCone(transform),
                    new TaskRunToTarget(transform)
                }),
                new TaskPatrol(transform, waypoints),
                });
                return root;
        }
    }
}