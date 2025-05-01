using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] Light lightSource;
    [SerializeField] AudioSource flickerAudio;
    bool isFlickering = false;

    void Awake() {
        lightSource = gameObject.GetComponent<Light>();
    }

    void Update() {
        if(isFlickering == false) {
            StartCoroutine(FlickerLight());
        }
    }

    IEnumerator FlickerLight() {
        isFlickering = true;
        lightSource.enabled = false; // Toggle light
        flickerAudio.Pause();
        yield return new WaitForSeconds(Random.Range(0.5f, 2f)); // Randomize delay time

        lightSource.enabled = true;
        flickerAudio.Play();
        yield return new WaitForSeconds(Random.Range(0.01f, 0.5f));
        isFlickering = false;
    }
}
