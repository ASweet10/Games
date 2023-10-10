using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject cursorUI;
    [SerializeField] GameObject quitGameOptionUI;
    [SerializeField] LevelController levelController;
    [SerializeField] GameObject popUpText;
    [SerializeField] AudioSource toolAudioSource;
    [SerializeField] GameObject distortScreenObj;
    [SerializeField] ScriptableRendererFeature distortFeature;

    [Header ("Main Menu Only")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject creditsUI;
    [SerializeField] GameObject exposition1;
    [SerializeField] GameObject exposition2;
    [SerializeField] GameObject exposition3;
    [SerializeField] GameObject exposition4;
    [SerializeField] GameObject garageObject;
    [SerializeField] GameObject mainMenuExposition;
    [SerializeField] GameObject playGameButton;
    public static bool gamePaused = false;

    void Awake()
    {
        if (levelController == null)
        {
            levelController = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>();
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(currentScene == 1)
        {
            //Lock cursor to center of game window
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            distortFeature.SetActive(false);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            int buildIndex = currentScene.buildIndex;
            if(buildIndex == 1)
            {
                if(gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }

    //Main Menu: Play Button
    public void MenuPlayButton()
    {
        EnableMainMenuExposition();
    }
    public void EnableMainMenuExposition()
    {
        garageObject.SetActive(false);
        mainMenuUI.SetActive(false);

        mainMenuExposition.SetActive(true);
        StartCoroutine(PlayIntroExposition());
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        cursorUI.SetActive(true);
        popUpText.SetActive(true);
        distortScreenObj.SetActive(true);
        AudioListener.volume = 1f;
        Cursor.lockState = CursorLockMode.Locked; //Lock cursor to center of window
        Time.timeScale = 1f;
        gamePaused = false;
    }
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        cursorUI.SetActive(false);
        popUpText.SetActive(false);
        distortScreenObj.SetActive(false);
        AudioListener.volume = 0;
        toolAudioSource.Pause();
        Cursor.lockState = CursorLockMode.None; //Unlock cursor
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void EnableCredits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }
    public void DisableCredits()
    {
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void EnableQuitGameOption()
    {
        quitGameOptionUI.SetActive(true);
    }
    public void DeclineQuitGame()
    {
        quitGameOptionUI.SetActive(false);
    }
    public void ConfirmQuitGame()
    {
        Application.Quit();
    }

    private IEnumerator PlayIntroExposition()
    {
        yield return new WaitForSeconds(1f);
        exposition1.SetActive(true);
        yield return new WaitForSeconds(6f);
        exposition2.SetActive(true);
        yield return new WaitForSeconds(6f);
        exposition3.SetActive(true);
        yield return new WaitForSeconds(5f);
        exposition4.SetActive(true);
        yield return new WaitForSeconds(5f);
        playGameButton.SetActive(true);
    }
}
