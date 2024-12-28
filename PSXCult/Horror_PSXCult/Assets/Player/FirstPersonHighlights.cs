using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonHighlights : MonoBehaviour
{
    KeyCode interactKey = KeyCode.E;
    GameEvents gameEvents;
    GameController gameController;
    Interactables interactables;
    [SerializeField] PickUpObjects pickupObjects;
    FirstPersonController fpController;

    [SerializeField] Camera mainCamera;
    public bool CanInteract;
    
    [Header("Highlights")]
    GameObject lastHighlightedObject;
    [SerializeField] Sprite normalCursor;
    [SerializeField] Sprite handCursor;
    [SerializeField] Image cursorImage;
    [SerializeField] TMP_Text interactText; // Text displayed on hover

    [Header("Interact Texts")]
    [SerializeField] string trashString = "It smells awful...";
    [SerializeField] string myCarString = "My car. An old piece of shit but it's reliable";
    [SerializeField] string davidCarString = "David's new wheels. I'm sure he'll brag about it";
    [SerializeField] string needsZippoAndLighterFluidString = "I'm gonna need lighter fluid and a lighter";
    [SerializeField] string needsZippoString = "I still need a source of fire...";
    [SerializeField] string needsLighterFluidString = "I still need lighter fluid...";    
    
    void Awake() {
        gameEvents = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEvents>(); 
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); 
        interactables = GameObject.FindGameObjectWithTag("GameController").GetComponent<Interactables>();
        fpController = gameObject.GetComponent<FirstPersonController>();
    }

    void Update() {
        //if(CanInteract) { HandleInteraction(); }
    }
    void DetermineInteractionType(GameObject hitObj) {
        switch(hitObj.GetComponent<Collider>().gameObject.tag) {
            case "Door":
                interactables.HandleGasStationDoor();
                break;
            case "MissingPoster":
                fpController.DisablePlayerMovement(true, false);
                interactables.ToggleMissingUI(hitObj.gameObject.name, true);
                break;
            case "Newspaper":
                fpController.DisablePlayerMovement(true, false);
                interactables.ToggleMissingUI(hitObj.gameObject.name, true);
                break;
            case "Car Note":
                interactables.ToggleMissingUI(hitObj.gameObject.name, true);
                gameController.playerHasReadCarNote = true;
                gameController.HandleNextObjective();
                fpController.DisablePlayerMovement(true, false);
                break;
            case "Drinks":
                interactables.ToggleDrinksUI(true);
                fpController.DisablePlayerMovement(true, true);
                break;
            case "Pickup":
                //pickupObjects.HandlePickUpObject(hitObj);
                break;
            case "Trash":
                StartCoroutine(gameController.DisplayPopupMessage(trashString));
                break;
            case "My Car":
                if(gameController.hasPurchasedGas) {
                    if(gameController.playerAtPark) {
                        // Allow player to leave right away? ending 1
                    } else {
                        gameEvents.StartDriveToParkCutscene();
                    }
                }
                StartCoroutine(gameController.DisplayPopupMessage(myCarString));
                break;
            case "David's Car":
                StartCoroutine(gameController.DisplayPopupMessage(davidCarString));
                break;
            case "Red Herring":
                StartCoroutine(interactables.ToggleArcade(true));
                interactables.playingArcadeGame = true;
                fpController.DisablePlayerMovement(true, false);
                break;
            case "Firewood":
                gameEvents.HandleCollectFirewood();
                hitObj.SetActive(false);
                break;
            case "Zippo":
                gameEvents.HandleCollectZippo();
                hitObj.SetActive(false);
                break;
            case "Lighter Fluid":
                gameEvents.HandleCollectLighterFluid();
                hitObj.SetActive(false);
                break;
            case "Start Fire":
                if(!gameController.hasLighterFluid) {
                    if(!gameController.hasZippo) {
                        StartCoroutine(gameController.DisplayPopupMessage(needsZippoAndLighterFluidString));
                    } else {
                        StartCoroutine(gameController.DisplayPopupMessage(needsLighterFluidString));
                    }
                } else if (!gameController.hasZippo) {
                    StartCoroutine(gameController.DisplayPopupMessage(needsZippoString));
                } else {
                    StartCoroutine(gameEvents.StartCampFire());
                }
                break;
            case "Build Campfire":
                gameEvents.HandleBuildFire();
                break;
            case "Head To Park":
                StartCoroutine(gameController.HandleDriveToParkCutscene());
                break;
            case "HiddenItem":
                break;
            case "CarKeys":
                gameEvents.HandleCollectCarKeys();
                hitObj.SetActive(false);
                break;
            case "Cashier":
                var cashierTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                cashierTrigger.TriggerDialogue();
                var cashierCharacter = hitObj.GetComponent<AICharacter>();
                cashierCharacter.RotateAndStartTalking();
                break;
            case "AJ":
                var ajTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                ajTrigger.TriggerDialogue();
                var ajCharacter = hitObj.GetComponent<AICharacter>();
                ajCharacter.RotateAndStartTalking();
                break;
            case "David":
                var davidTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                davidTrigger.TriggerDialogue();
                var davidCharacter = hitObj.GetComponent<AICharacter>();
                davidCharacter.RotateAndStartTalking();
                break;
            case "Hunter":
                var hunterTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                hunterTrigger.TriggerDialogue();
                var hunterCharacter = hitObj.GetComponent<AICharacter>();
                hunterCharacter.RotateAndStartTalking();
                break;
            default:
                ClearHighlighted();
                break;
        }
    }
    void HandleInteraction() {
        float rayDistance = 50f;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Ray from center of the viewport
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, rayDistance)) {
            GameObject hitObj = rayHit.collider.gameObject;  // Get object that was hit
            if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 6f) {
                HighlightObject(hitObj, true);
                if(Input.GetKeyDown(interactKey)) {
                    DetermineInteractionType(hitObj);
                }
            } else {
                ClearHighlighted();
                if(hitObj.tag != "Untagged") {
                    var outline = hitObj.GetComponent<Outline>();
                    if(outline != null) {
                        outline.enabled = false;
                    }

                }
            }
        }
    }
    public void ClearHighlighted() {
        if (lastHighlightedObject != null) {
            //lastHighlightedObject.GetComponent<MeshRenderer>().material = originalMat;
            lastHighlightedObject = null;
            cursorImage.enabled = true;
            cursorImage.sprite = normalCursor;
            interactText.enabled = false;
        }
    }
    void HighlightObject(GameObject hitObj, bool uiEnabled) {
        if (lastHighlightedObject != hitObj) {
            ClearHighlighted();
            lastHighlightedObject = hitObj;
            var outline = hitObj.GetComponentInChildren<Outline>();
            if(outline != null) {
                outline.enabled = true;
            }

            if(uiEnabled) {
                interactText.enabled = true;
                if(hitObj.tag == "Untagged" || hitObj.tag == "Player" || hitObj.tag == "Tile") {
                    cursorImage.enabled = false;
                    interactText.text = "";
                } else if(hitObj.tag == "Pickup") {
                    cursorImage.enabled = true;
                    cursorImage.sprite = handCursor;
                }
                else {
                    cursorImage.enabled = false;
                    interactText.text = hitObj.tag;
                }
            }
        }
    }
}