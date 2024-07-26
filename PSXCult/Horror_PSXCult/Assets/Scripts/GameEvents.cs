using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField] GameObject hunter;
    GameController gameController;

    [Header("Campfire")]
    [SerializeField] GameObject fireSmall;
    [SerializeField] GameObject fireMediumSmoke;
    [SerializeField] GameObject fireBigSmoke;
    [SerializeField] Transform campfirePosition;
    [SerializeField] float smallFireTime = 5f;
    [SerializeField] float mediumFireTime = 10f;
    [SerializeField] GameObject[] firewood;
    int firewoodNeeded;
    int firewoodMax = 6;

    [Header ("Tents")]
    [SerializeField] GameObject[] unfinishedTents;
    [SerializeField] GameObject[] finishedTentObjects;
    [SerializeField] GameObject[] tentFlaps;

    [Header("Night")]
    [SerializeField] Material nightSkyboxMat;
    [SerializeField] GameObject directionalLightDay;
    [SerializeField] GameObject directionalLightNight;
    [SerializeField] GameObject davidRef;

    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        firewoodNeeded = firewoodMax;
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
        firewoodNeeded --;
        if(firewoodNeeded > 0) {
            StartCoroutine(gameController.DisplayPopupMessage("Need " + firewoodNeeded + " firewood"));
        } else {
            StopCoroutine(gameController.DisplayPopupMessage(""));
            gameController.playerNeedsFirewood = false;
            StartCoroutine(gameController.HandleNextObjective());
            foreach(GameObject wood in firewood) {
                wood.tag = "";
                var outline = wood.GetComponent<Outline>();
                outline.enabled = false;
            }
            foreach(GameObject tent in finishedTentObjects) {
                tent.SetActive(true);
            }
            foreach(GameObject tentFlap in tentFlaps) {
                tentFlap.SetActive(true);
            }
            SpawnHunterAtCamp();
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

    public void StartDriveToParkCutscene() {

    }
    public void HandleWalkToCampsite() {

        //if at campsite
        // friends move to tent area
        // Each friend moves to their tent
        // Once at tent, enable
        foreach (GameObject tent in unfinishedTents) {
            // Move to tent
            // change state to "working"
            tent.SetActive(true);
        }
        // start setting up (working animation)
        // if you talk to them they stand up
    }
}