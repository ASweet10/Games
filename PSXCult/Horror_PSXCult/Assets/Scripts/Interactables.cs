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

    [Header ("Ice Cream")]
    [SerializeField] GameObject[] iceCreamOptions;
    [SerializeField] GameObject iceCreamUI;
    [SerializeField] TMP_Text iceCreamTitle;
    [SerializeField] GameObject iceCream3DObject;
    [SerializeField] Animator iceCreamAnimator;
    public int iceCreamIndex;

    [Header ("Drinks")]
    [SerializeField] GameObject[] drinkOptions;
    [SerializeField] GameObject drinksUI;
    [SerializeField] GameObject drinks3DObject;
    public int drinksIndex;

    [SerializeField] Camera arcadeCamera;
    [SerializeField] Camera normalCamera;
    bool gameRunning = false;

    void Start () {
        drinksIndex = 0;
        iceCreamIndex = 0;
    }
    public void ToggleDrinksUI(bool choice) {   // Drinks in gas station
        drinksUI.SetActive(choice);
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
    public void ToggleIceCreamUI(bool choice) {
        iceCreamUI.SetActive(choice);
        UICamera.SetActive(choice);
        iceCreamOptions[0].SetActive(choice);
        if(choice) {
            iceCreamTitle.text = iceCreamOptions[iceCreamIndex].name;
        }
    }
    public void Disable3DIceCream() {
        foreach(GameObject iceCream in iceCreamOptions) {
            iceCream.SetActive(false);
        }
    }
    public void HandleIceCreamAnimation(bool setDoorStatus) {
        if(setDoorStatus == true) {
            iceCreamAnimator.Play("OpenIceCreamDoor");
        } else {
            iceCreamAnimator.Play("CloseIceCreamDoor");
        }

    }
    public void HandlePreviousIceCreamButton() {
        iceCreamOptions[iceCreamIndex].SetActive(false);
        if(iceCreamIndex == 0) {
            iceCreamIndex = iceCreamOptions.Length - 1;
        } else {
            iceCreamIndex --;
        }
        iceCreamOptions[iceCreamIndex].SetActive(true);
        iceCreamTitle.text = iceCreamOptions[iceCreamIndex].name;
    }
    public void HandleNextIceCreamButton() {
        iceCreamOptions[iceCreamIndex].SetActive(false);
        if(iceCreamIndex == iceCreamOptions.Length - 1) {
            iceCreamIndex = 0;
        } else {
            iceCreamIndex ++;
        }
        iceCreamOptions[iceCreamIndex].SetActive(true);
        iceCreamTitle.text = iceCreamOptions[iceCreamIndex].name;
    }
    public void ToggleArcadeGame(bool choice) {
        if(gameRunning) {
            normalCamera.enabled = true;
            arcadeCamera.enabled = false;
            gameRunning = false;
        } else {
            normalCamera.enabled = false;
            arcadeCamera.enabled = true;
            gameRunning = true;
        }
    }
}