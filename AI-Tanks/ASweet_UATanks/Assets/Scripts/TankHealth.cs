using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TankData component needs to be added to game object this script is attached to
[RequireComponent (typeof(TankData))]
public class TankHealth : MonoBehaviour
{
    public Game_Manager gameManager;
    [SerializeField] TankSpawner tankSpawner;
    public TankData tankData;
    public AudioClip explosionSFX;
    public GameObject explosionVFX;

    public GameObject lowHealthSmokeVFX;
    [SerializeField] GameObject healthBarUI;
    [SerializeField] Slider healthBarSlider;

    AudioSource audioSource;
    bool canTakeDamage = true;
    float maxHealth;
    float currentHealth;
    float restHealRate = 1f;
    void Awake() {
        if(gameObject.tag == "PlayerOneTank") {
            if(SceneManager.GetActiveScene().buildIndex != 0){
                Debug.Log(SceneManager.GetActiveScene().buildIndex);
                healthBarUI.SetActive(true);
            }
        }
    }
    void Start() {
        tankData = gameObject.GetComponent<TankData>();
        audioSource = gameObject.GetComponent<AudioSource>();
        tankSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<TankSpawner>();
        maxHealth = tankData.maxHealth;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game_Manager>();
        if(gameObject.tag == "PlayerOneTank") {
            if(SceneManager.GetActiveScene().buildIndex != 0){
                Debug.Log(SceneManager.GetActiveScene().buildIndex);
                healthBarUI.SetActive(true);
            }
        }
        ResetHealth();
        canTakeDamage = true;
    }
    void Update() {
        if(currentHealth <= 3) {
            lowHealthSmokeVFX.SetActive(true);
        } else {
            lowHealthSmokeVFX.SetActive(false);
        }
        Debug.Log(gameObject.name + " health: " + currentHealth);
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        if(gameObject.tag == "PlayerOneTank") {
            if(healthBarSlider != null) {
               healthBarSlider.value = currentHealth;
            }
        }
    }
    public void DamageTank() {
        if(canTakeDamage == true) {
            Debug.Log("hit!");
            currentHealth -= tankData.shellDamage;
            if(currentHealth <= 0) {
                DestroyTank();
            }
        }
    }
    public void DestroyTank() {
        audioSource.clip = explosionSFX;
        AudioSource.PlayClipAtPoint(explosionSFX, transform.position, 0.5f);
        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        if(gameObject.tag == "PlayerOneTank") {
            gameManager.DecreasePlayerLife();
            gameObject.SetActive(false);
        } else {
            tankSpawner.HandleEnemyRespawn(gameObject);
            //ResetHealth();
            gameObject.SetActive(false);
        }
    }

    public void ToggleCanTakeDamage(bool choice) {
        if(choice) {
            canTakeDamage = true;
        } else {
            canTakeDamage = false;
        }
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