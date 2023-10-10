using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Triggers : MonoBehaviour
{
    GameController gameController;
    [SerializeField] AudioSource audioSource;
    bool triggerActive = true;

    [SerializeField] enum TriggerType {A, B, C}
    [SerializeField] TriggerType triggerType;

    void Awake() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            if(triggerActive) {
                HandleTrigger();
            }
        }
    }

    void HandleTrigger() {
        if(!audioSource.isPlaying) {
            audioSource.Play();
        }

        //triggerActive = false; // Single use
    }
}