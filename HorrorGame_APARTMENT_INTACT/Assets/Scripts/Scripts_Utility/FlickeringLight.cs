using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] float flickerDelay;
    [SerializeField] Light lightToFlicker;
    [SerializeField] AudioSource flickerAudio;
    bool isFlickering = false;

    void Awake()
    {
        if(lightToFlicker == null)
        {
            lightToFlicker = gameObject.GetComponent<Light>();
        }
    }

    void Update()
    {
        if(isFlickering == false)
        {
            StartCoroutine(FlickerLight());
        }
    }
    
    //Toggle light and randomize delay time
    IEnumerator FlickerLight()
    {
        isFlickering = true;
        lightToFlicker.enabled = false;
        flickerAudio.Pause();
        flickerDelay = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(flickerDelay);

        lightToFlicker.enabled = true;
        flickerAudio.Play();
        flickerDelay = Random.Range(0.01f, 0.5f);
        yield return new WaitForSeconds(flickerDelay);
        isFlickering = false;
    }
}
