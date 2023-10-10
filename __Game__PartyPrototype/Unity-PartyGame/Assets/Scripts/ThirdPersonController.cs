using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    float runMultiplier;
    PlayerInput playerInput;
    CharacterController characterController;
    Animator anim;

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    float rotationFactorPerFrame = 10.0f;
    bool isMovementPressed;
    bool isRunPressed;

    private void Awake() {
        runMultiplier = moveSpeed * 3.0f;
        playerInput = new PlayerInput();
        characterController = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        //Started: When input begins
        playerInput.CharacterControls.Move.started += HandleMovementInput;
        //Canceled: When input ends / key let go
        playerInput.CharacterControls.Move.canceled += HandleMovementInput;
        //Performed: Shows change in input for values between 0 and 1 (controller joystick)
        playerInput.CharacterControls.Move.performed += HandleMovementInput;

        playerInput.CharacterControls.Run.started += HandleRun;
        playerInput.CharacterControls.Run.canceled += HandleRun;
    }

    private void Update() {
        HandleAnimation();
        HandleRotation();
        if(isRunPressed){
            characterController.Move(currentRunMovement * Time.deltaTime);
        } else {
            characterController.Move(currentMovement * Time.deltaTime);
        }

    }

    private void HandleMovementInput(InputAction.CallbackContext context) {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * moveSpeed;
        currentMovement.z = currentMovementInput.y * moveSpeed;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    private void HandleRun(InputAction.CallbackContext context) {
        isRunPressed = context.ReadValueAsButton();
    }

    private void HandleAnimation() {
        bool isWalking = anim.GetBool(isWalkingHash);
        bool isRunning = anim.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking) {
            anim.SetBool(isWalkingHash, true);
        }
        if(!isMovementPressed && isWalking) {
            anim.SetBool(isWalkingHash, false);
        }

        //Run if movement and run both pressed, and not already running
        if((isMovementPressed && isRunPressed) && !isRunning) {
            anim.SetBool(isRunningHash, true);
        } else if((!isMovementPressed || !isRunPressed) && isRunning) {
            anim.SetBool(isRunningHash, false);
        }
    }

    private void HandleRotation() {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        //Current rotation of the character
        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed){
            //Creates new rotation based on where player is pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
        }

    }
    
    private void HandleGravity() {
        if(characterController.isGrounded){
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        } else {
            float gravity = -9.8f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }

    private void OnEnable() {
        playerInput.CharacterControls.Enable();
    }
    private void OnDisable() {
        playerInput.CharacterControls.Disable();
    }
}
