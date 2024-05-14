using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactables : MonoBehaviour 
{
    [SerializeField] GameObject missingOneUI;
    [SerializeField] GameObject missingTwoUI;
    [SerializeField] GameObject missingThreeUI;
    [SerializeField] GameObject missingFourUI;
    [SerializeField] GameObject missingNewsArticle;
    [SerializeField] GameObject UICamera;
    [SerializeField] TMP_Text interactText;


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
    [SerializeField] AudioClip arcadeMusic;
    [SerializeField] AudioClip arcadeCoinSFX;
    bool playingArcadeGame = false;
    public bool PlayingArcadeGame {
        get { return playingArcadeGame; }
        set { playingArcadeGame = value; }
    }

    void Start () {
        drinkIndex = 0;
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
                missingNewsArticle.SetActive(choice);
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
    
    // Level 1: car park, woods, lake
    // Level 2: woods, camp grounds
    // Level 3: ?

    public void ToggleArcade(bool playingGame) {
        if(playingGame) {
            arcadeCamera.enabled = true;
            normalCamera.enabled = false;
            arcadeController.enabled = true;
            firstPersonController.enabled = false;
            mouseLook.enabled = false;
            interactText.text = "";
            StartCoroutine(StartArcadeGame());
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
        }
    }

    IEnumerator StartArcadeGame() {
        arcadeSound.Play();
        yield return new WaitForSeconds(2f);
        arcadeStartScreen.SetActive(false);
        arcadeLevelOne.SetActive(true);
        arcadeController.ResetArcadePlayerPosition();

        arcadeController.ResetStartTime(); // not working; wolf autospawns when you play again

        arcadeController.CanMove = true;
        arcadeWolfScript.ResetWolfPosition();
        arcadeSound.clip = arcadeMusic;
        arcadeSound.Play();
        arcadeSound.loop = true;
    }
}