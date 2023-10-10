using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    [SerializeField] int maxLaps = 3; 
    [SerializeField] TMPro.TMP_Text currentLapTimeText;
    [SerializeField] TMPro.TMP_Text lapOneTimeText;
    [SerializeField] TMPro.TMP_Text lapTwoTimeText;
    [SerializeField] TMPro.TMP_Text lapThreeTimeText;
    [SerializeField] TMPro.TMP_Text popupText;

    [SerializeField] Image abilityOneImage;
    [SerializeField] Image abilityTwoImage;
    [SerializeField] Sprite[] abilitySprites;

    int lapCounter = 1;
    float currentLapTime = 0f;
    float lapOneTime = 0f;
    float lapTwoTime = 0f;
    float lapThreeTime = 0f;
    float startTime;
    bool hasStartedLap = false;


    void Start() {
        lapCounter = 1;
        currentLapTime = 0f;
        lapOneTime = 0f;
        lapTwoTime = 0f;
        lapThreeTime = 0f;
        if(lapTwoTimeText.isActiveAndEnabled) {
            lapTwoTimeText.enabled = false;
        }
        if(lapThreeTimeText.isActiveAndEnabled) {
            lapThreeTimeText.enabled = false;
        }
        startTime = Time.time;
        StartLap();
    }
    void Update() {
        Debug.Log(hasStartedLap);
        if(hasStartedLap) {
            IncrementLapTime();
        }

        SetCarUI();
    }
    void SetCarUI() {
        currentLapTimeText.text = "Current: " + currentLapTime.ToString("f4");
    }

    public void StartLap() {
        hasStartedLap = true;
        startTime = Time.time;
        currentLapTime = 0f;
    }

    public void FinishLap() {
        lapCounter ++;
        switch(lapCounter){
            case 1:
                break;
            case 2:
                lapOneTimeText.enabled = true;
                lapOneTimeText.text = currentLapTime.ToString();
                break;
            case 3:
                lapTwoTimeText.enabled = true;
                lapTwoTimeText.text = currentLapTime.ToString();
                //popupText.text = "Final Lap";
                break;
            case 4:
                lapThreeTimeText.enabled = true;
                lapThreeTimeText.text = currentLapTime.ToString();
                //popupText.text = "Finish";
                //currentLapTimeText.enabled = false;
                break;
        }
    }

    void IncrementLapTime() {
        currentLapTime = Time.time - startTime;
    }

    public void SetAbilityUI(int abilityNum, int abilityIndex, bool abilityReady) {
        if(abilityReady) {

        } else {
            switch(abilityNum) {
                case 1:
                    abilityOneImage.enabled = false;
                    break;
                case 2:
                    abilityOneImage.enabled = false;
                    break;
            }
        }
    }
}
