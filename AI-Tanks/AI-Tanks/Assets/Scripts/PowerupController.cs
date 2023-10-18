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
        CheckForPowerupCooldown();
    }
    public void GainPowerup(string powerup) {
        canGrabPowerup = false;
        startTime = Time.time;
        switch(powerup) {
            case "speed":
                data.moveSpeed *= speedModifier;
                data.maxTurnSpeed = newTurnSpeed;
                currentPowerup = "speed";
                break;
            case "armor":
                armor.SetCanUseArmor(true);
                armor.SetShieldUI(true);
                currentPowerup = "armor";
                break;
            case "damage":
                data.shellDamage *= damageModifier;
                currentPowerup = "damage";
                break;
            case "fireRate":
                data.shootReloadTimer /= fireRateModifier;
                currentPowerup = "fireRate";
                break;
            case "health":
                health.ResetHealth();
                currentPowerup = "health";
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
                break;
            case "armor":
                break;
            case "damage":
                data.shellDamage /= damageModifier;
                break;
            case "fireRate":
                data.shootReloadTimer *= fireRateModifier;
                break;
            case "health":
                break;
            default:
                break;
        }
        currentPowerup = "";
        startTime = 0;
    }
    void CheckForPowerupCooldown() {
        if(Time.time - startTime >= cooldown) {
            RemoveCurrentPowerup(currentPowerup);
            canGrabPowerup = true;
        }
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