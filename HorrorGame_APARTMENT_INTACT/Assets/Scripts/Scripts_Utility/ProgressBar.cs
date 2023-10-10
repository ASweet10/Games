using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject player;
    [SerializeField] GameObject blownFuseTrigger;
    [SerializeField] AudioSource progressDoneAudioSource;
    [SerializeField] AudioSource toolAudioSource;
    [SerializeField] AudioClip ratchetClip;
    [SerializeField] AudioClip screwBulbClip;
    [SerializeField] AudioClip unscrewBulbClip;


    [Header("Sink")]
    public Image sinkProgressBar;
    [SerializeField] GameObject sinkObj;
    [SerializeField] GameObject sinkLeakVFX;
    [SerializeField] GameObject sinkScareObj;
    [SerializeField] AudioSource sinkScareAudioSource;
    float sinkTimer = 0.01f;
    [SerializeField] float sinkDelayValue = 20f;

    [Header("Light")]
    public Image lightProgressBar;
    [SerializeField] GameObject lightObj;
    [SerializeField] GameObject faultyLight;
    [SerializeField] Light faultyLightComponent;
    [SerializeField] FlickeringLight flickeringScript;
    [SerializeField] AudioSource lightAUDIO;
    float lightTimer = 0.01f;
    [SerializeField] float lightDelayValue = 20f;

    public void UpdateSinkFill()
    {
        if(!gameController.sinkCompleted)
        {
            sinkTimer += Time.deltaTime / sinkDelayValue;
            sinkProgressBar.fillAmount = sinkTimer;
            toolAudioSource.clip = ratchetClip;

            if(!toolAudioSource.isPlaying)
            {
                toolAudioSource.Play();
            }

            if(sinkTimer >= 0.55f && sinkTimer <= 0.65f)
            {
                PlaySinkScareMoment();
            }
            else if(sinkTimer >= 1)
            {
                sinkTimer = 0f;
                sinkProgressBar.fillAmount = 0f;
                sinkProgressBar.enabled = false;
                progressDoneAudioSource.Play();

                //Disable sink VFX + SFX
                sinkLeakVFX.SetActive(false);
                toolAudioSource.Stop();
                sinkObj.tag = "Untagged";

                gameController.engagedInTask = false;

                if(gameController.trashCompleted && gameController.lightCompleted) {
                    blownFuseTrigger.SetActive(true);
                }
                else {
                    gameController.currentCheckpoint = 2;
                }

            }
        }
    }
    public void UpdateLightFill()
    {
        if(!gameController.lightCompleted)
        {
            lightTimer += Time.deltaTime / lightDelayValue;
            lightProgressBar.fillAmount = lightTimer;
            toolAudioSource.clip = unscrewBulbClip;

            if(!toolAudioSource.isPlaying)
            {
                toolAudioSource.Play();
            }

            if(lightTimer >= 0.45f && lightTimer <= 0.55f)
            {
                faultyLight.SetActive(false);
            }

            if(lightTimer >= 1)
            {
                lightTimer = 0f;
                lightProgressBar.fillAmount = 0f;
                lightProgressBar.enabled = false;
                progressDoneAudioSource.Play();
                faultyLight.SetActive(true);
                flickeringScript.enabled = false;
                faultyLightComponent.enabled = true;
                lightAUDIO.enabled = false;

                toolAudioSource.Stop();
                lightObj.tag = "Untagged";

                gameController.engagedInTask = false;

                if(gameController.trashCompleted && gameController.sinkCompleted) {
                    blownFuseTrigger.SetActive(true);
                }
                else {
                    gameController.currentCheckpoint = 2;
                }

            }
        }
    }

    void PlaySinkScareMoment()
    {
        if(!sinkScareAudioSource.isPlaying)
        {
            sinkScareObj.SetActive(true);
            sinkScareAudioSource.Play();
        }
    }
}
