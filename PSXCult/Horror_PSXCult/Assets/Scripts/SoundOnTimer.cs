using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnTimer : MonoBehaviour
{
    [SerializeField] AudioClip[] buildingGroanClips;
    [SerializeField] AudioClip[] thunderClips;
    [SerializeField] AudioSource audioSource;

    [SerializeField] float thunderWaitTime = 150f;
    [SerializeField] float buildingWaitTime = 45f;

    enum SoundType {Thunder, BuildingGroan}
    [SerializeField] SoundType soundType;

    bool canPlayAudio = true;

    void Awake() {
        if(audioSource == null) {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }
    
    void Update() {
        if(canPlayAudio) {
            StartCoroutine(PlaySoundThenWait());
        }
    }
    
    IEnumerator PlaySoundThenWait() {
        canPlayAudio = false;
        switch(soundType) {
            case SoundType.Thunder:
                audioSource.clip = thunderClips[Random.Range(0, thunderClips.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(thunderWaitTime);
                break;
            case SoundType.BuildingGroan:
                audioSource.clip = buildingGroanClips[Random.Range(0, buildingGroanClips.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(buildingWaitTime);
                break;         
            default:
                break;
        }
        canPlayAudio = true;
    }
}