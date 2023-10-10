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
    [SerializeField] Image sinkProgressImage;
    [SerializeField] Image lightProgressImage;
    Camera mainCamera;
    [SerializeField] GameController gameController;
    string noKeyFoundString = "It's locked but I see a key hole...";

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
                case "Lightswitch":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        //If power on / fuse not blown...
                        if(!gameController.fuseLightsOff)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<Lightswitch>().UseLightSwitch();
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;

                case "LockedDoor":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().InteractWithOnlyAudio();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
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
                case "KnockOnDoor":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().InteractWithOnlyAudio();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "Snacks":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        //If power on / fuse not blown...
                        if(!gameController.fuseLightsOff)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().InteractWithOnlyAudio();
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "ToolBox":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        if(gameController.currentCheckpoint == 1)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpToolBox();
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "ApartmentTrash":
                    if(gameController.currentCheckpoint == 3)
                    {
                        if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpApartmentTrash();
                            }
                        }
                        else
                        {
                            ClearHighlighted();
                        }
                    }
                    break;         
                case "GarbageChute":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 5f)
                    {
                        if(gameController.holdingTrash)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().InteractWithGarbageChute();
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "MainCar":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<CarInteractions>().AttemptToUseCar();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "Fuse":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        if(gameController.fuseLightsOff)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpFuse();
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "FuseBox":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        //Lights have gone out and player found fuse...
                        if(gameController.playerHasFuse)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().FixFuseBox();
                            }
                        }
                        /*
                        //All tasks done + fuse box has been fixed..
                        else if(gameController.currentCheckpoint == 7 && gameController.fuseBoxFixed)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().TurnFuseBoxOff();
                            }
                        }
                        */
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "Locker":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().InteractWithLocker();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "MainPaper":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpMainPaper();
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
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().EscapeAndWin();
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
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().EnableJournalNote();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "HiddenKey":
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
                    break;
                case "LockedDrawer":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            if(!gameController.playerNeedsKey)
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().InteractWithDrawer();
                            }
                            else
                            {
                                gameController.ShowPopupMessage(noKeyFoundString, 2, false);
                            }
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;
                case "LaundryWindow":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 5f)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<BasicInteractables>().InteractWithLaundryWindow();
                        }
                    }
                    else
                    {
                        ClearHighlighted();
                    }
                    break;

                case "SinkLeak":
                    if(gameController.currentCheckpoint == 6)
                    {
                        var sinkProgress = hitObj.gameObject.GetComponent<ProgressBar>();
                        if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKey(KeyCode.E))
                            {
                                sinkProgressImage.enabled = true;
                                sinkProgress.UpdateSinkFill();
                            }
                            else
                            {
                                sinkProgressImage.enabled = false;
                                toolAudioSource.Pause();
                            }
                        }
                        else
                        {
                            toolAudioSource.Pause();
                            sinkProgressImage.enabled = false;
                            ClearHighlighted();
                        }
                        break;
                    }
                    break;
                case "FixableLight":
                    if(gameController.currentCheckpoint == 0)
                    {
                        var lightProgress = hitObj.gameObject.GetComponent<ProgressBar>();
                        if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 5f)
                        {
                            Debug.Log("LIGHT");
                            HighlightObject(hitObj, true);
                            if(Input.GetKey(KeyCode.E))
                            {
                                lightProgressImage.enabled = true;
                                lightProgress.UpdateLightFill();
                            }
                            else
                            {
                                lightProgressImage.enabled = false;
                                toolAudioSource.Pause();
                            }
                        }
                        else
                        {
                            toolAudioSource.Pause();
                            lightProgressImage.enabled = false;
                            ClearHighlighted();
                        }
                        break;
                    }
                    break;
                    /*                      
                    }
                    else
                    {
                        toolAudioObj.SetActive(false);
                        break;
                    }
                    */                      

                case "StepLadder":
                    if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                    {
                        if(gameController.currentCheckpoint == 4)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKey(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpStepLadder();
                            }
                        }
                    }
                    break;

                //Default runs if no values matched, aka "Else"
                default:
                    sinkProgressImage.enabled = false;
                    /*
                    if(toolAudioObj.activeInHierarchy)
                    {
                        toolAudioSource.Pause();
                    }
                    */
                    toolAudioSource.Pause();
                    ClearHighlighted();
                    break;
            }
        }
    }
        /*

            if(hitObj.GetComponent<Collider>().gameObject.tag == "Lightswitch")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<Lightswitch>().UseLightSwitch();
                    }
                }
                else
                {
                    ClearHighlighted();
                }
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "LockedDoor")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<Triggers>().TriggerAudioOnly();
                    }
                }
                else
                {
                    ClearHighlighted();
                }
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "OpenableDoor")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        var doorScript = hitObj.GetComponent<Collider>().gameObject.GetComponent<DoorController>();
                        var doorClosed = doorScript.doorClosed;
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
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "KnockOnDoor")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<Triggers>().TriggerAudioOnly();
                    }
                }
                else
                {
                    ClearHighlighted();
                }
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "Snacks")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<Triggers>().TriggerAudioOnly();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "SinkLeak")
            {
                var progressRef = hitObj.gameObject.GetComponent<ProgressBar>();
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, false);
                    progressRef.canUpdateSink = true;
                    Debug.Log("true");
                }
                else
                {
                    progressRef.canUpdateSink = false;
                    Debug.Log("false");
                    ClearHighlighted();
                }                                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "FixableVendMachine")
            {
                var progressRef = hitObj.gameObject.GetComponent<ProgressBar>();
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, false);
                    progressRef.canUpdateVendMachine = true;
                }
                else
                {
                    progressRef.canUpdateVendMachine = false;
                    ClearHighlighted();
                }                                
            }    
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "LaundryWindow")
            {
                var progressRef = hitObj.gameObject.GetComponent<ProgressBar>();
                var triggerRef = hitObj.gameObject.GetComponent<Triggers>();
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 5f)
                {
                    if(triggerRef.canCloseWindow)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            triggerRef.CloseLaundryWindow();
                        }
                    }
                    else
                    {
                        progressRef.canUpdateWindow = true;
                        Debug.Log("window-true");
                    }
                }
                else
                {
                    progressRef.canUpdateWindow = false;
                    Debug.Log("window-false");
                    ClearHighlighted();
                }                                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "ToolBox")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    if(gameController.currentCheckpoint == 1 && !gameController.playerHasToolBox)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpToolBox();
                        }
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "PickUpBox")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    if(gameController.currentCheckpoint == 4)
                    {
                        if(!gameController.playerHasPickUpBox)
                        {
                            HighlightObject(hitObj, true);
                            if(Input.GetKeyDown(KeyCode.E))
                            {
                                hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpCardboardBox();
                            }
                        }
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "CoffeeTable")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    if(gameController.playerHasPickUpBox)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().InteractWithCoffeeTable();
                        }
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "MainCar")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<CarInteractions>().AttemptToUseCar();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "Fuse")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    if(gameController.currentCheckpoint == 4)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpFuse();
                        }
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "FuseBox")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    if(gameController.currentCheckpoint == 5 && gameController.playerHasFuse)
                    {
                        HighlightObject(hitObj, true);
                        if(Input.GetKeyDown(KeyCode.E))
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().InteractWithFuseBox();
                        }
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "Locker")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<Triggers>().InteractWithLocker();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "LockedDrawer")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        if(!gameController.playerNeedsKey)
                        {
                            hitObj.GetComponent<Collider>().gameObject.GetComponent<Triggers>().InteractWithDrawer();
                        }
                        else
                        {
                            StartCoroutine(gameController.ShowPopupMessage(noKeyFoundString, 2));
                        }
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "HiddenKey")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().TriggerFoundKey();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "ExpositionNote")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<Triggers>().InteractWithExpositionNote();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "EscapeLadder")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 5f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().TriggerEscape();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
            else if(hitObj.GetComponent<Collider>().gameObject.tag == "MainPaper")
            {
                if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 3f)
                {
                    HighlightObject(hitObj, true);
                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        hitObj.GetComponent<Collider>().gameObject.GetComponent<MainEvents>().PickUpMainPaper();
                    }
                }
                else
                {
                    ClearHighlighted();
                }                
            }
        }
        else
        {
            ClearHighlighted();
        }
        */
}
