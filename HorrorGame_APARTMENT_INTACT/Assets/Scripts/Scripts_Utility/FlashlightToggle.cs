using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] GameObject lightGO;
    [SerializeField] AudioSource flashlightAudioSource;
    [SerializeField] bool lightOn = false;
    bool canUseFlashlight = true;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canUseFlashlight)
            {
                ToggleFlashlight();
            }
        }
    }
    
    void ToggleFlashlight()
    {
        if(lightOn)
        {
            lightGO.SetActive(false);
            lightOn = false;
            if(!flashlightAudioSource.isPlaying)
            {
                flashlightAudioSource.Play();
            }
        }
        else
        {
            lightGO.SetActive(true);
            lightOn = true;
            if(!flashlightAudioSource.isPlaying)
            {
                flashlightAudioSource.Play();
            }
        }
    }

    public void DisableFlashlight()
    {
        canUseFlashlight = false;
        if(lightOn)
        {
            lightGO.SetActive(false);
        }
    }

    public bool ReturnLightStatus()
    {
        return lightOn;
    }
}
