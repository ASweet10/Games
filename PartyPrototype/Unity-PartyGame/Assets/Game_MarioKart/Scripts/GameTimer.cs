using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    //Tens and minute places on timer( 15:00 )
    [SerializeField] private Text minutesText;
    [SerializeField] private Text secondsText;
    [SerializeField] private float minutes = 15;
    private float seconds = 0;
    private bool canUpdateTimer = true;
    
    
    void Start() {
        ResetTimer();
    }

    void Update() {
        if(minutes + seconds != 0) {
            UpdateTimer();
        } else {
            //EvokeGameOver();
        }
    }
    public void UpdateTimer() {
        StartCoroutine(DecrementTimerSeconds());
    }
    public void StopTimer() {

    }
    private void ResetTimer() {

    }
    private IEnumerator DecrementTimerSeconds() {
        if(canUpdateTimer) {
            if(seconds == 0) {
                if(minutes == 0) {
                    yield break;
                    //Or call EvokeGameOver(); here??
                } else {
                    canUpdateTimer = false;
                    minutes --;
                    seconds = 59;
                    yield return new WaitForSeconds(1f);
                    canUpdateTimer = true;
                }
            } else {
                canUpdateTimer = false;
                seconds--;
                yield return new WaitForSeconds(1f);
                canUpdateTimer = true;
            }

            if(seconds < 10) {
                secondsText.text = "0" + seconds.ToString();
            } else {
                secondsText.text = seconds.ToString();
            }

            if(minutes < 10) {
                minutesText.text = "0" + minutes.ToString();
            } else {
                minutesText.text = minutes.ToString();
            }
        }

    }
}
