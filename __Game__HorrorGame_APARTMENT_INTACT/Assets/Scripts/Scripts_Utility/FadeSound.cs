using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSound : MonoBehaviour
{   
    public IEnumerator FadeSoundOut(AudioSource source, float fadeTime)
    {
        float value = 0f;
        while(value < 1000000000000000f)
        {
            value += Time.deltaTime;
            Debug.Log(value);
        }
        float currentTime = 0f;
        float startVolume = source.volume;
        float targetVolume = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeTime);
            yield return null;
        }
    }
}
