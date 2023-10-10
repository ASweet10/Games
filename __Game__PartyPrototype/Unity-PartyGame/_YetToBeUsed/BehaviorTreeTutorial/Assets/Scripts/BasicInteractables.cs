using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BasicInteractables : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameController gameController;


    [Header("Treasure Chest")]
    [SerializeField] private GameObject treasureChest;

    [SerializeField] enum InteractType {TreasureChest, EscapePoint}
    [SerializeField] InteractType interactType;

    void Start()
    {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        if(audioSource == null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
        }
    }

    public void InteractWithOnlyAudio() //Interact with objects + sfx only (locked door)
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}

