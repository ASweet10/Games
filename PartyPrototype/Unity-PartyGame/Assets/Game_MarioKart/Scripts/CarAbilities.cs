using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAbilities : MonoBehaviour
{
    [Header("Ability Controls")]
    [SerializeField] KeyCode abilityOneKey = KeyCode.Alpha1;
    [SerializeField] KeyCode abilityTwoKey = KeyCode.Alpha2;


    [Header("Objects")]
    [SerializeField] GameObject homingMissile;
    [SerializeField] Transform missileSpawnPoint;
    [SerializeField] Transform missileSpawnPointTwo;

    [Header("Parameters")]
    [SerializeField] float missileSpeed;
    [SerializeField] GameObject target;

    [SerializeField] string[] abilities;

    CarUI carUIScript;
    bool canUseAbilities = true;
    public bool abilityOneReady = false;
    public bool abilityTwoReady = false;
    public int abilityOneIndex;
    public int abilityTwoIndex;

    void Awake() {
        carUIScript = gameObject.GetComponent<CarUI>();
    }
    void Update() {
        if(canUseAbilities) {
            HandleWeaponInput();
        }
    }

    void HandleWeaponInput() {
        if(Input.GetKeyDown(abilityOneKey)) {
            if(abilityOneReady) {
                UseAbility(abilityOneIndex);
                carUIScript.SetAbilityUI(1, 0, false);
            }
        }
        if(Input.GetKeyDown(abilityTwoKey)) {
            if(abilityTwoReady) {
                UseAbility(abilityTwoIndex);
                carUIScript.SetAbilityUI(2, 0, false);
            }
        }
    }

    void UseAbility(int abilityIndex) {

        switch(abilityIndex) {
            case 0:
                UseSpeedBoost();
                break;
            case 1:
                UseInvulnerability();
                break;
            case 2:
                FireZeHomingMissile();
                break;
        }
    }




    void FireZeHomingMissile() {

        GameObject homingRocket = Instantiate(homingMissile, missileSpawnPoint.position, missileSpawnPoint.rotation);
        
        //rocketOne.transform.LookAt(target.transform);
        //StartCoroutine(SendHoming(rocketOne, target));
        //rocketTwo.transform.LookAt(target.transform);
        //StartCoroutine(SendHoming(rocketTwo, target));

        Debug.Log("firezemissiles");
        StartCoroutine(WaitForAbilityCooldown(1, 3));
    }
    private IEnumerator SendHoming(GameObject rocket, GameObject target) {
        while(Vector3.Distance(target.transform.position, rocket.transform.position) > 0.3f) {
            rocket.transform.position += (target.transform.position - rocket.transform.position).normalized * missileSpeed * Time.deltaTime;
            rocket.transform.LookAt(target.transform);
            yield return null;
        }
        Destroy(rocket);
    }

    /*
    private GameObject FindClosestTarget() {
        GameObject target = null;
       float current = Mathf.Infinity;
       float dist = 0f;
       while(dist < current) {
        //Run through all enemy vehicles within radius or FOV cone
        //If Vector3.Distance(target.transform.position - gameObject.transform.position)
        // is less than the current distance, that target is the new target
        current = dist;
       }

       return target;
    }
    */





    void UseInvulnerability() {

        StartCoroutine(WaitForAbilityCooldown(1, 3));
    }
    void UseSpeedBoost() {

        StartCoroutine(WaitForAbilityCooldown(1, 3));
    }
    /*
    private void UseTheMachineGun() {
        GameObject bullet = Instantiate(machineGunBullet, missileSpawnPointOne.position, missileSpawnPointOne.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        GameObject bulletTwo = Instantiate(machineGunBullet, missileSpawnPointTwo.position, missileSpawnPointTwo.rotation);
        Rigidbody bulletRBTwo = bulletTwo.GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * bulletSpeed);
        bulletRBTwo.AddForce(transform.forward * bulletSpeed);
        StartCoroutine(WaitForAbilityCooldown(5, 1f));
    }
    */
    private IEnumerator WaitForAbilityCooldown(int abilityNum, float duration) {
        switch(abilityNum) {
            case 1:
                abilityOneReady = false;
                yield return new WaitForSeconds(duration);
                abilityOneReady = true;
                yield break;
            case 2:
                abilityTwoReady = false;
                yield return new WaitForSeconds(duration);
                abilityTwoReady = true;
                yield break;
        }
    }


    public void ToggleWeapons(bool canUseAbilities) {
        if(canUseAbilities) {
            canUseAbilities = true;
        } else {
            canUseAbilities = false;
        }
    }
    public int RollForAbility() {
        return Random.Range(0, abilities.Length - 1);
    }
}