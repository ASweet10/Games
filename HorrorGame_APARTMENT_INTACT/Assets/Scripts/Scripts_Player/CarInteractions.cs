using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarInteractions : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip carEndingAudioClip;
    [SerializeField] LevelController levelController;
    [SerializeField] GameController gameController;
    [SerializeField] PlayerController playerController;
    [SerializeField] Collider openDoorTriggerCollider;
    [SerializeField] GameObject mainCarTrigger;
    [SerializeField] GameObject player;
    [SerializeField] Transform playerStartPoint;
    [SerializeField] GameObject carEndingUI;


    string cantLeaveYetString = "I should see what the job's about first...";
    string betterOffHereString = "Drove all the way here might as well make some money";
    
    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void AttemptToUseCar()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if(gameController.currentCheckpoint == 0)
        {
            gameController.ShowPopupMessage(cantLeaveYetString, 3, false);
        }
        else if(gameController.currentCheckpoint == 1)
        {
            gameController.ShowPopupMessage(betterOffHereString, 3, false);  
        }
        else if(gameController.canLeaveByCar)
        {
            carEndingUI.SetActive(true);
        }
    }

    public void SayYesToCarEnding()
    {
        StartCoroutine(StartSensibleCarEnding());
    }
    public void SayNoToCarEnding()
    {
        carEndingUI.SetActive(false);
    }
    
    IEnumerator StartSensibleCarEnding()
    {
        audioSource.clip = carEndingAudioClip;
        audioSource.Play();
        levelController.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        levelController.LoadLevel(2);
    }
}
