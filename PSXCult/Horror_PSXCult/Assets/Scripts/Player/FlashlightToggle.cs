using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    
    [SerializeField] KeyCode flashlightKey = KeyCode.F;
    [SerializeField] GameObject flashlight;
    [SerializeField] AudioSource flashlightAudioSource;
    public bool flashlightActive = false;
    bool canUseFlashlight = true;

    void Update() {
        HandleFlashlightInteraction();
    }
    
    void HandleFlashlightInteraction() {
        if(Input.GetKeyDown(KeyCode.F)) {
            if(canUseFlashlight) {
                flashlightActive = !flashlightActive;
                flashlight.SetActive(flashlightActive);
                if(!flashlightAudioSource.isPlaying) {
                    flashlightAudioSource.Play();
                }
            }
        }
    }
    public void ToggleFlashlightStatus(bool newStatus) {
        canUseFlashlight = newStatus;
        if(flashlight.activeInHierarchy) {
            flashlight.SetActive(false);
        }
    }
}
