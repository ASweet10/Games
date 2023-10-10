using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    public static string DisplayName { get; private set;}

    private const string PlayerPrefsNameKey = "Player";

    private void Start() => SetupInputField();

    private void SetupInputField() {
        //If name doesn't exist in player prefs, return
        if(!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
        
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;

        //Continue button only interactable if name exists
        continueButton.interactable = !string.IsNullOrEmpty(defaultName);
    }

    public void SavePlayerName() {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
