using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlights : MonoBehaviour
{
    GameObject lastHighlightedObject;

    [SerializeField] Image cursorUI;
    [SerializeField] AudioSource toolAudioSource;
    [SerializeField] GameObject toolAudioObj;
    [SerializeField] Sprite normalCursor;
    [SerializeField] Sprite interactCursor;
    Camera mainCamera;
    [SerializeField] GameController gameController;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        HighlightObjectInCenterOfCam();
    }

    void HighlightObject(GameObject gameObject, bool uiEnabled)
    {
        if (lastHighlightedObject != gameObject)
        {
            ClearHighlighted();

            lastHighlightedObject = gameObject;
            if(uiEnabled)
            {
                cursorUI.sprite = interactCursor;
            }
        }
    } 

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            //lastHighlightedObject.GetComponent<MeshRenderer>().material = originalMat;
            
            lastHighlightedObject = null;
            cursorUI.sprite = normalCursor;
        }
    } 
    
    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 50f;
        // Ray from the center of the viewport.
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;
        // Check if we hit something.
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            // Get the object that was hit.
            GameObject hitObj = rayHit.collider.gameObject;

            switch(hitObj.GetComponent<Collider>().gameObject.tag)
            {
                case "OpenableDoor":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            var doorScript = hitObj.GetComponent<Collider>().gameObject.GetComponent<DoorController>();
                            bool doorClosed = doorScript.ReturnDoorStatus();
                            if(doorClosed)
                            {
                                doorScript.OpenDoor();
                            }
                            else
                            {
                                doorScript.CloseDoor();
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "Escape":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 6f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            //hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().EscapeAndWin();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "ExpositionNote":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            //hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().EnableJournalNote();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "TreasureChest":
                /*
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().InteractWithHiddenKey();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                */
                    break;
                //Default runs if no values matched, aka "Else"
                default:
                    ClearHighlighted();
                    break;
            }
        }
    }
}
