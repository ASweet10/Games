using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] AudioSource progressDoneAudioSource;
    public Image progressBar;
    float timer = 0.01f;
    [SerializeField] float delayValue = 20f;
    bool taskCompleted;

    public void UpdateProgressBarFill() {
        if(!taskCompleted) {
            timer += Time.deltaTime / delayValue;
            progressBar.fillAmount = timer;

            if(timer >= 0.55f && timer <= 0.65f) {
                
            }
            else if(timer >= 1) {
                timer = 0f;
                progressBar.fillAmount = 0f;
                progressBar.enabled = false;
                progressDoneAudioSource.Play();

                taskCompleted = true;
            }
        }
    }
}