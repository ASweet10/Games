using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEvents : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] LevelController levelController;
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip collectedSFX;

    [Header("Flickering Light")]   
    [SerializeField] GameObject stepLadderObject;
    [SerializeField] GameObject heldStepLadder;
    [SerializeField] GameObject placeLadderTrigger;


    [Header("Trash")]
    [SerializeField] GameObject kitchenTrashBin;   
    [SerializeField] GameObject apartmentTrashObj;
    [SerializeField] GameObject heldGarbage;
    [SerializeField] GameObject apartmentFlySystem;
    [SerializeField] GameObject heldGarbageFlySystem;

    [SerializeField] GameObject dumpsterObj;
    [SerializeField] GameObject apartmentDoorMonster;
    [SerializeField] GameObject despawnDoorMonsterTrigger;


    [Header("MainInteractables")]
    [SerializeField] GameObject mainPaper;
    [SerializeField] GameObject keyOnDesk;
    [SerializeField] GameObject apartment3BDoor;
    [SerializeField] AudioClip mainPaperSFX;
    [SerializeField] GameObject fuse;
    [SerializeField] GameObject toolBox;
    [SerializeField] Light[] breakerLights;

    string useFuseString = "Restore power to the basement";
    string mainPaperString = "Grab toolbox left for you in office";
    string keyString = "There's a small key hidden in the book.";

    void Start()
    {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        if (levelController == null)
        {
            levelController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        }
        if(audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public void PickUpMainPaper()
    {
        AudioSource.PlayClipAtPoint(mainPaperSFX, gameObject.transform.position);
        gameController.ShowPopupMessage(mainPaperString, 3.5f, true);
        mainPaper.SetActive(false);
        keyOnDesk.SetActive(false);
        gameController.currentCheckpoint = 1;
    }    

    public void PickUpToolBox()
    {
        AudioSource.PlayClipAtPoint(collectedSFX, gameObject.transform.position);
        toolBox.SetActive(false);
        gameController.currentCheckpoint = 2;
        apartment3BDoor.tag = "OpenableDoor"; //Can now open apartment door
    }

    public void PickUpApartmentTrash()
    {
        kitchenTrashBin.tag = "Untagged";
        apartmentTrashObj.SetActive(false);
        apartmentFlySystem.SetActive(false);
        gameController.holdingTrash = true;

        heldGarbage.SetActive(true);
        heldGarbageFlySystem.SetActive(true);
    }

    public void PickUpStepLadder()
    {
        AudioSource.PlayClipAtPoint(collectedSFX, gameObject.transform.position);
        heldStepLadder.SetActive(true);
        placeLadderTrigger.SetActive(true);
        gameController.currentCheckpoint = 5;
        gameObject.SetActive(false);
    }

    public void PickUpFuse()
    {
        gameController.playerHasFuse = true;
        AudioSource.PlayClipAtPoint(collectedSFX, gameObject.transform.position);
        gameController.ShowPopupMessage(useFuseString, 3.5f, false);
        fuse.SetActive(false);
    }

    public void InteractWithGarbageChute()
    {
        AudioSource.PlayClipAtPoint(collectedSFX, gameObject.transform.position);
        heldGarbage.SetActive(false);
        heldGarbageFlySystem.SetActive(false);
        dumpsterObj.tag = "Untagged";

        apartmentDoorMonster.SetActive(true);
        despawnDoorMonsterTrigger.SetActive(true);

        gameController.engagedInTask = false;
        gameController.currentCheckpoint = 2;
    }

    public void FixFuseBox()
    {
        foreach(Light light in breakerLights)
        {
            light.enabled = true;
        }
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        gameController.fuseLightsOff = false;
    }

    public void InteractWithHiddenKey()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        gameController.ShowPopupMessage(keyString, 3.5f, false);
        gameController.playerNeedsKey = false;

        gameObject.tag = "Untagged";
    }

    public void EscapeAndWin()
    {
        StartCoroutine(EnableEscapeCutscene());
    }
    IEnumerator EnableEscapeCutscene()
    {
        levelController.FadeToBlack();
        playerController.ToggleMovement(false);
        musicAudioSource.Stop();
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(2.5f);
        levelController.LoadLevel(4);
    }
}