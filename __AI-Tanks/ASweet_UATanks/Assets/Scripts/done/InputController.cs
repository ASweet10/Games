using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    TankMotor motor;
    TankShoot shoot;
    TankData data;
    Armor armor;
    void Start() {
        motor = gameObject.GetComponent<TankMotor>();
        shoot = gameObject.GetComponent<TankShoot>();
        data = gameObject.GetComponent<TankData>();
        armor = gameObject.GetComponent<Armor>();
    }

    void Update() {
        if(Input.GetKey(KeyCode.Q) && armor.ReturnCanUseArmorStatus()) {
            armor.ActivateArmor();
        }
        if(Input.GetKey(KeyCode.Space) && shoot.ReturnCanFireStatus()) {
            shoot.FireShell();
        }
        if (Input.GetKey(KeyCode.W)) {
            motor.MoveTank(data.moveSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            motor.MoveTank(-data.moveSpeed);
        }
        if (Input.GetKey(KeyCode.A)) {
            motor.RotateTank(-data.turnSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            motor.RotateTank(data.turnSpeed);
        }
    }
}
