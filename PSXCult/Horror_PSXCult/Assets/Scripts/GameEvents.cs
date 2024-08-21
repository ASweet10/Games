using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [SerializeField] GameObject hunter;
    GameController gameController;
    [SerializeField] AudioSource itemPickupAudio;

    [Header("Campfire")]
    [SerializeField] GameObject fireSmall;
    [SerializeField] GameObject fireMediumSmoke;
    [SerializeField] GameObject fireBigSmoke;
    [SerializeField] GameObject campfire;
    [SerializeField] GameObject campfireCollider;
    [SerializeField] GameObject campfireTransparent;
    [SerializeField] Transform campfirePosition;
    [SerializeField] GameObject zippo;
    [SerializeField] GameObject lighterFluid;
    [SerializeField] float smallFireTime = 5f;
    [SerializeField] float mediumFireTime = 10f;
    [SerializeField] GameObject[] firewood;
    int firewoodCollected;
    int firewoodMax = 2;  //  change to 6 for game, 2 for testing

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
        firewoodCollected = 0;
        //TransitionToNighttime();
    }

    public void HandleCollectFirewood() {
        firewoodCollected ++;
        itemPickupAudio.Play();
        if(firewoodCollected < firewoodMax) {
            StartCoroutine(gameController.DisplayPopupMessage("Collect firewood (" + firewoodCollected + " of " + firewoodMax + ")"));
        } else {
            StartCoroutine(gameController.DisplayPopupMessage("Time to build a fire"));
            gameController.playerNeedsFirewood = false;
            StartCoroutine(gameController.HandleNextObjective());

            campfireTransparent.SetActive(true);
            zippo.tag = "Zippo";
            zippo.GetComponent<BoxCollider>().enabled = true;
            lighterFluid.tag = "Lighter Fluid";
            lighterFluid.GetComponent<BoxCollider>().enabled = true;

            foreach(GameObject wood in firewood) {
                wood.tag = "Untagged";
                var outline = wood.GetComponent<Outline>();
                outline.enabled = false;

                var woodCollider = wood.GetComponent<BoxCollider>();
                woodCollider.enabled = false;
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

    public void HandleBuildFire() {
        campfireTransparent.SetActive(false);
        campfire.SetActive(true);
        campfire.tag = "Start Fire";
    }

    public void HandleCollectZippo() {
        gameController.playerNeedsZippo = false;
        itemPickupAudio.Play();
        if(!gameController.playerNeedsLighterFluid) {
            StartCoroutine(gameController.HandleNextObjective());
        }
    }
    public void HandleCollectLighterFluid() {
        gameController.playerNeedsLighterFluid = false;
        itemPickupAudio.Play();
        if(!gameController.playerNeedsZippo) {
            StartCoroutine(gameController.HandleNextObjective());
        }
    }
    public IEnumerator StartCampFire() {
        campfire.tag = "Untagged";
        campfireCollider.tag = "Untagged";
        // Animation of lighter fluid pouring onto fire
        // Animation of zippo starting fire? 
        //   or match thrown onto fire? (change from zippo -> match?)
        var smallFire = Instantiate(fireSmall, campfirePosition.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(smallFireTime);
        Destroy(smallFire);
        var mediumFire = Instantiate(fireMediumSmoke, campfirePosition.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(mediumFireTime);
        Destroy(mediumFire);
        Instantiate(fireBigSmoke, campfirePosition.position, Quaternion.identity);
        gameController.fireStarted = true;
        gameController.HandleNextObjective();
    }


    public void HandleCollectCarKeys() {
        gameController.playerNeedsCarKeys = false;
        itemPickupAudio.Play();
        StartCoroutine(gameController.HandleNextObjective());
        // enable car keys in pause menu
        // change player car tag
        // spawn additional bad guys?
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

}