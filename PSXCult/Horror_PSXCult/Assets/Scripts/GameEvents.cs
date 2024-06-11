using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField] GameObject hunter;
    [SerializeField] GameObject fireSmall;
    [SerializeField] GameObject fireMediumSmoke;
    [SerializeField] GameObject fireBigSmoke;
    [SerializeField] Transform campfirePosition;
    [SerializeField] float smallFireTime = 5f;
    [SerializeField] float mediumFireTime = 10f;

    void Start() {
        //StartCoroutine(StartCampFire());
    }

    public IEnumerator StartCampFire() {
        var smallFire = Instantiate(fireSmall, campfirePosition.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(smallFireTime);
        Destroy(smallFire);
        var mediumFire = Instantiate(fireMediumSmoke, campfirePosition.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(mediumFireTime);
        Destroy(mediumFire);
        Instantiate(fireBigSmoke, campfirePosition.position, Quaternion.identity);
    }

    public void SpawnHunterAtCamp() {
        hunter.SetActive(true);
    }

}