using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attached to Powerup objects to call OnTriggerEnter
public class Powerup : MonoBehaviour
{
    [SerializeField] AudioClip powerupSFX;
    enum PowerupType { speed, damage, armor, health, fireRate }
    [SerializeField] PowerupType powerupType;

    float rotationSpeed = 2f;
    enum RotationVector{xAxis, yAxis, zAxis};
    [SerializeField] RotationVector rotationVector = RotationVector.yAxis;


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

    // Called when other gameobject collides with this object 
    void OnTriggerEnter(Collider other) {
        PowerupController powController = other?.GetComponent<PowerupController>();
 
        if(powController != null) {
            if(powController.CanGrabPowerup()) {
                powController.GainPowerup(powerupType.ToString());
                AudioSource.PlayClipAtPoint(powerupSFX, transform.position, 0.5f);
                powController.EnablePowerupVFX();
                Destroy(gameObject);
            }
        }
    }
}