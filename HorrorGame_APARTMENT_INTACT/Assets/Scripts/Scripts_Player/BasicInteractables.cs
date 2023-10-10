using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BasicInteractables : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameController gameController;


    [Header("Locker Door")]
    [SerializeField] Animator lockerDoorAnim;
    [SerializeField] GameObject chosenLockerDoor;
    [SerializeField] MonsterAI monsterAI;
    bool lockerDoorClosed = true;


    [Header("Laundry Window")]
    [SerializeField] Animator laundryWindowAnim;
    [SerializeField] AudioSource laundryRainAudio;
    [SerializeField] AudioClip closeWindowSFX;
    [SerializeField] AudioClip openWindowSFX;
    bool windowClosed = false;

    [Header("Head Office Locked Drawer")]
    [SerializeField] Animator lockedDrawerAnim;
    [SerializeField] Collider drawerBoxCollider;
    [SerializeField] GameObject underDrawerCollider;
    [SerializeField] GameObject journalUI;
    [SerializeField] GameObject distortScreenObj;


    [SerializeField] enum InteractType {LockerHide, LaundryWindow, Drawer, ExpositionNote}
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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(interactType == InteractType.LockerHide) //If player standing in locker...
            {
                if(monsterAI.enabled)
                {
                    if(lockerDoorClosed)
                    {
                        if(monsterAI.playerCanHide) //Player not recently seen by monster...
                        {
                            monsterAI.playerIsHiding = true; //Player hidden from monster's view
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(interactType == InteractType.LockerHide)
        {
            if(monsterAI.enabled)
            {
                monsterAI.playerIsHiding = false; //Player can be seen by monster
            }
        }
    }

    public void InteractWithOnlyAudio() //Interact with objects + sfx only (locked door)
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void InteractWithLocker()
    {
        if(lockerDoorAnim == null)
        {
            lockerDoorAnim = gameObject.GetComponent<Animator>();
        }

        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if(lockerDoorClosed)
        {
            lockerDoorClosed = false;
            lockerDoorAnim.Play("OpenLockerDoor");
        }
        else
        {
            lockerDoorClosed = true;
            lockerDoorAnim.Play("CloseLockerDoor");
        }
    }

    public void InteractWithLaundryWindow()
    {
        //Open window
        if(windowClosed)
        {
            audioSource.clip = openWindowSFX;
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            laundryWindowAnim.Play("OpenWindow");
            laundryRainAudio.volume = 0.8f; //Increase sfx volume
            windowClosed = false;
        }
        //Close window
        else
        {
            audioSource.clip = closeWindowSFX;
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }            
            laundryWindowAnim.Play("CloseWindow");
            laundryRainAudio.volume = 0.3f; //Decrease sfx volume
            windowClosed = true;
        }
    }

    public void InteractWithDrawer()
    {
        lockedDrawerAnim.Play("DrawerOpen", 0, 0.0f);
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        underDrawerCollider.SetActive(true);
        drawerBoxCollider.enabled = false;
    }

    public void EnableJournalNote()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        journalUI.SetActive(true);
        distortScreenObj.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DisableJournalNote()
    {
        journalUI.SetActive(false);
        distortScreenObj.SetActive(true);
    }

}

