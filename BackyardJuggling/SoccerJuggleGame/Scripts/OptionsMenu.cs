using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Sprite[] ballArray = new Sprite[8];
    public Sprite[] flagArray = new Sprite[8];

    public GameObject ballMenuSpawnPoint;
    public GameObject flagSpawnPoint;
    public GameObject gameBall;
    public GameObject background;

    public AudioSource gameBallAudio;
    public AudioSource bonusBall1Audio;
    public AudioSource bonusBall2Audio;
    public AudioSource backgroundAudio;
    public AudioSource highScoreSFXAudio;
    public AudioSource shoeZoneAudio;

    public Image mainMenuBallImage;

    GameObject bonusBall1;
    GameObject bonusBall2;

    public ChangeBG changeBGScript;
    public ColorLerper colorLerpScript;
    public Dropdown dropdownColorOne;

    Image optionsMenuBallImage;
    Image flagImage;

    SpriteRenderer gameBallSR;
    public SpriteRenderer bonusBall1SR;
    public SpriteRenderer bonusBall2SR;

    //ballMenuImage array index
    int ballIndex = 0;
    //flagImage array index
    int flagIndex = 0;
    void Start()
    {
        optionsMenuBallImage = ballMenuSpawnPoint.GetComponent<Image>();
        flagImage = flagSpawnPoint.GetComponent<Image>();
        gameBallSR = gameBall.GetComponent<SpriteRenderer>();
        bonusBall1 = GameObject.FindGameObjectWithTag("Bonus1");
        bonusBall2 = GameObject.FindGameObjectWithTag("Bonus2");

        optionsMenuBallImage.color = new Color(255, 255, 255, 1f);
        flagImage.color = new Color(255, 255, 255, 1f);
        optionsMenuBallImage.sprite = ballArray[0];
        flagImage.sprite = flagArray[0];
    }    
    void Update()
    {
        optionsMenuBallImage.sprite = ballArray[ballIndex];
        flagImage.sprite = flagArray[flagIndex];
        SetDropdownChoice();
    }
    //Options menu right arrow UI button
    public void PressRightArrow()
    {
        //Keep index in bounds of array
        if(ballIndex < 7)
        {
            ballIndex++;
        }
        if(flagIndex < 7)
        {
            flagIndex++;
        }
    }
    //Options menu left arrow UI button
    public void PressLeftArrow()
    {
        //Keep index in bounds of array
        if (ballIndex > 0)
        {
            ballIndex--;
        }
        if (flagIndex > 0)
        {
            flagIndex--;
        }
    }

    //-Options menu ball selection
    //-Set current selection for:
    //--Main menu ball image
    //--Game ball
    //--Bonus ball 1 and 2
    public void SetCurrentBallChoice()
    {
        gameBallSR.sprite = ballArray[ballIndex];
        if(bonusBall1SR != null)
        {
            bonusBall1SR.sprite = ballArray[ballIndex];
        }
        if(bonusBall2SR != null)
        {
            bonusBall2SR.sprite = ballArray[ballIndex];
        }
        if(mainMenuBallImage != null)
        {
            mainMenuBallImage.sprite = ballArray[ballIndex];
        }
    }
    //Enable or disable SFX based on UI selection
    public void SFXSliderChanged(bool newValue)
    {
        gameBallAudio.enabled = newValue;
        bonusBall1Audio.enabled = newValue;
        bonusBall2Audio.enabled = newValue;
        highScoreSFXAudio.enabled = newValue;
        shoeZoneAudio.enabled = newValue;
    }
    //Enable or disable music based on UI selection
    public void BGMSliderChanged(bool newValue)
    {
        if(newValue == true)
        {
            changeBGScript.SetBGM(true);
            changeBGScript.SetAmbience(false);
        }
        else
        {
            changeBGScript.SetBGM(false);
        }
    }
    //Enable or disable ambiance based on UI selection
    public void AmbienceSliderChange(bool newValue)
    {
        if(newValue == true)
        {
            changeBGScript.SetAmbience(true);
            changeBGScript.SetBGM(false);
        }
        else
        {
            changeBGScript.SetAmbience(false);
        }
    }
    //Set new ring color if changed by dropdown value
    public void SetDropdownChoice()
    {
        switch(dropdownColorOne.value)
        {
            case 1:
                colorLerpScript.startColor = Color.blue;
                break;
            case 2:
                colorLerpScript.startColor = Color.red;
                break;
            case 3:
                colorLerpScript.startColor = Color.green;
                break;
            case 4:
                colorLerpScript.startColor = new Color(1f, 0.64f, 0f);
                break;
            case 5:
                colorLerpScript.startColor = new Color(0.4f, 0.2f, 0.6f);
                break;
        }
    }
}
