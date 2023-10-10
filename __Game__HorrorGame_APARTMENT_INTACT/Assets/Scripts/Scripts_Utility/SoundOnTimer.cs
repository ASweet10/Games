using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnTimer : MonoBehaviour
{
    [SerializeField] AudioClip[] buildingGroanClips;
    [SerializeField] AudioClip[] thunderClips;
    [SerializeField] AudioClip[] monsterWheezeClips;
    [SerializeField] AudioSource audioSource;

    [SerializeField] float thunderWaitTime = 150f;
    [SerializeField] float wheezeWaitTime = 15f;
    [SerializeField] float groanWaitTime = 45f;

    enum SoundType {Thunder, MonsterWheeze, BuildingGroan}
    [SerializeField] SoundType soundType;

    bool canPlayAudio = true;

    void Awake()
    {
        if(audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }
    
    void Update()
    {
        if(canPlayAudio)
        {
            StartCoroutine(PlaySoundThenWait());
        }
    }
    
    IEnumerator PlaySoundThenWait()
    {
        canPlayAudio = false;
        int clipIndex;
        float waitTime = 0f;

        if(soundType == SoundType.Thunder)
        {
            clipIndex = Random.Range(0, thunderClips.Length);
            audioSource.clip = thunderClips[clipIndex];

            waitTime = thunderWaitTime;
        }
        else if(soundType == SoundType.MonsterWheeze)
        {
            clipIndex = Random.Range(0, monsterWheezeClips.Length);
            audioSource.clip = monsterWheezeClips[clipIndex];

            waitTime = wheezeWaitTime;
        }
        else if(soundType == SoundType.BuildingGroan)
        {
            clipIndex = Random.Range(0, buildingGroanClips.Length);
            audioSource.clip = buildingGroanClips[clipIndex];

            waitTime = groanWaitTime;
        }

        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
        yield return new WaitForSeconds(waitTime);
        canPlayAudio = true;
    }
}
