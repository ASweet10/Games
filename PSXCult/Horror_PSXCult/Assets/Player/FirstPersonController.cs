using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour
{
    CharacterController controller;
    Transform tf;
    [SerializeField] Camera mainCamera;
    [SerializeField] MouseLook mouseLook;
    TerrainTexDetector terrainTexDetector;
    [SerializeField] FlashlightToggle flashlight;
    FirstPersonHighlights fpHighlights;


    bool isSprinting => canSprint && Input.GetKey(sprintKey);
    bool isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    bool shouldJump => controller.isGrounded && Input.GetKeyDown(jumpKey);
    bool shouldCrouch => !duringCrouchAnimation && controller.isGrounded && Input.GetKeyDown(KeyCode.C);
    bool IsSliding { 
        get {
            if(controller.isGrounded && Physics.Raycast(tf.position, Vector3.down, out RaycastHit slopeHit, 2f)) {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            } else {
                return false;
            }
        }
    }
    
    
    [Header("Controls")]
    KeyCode sprintKey = KeyCode.LeftShift;
    KeyCode jumpKey = KeyCode.Space;


    [Header("Movement")]
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 20f;    
    float gravityValue = 9.8f;
    Vector2 currentInput;
    Vector3 currentMovement;


    [Header("Footsteps")]
    AudioSource footstepAudioSource;
    [SerializeField] AudioClip[] grassClips;
    [SerializeField] AudioClip[] concreteClips;
    [SerializeField] AudioClip[] dirtClips;
    float baseStepSpeed = 0.5f;
    float crouchStepMultiplier = 1.5f;
    float sprintStepMultiplier = 0.6f;
    float footstepTimer = 0;
    float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;
    public int terrainDataIndex;



    [Header("Health & Stamina")]
    [SerializeField, Range(1, 20)] float maxStamina = 15f;
    [SerializeField] AudioSource windedAudioSource;
    [SerializeField] GameObject bleedingUI;
    [SerializeField] RawImage bloodParticles;
    [SerializeField] RawImage bloodTint;
    float currentStamina;
    float lastStabbedTime;
    float maxHealth = 15;
    float currentHealth;
    bool canTakeDamage = true;
    bool playerHasTakenDamage = false;
    bool canSprint = true;


    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;
    bool canJump = true;

    // Sliding
    Vector3 hitPointNormal; // Angle of floor
    float slopeSpeed = 8f;

    [Header("Crouch")]
    [SerializeField] float crouchHeight = 0.5f;
    [SerializeField] float standHeight = 2f;
    [SerializeField] Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] Vector3 standCenter = new Vector3(0, 0, 0);
    [SerializeField] float timeToCrouch = 0.25f;
    [SerializeField, Range(1, 5)] float crouchSpeed = 2.5f;
    bool isCrouching;
    bool duringCrouchAnimation;
    bool canCrouch = true;


    [Header("Headbob")]
    [SerializeField] float walkBobSpeed = 14f;
    [SerializeField] float walkBobAmount = 0.05f;
    [SerializeField] float crouchBobSpeed = 8f;
    [SerializeField] float crouchBobAmount = 0.025f;
    [SerializeField] float sprintBobSpeed = 18f;
    [SerializeField] float sprintBobAmount = 0.1f;
    float defaultYPosition = 0;
    float timer;


    GameController gameController;

    public bool CanMove { get; private set; } = true;

    
    void Awake() {
        fpHighlights = gameObject.GetComponent<FirstPersonHighlights>();
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
        terrainTexDetector = gameObject.GetComponent<TerrainTexDetector>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        defaultYPosition = mainCamera.transform.localPosition.y; // Return camera to default position when not moving
        tf = gameObject.GetComponent<Transform>();
    }

    void Start() {
        DisablePlayerMovement(false);
        currentStamina = maxStamina;
        currentHealth = maxHealth;
    }
    void Update() {
        if(CanMove) {
            terrainDataIndex = terrainTexDetector.GetActiveTerrainTextureIdx(tf.position);
            Debug.Log("Index: " + terrainDataIndex);
            HandleMovementInput();
            if(canCrouch) { AttemptToCrouch(); }

            // if(canJump) { HandleJump(); }
            HandleHeadbobEffect();
            HandleStamina();
            HandleMovementSFX();
            ApplyFinalMovement();
        }
    }
    void HandleMovementInput() {
        currentInput.x = Input.GetAxis("Vertical") * (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed :  walkSpeed);
        currentInput.y = Input.GetAxis("Horizontal") * (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed);

        float currentMovementY = currentMovement.y;
        currentMovement = (tf.forward * currentInput.x) + (transform.right * currentInput.y);
        currentMovement.y = currentMovementY;
    }

    void ApplyFinalMovement() {
        if(!controller.isGrounded){
            currentMovement.y -= gravityValue * Time.deltaTime; // Apply gravity
            if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                currentMovement.y = 0;
            }
        }

        if(IsSliding) {
            currentMovement += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }
        controller.Move(currentMovement * Time.deltaTime);
    }
    
    public void AttemptToCrouch() {
        if(shouldCrouch) {
            StartCoroutine(CrouchOrStand());
        }
    }
    private IEnumerator CrouchOrStand() {
        // If you try to stand up and hit anything 1 unit above, cancel and remain crouched
        if (isCrouching && Physics.Raycast(mainCamera.transform.position, Vector3.up, 1f)) {
            yield break;
        }
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

    public void TakeDamage(string type) {
        if(canTakeDamage) {
            switch(type) {
                case "Stab":
                    StartCoroutine(TakeDamageAndWait(2));
                    break;
                case "BearTrap":
                    StartCoroutine(TakeDamageAndWait(1));
                    break;
            }

        }
    }
    IEnumerator TakeDamageAndWait(int damageValue) {
        canTakeDamage = false;
        playerHasTakenDamage = true;
        lastStabbedTime = Time.time;
        currentHealth --;
        Debug.Log("health: " + currentHealth);
        switch(currentHealth) {
            case float currentHealth when currentHealth < 15f && currentHealth > 10:
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
            case float currentHealth when currentHealth <= 0:
                bleedingUI.SetActive(false);
                StopCoroutine(HandleRegenerateHealth());
                StartCoroutine(gameController.HandlePlayerDeath());
                break;
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(HandleRegenerateHealth());
        playerHasTakenDamage = false;
        canTakeDamage = true;
    }

    void HandleBleedEffect() {
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
    IEnumerator HandleRegenerateHealth() {
        while(currentHealth < maxHealth) {
            if(playerHasTakenDamage) {
                break;
            }
            currentHealth += 1 * Time.deltaTime;
            if(currentHealth >= maxHealth) {
                currentHealth = maxHealth;
            }
            yield return new WaitForSeconds(0.5f);
            Debug.Log(currentHealth);
        }
    }
    void HandleStamina() {
        if(currentStamina < 0) {
            currentStamina = 0;
            windedAudioSource.Play();
            canSprint = false;
        }

        if(isSprinting) {
            currentStamina -= 1f * Time.deltaTime; // Sprinting
        } else {
            if(canSprint == false) {
                if(currentStamina < maxStamina) { // Not sprinting, regenerate stamina
                    currentStamina += 1f * Time.deltaTime;
                } else {
                    currentStamina = maxStamina;
                    canSprint = true;
                }
            }
        }
    }

    void HandleMovementSFX() {
        if(!controller.isGrounded || !isMoving) {
            footstepAudioSource.Stop();
            return;
        }

        footstepTimer -= Time.deltaTime; // Play one footstep per second? what is this

        if(footstepTimer <= 0) {
            if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 4)) {
                if(hit.collider.tag == "Tile") {
                    Debug.Log("tile");
                    footstepAudioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
                } else {
                    switch(terrainDataIndex) {
                        case 0:
                            footstepAudioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
                            break;
                        case 1:
                            footstepAudioSource.PlayOneShot(dirtClips[Random.Range(0, dirtClips.Length - 1)]);
                            break;
                        case 5:
                            footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                            break;
                    }
                }
            }

            footstepTimer = GetCurrentOffset;
        }

    }
    void HandleHeadbobEffect() {
        if(!controller.isGrounded) {
            return;
        }
        if(Mathf.Abs(currentMovement.x) > 0.1f || Mathf.Abs(currentMovement.z) > 0.1f) {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
             // return Sin of angle; between -1 and 1
             //  Multiply this value by amount depending on current movement
            mainCamera.transform.localPosition = new Vector3(
                mainCamera.transform.localPosition.x,
                defaultYPosition + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                mainCamera.transform.localPosition.z
            );
        }
    }

    public void DisablePlayerMovement(bool disableMovement) {
        if (disableMovement) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            CanMove = false;
            fpHighlights.CanInteract = false;
            mouseLook.CanRotateMouse = false;
            currentMovement = Vector3.zero;
            flashlight.ToggleFlashlightStatus(false);
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of window
            CanMove = true;
            fpHighlights.CanInteract = true;
            mouseLook.CanRotateMouse = true;
            flashlight.ToggleFlashlightStatus(true);
        }
    }
 
    
    
    void HandleJump() {
        if(shouldJump) {
            currentMovement.y = jumpForce;
        }
    }
}