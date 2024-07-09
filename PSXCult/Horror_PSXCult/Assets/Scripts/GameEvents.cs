using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField] GameObject hunter;

    [Header("Campfire")]
    [SerializeField] GameObject fireSmall;
    [SerializeField] GameObject fireMediumSmoke;
    [SerializeField] GameObject fireBigSmoke;
    [SerializeField] Transform campfirePosition;
    [SerializeField] float smallFireTime = 5f;
    [SerializeField] float mediumFireTime = 10f;
    [SerializeField] GameObject[] firewood;
    int firewoodCount = 0;
    int firewoodMax = 6;
    bool needsFirewood = true;
    public bool NeedsFirewood {
        get { return needsFirewood; }
        set { needsFirewood = value; }
    }

    [Header("Night")]
    [SerializeField] Material nightSkyboxMat;
    [SerializeField] GameObject directionalLightDay;
    [SerializeField] GameObject directionalLightNight;
    [SerializeField] GameObject[] tentFlaps;
    [SerializeField] GameObject davidRef;

    void Start() {
        //TransitionToNighttime();
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
    public void HandleCollectFirewood() {
        firewoodCount ++;
        if(firewoodCount >= firewoodMax) {
            firewoodCount = 6;
            needsFirewood = false;
            foreach(GameObject wood in firewood) {
                wood.tag = "";
                var outline = wood.GetComponent<Outline>();
                outline.enabled = false;
            }
        }
    }

    public void SpawnHunterAtCamp() {
        hunter.SetActive(true);
    }
    public void TransitionToNighttime() {
        RenderSettings.skybox = nightSkyboxMat;
        directionalLightDay.SetActive(false);
        directionalLightNight.SetActive(true);
        foreach(GameObject tentFlap in tentFlaps) {
            tentFlap.SetActive(false);
        }
        davidRef.tag = "Untagged"; // Set tag to null; disable dialog
        var davidCharacter = davidRef.GetComponent<AICharacter>();
        davidCharacter.StateRef = AICharacter.State.dead;
    }
}