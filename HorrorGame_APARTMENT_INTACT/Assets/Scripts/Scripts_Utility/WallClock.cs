using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for DateTime and TimeSpan

public class WallClock : MonoBehaviour
{
    AudioSource wallClockAudio;
    [SerializeField] Transform secondHand;
    const float secondsToDegrees = 360f/60f;
    TimeSpan timeSpan;

    void Awake()
    {
        wallClockAudio = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!wallClockAudio.isPlaying)
        {
            wallClockAudio.Play();
        }
        AnimateMinuteHand();
    }
    
    void AnimateMinuteHand()
    {
        timeSpan = DateTime.Now.TimeOfDay;
        //Multiply by negative secondsToDegrees because we're looking down Z-axis
        secondHand.localRotation = Quaternion.Euler(
            0f, 0f, (float)timeSpan.TotalSeconds * secondsToDegrees
        );
    }
}
