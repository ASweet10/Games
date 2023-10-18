using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requires TankData component on this game object
[RequireComponent (typeof(TankData))]
public class AITank : MonoBehaviour
{
    [SerializeField] Transform target;
    TankData data;
    TankMotor motor;
    TankHealth health;
    TankShoot shoot;
    Armor armor;
    TankSpawner tankSpawner;
    State lastState = State.patrol; // Keep track of last state so tank can resume behavior
    [SerializeField] float aiRadius = 10f;
    [SerializeField] float powerupRadius = 13f;
    [SerializeField] Transform[] waypoints;

    bool hitWaypoint;
    GameObject powerupToSeek = null;
    int avoidanceStage = 0;
    int currentWP = 0;
    float stateEnterTime;
    float closeEnoughToWP = 3f;
    float avoidanceTime = 2f;
    float exitTime;
    [SerializeField] GameObject[] strafePoints;

    enum AIType{StartMenuA, StartMenuB, Bomber, Normal, Aggressive};
    [SerializeField] AIType aiType = AIType.Normal;
    enum State{patrol, chaseandshoot, strafe, chase, flee, rest, seekingPowerup};
    [SerializeField] State state = State.patrol;

    void Start() {
        data = gameObject.GetComponent<TankData>();
        motor = gameObject.GetComponent<TankMotor>();
        health = gameObject.GetComponent<TankHealth>();
        shoot = gameObject.GetComponent<TankShoot>();
        armor = gameObject.GetComponent<Armor>();
        target = GameObject.FindGameObjectWithTag("PlayerOneTank").transform;
        tankSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<TankSpawner>();
        waypoints = tankSpawner.ReturnWaypoints().ToArray();
    }
    void Update() {
        target = FindTarget();
        HandleAIBehavior();
    }
    void AvoidTarget() {
        if(avoidanceStage == 1) {
            motor.RotateTank(-1 * data.turnSpeed); //Rotate left
            if(CanMove(data.moveSpeed)) {
                avoidanceStage = 2;
            }
            exitTime = avoidanceTime; //Set # of seconds spent in stage 2
        }
        else if(avoidanceStage == 2) {
            if(CanMove(data.moveSpeed)) {
                exitTime -= Time.deltaTime; //Subtract frame time from timer and move
                motor.MoveTank(data.moveSpeed);
                if(exitTime <= 0) { //Have moved for long enough?
                    avoidanceStage = 0; //Enable chase mode
                }
            } else {
                avoidanceStage = 1; //Can't move forward, back to stage 1
            }
        }
    }
    void HandleAIBehavior() {
        if(armor.ReturnCanUseArmorStatus()) {
            armor.ActivateArmor();
        }
        if(state == State.chase) {
            if (avoidanceStage != 0) {
                AvoidTarget();
            } else {
                if(target != null){
                    ChaseTarget(); 
                } else {
                    ChangeState(State.patrol);
                }
            }
            lastState = State.chase;
            if(health.ReturnCurrentHealth() < data.maxHealth * 0.5f) { // If hp < 50%...
                ChangeState(State.flee);
            }
            if(Vector3.Distance(transform.position, target.position) <= aiRadius) { // If player within range...
                ChangeState(State.chaseandshoot);
            }
            if(PowerupInRange() != null) {
                powerupToSeek = PowerupInRange();
                ChangeState(State.seekingPowerup);
            }
        }
        else if(state == State.chaseandshoot) {
            if(avoidanceStage != 0) {
                AvoidTarget();
            } else {
                if(shoot.ReturnCanFireStatus()) {
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, transform.forward, out hit, 10f)) {
                        if(hit.collider.CompareTag("PlayerOneTank")) {
                            shoot.FireShell();
                        }
                    }
                } else {
                    ChangeState(State.strafe);
                }
                if(target != null){
                    ChaseTarget();
                } else {
                    ChangeState(State.patrol);
                }
            }
            lastState = State.chaseandshoot;
            if(health.ReturnCurrentHealth() < data.maxHealth * 0.5f) { //If hp < 50%...
                ChangeState(State.flee);
            }
            if(PowerupInRange() != null){
                powerupToSeek = PowerupInRange();
                ChangeState(State.seekingPowerup);
            }
        }
        else if(state == State.strafe) {
            if(avoidanceStage != 0) {
                AvoidTarget();
            } else {
                HandleStrafe();
            }
            lastState = State.strafe;
            if(health.ReturnCurrentHealth() < data.maxHealth * 0.5f) { //If hp < 50%...
                ChangeState(State.flee);
            }
            if(shoot.ReturnCanFireStatus()) {
                ChangeState(State.chaseandshoot);
            }
            if(PowerupInRange() != null){
                powerupToSeek = PowerupInRange();
                ChangeState(State.seekingPowerup);
            }
        }
        else if(state == State.patrol) {
            if(avoidanceStage != 0) {
                AvoidTarget();
            } else {
                HandlePatrol();
            }
            lastState = State.patrol;
            if(health.ReturnCurrentHealth() < data.maxHealth * 0.5f) { // If hp < 50%
                ChangeState(State.flee);
            }
            if(Time.time >= stateEnterTime + 10 && hitWaypoint == false) {
                ChangeState(State.chase);
            }
            if(Vector3.Distance(transform.position, target.position) <= aiRadius) { // If player within range...
                ChangeState(State.chaseandshoot);
            }
            if(PowerupInRange() != null) {
                powerupToSeek = PowerupInRange();
                ChangeState(State.seekingPowerup);
            }
        }
        else if(state == State.seekingPowerup) {
            if(avoidanceStage != 0) {
                AvoidTarget();
            } else {
                if(powerupToSeek != null){
                    GoToPowerup(powerupToSeek);
                } else {
                    ChangeState(lastState); // Powerup gone; resume prior behavior
                }
            }
        }
        else if(state == State.flee) {
            if(avoidanceStage != 0) {
                AvoidTarget();
            } else {
                Flee();
            }
            lastState = State.flee;
            if(Vector3.Distance(transform.position, target.position) >= aiRadius) { // Fleeing & outside range?
                if(health.ReturnCurrentHealth() >= (data.maxHealth * 0.5f)) {
                    ChangeState(State.chase);
                } else {
                    ChangeState(State.rest);
                }
            } else {
                Flee();
            }
        }
        else if(state == State.rest) {
            Rest();
            lastState = State.rest;
            if(Vector3.Distance(transform.position, target.position) <= aiRadius) {
                ChangeState(State.flee);
            }
            else if(health.ReturnCurrentHealth() >= data.maxHealth) {
                ChangeState(State.chase);
            }
        }
    }

    GameObject PowerupInRange() {
        GameObject[] activePowerups = GameObject.FindGameObjectsWithTag("Powerup");
        foreach(GameObject powerup in activePowerups) {
            if(Vector3.Distance(transform.position, powerup.transform.position) <= powerupRadius) {
                return powerup;
            }
            else {
                return null;
            }
        }
        return null;
    }

    void GoToPowerup(GameObject powerup) {
        motor.RotateTowardsWP(powerup.transform.position);
        if(CanMove(data.moveSpeed)) { //Can we move (data.MoveSpeed) units away?
            motor.MoveTank(data.moveSpeed);
        }
        else {
            avoidanceStage = 1; // Obstacle avoidance stage 1
        }
    }

    void ChangeState(State newState) {
        state = newState;
        stateEnterTime = Time.time;
    }
    bool CanMove(float speed) {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed)) { //If raycast hits something...
            if(!hit.collider.CompareTag("PlayerOneTank")) { //If we don't hit the player...
                return false; //Can't move
            }
        }
        return true; //Otherwise, can move
    }

    Transform FindTarget() {
        GameObject playerOneRef = GameObject.FindGameObjectWithTag("PlayerOneTank");
        if(playerOneRef == null) {
            return null;
        } else {
            return playerOneRef.transform;
        }
    }
    void ChaseTarget() {
        motor.RotateTowardsWP(target.position);
        if(CanMove(data.moveSpeed)) {   // Can we move (data.MoveSpeed) units away?
            motor.MoveTank(data.moveSpeed);
        } else {
            avoidanceStage = 1; // Obstacle avoidance stage 1
        }
    }
    void HandlePatrol() {
        if (motor.RotateTowardsWP(waypoints[currentWP].position) == false) { // Tank unable to rotate?
            motor.MoveTank(data.aiPatrolMoveSpeed); // Move tank
        }

        if(Vector3.Distance(waypoints[currentWP].position, transform.position) < closeEnoughToWP) {
            hitWaypoint = true;
            if(currentWP < (waypoints.Length - 1)) {
                currentWP++;
            } else {
                currentWP--;
            }
            hitWaypoint = false;
        }
    }
    void HandleStrafe() {
        if (motor.RotateTowardsWP(strafePoints[Random.Range(0, strafePoints.Length)].transform.position) == false) { // Tank unable to rotate?
            motor.MoveTank(data.aiPatrolMoveSpeed); // Move tank
        } else {
            motor.RotateTank(data.maxTurnSpeed);
        }
    }
    void Flee() {
        Vector3 vectorToTarget = target.position - transform.position; // Vector from this object to target
        Vector3 vectorAwayFromTarget = vectorToTarget * -1; // Flip vector away from target
        vectorAwayFromTarget.Normalize(); // Give vector magnitude of 1
        vectorAwayFromTarget *= data.fleeDistance; // Multiply by fleeDistance to make vector that length
        Vector3 fleePosition = vectorAwayFromTarget + transform.position; // Create position to flee to
        motor.RotateTowardsWP(fleePosition);
        motor.MoveTank(data.moveSpeed);
    }
    void Rest() {
        health.RestTank();
    }
}