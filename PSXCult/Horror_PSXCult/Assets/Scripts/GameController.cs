using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject cursorUI;
    [SerializeField] GameObject quitGameOptionUI;
    [SerializeField] GameObject popupTextUI;
    [SerializeField] GameObject dialogueUI;

    [SerializeField] GameObject deathUI;


    [SerializeField] GameObject drinksUI;
    [SerializeField] GameObject missingOneUI;
    [SerializeField] GameObject missingTwoUI;
    [SerializeField] GameObject missingThreeUI;
    [SerializeField] GameObject missingFourUI;
    [SerializeField] GameObject missingNewsArticle;


    [SerializeField] Text popupText;
    [SerializeField] Text objectiveText;

    public int currentCheckpoint;
    public bool gamePaused = false;
    SceneFader sceneFader;
    Interactables interactables;
    [SerializeField] FirstPersonController fpController;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] string[] gameObjectives;


    [Header ("Main Menu")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject creditsUI;
    [SerializeField] GameObject[] expositionUIObjects;
    [SerializeField] Texture2D arrowCursor; 


    [Header ("Main Events")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip collectedSFX;

    void Awake() {
        sceneFader = gameObject.GetComponent<SceneFader>();
        interactables = gameObject.GetComponent<Interactables>();
        dialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
    }
    void Start() {
        currentCheckpoint = 0;

        if(SceneManager.GetActiveScene().buildIndex == 0) {  // If main menu
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked; //Lock cursor to center of game window
        }
    }

    void Update() {
        // consider moving all input logic to firstperson controller
        if(Input.GetKeyDown(KeyCode.Escape)) {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if(buildIndex == 1) {
                if(drinksUI.activeInHierarchy) {
                    interactables.ToggleDrinksUI(false);
                }
                else if(missingOneUI.activeInHierarchy) {
                    interactables.ToggleMissingUI(1, false);
                } 
                else if(missingTwoUI.activeInHierarchy) {
                    interactables.ToggleMissingUI(2, false);
                }
                else if(missingThreeUI.activeInHierarchy) {
                    interactables.ToggleMissingUI(3, false);
                }
                else if(missingFourUI.activeInHierarchy) {
                    interactables.ToggleMissingUI(4, false);
                }
                else if(missingNewsArticle.activeInHierarchy) {
                    interactables.ToggleMissingUI(5, false);
                } 
                else if(dialogueUI.activeInHierarchy) {
                    dialogueManager.DialogueStop();
                }
                else if(interactables.PlayingArcadeGame) {
                    interactables.ToggleArcade(false);
                    interactables.PlayingArcadeGame = false;
                }
                else {
                    if(gamePaused) {
                        ResumeGame();
                    }
                    else {
                        PauseGame();
                    }
                }
                fpController.CanMove = true;
                fpController.CanInteract = true;
                mouseLook.CanRotateMouse = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void ShowPopupMessage(string message, float delay) {
        StartCoroutine(DisplayMessage(message, delay));
    }
    IEnumerator DisplayMessage(string message, float delay) {
        popupText.text = message;
        popupText.enabled = true;
        yield return new WaitForSeconds(delay);
        popupText.enabled = false;
    }

    public void LoadLevel(int levelNumber) {
        SceneManager.LoadScene(levelNumber);
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        cursorUI.SetActive(true);
        popupTextUI.SetActive(true);
        AudioListener.volume = 1f;
        Cursor.lockState = CursorLockMode.Locked; //Lock cursor to center of window
        Time.timeScale = 1f;
        gamePaused = false;
    }
    public void PauseGame() {
        pauseMenuUI.SetActive(true);
        cursorUI.SetActive(false);
        popupTextUI.SetActive(false);
        AudioListener.volume = 0;
        Cursor.lockState = CursorLockMode.None; //Unlock cursor
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void HandlePlayerDeath() {
        deathUI.SetActive(true); // black screen?
        // timescale = 0
        // disable main camera, enable deathcamera
        // enable deathCharacter
        // play either random animation or one relating to where you were stabbed
        // after animation is done, you died fades in (red, chainsaw font)
        // once text fades in, buttons appear? or buttons always visible at bottom?
        // -buttons at 20% and 80% of screen horizontally and under animation & title
        // --buttons only text but change color on hover?
    }

    /*  IN-GAME  */

    public void OpenQuitGameUI() {
        quitGameOptionUI.SetActive(true);
    }
    public void DeclineQuitGame() {
        quitGameOptionUI.SetActive(false);
    }
    public void ConfirmQuitGame() {
        Application.Quit();
    }
    public void EscapeAndWin() {
        StartCoroutine(EnableEscapeCutscene());
    }
    IEnumerator EnableEscapeCutscene() {
        musicAudioSource.Stop();
        if(!audioSource.isPlaying) {
            audioSource.Play();
        }
        yield return new WaitForSeconds(2.5f);
    }

    /*  MENUS  */

    public void PlayGameButton() {
        StartCoroutine(sceneFader.FadeOutThenLoadScene(1));
    }
    public void ToggleCreditsUI(bool creditsActive) {
        if(creditsActive) {
            mainMenuUI.SetActive(false);
            creditsUI.SetActive(true);
        } else {
            creditsUI.SetActive(false);
            mainMenuUI.SetActive(true);
        }
    }

}