using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private CameraRotate cameraRotate;
    
    [Header("Menu Objects")]
    [SerializeField] private TMP_Text gameTitleText;
    [SerializeField] private Image gameImage;
    [SerializeField] private TMP_Text gameRulesText;
    [SerializeField] private GameObject gameBackground;
    [SerializeField] private GameObject backgroundFocalObject;

    [Header("Menu Arrays")]
    [SerializeField] private string[] gameTitles;
    [SerializeField] private Sprite[] gameImages;
    [SerializeField] private string[] gameRulesTexts;
    [SerializeField] private GameObject[] gameBackgrounds;
    [SerializeField] private GameObject[] backgroundFocalObjects;
    private int currentIndex = 0;

    [Header("RedAI")]
    [SerializeField] private Sprite plusSprite;
    [SerializeField] private Sprite redAISprite;
    [SerializeField] private Image redButtonImage;
    [SerializeField] private TMP_Text redAIText;
    public bool redAIActive;

    [Header("GreenAI")]
    [SerializeField] private Sprite greenAISprite;
    [SerializeField] private Image greenButtonImage;
    [SerializeField] private TMP_Text greenAIText;
    public bool greenAIActive;

    [Header("BlueAI")]   
    [SerializeField] private Sprite blueAISprite;
    [SerializeField] private Image blueButtonImage;
    [SerializeField] private TMP_Text blueAIText;
    public bool blueAIActive;
    public void Update() 
    {

    }
    public void RotateMenuRight() 
    {
        if(currentIndex < gameTitles.Length - 1)
        {
            currentIndex++;
            ChangeMenu(currentIndex);
        } else {
            currentIndex = 0;
            ChangeMenu(0);
        }
    }
    public void RotateMenuLeft()
    {
        if(currentIndex > 0)
        {
            currentIndex--;
            ChangeMenu(currentIndex);
        } else {
            currentIndex = gameTitles.Length;
            ChangeMenu(gameTitles.Length - 1);
        }
    }
    private void ChangeMenu(int newIndex)
    {
        gameTitleText.text = gameTitles[newIndex];
        gameImage.sprite = gameImages[newIndex];
        gameRulesText.text = gameRulesTexts[newIndex];
        for(int i = 0; i < gameTitles.Length; i++)
        {
            if(gameTitles[i] != gameTitles[newIndex])
            {
                gameBackgrounds[i].SetActive(false);
                backgroundFocalObjects[i].SetActive(false);
            }
        }
        gameBackgrounds[newIndex].SetActive(true);
        backgroundFocalObject = backgroundFocalObjects[newIndex];
        backgroundFocalObject.SetActive(true);
        cameraRotate.target = backgroundFocalObject;

    }
    public void LoadLevel(string choice) 
    {
        switch (choice)
        {
            case "menu":
                SceneManager.LoadScene(0);
                break;
            case "snowball":
                SceneManager.LoadScene(1);
                break;
            case "mariokart":
                SceneManager.LoadScene(2);
                break;
            case "tanks":
                SceneManager.LoadScene(3);
                break;
        }
    }
    public void ResetLevel() {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void AddAIEnemy(int choice) {
        if(choice == 1) {
            if(redAIActive) {
                redButtonImage.sprite = plusSprite;
                redAIText.text = "Click to Add Enemy";
                redAIActive = false;
            } else {
                redButtonImage.sprite = redAISprite;
                redAIText.text = "Roy";
                redAIActive = true;
            }
        } else if (choice == 2) {
            if(greenAIActive) {
                greenButtonImage.sprite = plusSprite;
                greenAIText.text = "Click to Add Enemy";
                greenAIActive = false;
            } else { 
                greenButtonImage.sprite = greenAISprite;
                greenAIText.text = "Gary";
                greenAIActive = true;
            }
        } else if (choice == 3) {
            if(blueAIActive) {
                blueButtonImage.sprite = plusSprite;
                blueAIText.text = "Click to Add Enemy";
                blueAIActive = false;
            } else {
                blueButtonImage.sprite = blueAISprite;
                blueAIText.text = "Bob";
                blueAIActive = true;
            }
        }
    }

}
