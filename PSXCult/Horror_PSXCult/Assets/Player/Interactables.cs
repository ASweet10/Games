using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactables : MonoBehaviour 
{
    KeyCode escape = KeyCode.Escape; // For build
    KeyCode twoKey = KeyCode.Alpha2; // for testing


    GameController gameController;
    [SerializeField] GameObject dialogueUI;
    DialogueManager dialogueManager;
    FirstPersonController fpController;
    FirstPersonHighlights fpHighlights;
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
    [SerializeField] GameObject useDrinkUI;
    [SerializeField] GameObject drinkCollider;
    [SerializeField] TMP_Text drinkTitle;
    [SerializeField] TMP_Text drinkDescription;
    public int drinkIndex;

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
    public bool playingArcadeGame;

    void Start () {
        playerInGasStation = false;
        playingArcadeGame = false;
        gameController = gameObject.GetComponent<GameController>();
        sceneFader = gameObject.GetComponent<SceneFader>();
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
        fpController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        fpHighlights = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonHighlights>();        
        drinkIndex = 0;
    }
    
    void Update() {
        HandleEscapeButtonLogic();
    }

    public void HandleGasStationDoor() {
        if(!gameController.holdingGasStationItem) { // Disable door when holding item
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

    public void ToggleUseDrinkUI(bool toggle) {
        useDrinkUI.SetActive(toggle);
    }

    public void PurchaseDrink() {
        gameController.chosenDrinkIndex = drinkIndex;
        gameController.hasDrink = true;
        drinkCollider.tag = "Untagged";
        fpHighlights.ClearHighlighted();
        Disable3DDrinks();
        drinkUI.SetActive(false);
        fpController.DisablePlayerMovement(false, false);
    }

    public void UseDrink() {
        switch(gameController.chosenDrinkIndex) {
            case 0:
                //"Orange Juice [+5 top speed 1 min]"
                break;
            case 1:
                //"Coffee [x2 max stamina 30 sec]"
                break;
            case 2:
                //"Noca Cola [+10 top speed 15 sec]"
                break;
            case 3:
                //"Energy Drink [Unlimited stamina 15 sec]"
                break;
        }
        ToggleUseDrinkUI(false);
        gameController.hasDrink = false;
        // Remove drink from pause menu, disable drink objects?
    }
    public void ToggleMissingUI(string posterName, bool choice) {
        switch (posterName) {
            case "MissingPosterMatthew":
                missingUI_Matthew.SetActive(choice);
                break;
            case "MissingPosterCouple":
                missingUI_Couple.SetActive(choice);
                break;
            case "MissingPosterNathan":
                missingUI_Nathan.SetActive(choice);
                break;
            case "MissingPosterMaria":
                missingUI_Maria.SetActive(choice);
                break;
            case "MissingPosterAmir":
                missingUI_Amir.SetActive(choice);
                break;
            case "Newspaper_GasStation":
                gasStationNewspaperUI.SetActive(choice);
                break;
            case "Newspaper_StatePark":
                stateParkNewspaperUI.SetActive(choice);
                break;
            case "StartingNote":
                carNoteUI.SetActive(choice);
                break;
        }
    }


    void HandleEscapeButtonLogic() {
        if(Input.GetKeyDown(twoKey)) {
            if(drinkUI.activeInHierarchy) {
                ToggleDrinksUI(false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(missingUI_Matthew.activeInHierarchy) {
                ToggleMissingUI("MissingPosterMatthew", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(missingUI_Couple.activeInHierarchy) {
                ToggleMissingUI("MissingPosterCouple", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(missingUI_Nathan.activeInHierarchy) {
                ToggleMissingUI("MissingPosterNathan", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(missingUI_Maria.activeInHierarchy) {
                ToggleMissingUI("MissingPosterMaria", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(missingUI_Amir.activeInHierarchy) {
                ToggleMissingUI("MissingPosterAmir", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(gasStationNewspaperUI.activeInHierarchy) {
                ToggleMissingUI("Newspaper_GasStation", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(stateParkNewspaperUI.activeInHierarchy) {
                ToggleMissingUI("Newspaper_StatePark", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(carNoteUI.activeInHierarchy) {
                ToggleMissingUI("StartingNote", false);
                fpController.DisablePlayerMovement(false, false);
                return;
            } else if(dialogueUI.activeInHierarchy) {
                dialogueManager.DialogueStop();
            } else if(playingArcadeGame) {
                StartCoroutine(ToggleArcade(false));
                playingArcadeGame = false;
            } else {
                if(gameController.gamePaused) {
                    gameController.ResumeGame();
                    Debug.Log("resume");
                }
                else {
                    gameController.PauseGame();
                }
            }
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