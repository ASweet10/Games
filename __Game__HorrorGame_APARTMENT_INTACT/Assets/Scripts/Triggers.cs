using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Triggers : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] MonsterAI monsterAI;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;
    bool triggerActive = true;

    [Header("Triggerable Sounds")]
    [SerializeField] float creakWaitTime = 15f;
    [SerializeField] AudioClip[] buildingCreakClips;

    [Header("Blown Fuse")]
    [SerializeField] Light[] breakerLights;    
    [SerializeField] GameObject finalClosetTrigger;
    [SerializeField] GameObject shadowBlurTrigger;
    [SerializeField] AudioSource tvStaticAudioSource;
    [SerializeField] AudioSource radioAudioSource;

    [Header("Apartment Fixes")]
    [SerializeField] GameObject heldStepLadder;
    [SerializeField] GameObject placedStepLadder;
   
    [Header("Scare Moments")]
    [SerializeField] GameObject blurMonster;
    [SerializeField] GameObject stopWhisperTrigger;
    [SerializeField] GameObject stairWhisperAudio;
    [SerializeField] GameObject apartmentDoorMonster;

    [Header("Flashlight Drop")]
    [SerializeField] FlashlightToggle flashlightToggle;
    [SerializeField] GameObject groundFlashlight;

    [Header("Finale Monster")]
    [SerializeField] GameObject spawnedFinaleMonster;
    [SerializeField] Light effectedLight;

    [Header("Final Closet")]
    [SerializeField] GameObject exteriorObjects;
    [SerializeField] GameObject maintenanceTunnelObject;
    [SerializeField] GameObject finalChaser;
    [SerializeField] Animator closetDoorAnim;
    [SerializeField] AudioSource closetCreakAudioSource;
    [SerializeField] AudioClip closetCreakAudio;
    [SerializeField] GameObject topOfStairsDoor;
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioClip rumbleLoop;

    [SerializeField] enum TriggerType {LockerHide, PsstScare, BuildingCreak, BreakableLights,
        StopWhisper, DropFlashlight, ShadowBlur, BlownFuse, FinaleMonster, PromptTrash, DestroyDoorMonster,
        PromptLadder, PlaceLadder, PromptSink, FinalCloset}
    [SerializeField] TriggerType triggerType;

    string trashString = "Drop trash down chute at end of hallway."; 
    string lightString = "Find step ladder to reach the light.";    
    string sinkString = "Tighten the bolt to stop the sink's leak";
    string dropFlashlightString = "Damnit I dropped the flashlight! \n I think it's broken....";
    string blownFuseString = "Find replacement fuse for fuse box";

    void Start() {
        if (gameController == null) {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        if(audioSource == null) {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        if(effectedLight == null) {
            effectedLight = gameObject.GetComponent<Light>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(triggerActive)
            {
                if(triggerType == TriggerType.PsstScare) {
                    TriggerOneShotSFX();
                }
                else if(triggerType == TriggerType.PromptTrash) {
                    PromptApartmentFix("trash");
                }
                else if(triggerType == TriggerType.PromptSink) {
                    PromptApartmentFix("sink");
                }
                else if(triggerType == TriggerType.PromptLadder) {
                    PromptApartmentFix("light");
                }
                else if(triggerType == TriggerType.PlaceLadder) {
                    PlaceStepLadder();
                }
                else if(triggerType == TriggerType.ShadowBlur) {
                    TriggerShadowBlur();
                }
                else if(triggerType == TriggerType.DestroyDoorMonster) {
                    TriggerDespawnApartmentDoorMonster();
                }
                else if(triggerType == TriggerType.StopWhisper) {
                    TriggerStopWhispering();
                }
                else if(triggerType == TriggerType.DropFlashlight) {
                    TriggerDropFlashlight();
                }
                else if(triggerType == TriggerType.BuildingCreak) {
                    StartCoroutine(TriggerAmbientSFX());
                }
                else if(triggerType == TriggerType.BlownFuse){
                    TriggerBlownFuse();
                }
                if(triggerType == TriggerType.FinalCloset) {
                    TriggerFinalCloset();
                }
                else if(triggerType == TriggerType.FinaleMonster) {
                    SpawnFinaleMonster();
                }
            }
        }
        else if(other.tag == "Monster")
        {
            if(triggerType == TriggerType.BreakableLights)
            {
                if(triggerActive)
                {
                    TriggerBrokenLight();
                    triggerActive = false; // Single use
                }
            }
        }
    }

    public void TriggerBrokenLight() //Monster triggers when close enough to light
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        effectedLight.enabled = false;

        triggerActive = false; // Single use
    }

    public void TriggerOneShotSFX()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        triggerActive = false; // Single use
    }

    public void PromptApartmentFix(string task) //Prompt task; Lock player in task until done
    {
        if(!gameController.engagedInTask)
        {
            if(task == "trash")
            {
                gameController.ShowPopupMessage(trashString, 4f, true);
                gameController.currentCheckpoint = 3;
            }
            else if(task == "light")
            {
                gameController.ShowPopupMessage(lightString, 4f, true);
                gameController.currentCheckpoint = 4;
                shadowBlurTrigger.SetActive(true);
            }
            else if(task == "sink")
            {
                gameController.ShowPopupMessage(sinkString, 4f, true);
                gameController.currentCheckpoint = 6;
            }
            gameController.engagedInTask = true;
            triggerActive = false; // Single use
        }
    }

    public void PlaceStepLadder() //Place ladder on floor if in inventory
    {
        placedStepLadder.SetActive(true);
        heldStepLadder.SetActive(false);
        triggerActive = false; // Single use
    }

    public void TriggerStopWhispering() //Disable whispers in basement
    {
        stairWhisperAudio.SetActive(false);
        triggerActive = false; // Single use
    }

    public void TriggerShadowBlur() //Shadow blurs past, spooky
    {
        blurMonster.SetActive(true);
        stopWhisperTrigger.SetActive(true);
        stairWhisperAudio.SetActive(true);

        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        triggerActive = false; // Single use
    }

    public void TriggerBlownFuse() //Lights go out, spooky
    {
        foreach(Light light in breakerLights)
        {
            light.enabled = false;
        }
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        musicAudioSource.Stop();
        radioAudioSource.Stop();
        tvStaticAudioSource.Stop();

        shadowBlurTrigger.SetActive(true);

        gameController.ShowPopupMessage(blownFuseString, 4f, false);
        gameController.fuseLightsOff = true;
        gameController.currentCheckpoint = 7;

        triggerActive = false; // Single use
    }

    public void TriggerDespawnApartmentDoorMonster() //Despawn monster if player gets too close
    {
        apartmentDoorMonster.SetActive(false);
    }


    public void TriggerDropFlashlight() //Drop flashlight in tunnel, no lightsource
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        groundFlashlight.SetActive(true);
        gameController.ShowPopupMessage(dropFlashlightString, 4f, false);
        flashlightToggle.DisableFlashlight();
        triggerActive = false; // Single use
    }

    public void TriggerFinalCloset() //Triggered near end of game in basement
    {
        topOfStairsDoor.SetActive(true); //Enable door at top of stairs, force confrontation
        maintenanceTunnelObject.SetActive(true);
        exteriorObjects.SetActive(false); //Disable exterior objects, avoid clipping
        
        StartCoroutine(OpenClosetAfterDelay());
        triggerActive = false; // Single use
    }
    
    IEnumerator OpenClosetAfterDelay() //Slowly creak door open + sfx
    {
        yield return new WaitForSeconds(1f);
        closetCreakAudioSource.clip = closetCreakAudio;
        closetCreakAudioSource.Play();
        closetDoorAnim.Play("ClosetCreakOpen");
        yield return new WaitForSeconds(1.5f);
        finalChaser.SetActive(true);
        musicAudioSource.Stop();
        musicAudioSource.clip = rumbleLoop;
        musicAudioSource.Play();
    }

    public void SpawnFinaleMonster() //Spawn monster in Maze
    {
        spawnedFinaleMonster.SetActive(true);
        triggerActive = false; // Single use
    }

    IEnumerator TriggerAmbientSFX()
    {
        int clipIndex;
        float waitTime = 0f;
        triggerActive = false;

        if(triggerType == TriggerType.BuildingCreak)
        {
            clipIndex = Random.Range(0, buildingCreakClips.Length);
            audioSource.clip = buildingCreakClips[clipIndex];

            waitTime = creakWaitTime;
        }

        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
        yield return new WaitForSeconds(waitTime);
        triggerActive = true;
    }
}