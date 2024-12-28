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
    [SerializeField] GameObject quitGameOptionUI;
    [SerializeField] Interactables interactables;
    [SerializeField] GameObject blackFadeGO;


    [Header("Objectives")]
    [SerializeField] TMP_Text popupText;
    [SerializeField] TMP_Text objectivePopupText;
    [SerializeField] TMP_Text objectiveTextInPauseMenu;
    [SerializeField] FirstPersonController fpController;
    [SerializeField] string[] gameObjectives;


    public bool holdingGasStationItem = false;
    public bool hasPurchasedGas = false;
    public bool hunterWarningComplete = false;
    public bool playerCaughtStealing = false;
    public bool playerAtPark = false;
    public bool playerHasReadCarNote = false;
    public bool playerNeedsFirewood = true;
    public bool tentCompleted = false;
    public bool hasZippo = false;
    public bool hasLighterFluid = false;
    public bool hasCarKeys = false;
    public bool fireStarted = false;

    public bool hasDrink = false;
    public int chosenDrinkIndex = 0;
    

    [Header ("Player Death & Checkpoints")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera deathCamera;
    [SerializeField] GameObject playerDeath3DObject;
    [SerializeField] AnimationClip[] playerDeathClips;
    [SerializeField] Animator playerDeathAnimator;
    [SerializeField] GameObject bloodPool;
    [SerializeField] GameObject deathUI;
    [SerializeField] Transform[] restartPositions;
    [SerializeField] GameObject[] killers;


    [Header ("Main Menu")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] GameObject[] expositionUIObjects;

    public bool gamePaused = false;
    SceneFader sceneFader;
    int currentCheckpoint = 0;
    public int currentObjective = 4;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //currentObjective = 0;
        //currentCheckpoint = 0;
        if(SceneManager.GetActiveScene().buildIndex == 0) {  // If main menu
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto);
            //Cursor.visible = true;
        }
        else {
            //Cursor.lockState = CursorLockMode.Locked; //Lock cursor to center of game window
            interactables.enabled = true;
        }
        sceneFader = gameObject.GetComponent<SceneFader>();
        //StartCoroutine(HandlePlayerDeath());
    }
    public IEnumerator HandlePlayerDeath() {    
        Debug.Log("Player dead");
        fpController.DisablePlayerMovement(true, true);
        deathCamera.enabled = true;
        mainCamera.enabled = false;
        deathUI.SetActive(true);
        playerDeath3DObject.SetActive(true);

        foreach(GameObject killer in killers) {
            Killer aiRef = killer.GetComponent<Killer>();
            aiRef.state = Killer.State.idle;
        }

        int deathClipIndex = Random.Range(0, playerDeathClips.Length - 1);
        deathClipIndex = 3;
        playerDeathAnimator.SetInteger("deathClipIndex", deathClipIndex);
        Debug.Log(deathClipIndex);

        yield return new WaitForSeconds(5f);
        if(deathClipIndex == 2) {
            bloodPool.SetActive(true);
        }
    }




    /**** Menus ****/
    public IEnumerator DisplayPopupMessage(string message) {
        popupText.text = message;
        yield return new WaitForSeconds(3f);
        popupText.text = "";
    }

    public IEnumerator HandleNextObjective() {
        currentObjective ++;
        //objectivePopupText.text = "Next Objective:  " + gameObjectives[currentObjective];
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
        StartCoroutine(sceneFader.FadeOutThenFadeIn(2, 1));
    }
    public void ReturnToMenuAfterCredits() {
        SceneManager.LoadScene(0);
    }
    public void ResumeGame() {
        blackFadeGO.SetActive(true);
        objectiveTextInPauseMenu.enabled = false;
        pauseMenuUI.SetActive(false);
        popupText.enabled = true;
        AudioListener.volume = 1f;
        fpController.DisablePlayerMovement(false, false);
        //Time.timeScale = 1f;
        gamePaused = false;
    }
    public void PauseGame() {
        blackFadeGO.SetActive(true);
        pauseMenuUI.SetActive(true);
        popupText.enabled = false;
        objectiveTextInPauseMenu.enabled = true;
        objectiveTextInPauseMenu.text = gameObjectives[currentObjective];
        AudioListener.volume = 0.3f;
        fpController.DisablePlayerMovement(true, true);
        //Time.timeScale = 0f;
        gamePaused = true;
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
    public void ReplayFromDeath() {
        playerRef.transform.position = restartPositions[currentCheckpoint].position;
        playerRef.transform.rotation = restartPositions[currentCheckpoint].rotation;
    }
}