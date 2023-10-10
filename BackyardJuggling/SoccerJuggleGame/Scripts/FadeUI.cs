using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    public CanvasGroup highScoreText;
    //Fade in selected UI from its current alpha (set at 0) to 1
    public void FadeUIIn()
    {
        StartCoroutine(FadeUIElement(highScoreText, highScoreText.alpha, 1));
    }
    //Fade in selected UI from its current alpha (set at 1) to 0
    public void FadeUIOut()
    {
        StartCoroutine(FadeUIElement(highScoreText, highScoreText.alpha, 0));
    }
    //Fade given UI element from "start" value to "end" value over "lerpTime" seconds
    public IEnumerator FadeUIElement(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        //Start time = current Time.time
        float lerpStartTime = Time.time;
        //Time since start = current time - start time
        float timeSinceLerpStart = Time.time - lerpStartTime;
        float percentComplete = timeSinceLerpStart / lerpTime;

        while(true)
        {
            timeSinceLerpStart = Time.time - lerpStartTime;
            percentComplete = timeSinceLerpStart / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentComplete);
            highScoreText.alpha = currentValue;
            
            if(percentComplete >= 1)
            {
                break;
            }
            //Wait for end of frame, after all cameras rendered but before displaying
            // the frame on the screen
            yield return new WaitForEndOfFrame();
        }
    }
}
