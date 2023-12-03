using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Camera mainCamera;


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
    float runMultiplier;
    float gravityValue = 9.8f;
    float verticalSpeed;
    Vector2 currentInput;
    Vector3 currentMovement;

    [Header("Audio")]
    AudioSource footstepAudioSource;



    [Header("Stamina")]
    [SerializeField, Range(1, 20)] float maxStamina = 15f;
    [SerializeField] AudioSource windedAudioSource;
    float currentStamina;



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

    [SerializeField] Image cursorUI;
    [SerializeField] Sprite normalCursor;
    [SerializeField] Sprite interactCursor;

    bool canMove = true;
    bool canSprint = true;
    bool canJump = true;
    bool canInteract = true;
      
    
    void Awake() {
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Start() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        currentStamina = maxStamina;
    }
    void Update() {
        if(canMove) {
            HandleMovement();
        }
        if(canJump) {
            HandleJump();
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

    void HandleJump() {
        if(controller.isGrounded && Input.GetKey(jumpKey)){
            Debug.Log("jump");
            currentMovement.y = jumpForce;
        }
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


    void HighlightObject(GameObject gameObject, bool uiEnabled) {
        if (lastHighlightedObject != gameObject) {
            ClearHighlighted();
            lastHighlightedObject = gameObject;
            if(uiEnabled) {
                cursorUI.sprite = interactCursor;
            }
        }
    } 

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null) {
            //lastHighlightedObject.GetComponent<MeshRenderer>().material = originalMat;
            lastHighlightedObject = null;
            cursorUI.sprite = normalCursor;
        }
    } 
    
    void HandleInteraction() {
        float rayDistance = 50f;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Ray from center of the viewport
        RaycastHit rayHit;

        // Check if we hit something
        if (Physics.Raycast(ray, out rayHit, rayDistance)) {
            GameObject hitObj = rayHit.collider.gameObject;  // Get object that was hit

            if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 4f) {
                HighlightObject(hitObj, true);
                if(Input.GetKeyDown(KeyCode.E)) {
                    switch(hitObj.GetComponent<Collider>().gameObject.tag) {
                        case "Escape":
                            break;
                        case "JournalNote":
                            //hitObj.GetComponent<Collider>().gameObject.GetComponent<Interactable>().EnableJournalNote();
                            break;
                        case "HiddenItem":
                            break;
                        default:
                            ClearHighlighted();
                            break;
                    }
                }
            }
            else {
                ClearHighlighted();
            }
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

    /*
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
    */
}