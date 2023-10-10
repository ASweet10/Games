using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attached to Powerup objects to call OnTriggerEnter
public class Powerup : MonoBehaviour
{
    public AudioClip powerupSFX;
    public AudioClip badPowerupSFX;
    public enum PowerupType { speed, damage, armor, health, fireRate }
    public PowerupType powerupType;

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