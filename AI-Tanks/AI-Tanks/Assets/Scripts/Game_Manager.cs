using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager GMInstance; // Static; shares value among all instances of class
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject startMenuUI;
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] TankSpawner tankSpawner;
    [SerializeField] AudioManager audioManager;
    TankHealth playerTankHealth;
    public GameObject[] powerupSpawnerArray;

    [Header("Score")]
    [SerializeField] Text gameOverHighScoreText;
    [SerializeField] Text inGameHighScoreText;
    [SerializeField] Text playerScoreText;
    int highScore;
    int playerScore;
    
    [Header("UI")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    float sfxSliderValue;
    float musicSliderValue;

    void Awake() {
        if(GMInstance == null) {
            GMInstance = this;
        } else {
            Debug.LogError("Error: Only one instance of GameManager can exist");
            Destroy(gameObject);
        }
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        tankSpawner = gameObject.GetComponent<TankSpawner>();
        playerTankHealth = GameObject.FindGameObjectWithTag("PlayerOneTank").GetComponent<TankHealth>();
        LoadPlayerPrefs();
    }
    void Start() {
        if(PlayerPrefs.HasKey("HighScore")) {
            highScore = PlayerPrefs.GetInt("HighScore");
        } else {
            highScore = 0;
        }
        playerScore = 0;
        Time.timeScale = 1f;
    }
    void Update() {
        CheckForNewHighScore();
        if(gameOverHighScoreText.IsActive()) {
            gameOverHighScoreText.text = highScore.ToString();
        }
        if(inGameHighScoreText.IsActive()) {
            inGameHighScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }
        if(playerScoreText.IsActive()) {
            playerScoreText.text = "SCORE:  " + playerScore.ToString();
        }
    }
    public void PlayGame() {
        startMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;

        mapGenerator.DestroyGrid();
        mapGenerator.GenerateGrid();
        if(mapGenerator.ReturnGridGeneratedStatus()) {
            playerScore = 0;
            playerTankHealth.ResetHealth();
            playerScoreText.gameObject.SetActive(true);
            tankSpawner.HandleInitialTankSpawn();

            powerupSpawnerArray = GameObject.FindGameObjectsWithTag("PowerupSpawner");
            audioManager.EnableGameMusic();
            inGameHighScoreText.gameObject.SetActive(true);
        }
    }
   
    public void CheckForNewHighScore() {
        if(playerScore > highScore) {
            highScore = playerScore;
        }
    }
    public void HandleGameOver() {
        playerScoreText.gameObject.SetActive(false);
        inGameHighScoreText.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }


    /*   UI    */
    public void ChooseMapOfDay() {
        mapGenerator.randomSpawnType = MapGenerator.RandomSpawnType.mapOfDay;
        PlayerPrefs.SetString("randomMode", "mapOfDay");
        PlayerPrefs.Save();
    }
    public void ChooseRandomMap() {
        mapGenerator.randomSpawnType = MapGenerator.RandomSpawnType.random;
        PlayerPrefs.SetString("randomMode", "random");
        PlayerPrefs.Save();
    }

    // Load player prefs; Player prefs: stored locally & persist between sessions
    public void LoadPlayerPrefs() {
        string randomMode;
        if(PlayerPrefs.HasKey("sfxVolume")) {
            sfxSliderValue = PlayerPrefs.GetFloat("sfxVolume");
            sfxSliderValue = Mathf.Pow(10, sfxSliderValue / 20);
            audioManager.masterAudioMixer.SetFloat("sfxVolume", Mathf.Log(sfxSliderValue) * 20);
        } else {
            sfxSliderValue = sfxSlider.maxValue;
        }

        if(PlayerPrefs.HasKey("musicVolume")) {
            musicSliderValue = PlayerPrefs.GetFloat("musicVolume");
            musicSliderValue = Mathf.Pow(10, musicSliderValue / 20);
            audioManager.masterAudioMixer.SetFloat("musicVolume", Mathf.Log(musicSliderValue) * 20);
        } else {
            musicSliderValue = musicSlider.maxValue;
        }
        if(PlayerPrefs.HasKey("randomMode")) {
            randomMode = PlayerPrefs.GetString("randomMode");
        } else {
            randomMode = "presetSeed";
        }

        switch(randomMode) {
            case "mapOfDay":
                mapGenerator.randomSpawnType = MapGenerator.RandomSpawnType.mapOfDay;
                break;
            case "random":
                mapGenerator.randomSpawnType = MapGenerator.RandomSpawnType.random;
                break;
            default:
                mapGenerator.randomSpawnType = MapGenerator.RandomSpawnType.presetSeed;
                break;
        }
    }
    public void AddPoint(){
        playerScore += 1;
    }
    public void QuitGame() {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
        Application.Quit();
    }
}