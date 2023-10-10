using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attached to tanks; Keep track of which powerup is active
[RequireComponent (typeof(TankData))]
public class PowerupController : MonoBehaviour
{
    [SerializeField] float speedModifier = 2f;
    [SerializeField] float oldTurnSpeed;
    [SerializeField] float newTurnSpeed = 150f;
    [SerializeField] float damageModifier = 2f;
    [SerializeField] float fireRateModifier = 2f;
    [SerializeField] float duration;
    TankData data;
    TankHealth health;
    Armor armor;
    [SerializeField] ParticleSystem powerupVFX;
    bool canGrabPowerup = true;
    [SerializeField] float cooldown = 10f;
    float startTime;
    string currentPowerup;

    void Start() {
        data = gameObject.GetComponent<TankData>();
        health = gameObject.GetComponent<TankHealth>();
        armor = gameObject.GetComponent<Armor>();
        oldTurnSpeed = data.maxTurnSpeed;
    }
    void Update() {
        if(!canGrabPowerup) {
            CheckForPowerupCooldown();
        }
    }
    public void GainPowerup(string powerup) {
        canGrabPowerup = false;
        switch(powerup) {
            case "speed":
                data.moveSpeed *= speedModifier;
                data.maxTurnSpeed = newTurnSpeed;
                startTime = Time.time;
                currentPowerup = "speed";
                break;
            case "armor":
                armor.SetCanUseArmor(true);
                armor.SetShieldUI(true);
                break;
            case "damage":
                data.shellDamage *= damageModifier;
                startTime = Time.time;
                currentPowerup = "speed";
                break;
            case "fireRate":
                data.shootReloadTimer /= fireRateModifier;
                startTime = Time.time;
                currentPowerup = "speed";
                break;
            case "health":
                health.ResetHealth();
                break;
            default:
                break;
        }
    }
    void RemoveCurrentPowerup(string powerup) {
        switch(powerup) {
            case "speed":
                data.moveSpeed /= speedModifier;
                data.maxTurnSpeed = oldTurnSpeed;
                currentPowerup = "";
                startTime = 0;
                break;
            case "armor":
                break;
            case "damage":
                data.shellDamage /= damageModifier;
                currentPowerup = "";
                startTime = 0;
                break;
            case "fireRate":
                data.shootReloadTimer *= fireRateModifier;
                currentPowerup = "";
                startTime = 0;
                break;
            case "health":
                break;
            default:
                break;
        }
    }
    public void CheckForPowerupCooldown() {
        if(Time.time - startTime >= cooldown) {
            RemoveCurrentPowerup(currentPowerup);
            canGrabPowerup = true;
        }
        //Debug.Log(Time.time - startTime);
    }
    public void EnablePowerupVFX() {
        powerupVFX.gameObject.SetActive(true);
        StartCoroutine(WaitAndDisablePowerupRing());
    }
    IEnumerator WaitAndDisablePowerupRing() {
        canGrabPowerup = false;
        yield return new WaitForSecondsRealtime(2f);
        powerupVFX.gameObject.SetActive(false);
        canGrabPowerup = true;
    }
    public bool CanGrabPowerup() {
        return canGrabPowerup;
    }
}