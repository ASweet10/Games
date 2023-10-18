using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TankData component needs to be added to game object this script is attached to
[RequireComponent (typeof(TankData))]
public class TankHealth : MonoBehaviour
{
    Game_Manager gameManager;
    TankSpawner tankSpawner;
    TankData tankData;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] GameObject explosionVFX;

    [SerializeField] GameObject lowHealthSmokeVFX;
    bool canTakeDamage = true;
    float maxHealth;
    float currentHealth;
    float restHealRate = 1f;

    void Start() {
        tankData = gameObject.GetComponent<TankData>();
        tankSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<TankSpawner>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game_Manager>();
        maxHealth = tankData.maxHealth;
        ResetHealth();
        canTakeDamage = true;
    }
    void Update() {
        if(currentHealth <= 50) {
            lowHealthSmokeVFX.SetActive(true);
        } else {
            lowHealthSmokeVFX.SetActive(false);
        }
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }
    public void DamageTank() {
        if(canTakeDamage == true) {
            currentHealth -= tankData.shellDamage;
            if(currentHealth <= 0) {
                DestroyTank();
            }
        }
        Debug.Log(gameObject.name + " health: " + currentHealth);
    }
    public void DestroyTank() {
        AudioSource.PlayClipAtPoint(explosionSFX, transform.position, 0.5f);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        if(gameObject.tag == "PlayerOneTank") {
            gameManager.HandleGameOver();
        } else {
            gameManager.AddPoint();
            ResetHealth();
            tankSpawner.HandleEnemyRespawn(gameObject.name);
        }
    }

    public void ToggleCanTakeDamage(bool choice) {
        canTakeDamage = choice;
    }
    public void RestTank() {
        currentHealth += restHealRate * Time.deltaTime; //Increase HP by (restHealRate / sec)
    }
    public float ReturnCurrentHealth() {
        return currentHealth;
    }
    public void ResetHealth() {
        currentHealth = maxHealth;
    }
}