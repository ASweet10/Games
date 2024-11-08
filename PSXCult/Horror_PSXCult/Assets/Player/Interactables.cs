using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactables : MonoBehaviour 
{
    GameController gameController;
    [SerializeField] GameObject dialogueUI;
    DialogueManager dialogueManager;
    FirstPersonController fpController;
    GameObject player;
    SceneFader sceneFader;

    [Header ("UI Objects")]
    [SerializeField] GameObject missingUI_Matthew;
    [SerializeField] GameObject missingUI_Couple;
    [SerializeField] GameObject missingUI_Nathan;
    [SerializeField] GameObject missingUI_Maria;
    [SerializeField] GameObject missingUI_Amir;
    [SerializeField] GameObject gasStationNewspaperUI;
    [SerializeField] GameObject stateParkNewspaperUI;
    [SerializeField] GameObject carNoteUI;
    [SerializeField] GameObject UICamera;
    [SerializeField] TMP_Text interactText;


    [Header ("Gas Station Door")]
    [SerializeField] AudioSource gasStationBellAudio;
    [SerializeField] Transform gasStationSpawnpoint;
    [SerializeField] Transform gasStationParkingLotSpawnpoint;
    bool playerInGasStation = false;

    [Header ("Drinks")]
    [SerializeField] GameObject[] drinkOptions;
    [SerializeField] String[] drinkDescriptions;
    [SerializeField] GameObject drinkUI;
    [SerializeField] TMP_Text drinkTitle;
    [SerializeField] TMP_Text drinkDescription;
    public int drinkIndex;

    [Header("Dog")]
    [SerializeField] Dog dog;


    [Header("Arcade")]
    [SerializeField] GameObject arcadeStartScreen;
    [SerializeField] GameObject arcadeLevelOne;
    [SerializeField] GameObject arcadeDeathUI;
    [SerializeField] Camera arcadeStartCamera;
    [SerializeField] Camera arcadePlayerCamera;
    [SerializeField] Camera gameCamera;
    [SerializeField] ArcadeController arcadeController;
    [SerializeField] ArcadeWolf arcadeWolfScript;
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] AudioSource arcadeCoinSound;
    [SerializeField] AudioSource arcadeMusicLoop;
    [SerializeField] AudioClip arcadeMusic;
    [SerializeField] AudioClip arcadeCoinSFX;
    [SerializeField] TMP_Text escapeToExitText;
    public bool playingArcadeGame = false;

    [Header("Object Pickup")]
    GameObject heldObj;
    Rigidbody heldObjRb;
    Transform objectHoldPos;
    bool canDrop = false;

    void Start () {
        playerInGasStation = false;
        playingArcadeGame = false;
        gameController = gameObject.GetComponent<GameController>();
        sceneFader = gameObject.GetComponent<SceneFader>();
        player = GameObject.FindGameObjectWithTag("Player");
        dog = GameObject.FindGameObjectWithTag("Rusty").GetComponent<Dog>();
        dialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
        fpController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        drinkIndex = 0;
    }
    
    void Update() {
        HandleEscapeButtonLogic();
        HandleObjectLogic();
    }

    public void HandleGasStationDoor() {
        StartCoroutine(sceneFader.FadeOutThenFadeIn(1, 2));
        gasStationBellAudio.Play();
        if(playerInGasStation) {
            player.transform.position = gasStationParkingLotSpawnpoint.position;
            playerInGasStation = false;
        } else {
            player.transform.position = gasStationSpawnpoint.position;
            playerInGasStation = true;
        }
    }

    public void HandleInteractWithDog() {
        dog.state = Dog.State.barking;
    }

    public void ToggleDrinksUI(bool choice) {   // Drinks in gas station
        drinkUI.SetActive(choice);
        UICamera.SetActive(choice);
        drinkOptions[0].SetActive(choice);
        if(choice) {
            drinkTitle.text = drinkOptions[drinkIndex].name;
            drinkDescription.text = drinkDescriptions[drinkIndex];
        } else {
            foreach(GameObject drink in drinkOptions) {
                drink.SetActive(false);
            }
        }
    }
    public void Disable3DDrinks() {
        foreach(GameObject drink in drinkOptions) {
            drink.SetActive(false);
        }
    }

    public void PurchaseDrink(string drinkName) {
        switch (drinkName) {
            case "Apple Juice":
                break;
            case "Coffee":
                break;
            case "Noca Cola":
                break;
            case "Orange Juice":
                break;
        }
    }
    public void ToggleMissingUI(int posterNumber, bool choice) {
        switch (posterNumber) {
            case 1:
                missingUI_Matthew.SetActive(choice);
                break;
            case 2:
                missingUI_Couple.SetActive(choice);
                break;
            case 3:
                missingUI_Nathan.SetActive(choice);
                break;
            case 4:
                missingUI_Maria.SetActive(choice);
                break;
            case 5:
                missingUI_Amir.SetActive(choice);
                break;
            case 6:
                gasStationNewspaperUI.SetActive(choice);
                break;
            case 7:
                stateParkNewspaperUI.SetActive(choice);
                break;
            case 8:
                carNoteUI.SetActive(choice);
                break;
        }
    }

    public void HandlePickUpObject(GameObject obj) {
        if (obj.GetComponent<Rigidbody>()) {
            heldObj = obj;
            heldObjRb = obj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void HandleEscapeButtonLogic() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            // Maybe loop through array; If any are active, set all inactive
            if(drinkUI.activeInHierarchy) {
                ToggleDrinksUI(false);
            } else if(missingUI_Matthew.activeInHierarchy) {
                ToggleMissingUI(1, false);
            } else if(missingUI_Couple.activeInHierarchy) {
                ToggleMissingUI(2, false);
            } else if(missingUI_Nathan.activeInHierarchy) {
                ToggleMissingUI(3, false);
            } else if(missingUI_Maria.activeInHierarchy) {
                ToggleMissingUI(4, false);
            } else if(missingUI_Amir.activeInHierarchy) {
                ToggleMissingUI(5, false);
            } else if(gasStationNewspaperUI.activeInHierarchy) {
                ToggleMissingUI(6, false);
            } else if(stateParkNewspaperUI.activeInHierarchy) {
                ToggleMissingUI(7, false);
            } else if(carNoteUI.activeInHierarchy) {
                ToggleMissingUI(8, false);
            } else if(dialogueUI.activeInHierarchy) {
                dialogueManager.DialogueStop();
            } else if(playingArcadeGame) {
                StartCoroutine(ToggleArcade(false));
                playingArcadeGame = false;
            }
            else {
                if(gameController.gamePaused) {
                    gameController.ResumeGame();
                }
                else {
                    gameController.PauseGame();
                }
            }
            fpController.DisablePlayerMovement(false);
        }
    }
    void HandleObjectLogic() {
        if (heldObj != null) {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) {
                StopObjectClipping();
                ThrowObject();
            }

        }
    }
    void MoveObject() {

    }
    void RotateObject() {

    }
    void StopObjectClipping() {

    }
    void ThrowObject() {

    }
    public void HandlePreviousDrinkButton() {
        drinkOptions[drinkIndex].SetActive(false);
        if(drinkIndex == 0) {
            drinkIndex = drinkOptions.Length - 1;
        } else {
            drinkIndex --;
        }
        drinkOptions[drinkIndex].SetActive(true);
        drinkTitle.text = drinkOptions[drinkIndex].name;
        drinkDescription.text = drinkDescriptions[drinkIndex];
    }
    public void HandleNextDrinkButton() {
        drinkOptions[drinkIndex].SetActive(false);
        if(drinkIndex == drinkOptions.Length - 1) {
            drinkIndex = 0;
        } else {
            drinkIndex ++;
        }
        drinkOptions[drinkIndex].SetActive(true);
        drinkTitle.text = drinkOptions[drinkIndex].name;
        drinkDescription.text = drinkDescriptions[drinkIndex];
    }
    
    public IEnumerator ToggleArcade(bool playingGame) {
        if(playingGame) {
            arcadeStartCamera.enabled = true;
            gameCamera.enabled = false;
            arcadeController.enabled = true;
            firstPersonController.enabled = false;
            mouseLook.enabled = false;
            interactText.text = "";

            arcadeCoinSound.Play();
            yield return new WaitForSeconds(1.5f);
            arcadeMusicLoop.Stop();
            arcadeStartScreen.SetActive(false);
            arcadeLevelOne.SetActive(true);
            arcadePlayerCamera.enabled = true;
            arcadeStartCamera.enabled = false;

            arcadeController.ResetArcadePlayerPosition();
            arcadeWolfScript.ResetWolfPosition();
            arcadeController.ResetStartTime(); // not working; wolf autospawns when you play again

            arcadeController.CanMove = true;
            arcadeCoinSound.clip = arcadeMusic;
            arcadeCoinSound.Play();
            arcadeCoinSound.loop = true;

        } else {
            gameCamera.enabled = true;
            arcadeStartCamera.enabled = false;
            arcadePlayerCamera.enabled = false;

            arcadeController.enabled = false;
            firstPersonController.enabled = true;
            mouseLook.enabled = true;
            arcadeCoinSound.Stop();
            arcadeCoinSound.clip = arcadeCoinSFX; // reset for next game

            if(arcadeLevelOne.activeInHierarchy) {
                arcadeLevelOne.SetActive(false);
            }
            if(arcadeDeathUI.activeInHierarchy) {
                arcadeDeathUI.SetActive(false);
            }
            arcadeStartScreen.SetActive(true);
            arcadeMusicLoop.Play();
            escapeToExitText.enabled = false;
        }
    }
}