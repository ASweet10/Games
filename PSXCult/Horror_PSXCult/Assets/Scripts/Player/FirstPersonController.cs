using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Camera mainCamera;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] FlashlightToggle flashlight;
    Interactables interactables;

    bool isSprinting => canSprint && Input.GetKey(sprintKey);
    bool isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    bool shouldJump => controller.isGrounded && Input.GetKey(jumpKey);


    [Header("Controls")]
    KeyCode interactKey = KeyCode.E;
    KeyCode sprintKey = KeyCode.LeftShift;
    KeyCode jumpKey = KeyCode.Space;


    [Header("Movement")]
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 20f;    
    [SerializeField] AudioClip walkAudioClip;
    [SerializeField] AudioClip runAudioClip;
    float gravityValue = 9.8f;
    Vector2 currentInput;
    Vector3 currentMovement;
    AudioSource footstepAudioSource;


    [Header("Health & Stamina")]
    [SerializeField, Range(1, 20)] float maxStamina = 15f;
    [SerializeField] AudioSource windedAudioSource;
    [SerializeField] GameObject bleedingUI;
    [SerializeField] RawImage bloodParticles;
    [SerializeField] RawImage bloodTint;
    float currentStamina;
    float lastStabbedTime;
    int maxHealth = 3;
    int currentHealth;
    bool canTakeDamage = true;
    bool playerHasTakenDamage = false;

    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;

    
    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standCenter = new Vector3(0, 0, 0);
    //[SerializeField] private Vector3 cameraCrouchPosition = new Vector3(0, 0.5f, 0);
    //[SerializeField] private Vector3 cameraStandPosition = new Vector3(0, 0, 0);
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField, Range(1, 5)] private float crouchSpeed = 2.5f;
    [SerializeField] GameObject cameraHolder;
    private bool isCrouching;
    private bool duringCrouchAnimation;
    private bool canCrouch = true;
    

    [Header("Highlights")]
    GameObject lastHighlightedObject;
    [SerializeField] GameObject cursor;
    [SerializeField] TMP_Text interactText; // Text displayed on hover
    [SerializeField] TMP_Text popupText; // Popup text after interaction

    [Header("Interact Texts")]
    [SerializeField] string trashString = "It smells awful...";
    [SerializeField] string myCarString = "My car. A old piece of shit but reliable";
    GameController gameController;
    GameEvents gameEvents;
    DialogueManager dialogueManager;
    bool canSprint = true;
    bool canMove = true;
    public bool CanMove {
        get { return canMove; }
        set { canMove = value; }
    }
    bool canInteract = true;
    public bool CanInteract {
        get { return canInteract; }
        set { canInteract = value;}
    } 
    
    void Awake() {
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dialogueManager = gameObject.GetComponent<DialogueManager>();
        interactables = GameObject.FindGameObjectWithTag("GameController").GetComponent<Interactables>();
        gameEvents = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEvents>(); 
    }

    void Start() {
        //DisablePlayerMovement(false);
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }
    void Update() {
        if(canMove) {
            HandleMovement();
        }
        if(canCrouch) {
            AttemptToCrouch();
        }
        if(canInteract) {
            HandleInteraction();
        }

        ApplyFinalMovement();
        if(isMoving) {
            HandleHeadBobEffect();
           if(!footstepAudioSource.isPlaying) {
            if(isSprinting) {
                footstepAudioSource.clip = runAudioClip;
            } else {
                footstepAudioSource.clip = walkAudioClip;
            }
            footstepAudioSource.Play();
           }
        } else {
            footstepAudioSource.Stop();
        }
        HandleStamina();
        HandleFadeOutBleedEffect();
    }
    void HandleMovement() {
        currentInput.x = Input.GetAxis("Vertical") * (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed :  walkSpeed);
        currentInput.y = Input.GetAxis("Horizontal") * (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed);

        float currentMovementY = currentMovement.y;
        currentMovement = (transform.forward * currentInput.x) + (transform.right * currentInput.y);
        currentMovement.y = currentMovementY;
    }

    void ApplyFinalMovement() {
        if(!controller.isGrounded){
            currentMovement.y -= gravityValue * Time.deltaTime; // Apply gravity
            if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                currentMovement.y = 0;
            }
        }
        controller.Move(currentMovement * Time.deltaTime);
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
                if(hitObj.tag != "Untagged") {
                    cursor.SetActive(false);
                    interactText.text = hitObj.tag;
                } else {
                    interactText.text = "";
                }
            }
        }
    }
    void ClearHighlighted() {
        if (lastHighlightedObject != null) {
            //lastHighlightedObject.GetComponent<MeshRenderer>().material = originalMat;
            lastHighlightedObject = null;
            cursor.SetActive(true);
            interactText.enabled = false;
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
                    switch(hitObj.GetComponent<Collider>().gameObject.tag) {
                        case "Door":
                            interactables.HandleGasStationDoor();
                            break;
                        case "MissingPosterOne":
                            interactables.ToggleMissingUI(1, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPosterTwo":
                            interactables.ToggleMissingUI(2, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPosterThree":
                            interactables.ToggleMissingUI(3, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPosterFour":
                            interactables.ToggleMissingUI(4, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingNewsArticle":
                            interactables.ToggleMissingUI(5, true);
                            DisablePlayerMovement(true);
                            break;
                        case "Drinks":
                            interactables.ToggleDrinksUI(true);
                            DisablePlayerMovement(true);
                            break;
                        case "Trash":
                            StartCoroutine(DisplayPopupText(trashString));
                            break;
                        case "My Car":
                            if(gameController.hasPurchasedGas) {
                                if(gameController.playerAtPark) {
                                    // Allow player to leave right away? ending 1
                                    StartCoroutine(DisplayPopupText(trashString));
                                } else {
                                    gameEvents.StartDriveToParkCutscene();
                                }
                            }
                            StartCoroutine(DisplayPopupText(myCarString));
                            break;
                        case "Arcade":
                            StartCoroutine(interactables.ToggleArcade(true));
                            interactables.PlayingArcadeGame = true;
                            DisablePlayerMovement(true);
                            break;
                        case "Firewood":
                            if(gameController.playerNeedsFirewood) {
                                gameEvents.HandleCollectFirewood();
                                hitObj.SetActive(false);
                            }
                            break;
                        case "Head To Park":
                            StartCoroutine(gameController.HandleDriveToParkCutscene());
                            break;
                        case "Escape":
                            StartCoroutine(gameController.HandleEscapeCutscene());
                            break;
                        case "HiddenItem":
                            break;
                        case "CarKeys":
                            break;
                        case "Cashier":
                            var cashierTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                            cashierTrigger.TriggerDialogue();
                            var cashierCharacter = hitObj.GetComponent<AICharacter>();
                            cashierCharacter.RotateAndStartTalking();
                            Debug.Log("cashier");
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

    IEnumerator DisplayPopupText(string displayText) {
        popupText.text = displayText;
        yield return new WaitForSeconds(3f);
        popupText.text = "";
    }
    
    public void AttemptToCrouch() {
        if(!duringCrouchAnimation && controller.isGrounded) {
            if(Input.GetKeyDown(KeyCode.C)) {
                StartCoroutine(CrouchOrStand());
            }
        }
    }
    private IEnumerator CrouchOrStand() {
        duringCrouchAnimation = true;

        float timeElapsed = 0f;

        // Change height
        float targetHeight = isCrouching ? standHeight : crouchHeight;
        float currentHeight = controller.height;
        // Change center so you don't fall through floor
        Vector3 targetCenter = isCrouching ? standCenter : crouchCenter;
        Vector3 currentCenter = controller.center;

        while(timeElapsed < timeToCrouch) {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;
        controller.center = targetCenter;
        isCrouching = !isCrouching;
        duringCrouchAnimation = false;
    }

    public void TakeDamage() {
        if(canTakeDamage) {
            StartCoroutine(TakeDamageAndWait());
        }
    }
    IEnumerator TakeDamageAndWait() {
        canTakeDamage = false;
        playerHasTakenDamage = true;
        lastStabbedTime = Time.time;
        currentHealth --;
        Debug.Log("health: " + currentHealth);
        switch(currentHealth) {
            case 2:
                bleedingUI.SetActive(true);
                float alpha2 = 0.2f;
                Color particleColor = bloodParticles.color;
                particleColor.a = alpha2;
                bloodParticles.color = particleColor;
                break;
            case 1:
                bleedingUI.SetActive(true);
                float alpha1 = 1f;
                particleColor = bloodParticles.color;
                particleColor.a = alpha1;
                bloodParticles.color = particleColor;
                // heavy breathing audio? 
                break;
            case int currentHealth when currentHealth <= 0:
                bleedingUI.SetActive(false);
                StartCoroutine(gameController.HandlePlayerDeath());
                break;
        }
        yield return new WaitForSeconds(2);
        playerHasTakenDamage = false;
        canTakeDamage = true;
    }

    void HandleFadeOutBleedEffect() {
        if(bleedingUI.activeInHierarchy && Time.time - lastStabbedTime > 3f) {
            while (bloodParticles.color.a > 0) {
                bloodParticles.color = new Color(bloodParticles.color.r, bloodParticles.color.g, bloodParticles.color.b, bloodParticles.color.a - (Time.deltaTime / 6));
                if(playerHasTakenDamage) {
                    break;
                }
            }
            if(!playerHasTakenDamage) {
                bleedingUI.SetActive(false);
            }
        }
    }

    void HandleStamina() {
        if(currentStamina <= 0) {
            currentStamina = 0;
            windedAudioSource.Play();
            canSprint = false;
        }
        else {
            if(canSprint) {
                if(isSprinting) {
                    currentStamina -= 1f * Time.deltaTime;
                } else {
                    if(currentStamina < maxStamina) {
                        currentStamina += 0.75f * Time.deltaTime;
                    } else {
                        currentStamina = maxStamina;
                        canSprint = true;
                    }
                }
            } else {
                if(currentStamina < maxStamina) {
                    currentStamina += 0.75f * Time.deltaTime;
                } else {
                    currentStamina = maxStamina;
                    canSprint = true;
                }
            }
        }
        Debug.Log(currentStamina);
    }

    void HandleHeadBobEffect() {

    }

    public void DisablePlayerMovement(bool disableMovement) {
        if (disableMovement) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            canMove = false;
            canInteract = false;
            mouseLook.CanRotateMouse = false;   
            currentMovement = Vector3.zero;
            flashlight.ToggleFlashlightStatus(false);
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            canMove = true;
            canInteract = true;
            mouseLook.CanRotateMouse = true;
            flashlight.ToggleFlashlightStatus(true);
        }
    }
}