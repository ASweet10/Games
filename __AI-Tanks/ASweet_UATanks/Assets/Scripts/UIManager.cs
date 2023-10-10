using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject gameOverUI;
    public MapGenerator mapGenerator;
    public Game_Manager gameManager;
    public AudioManager audioManager;
    public Slider sfxSlider;
    public Slider musicSlider;
    private float sfxSliderValue;
    private float musicSliderValue;
    void Awake() {
        LoadPlayerPrefs();
    }
    void Start() {
        mapGenerator = gameObject.GetComponent<MapGenerator>();
        gameManager = gameObject.GetComponent<Game_Manager>();
    }
    //Options menu button
    public void EnableOptionsMenu() {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        LoadPlayerPrefs();
        sfxSlider.value = sfxSliderValue;
        musicSlider.value = musicSliderValue;
    }
    //Options menu => start menu Button
    public void EnableStartMenu() {
        optionsMenu.SetActive(false);
        gameOverUI.SetActive(false);
        audioManager.EnableMenuMusic();
        startMenu.SetActive(true);
    }
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
    //Load all options data stored in player prefs.
    // Called in start function of UIManager. Will load all previous session options data
    //  Player prefs are stored locally and persist between sessions/scene changes
    public void LoadPlayerPrefs() {
        string randomMode;
        if(PlayerPrefs.HasKey("sfxVolume")) {
            sfxSliderValue = PlayerPrefs.GetFloat("sfxVolume");
            sfxSliderValue = Mathf.Pow(10, (sfxSliderValue / 20));
            audioManager.masterAudioMixer.SetFloat("sfxVolume", Mathf.Log(sfxSliderValue) * 20);
        } else {
            sfxSliderValue = sfxSlider.maxValue;
        }

        if(PlayerPrefs.HasKey("musicVolume")) {
            musicSliderValue = PlayerPrefs.GetFloat("musicVolume");
            musicSliderValue = Mathf.Pow(10, (musicSliderValue / 20));
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




    
    public void QuitGame() {
        //PlayerPrefs.SetInt("HighScore", highScore);
        //PlayerPrefs.Save();
        Application.Quit();
    }
}