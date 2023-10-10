using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Animator myDoor;
    
    [SerializeField] AudioClip[] openClips;
    [SerializeField] AudioClip[] closeClips;
    AudioClip chosenClip;

    [SerializeField] enum DoorType {BuildingFrontDoor, ApartmentFrontDoor, ApartmentDoor};
    [SerializeField] DoorType doorType;
    
    bool doorClosed = true;
    bool canInteractWithDoor = true;

    public void OpenDoor()
    {
        if(canInteractWithDoor)
        {
            canInteractWithDoor = false;

            chosenClip = openClips[Random.Range(0, openClips.Length)];
            AudioSource.PlayClipAtPoint(chosenClip, transform.position, 0.5f);

            if(doorType == DoorType.BuildingFrontDoor)
            {
                myDoor.Play("OpenBuildingFrontDoor", 0, 0.0f);
            }
            else if(doorType == DoorType.ApartmentFrontDoor)
            {
                myDoor.Play("OpenApartmentFrontDoor", 0, 0.0f);
            }
            else if(doorType == DoorType.ApartmentDoor)
            {
                myDoor.Play("OpenApartmentDoor", 0, 0.0f);
            }
            doorClosed = false;
            StartCoroutine(DelayDoor());
        }

    }

    public void CloseDoor()
    {
        if(canInteractWithDoor)
        {
            canInteractWithDoor = false;
            chosenClip = closeClips[Random.Range(0, closeClips.Length)];
            AudioSource.PlayClipAtPoint(chosenClip, transform.position, 0.5f);

            if(doorType == DoorType.BuildingFrontDoor)
            {
                myDoor.Play("CloseBuildingFrontDoor", 0, 0.0f);
            }
            else if(doorType == DoorType.ApartmentFrontDoor)
            {
                myDoor.Play("CloseApartmentFrontDoor", 0, 0.0f);
            }
            else if(doorType == DoorType.ApartmentDoor)
            {
                myDoor.Play("CloseApartmentDoor", 0, 0.0f);
            }
            doorClosed = true;
            StartCoroutine(DelayDoor());
        }

    }

    public bool ReturnDoorStatus()
    {
        return doorClosed;
    }
    
    IEnumerator DelayDoor()
    {
        yield return new WaitForSeconds(1f);
        canInteractWithDoor = true;
    }
}
