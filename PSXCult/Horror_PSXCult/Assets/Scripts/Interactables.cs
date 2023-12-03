using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour 
{
    [SerializeField] GameObject drinksUI;
    [SerializeField] GameObject missingOneUI;
    [SerializeField] GameObject missingTwoUI;
    [SerializeField] GameObject missingThreeUI;
    public void ToggleDrinksUI(bool choice) {   // Drinks in gas station
        drinksUI.SetActive(choice);
    }
    public void ToggleMissingOneUI(bool choice) {   // Missing poster 1
        missingOneUI.SetActive(choice);
    }
    public void ToggleMissingTwoUI(bool choice) {   // Missing poster 2
        missingTwoUI.SetActive(choice);
    }
    public void ToggleMissingThreeUI(bool choice) {   // Missing poster 3
        missingThreeUI.SetActive(choice);
    }
}