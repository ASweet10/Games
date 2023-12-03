using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControllerTwo : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] Camera mainCamera;

    bool isSprinting => canSprint && Input.GetKey(sprintKey);

    [Header("Controls")]
    KeyCode interactKey = KeyCode.E;
    KeyCode sprintKey = KeyCode.LeftShift;
    KeyCode jumpKey = KeyCode.Space;
    KeyCode crouchKey = KeyCode.C;


    [Header("Movement Parameters")]
    [SerializeField] float walkSpeed = 15f;
    [SerializeField] float sprintSpeed = 30f;
    float runMultiplier;
    float gravityValue = 9.8f;
    float verticalSpeed;

    Vector2 currentInput;
    Vector3 currentMovement;

    [Header("Interactions")]
    [SerializeField] Vector3 interactionRaypoint = default;
    [SerializeField] float interactionRange = 50f;
    [SerializeField] LayerMask interactionLayer = default;
    Interactable currentInteractable;

    [Header("Jump Parameters")]
    [SerializeField] float jumpForce = 10f;

    [Header("Crouch Parameters")]
    [SerializeField] float crouchHeight = 0.5f;
    [SerializeField] float standingHeight = 2.1f;
    [SerializeField] float timeToCrouch = 0.25f;
    [SerializeField, Range(1, 5)] private float crouchSpeed = 2.5f;
    Vector3 standingCenter = new Vector3(0, 0, 0);
    Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    bool isCrouching;
    bool duringCrouchAnimation;

    bool canCrouch = true;
    bool canMove = true;
    bool canSprint = true;
    bool canJump = true;
    bool canInteract = true;

    void Awake()
    {
        //footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {
        if(canMove){
            HandleMovement();
        }
        if(canJump){
            HandleJump();
        }
        if(canInteract){
            HandleInteractionCheck();
            HandleInteractionInput();
        }
        if(canCrouch){
            HandleCrouch();
        }
        ApplyFinalMovement();
    }
    void HandleMovement() {
        currentInput.x = Input.GetAxis("Vertical") * (isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed);
        currentInput.y = Input.GetAxis("Horizontal") * (isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed);

        float currentMovementY = currentMovement.y;
        currentMovement = (transform.forward * currentInput.x) + (transform.right * currentInput.y);
        currentMovement.y = currentMovementY;
    }

    void HandleJump() {
        if(controller.isGrounded && Input.GetKey(jumpKey)){
            currentMovement.y = jumpForce;
        }
    }

    void HandleInteractionCheck() {
        // Ray from the center of the viewport.
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if(Physics.Raycast(ray, out RaycastHit hit, interactionRange)){
            //If interactable exists
            // Also, check new object is different from previous (In case they are close together)
            if(hit.collider.gameObject.layer == 9){
                if(currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()){
                    hit.collider.TryGetComponent<Interactable>(out currentInteractable);

                    if(currentInteractable){ //If currentInteractable exists
                        currentInteractable.OnFocus();
                    }
                }
            }
        }
        else if(currentInteractable){
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }
    void HandleInteractionInput() {
        if(Input.GetKeyDown(interactKey) && (currentInteractable != null)) {
            if(Physics.Raycast(mainCamera.ViewportPointToRay(interactionRaypoint), out RaycastHit hit, interactionRange, interactionLayer)){
                currentInteractable.OnInteract();
            }
        }
    }

    void ApplyFinalMovement(){
        if(!controller.isGrounded){
            //Apply gravity
            currentMovement.y -= gravityValue * Time.deltaTime;
            //Landing frame; reset y value to 0
            if(controller.velocity.y < -1 && controller.isGrounded){
                currentMovement.y = 0;
            }
        }
        controller.Move(currentMovement * Time.deltaTime);
    }

    public void HandleCrouch()
    {
        if(!duringCrouchAnimation && controller.isGrounded)
        {
            if(Input.GetKeyDown(crouchKey))
            {
                StartCoroutine(CrouchOrStand());
            }
        }
    }
    private IEnumerator CrouchOrStand()
    {
        //If crouching & object above
        if(isCrouching && Physics.Raycast(mainCamera.transform.position, Vector3.up, 1f)){
            yield break;
        }

        duringCrouchAnimation = true;

        float timeElapsed = 0f;
        float currentHeight = controller.height;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = controller.center;

        while(timeElapsed < timeToCrouch)
        {
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
}
