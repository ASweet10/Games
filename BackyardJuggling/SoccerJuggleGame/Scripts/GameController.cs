using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text tapCountText;
    public Text gameOverCountText;
    public Text gameOverRecordText;
    public Text pauseMenuRecordText;

    public GameObject gameOverMenu;
    public GameObject ballStartPoint;
    public GameObject ballStasisPoint;
    public GameObject mainMenu;
    public GameObject gameHUD;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject rulesMenu;
    public GameObject gameCredits;
    public GameObject gameBall;
    public GameObject bonusBall1;
    public GameObject bonusBall2;

    public GameObject ShoeUI;
    public GameObject Shoe2UI;
    public GameObject Shoe3UI;

    public Rigidbody2D gameBallRB2D;

    public ChangeBG changeBGScript;
    public FadeUI fadeUIScript;
    public SaveLoad saveLoadScript;
    private BonusBall bonusBallScript;
    public Shoe shoeScript;

    public AudioSource audioSource;

    public int tapCount;
    public bool stasisEnabled;
    public float maxRotationTimer = 5f;

    int tapRecord;
    bool gamePaused = false;
    bool highScoreSFXCanPlay;
    
    void Start()
    {
        gameBallRB2D = gameBall.GetComponent<Rigidbody2D>();
        bonusBallScript = gameObject.GetComponent<BonusBall>();
        tapRecord = saveLoadScript.ReadRecordFromFile();
        changeBGScript.StartBGM();
        audioSource = gameObject.GetComponent<AudioSource>();
        highScoreSFXCanPlay = true;
    }
    void Update()
    {
        CompareCountToRecord();
        tapCountText.text = tapCount.ToString();
    }
    public void CompareCountToRecord()
    {
        //If new high score..
        if(tapCount >= tapRecord)
        {
            tapRecord = tapCount;
            //Play audio & enable high score UI
            if(highScoreSFXCanPlay)
            {
                fadeUIScript.FadeUIIn();
                audioSource.Play();
                highScoreSFXCanPlay = false;
                StartCoroutine(WaitToFadeOut());
            }
        }
    }
    public void SetNewTapRecord(int newRecord)
    {
        if (newRecord > tapRecord)
        {
            tapRecord = newRecord;
            saveLoadScript.SaveRecordToFile(newRecord);
        }
    }
    public void EnableGameOverMenu()
    {
        Time.timeScale = 0;
        ChangeBonusBallState(1, false);
        ChangeBonusBallState(2, false);
        changeBGScript.StopBGM();
        gameBallRB2D.gravityScale = 0f;
        gameHUD.SetActive(false);
        gameBall.SetActive(false);
        SetNewTapRecord(tapRecord);

        gameOverCountText.text = tapCount.ToString();
        gameOverRecordText.text = tapRecord.ToString();
        gameOverMenu.SetActive(true);        
    }

    //Menu Buttons    
    public void EnableOptionsMenu()  // Pause / Main Menus to Options Menu
    {
        gameBall.SetActive(false);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }    
    public void EnableMainMenu()  // Options Menu to Main Menu
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void EnableRulesMenu()
    {
        mainMenu.SetActive(false);
        rulesMenu.SetActive(true);
    }

    public void EnableCredits()
    {
        gameCredits.SetActive(true);
    }
    public void DisableCredits()
    {
        gameCredits.SetActive(false);
    }
    
    public void PlayGame()
    {
        tapCount = 0;
        shoeScript.shoeCount = 3;
        highScoreSFXCanPlay = true;
        gameBall.transform.position = ballStartPoint.transform.position;
        gameBallRB2D.gravityScale = 0.85f;
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        rulesMenu.SetActive(false);

        gameHUD.SetActive(true);
        ShoeUI.SetActive(true);
        Shoe2UI.SetActive(true);
        Shoe3UI.SetActive(true);

        gameBall.SetActive(true);
        bonusBallScript.bonus1CanSpawn = true;
        bonusBallScript.bonus2CanSpawn = true;
        
        if(changeBGScript.bgmEnabled == true)
        {
            changeBGScript.StartBGM();
        }
        if (changeBGScript.ambienceEnabled == true)
        {
            changeBGScript.StartBGM();
        }
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        changeBGScript.PauseBGM();
        gameBall.SetActive(false);
        if(bonusBall1.activeInHierarchy)
        {
            bonusBallScript.bonusBall1Active = true;
            ChangeBonusBallState(1, false);
        }
        if(bonusBall2.activeInHierarchy)
        {
            bonusBallScript.bonusBall2Active = true;
            ChangeBonusBallState(2, false);
        }
        gameHUD.SetActive(false);
        pauseMenuRecordText.text = tapRecord.ToString();
        pauseMenu.SetActive(true);
    }
    public void UnpauseGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        if(changeBGScript.CheckBGM() == true)
        {
            changeBGScript.StartBGM();
        }
        if(changeBGScript.CheckAmbience() == true)
        {
            changeBGScript.StartBGM();
        }
        gameBall.SetActive(true);

        if(bonusBallScript.bonusBall1Active == true)
        {
            ChangeBonusBallState(1, true);
        }
        if(bonusBallScript.bonusBall2Active == true)
        {
            ChangeBonusBallState(2, true);
        }
        gameHUD.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void QuitGame()
    {
        saveLoadScript.SaveRecordToFile(tapRecord);
        if(saveLoadScript.doneSaving)
        {
            Application.Quit();
        }
    }
    public void PressBackButton()
    {
        // Options Menu to Pause Menu, mid-game
         if (gamePaused == true)
         {
            optionsMenu.GetComponent<OptionsMenu>().SetCurrentBallChoice();
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
         }
        // Options Menu to Main Menu
         else
         {
            optionsMenu.GetComponent<OptionsMenu>().SetCurrentBallChoice();
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
         }
    }
    //End Menu Buttons

    //Change bonus ball state on pause/unpause 
    public void ChangeBonusBallState(int ballNumber, bool state)
    {
        if(ballNumber == 1)
        {
            bonusBall1.SetActive(state);
        }
        else if(ballNumber == 2)
        {
            bonusBall2.SetActive(state);
        }
    }

    //-Put ball into stasis mode if saved by shoe
    //-Turn off gravity scale, and change velocity and angular velocity to 0
    //-Start RotateObject coroutine
    public void EnableBallStasis()
    {
        stasisEnabled = true;
        gameBallRB2D.gravityScale = 0f;
        gameBallRB2D.velocity = Vector3.zero;
        gameBallRB2D.angularVelocity = 0f;
        gameBall.transform.position = ballStasisPoint.transform.position;
        StartCoroutine(RotateObject());
    }
    //Slowly rotate ball in place while in stasis
    IEnumerator RotateObject()
    {
        float timer = 0f;
        while(timer <= maxRotationTimer)
        {
            gameBall.transform.Rotate(new Vector3(0, 0, 1080) * Time.deltaTime * 0.25f);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator WaitToFadeOut()
    {
        yield return new WaitForSeconds(2);
        fadeUIScript.FadeUIOut();
    }
}
