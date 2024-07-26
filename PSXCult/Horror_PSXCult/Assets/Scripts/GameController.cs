using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject playerRef;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject cursorUI;
    [SerializeField] GameObject quitGameOptionUI;
    [SerializeField] GameObject popupTextUI;
    [SerializeField] GameObject dialogueUI;

    [SerializeField] GameObject drinksUI;
    [SerializeField] GameObject missingOneUI;
    [SerializeField] GameObject missingTwoUI;
    [SerializeField] GameObject missingThreeUI;
    [SerializeField] GameObject missingFourUI;
    [SerializeField] GameObject missingNewsArticle;

    [Header("Objectives")]
    [SerializeField] TMP_Text popupText;
    [SerializeField] TMP_Text objectivePopupText;
    [SerializeField] TMP_Text objectiveTextInPauseMenu;
    [SerializeField] FirstPersonController fpController;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] string[] gameObjectives;

    public bool holdingGasStationItem = false;
    public bool hasPurchasedGas = false;
    public bool hunterWarningComplete = false;
    public bool playerCaughtStealing = false;
    public bool playerAtPark = false;
    public bool playerNeedsFirewood = true;
    public bool tentCompleted = false;

    [Header ("Player Death & Checkpoints")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera deathCamera;
    [SerializeField] GameObject playerDeath3DObject;
    [SerializeField] AnimationClip[] playerDeathClips;
    [SerializeField] Animator playerDeathAnimator;
    [SerializeField] GameObject bloodPool;
    [SerializeField] GameObject deathUI;
    [SerializeField] Transform[] restartPositions;

    [Header ("Main Menu")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] GameObject[] expositionUIObjects;

    public bool gamePaused = false;
    SceneFader sceneFader;
    Interactables interactables;
    int currentCheckpoint = 0;
    public int currentObjective = 0;

    

    void Start() {
        currentObjective = 0;
        currentCheckpoint = 0;
        if(SceneManager.GetActiveScene().buildIndex == 0) {  // If main menu
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto);
            //Cursor.visible = true;
        }
        else {
            //Cursor.lockState = CursorLockMode.Locked; //Lock cursor to center of game window
            dialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
        }
        sceneFader = gameObject.GetComponent<SceneFader>();
        interactables = gameObject.GetComponent<Interactables>();
        //StartCoroutine(HandlePlayerDeath());
    }

    void Update() {
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
                    StartCoroutine(interactables.ToggleArcade(false));
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
                fpController.DisablePlayerMovement(false);
            }
        }
    }

    public IEnumerator HandleIntroCutscene() {
        //First cutscene; player driving down road & title card
        yield return null;
    }
    public IEnumerator HandleDriveToParkCutscene() {
        Debug.Log("drive to park");
        //fade to black
        //cutscene shows player driving to park
        //park car and see friend there
        yield return null;
    }
    public IEnumerator HandleEscapeCutscene() {
        //Fade to black
        yield return new WaitForSeconds(2.5f);
        //Fade in from black
        //Play music
        //Cutscene watching player drive away
        //Credits scroll down screen
    }

    public IEnumerator HandlePlayerDeath() {
        Debug.Log("Player dead");
        fpController.DisablePlayerMovement(true);
        deathCamera.enabled = true;
        mainCamera.enabled = false;
        deathUI.SetActive(true);
        playerDeath3DObject.SetActive(true);

        int deathClipIndex = Random.Range(0, playerDeathClips.Length - 1);
        deathClipIndex = 3;
        playerDeathAnimator.SetInteger("deathClipIndex", deathClipIndex);
        Debug.Log(deathClipIndex);

        yield return new WaitForSeconds(5f);
        if(deathClipIndex == 2) {
            bloodPool.SetActive(true);
        }
    }
    public void ReplayFromDeath() {
        playerRef.transform.position = restartPositions[currentCheckpoint].position;
        playerRef.transform.rotation = restartPositions[currentCheckpoint].rotation;
    }

    /*  MENUS, TEXT, etc.  */
    public IEnumerator DisplayPopupMessage(string message) {
        popupText.text = message;
        popupText.enabled = true;
        yield return new WaitForSeconds(3);
        popupText.enabled = false;
    }

    public IEnumerator HandleNextObjective() {
        currentObjective ++;
        objectivePopupText.text = "Next Objective:  " + gameObjectives[currentObjective];
        objectivePopupText.enabled = true;
        yield return new WaitForSeconds(4);
        objectivePopupText.enabled = false;
    }
    public void OpenQuitGameUI() {
        quitGameOptionUI.SetActive(true);
    }
    public void DeclineQuitGame() {
        quitGameOptionUI.SetActive(false);
    }
    public void ConfirmQuitGame() {
        Application.Quit();
    }
    public void ToggleOptionsMenu(bool toggle) {
        mainMenuUI.SetActive(!toggle);
        optionsMenuUI.SetActive(toggle);
    }
    public void PlayGameButton() {
        StartCoroutine(sceneFader.FadeOutThenLoadScene(1));
    }
    public void ReturnToMenuAfterCredits() {
        StartCoroutine(sceneFader.FadeOutThenLoadScene(0));
    }
    public void ResumeGame() {
        objectiveTextInPauseMenu.enabled = false;
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
        objectiveTextInPauseMenu.enabled = true;
        objectiveTextInPauseMenu.text = gameObjectives[currentObjective];
        AudioListener.volume = 0;
        Cursor.lockState = CursorLockMode.None; //Unlock cursor
        Time.timeScale = 0f;
        gamePaused = true;
    }
}