using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place this on door you want to open; Needs animator
public class DoorController : MonoBehaviour
{
    [SerializeField] Animator myDoor;
    
    [SerializeField] AudioClip openDoorClip;
    [SerializeField] AudioClip closeDoorClip;
    [SerializeField] AudioSource gasStationBellAudio;
    [SerializeField] AudioClip gasStationBellClip;
    
    bool doorClosed = true;
    public bool DoorClosed {
        get { return doorClosed; }
        set { doorClosed = value; }
    }
    bool canInteractWithDoor = true;

    public void OpenDoor() {
        if(canInteractWithDoor) {
            canInteractWithDoor = false;
            AudioSource.PlayClipAtPoint(openDoorClip, transform.position, 0.5f);
            AudioSource.PlayClipAtPoint(gasStationBellClip, transform.position, 0.5f);
            myDoor.Play("GasStationDoorOpen", 0, 0.0f);
            doorClosed = false;
            StartCoroutine(DelayDoor());
        }
    }

    public void CloseDoor() {
        if(canInteractWithDoor) {
            canInteractWithDoor = false;
            AudioSource.PlayClipAtPoint(closeDoorClip, transform.position, 0.5f);
            AudioSource.PlayClipAtPoint(gasStationBellClip, transform.position, 0.5f);
            myDoor.Play("GasStationDoorClose", 0, 0.0f);
            doorClosed = true;
            StartCoroutine(DelayDoor());
        }
    }
    
    IEnumerator DelayDoor() {
        yield return new WaitForSeconds(1f);
        canInteractWithDoor = true;
    }
}