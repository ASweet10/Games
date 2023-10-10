using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    [SerializeField] AudioClip walkSFX;
    [SerializeField] AudioClip runSFX;

    public bool canMove { get; private set; }
    [Header("Movement Parameters")]
    [SerializeField, Range(1, 10)] float walkSpeed = 3f;
    [SerializeField, Range(1, 20)] float sprintSpeed = 7f;
    [SerializeField] float gravityValue = 9.8f;
    Vector3 moveDirection;
    Vector2 currentInput;
    float rotationX = 0;
    float currentSpeed;


    [Header("Stamina")]
    [SerializeField, Range(1, 20)] float maxStamina = 15f;
    [SerializeField] AudioSource windedAudioSource;
    float currentStamina;
    
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource footstepAudioSource;
    /*
    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.1f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField, Range(1, 5)] private float crouchSpeed = 2.5f;
    private bool isCrouching;
    private bool duringCrouchAnimation;
    private bool canCrouch = true;
    */

    void Awake() {
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
    }
    void Start() {
        currentStamina = maxStamina;
    }
    void Update() {
        if(canMove) {
            HandleMovementInput();

            ApplyFinalMovements();
        }
    }   

    void HandleMovementInput() {
        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + transform.TransformDirection(Vector3.right * currentInput.y);
        moveDirection.y = moveDirectionY;

        //If player moving
        if(currentInput.x != 0 || currentInput.y != 0) {
            HandleFootstepAudio();
            HandleStamina();
        }
    }

    void HandleFootstepAudio() {        
        if(!footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
    }

    void ApplyFinalMovements(){
        if(!controller.isGrounded){
            moveDirection.y -= gravityValue * Time.deltaTime;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    void HandleStamina()
    {
        if(currentSpeed > walkSpeed) {  // Sprinting, lose stamina
            currentStamina -= 1 * Time.deltaTime;
        }
        else {                          // Walking, repair stamina
            currentStamina += 1.5f * Time.deltaTime;
        }


        if(currentStamina <= 0) {
            windedAudioSource.Play();
            //canRun = false;
        }
        else if(currentStamina >= maxStamina) {
            currentStamina = maxStamina;
            //canRun = true;
        }
    }

    public void PauseFootstepAudio() {
        footstepAudioSource.Pause();
    }
    public void ToggleMovement(bool choice) {
        canMove = choice;
    }

    //Not used in this game
    /*
    public void AttemptToCrouch()
    {
        if(!duringCrouchAnimation && controller.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(CrouchOrStand());
            }
        }
    }
    private IEnumerator CrouchOrStand()
    {
        duringCrouchAnimation = true;

        float timeElapsed = 0f;
        float currentHeight = controller.height;
        float targetHeight;
        if(isCrouching)
        {
            targetHeight = standingHeight;
            isCrouching = false;
        }
        else
        {
            targetHeight = crouchHeight;
            isCrouching = true;
        }

        while(timeElapsed > timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;

        duringCrouchAnimation = false;
    }
    */
}


