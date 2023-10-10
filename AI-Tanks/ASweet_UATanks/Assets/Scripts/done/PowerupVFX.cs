using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupVFX : MonoBehaviour
{
    float rotationSpeed = 2f;
    public enum RotationVector{xAxis, yAxis, zAxis};
    public RotationVector rotationVector = RotationVector.yAxis;

    void Update() {
        HandleRotation();
    }
    void HandleRotation() {
        if(rotationVector == RotationVector.xAxis) {
            transform.Rotate(new Vector3(1, 0, 0), 45 * Time.deltaTime * rotationSpeed);
        }
        else {
            transform.Rotate(new Vector3(0, 1, 0), 45 * Time.deltaTime * rotationSpeed);
        }
    }
}