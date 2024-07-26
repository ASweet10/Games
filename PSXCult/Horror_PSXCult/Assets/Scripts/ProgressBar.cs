using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject player;
    [SerializeField] AudioSource progressDoneAudioSource;

    [Header("Tents")]
    public Image tentProgressBar;
    [SerializeField] AudioSource tentAudio;
    [SerializeField] GameObject tentObj;
    float tentTimer = 0.01f;
    [SerializeField] float tentDelayValue = 20f;

    public void UpdateTentFill() {
        if(!gameController.tentCompleted) {
            tentTimer += Time.deltaTime / tentDelayValue;
            tentProgressBar.fillAmount = tentTimer;

            if(!tentAudio.isPlaying) {
                tentAudio.Play();
            }
            /*
            if(tentTimer >= 0.55f && tentTimer <= 0.65f) {
                PlaySinkScareMoment();
            }
            */
            else if(tentTimer >= 1) {
                tentTimer = 0f;
                tentProgressBar.fillAmount = 0f;
                tentProgressBar.enabled = false;
                progressDoneAudioSource.Play();

                tentAudio.Stop();
                tentObj.tag = "Untagged";
                StartCoroutine(gameController.HandleNextObjective());
                //gameController.engagedInTask = false;
            }
        }
    }
}