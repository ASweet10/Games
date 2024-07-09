using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Camera mainCamera;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Interactables interactables;


    /* Head Bob Effect */
    [SerializeField] Animation anim;
    bool left = true;
    bool right = false;


    bool isSprinting => canSprint && Input.GetKey(sprintKey);
    bool isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    bool shouldJump => controller.isGrounded && Input.GetKey(jumpKey);


    [Header("Controls")]
    KeyCode interactKey = KeyCode.E;
    KeyCode sprintKey = KeyCode.LeftShift;
    KeyCode jumpKey = KeyCode.Space;


    [Header("Movement")]
    [SerializeField, Range(3, 5)] float walkSpeed = 5f;
    [SerializeField, Range(6, 10)] float sprintSpeed = 10f;
    float gravityValue = 9.8f;
    Vector2 currentInput;
    Vector3 currentMovement;

    [Header("Audio")]
    AudioSource footstepAudioSource;


    [Header("Health & Stamina")]
    [SerializeField, Range(1, 20)] float maxStamina = 15f;
    [SerializeField] AudioSource windedAudioSource;
    [SerializeField] GameObject bleedingUI;
    [SerializeField] RawImage bloodParticles;
    [SerializeField] RawImage bloodTint;
    float currentStamina;
    int maxHealth = 3;
    int currentHealth;

    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;

    
    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.1f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField, Range(1, 5)] private float crouchSpeed = 2.5f;
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

    GameController gameController;
    GameEvents gameEvents;
    DialogueManager dialogueManager;
    bool playerHoldingGasStationItem = false;
    public bool playerHoldingGasItem {
        get { return playerHoldingGasStationItem; }
        set { playerHoldingGasStationItem = value; }
    }
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
           //HandleHeadBobEffect();
        }
    }
    void HandleMovement() {
        currentInput.x = Input.GetAxis("Vertical") * (isSprinting ? sprintSpeed : walkSpeed);
        currentInput.y = Input.GetAxis("Horizontal") * (isSprinting ? sprintSpeed : walkSpeed);

        float currentMovementY = currentMovement.y;
        currentMovement = (transform.forward * currentInput.x) + (transform.right * currentInput.y);
        currentMovement.y = currentMovementY;

        //HandleStamina();
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
                    interactText.text = "[ " + hitObj.tag + " ]";
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
                if(Input.GetKeyDown(KeyCode.E)) {
                    switch(hitObj.GetComponent<Collider>().gameObject.tag) {
                        case "Escape":
                            gameController.EscapeAndWin();
                            break;
                        case "JournalNote":
                            //hitObj.GetComponent<Collider>().gameObject.GetComponent<Interactable>().EnableJournalNote();
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
                        case "Arcade":
                            interactables.ToggleArcade(true);
                            interactables.PlayingArcadeGame = true;
                            DisablePlayerMovement(true);
                            break;
                        case "Firewood":
                            if(gameEvents.NeedsFirewood) {
                                gameEvents.HandleCollectFirewood();
                                hitObj.SetActive(false);
                            }
                            break;
                        case "HiddenItem":
                            break;
                        case "CarKeys":
                            break;
                        case "Cashier":
                            var cashierTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                            cashierTrigger.TriggerDialogue();
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
    public void DisablePlayerMovement(bool disableMovement) {
        if (disableMovement) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            canMove = false;
            canInteract = false;
            mouseLook.CanRotateMouse = false;   
            currentMovement = Vector3.zero;
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            canMove = true;
            canInteract = true;
            mouseLook.CanRotateMouse = true;
        }
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
        float currentHeight = controller.height;
        float targetHeight;
        if(isCrouching) {
            targetHeight = standingHeight;
            isCrouching = false;
        }
        else {
            targetHeight = crouchHeight;
            isCrouching = true;
        }

        while(timeElapsed > timeToCrouch) {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;
        duringCrouchAnimation = false;
    }

    public void TakeDamage() {
        currentHealth --;
        switch(currentHealth) {
            case 2:
                bleedingUI.SetActive(true);
                float alpha2 = 0.5f;
                Color particleColor = bloodParticles.color;
                particleColor.a = alpha2;
                bloodParticles.color = particleColor;

                Color bloodTintColor = bloodTint.color;
                bloodTintColor.a = alpha2;
                bloodTint.color = bloodTintColor;
                break;
            case 1:
                bleedingUI.SetActive(true);
                float alpha1 = 1f;
                particleColor = bloodParticles.color;
                particleColor.a = alpha1;
                bloodParticles.color = particleColor;

                bloodTintColor = bloodTint.color;
                bloodTintColor.a = alpha1;
                bloodTint.color = bloodTintColor;
                // heavy breathing audio? 
                break;
            case 0:
                bleedingUI.SetActive(false);
                gameController.HandlePlayerDeath();
                break;
        }
    }

    void HandleStamina() {
        if(currentStamina <= 0) {
            windedAudioSource.Play();
            canSprint = false;
        }
        else {
            if(isSprinting) {
                currentStamina --;
            } else {
                currentStamina += 0.75f * Time.deltaTime;
            }
        }
        Debug.Log(currentStamina);
    }
    void HandleHeadBobEffect() {
        if(controller.isGrounded) {
            if(left == true) {
                if(!anim.isPlaying) {
                    anim.Play("WalkLeft");
                    left = false;
                    right = true;
                }
            }
            if(right == true) {
                if(!anim.isPlaying)
                {
                    anim.Play("WalkRight");
                    right = false;
                    left = true;
                }
            }
        }
    }
}