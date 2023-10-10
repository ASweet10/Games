using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    public float moveSpeed = 3f; //Speed for MoveTank in meters/sec
    public float aiPatrolMoveSpeed = 6f; //Speed for AIPatrol tank in meters/sec
    public float turnSpeed = 180f; //Turnspeed for RotateTank in degrees/sec
    public float maxTurnSpeed = 100f;


    public float fleeDistance = 5f;
    public float shellSpeed = 1000f; //Tank.Shoot speed
    public float shootReloadTimer = 3f;
    public float shellDamage = 10f;
    public float maxHealth = 100f;

}
