using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[RequireComponent (typeof(TankData))]
public class TankMotor : MonoBehaviour
{
    CharacterController characterController;
    TankData data;
    float vertSpeed = 0;

    void Start() {
        data = gameObject.GetComponent<TankData>();
        characterController = gameObject.GetComponent<CharacterController>();
    }
    public void MoveTank(float speed) { // Negative value is backwards
        Vector3 speedVector = transform.forward; // Forward vector direction tank is facing
        if(!characterController.isGrounded) {
            vertSpeed -= 9.8f * Time.deltaTime;
        } else {
            vertSpeed = 0;
        }
        speedVector *= speed;
        speedVector.y = vertSpeed;
        characterController.SimpleMove(speedVector); // SimpleMove multiplies by Time.deltaTime automatically
    }
    public void RotateTank(float speed) {
        // Vector3.up: (0,1,0). Rotate about the Y-axis.
        //   Positive value rotates right, negative value rotates left
        Vector3 rotateVector = Vector3.up; // Rotation value in euler angles

        speed = Mathf.Clamp(speed, -data.maxTurnSpeed, data.maxTurnSpeed); // Clamp and keep between min/max values
        rotateVector *= speed * Time.deltaTime;
        transform.Rotate(rotateVector, Space.Self); // Rotate in relation to object itself (self) instead of scene (world)
    }
    public bool RotateTowardsWP(Vector3 targetWP) {
        Vector3 vectorToNextWP;
        vectorToNextWP = targetWP - transform.position; // Distance between two points, starting at target

        if(vectorToNextWP != Vector3.zero) {
            // Find quaternion looks down vector; Quaternions are math objects that tell how to rotate an object
            Quaternion targetRotation = Quaternion.LookRotation(vectorToNextWP);
            if (targetRotation != transform.rotation) {
                // RotateTowards "from" current rotation "to" target rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.turnSpeed * Time.deltaTime);
                return true; // Rotated, true
            }
            else {
                return false; // Already facing that direction, false
            }
        } else {
            return false;
        }
    }
}