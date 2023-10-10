using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBG : MonoBehaviour
{
    public Sprite dayBGSprite;
    public Sprite nightBGSprite;

    public AudioSource audioSource;
    public AudioClip dayAmbience;
    public AudioClip nightAmbience;
    public AudioClip backgroundMusic;

    public OptionsMenu optionsScript;

    public bool dayChosen;
    public bool nightChosen;
    public bool bgmEnabled;
    public bool ambienceEnabled;
  
    public SpriteRenderer backgroundSR;

    void Start()
    {
        dayChosen = true;
        bgmEnabled = true;
    }
    //-UI Button in Options Menu
    //-Change background sprite and ambience audio
    public void ChooseDayBG()
    {
        nightChosen = false;
        backgroundSR.sprite = dayBGSprite;
        if(ambienceEnabled)
        {
            audioSource.clip = dayAmbience;
        }
        if(bgmEnabled)
        {
            audioSource.clip = backgroundMusic;
        }
        dayChosen = true;
    }
    //-UI Button in Options Menu
    //-Change background sprite and ambience audio
    public void ChooseNightBG()
    {
        dayChosen = false;
        backgroundSR.sprite = nightBGSprite;
        if(ambienceEnabled)
        {
            audioSource.clip = nightAmbience;
        }
        if(bgmEnabled)
        {
            audioSource.clip = backgroundMusic;
        }
        nightChosen = true;
    }
    public bool CheckBGM()
    {
        if(bgmEnabled)
        {
            return true;
        }
        else{
            return false;
        }
    }
    public bool CheckAmbience()
    {
        if(ambienceEnabled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //UI Toggle for music in Options Menu
    public void SetBGM(bool choice)
    {
        if (choice == true)
        {
            bgmEnabled = true;
            audioSource.clip = backgroundMusic;
        }
        else
        {
            bgmEnabled = false;
        }
    }
    //UI Toggle for ambiance in Options Menu
    public void SetAmbience(bool choice)
    {
        if (choice == true)
        {
            if(dayChosen)
            {
                audioSource.clip = dayAmbience;
                ambienceEnabled = true;
            }
            else if(nightChosen)
            {
                audioSource.clip = nightAmbience;
                ambienceEnabled = true;
            }
        }
        else
        {
            ambienceEnabled = false;
        }
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
    public void StartBGM()
    {
        audioSource.Play();
    }
    public void PauseBGM()
    {
        audioSource.Pause();
    }
}
