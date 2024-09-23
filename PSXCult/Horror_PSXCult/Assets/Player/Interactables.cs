using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactables : MonoBehaviour 
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject dialogueUI;
    DialogueManager dialogueManager;
    FirstPersonController fpController;

    [Header ("UI Objects")]
    [SerializeField] GameObject missingOneUI;
    [SerializeField] GameObject missingTwoUI;
    [SerializeField] GameObject missingThreeUI;
    [SerializeField] GameObject missingFourUI;
    [SerializeField] GameObject missingFiveUI;
    [SerializeField] GameObject gasStationNewspaperUI;
    [SerializeField] GameObject stateParkNewspaperUI;
    [SerializeField] GameObject carNoteUI;
    [SerializeField] GameObject UICamera;
    [SerializeField] TMP_Text interactText;
    [SerializeField] GameObject gasStationDoor;
    DoorController door;


    [Header ("Drinks")]
    [SerializeField] GameObject[] drinkOptions;
    [SerializeField] String[] drinkDescriptions;
    [SerializeField] GameObject drinkUI;
    [SerializeField] TMP_Text drinkTitle;
    [SerializeField] TMP_Text drinkDescription;
    public int drinkIndex;


    [Header("Arcade")]
    [SerializeField] GameObject arcadeStartScreen;
    [SerializeField] GameObject arcadeLevelOne;
    [SerializeField] GameObject arcadeDeathUI;
    [SerializeField] Camera arcadeCamera;
    [SerializeField] Camera normalCamera;
    [SerializeField] ArcadeController arcadeController;
    [SerializeField] ArcadeWolf arcadeWolfScript;
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] AudioSource arcadeSound;
    [SerializeField] AudioSource arcadeMusicLoop;
    [SerializeField] AudioClip arcadeMusic;
    [SerializeField] AudioClip arcadeCoinSFX;
    [SerializeField] TMP_Text escapeToExitText;
    public bool playingArcadeGame = false;


    void Start () {
        gameController = gameObject.GetComponent<GameController>();
        dialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
        fpController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        door = gasStationDoor.GetComponent<DoorController>();
        drinkIndex = 0;
    }
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            // Maybe loop through array; If any are active, set all inactive
            if(drinkUI.activeInHierarchy) {
                ToggleDrinksUI(false);
            } else if(missingOneUI.activeInHierarchy) {
                ToggleMissingUI(1, false);
            } else if(missingTwoUI.activeInHierarchy) {
                ToggleMissingUI(2, false);
            } else if(missingThreeUI.activeInHierarchy) {
                ToggleMissingUI(3, false);
            } else if(missingFourUI.activeInHierarchy) {
                ToggleMissingUI(4, false);
            } else if(missingFiveUI.activeInHierarchy) {
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

    public void HandleGasStationDoor() {
        if(door.DoorClosed) {
            door.OpenDoor();
        } else {
            door.CloseDoor();
        }
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
    public void ToggleMissingUI(int posterNumber, bool choice) {
        switch (posterNumber) {
            case 1:
                missingOneUI.SetActive(choice);
                break;
            case 2:
                missingTwoUI.SetActive(choice);
                break;
            case 3:
                missingThreeUI.SetActive(choice);
                break;
            case 4:
                missingFourUI.SetActive(choice);
                break;
            case 5:
                missingFiveUI.SetActive(choice);
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
            arcadeCamera.enabled = true;
            normalCamera.enabled = false;
            arcadeController.enabled = true;
            firstPersonController.enabled = false;
            mouseLook.enabled = false;
            interactText.text = "";
            arcadeSound.Play();
            yield return new WaitForSeconds(2f);
            arcadeMusicLoop.Stop();
            arcadeStartScreen.SetActive(false);
            arcadeLevelOne.SetActive(true);
            arcadeController.ResetArcadePlayerPosition();

            arcadeController.ResetStartTime(); // not working; wolf autospawns when you play again

            arcadeController.CanMove = true;
            arcadeWolfScript.ResetWolfPosition();
            arcadeSound.clip = arcadeMusic;
            arcadeSound.Play();
            arcadeSound.loop = true;
        } else {
            normalCamera.enabled = true;
            arcadeCamera.enabled = false;
            arcadeController.enabled = false;
            firstPersonController.enabled = true;
            mouseLook.enabled = true;
            arcadeSound.Stop();
            arcadeSound.clip = arcadeCoinSFX; // reset for next game
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