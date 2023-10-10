using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager GMInstance; // Static; shares value among all instances of class
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject startMenuUI;
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] TankSpawner tankSpawner;
    AudioManager audioManager;
    public Game_Manager gameManager;
    public GameObject[] powerupSpawnerArray;
    GameObject[] aiArray;

    [Header("Score")]
    public Text gameOverHighScoreText;
    public Text inGameHighScoreText;
    public Text playerScoreText;
    public int highScore;
    private int playerScore;

    [Header("Lives")]
    public Text livesText;
    public int maxLives = 3;
    private int playerLives;

    void Awake() {
        if(GMInstance == null) {
            GMInstance = this;
        } else {
            Debug.LogError("Error: Only one instance of GameManager can exist");
            Destroy(gameObject);
        }
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        tankSpawner = gameObject.GetComponent<TankSpawner>();
        gameManager = gameObject.GetComponent<Game_Manager>();
    }
    void Start() {
        if(PlayerPrefs.HasKey("HighScore")) {
            highScore = PlayerPrefs.GetInt("HighScore");
        } else {
            highScore = 0;
        }
        playerScore = 0;
        playerLives = maxLives;
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
            playerScoreText.text = "Score:  " + playerScore;
        }
        if(livesText.IsActive()) {
            livesText.text = ("Lives:  " + playerLives.ToString());
        }
    }
    public void PlayGame() {
        startMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;

        mapGenerator.DestroyGrid();
        mapGenerator.GenerateGrid();
        if(mapGenerator.gridGenerated) {
            playerLives = maxLives;
            livesText.gameObject.SetActive(true);
            playerScore = 0;
            playerScoreText.gameObject.SetActive(true);
            tankSpawner.HandleInitialTankSpawn();

            powerupSpawnerArray = GameObject.FindGameObjectsWithTag("PowerupSpawner");
            audioManager.EnableGameMusic();
            inGameHighScoreText.gameObject.SetActive(true);
            aiArray = GameObject.FindGameObjectsWithTag("PatrolAI");
        }
    }
   
    public void CheckForNewHighScore() {
        if(playerScore > highScore) {
            highScore = playerScore;
        }
    }
    public void OnApplicationQuit() {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }
    public void DecreasePlayerLife() {
        playerLives --;
        if(playerLives > 0) {
            tankSpawner.RespawnPlayer();
        } else { // Game Over
            livesText.gameObject.SetActive(false);
            playerScoreText.gameObject.SetActive(false);
            inGameHighScoreText.gameObject.SetActive(false);
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}