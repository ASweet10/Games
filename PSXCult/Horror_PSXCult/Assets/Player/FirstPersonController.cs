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
    Interactables interactables;

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
    KeyCode interactKey = KeyCode.E;
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


    [Header("Zoom")]
    [SerializeField] float zoomTime = 0.3f;
    [SerializeField] float zoomFOV = 30;
    float defaultFOV;


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


    [Header("Highlights")]
    GameObject lastHighlightedObject;
    [SerializeField] GameObject cursor;
    [SerializeField] TMP_Text interactText; // Text displayed on hover

    [Header("Interact Texts")]
    [SerializeField] string trashString = "It smells awful...";
    [SerializeField] string myCarString = "My car. An old piece of shit but it's reliable";
    [SerializeField] string davidCarString = "David's new wheels. I'm sure he'll brag about it";
    [SerializeField] string needsZippoAndLighterFluidString = "I'm gonna need lighter fluid and a lighter";
    [SerializeField] string needsZippoString = "I still need a source of fire...";
    [SerializeField] string needsLighterFluidString = "I still need lighter fluid...";    
    
    
    GameController gameController;
    GameEvents gameEvents;

    public bool CanMove { get; private set; } = true;
    public bool CanInteract { get; private set; } 
    
    void Awake() {
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
        controller = gameObject.GetComponent<CharacterController>();
        terrainTexDetector = gameObject.GetComponent<TerrainTexDetector>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        interactables = GameObject.FindGameObjectWithTag("GameController").GetComponent<Interactables>();
        gameEvents = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEvents>(); 
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
            if(CanInteract) { HandleInteraction(); }
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

    IEnumerator HandleZoomByProximity(GameObject target, bool shouldZoomIn) {
        float targetFOV = shouldZoomIn ? zoomFOV : defaultFOV;
        float startFOV = mainCamera.fieldOfView;
        float timeElapsed = 0;

        while(timeElapsed < zoomTime) {
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed / zoomTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
        yield return null;
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
            CanInteract = false;
            mouseLook.CanRotateMouse = false;
            currentMovement = Vector3.zero;
            flashlight.ToggleFlashlightStatus(false);
        } else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of window
            CanMove = true;
            CanInteract = true;
            mouseLook.CanRotateMouse = true;
            flashlight.ToggleFlashlightStatus(true);
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
                    interactText.text = "";
                } else {
                    cursor.SetActive(false);
                    interactText.text = hitObj.tag;
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
                        case "MissingPoster1":
                            interactables.ToggleMissingUI(1, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPoster2":
                            interactables.ToggleMissingUI(2, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPoster3":
                            interactables.ToggleMissingUI(3, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPoster4":
                            interactables.ToggleMissingUI(4, true);
                            DisablePlayerMovement(true);
                            break;
                        case "MissingPoster5":
                            interactables.ToggleMissingUI(5, true);
                            DisablePlayerMovement(true);
                            break;
                        case "GasStationNewspaper":
                            interactables.ToggleMissingUI(6, true);
                            DisablePlayerMovement(true);
                            break;
                        case "StateParkNewspaper":
                            interactables.ToggleMissingUI(7, true);
                            DisablePlayerMovement(true);
                            break;
                        case "Car Note":
                            interactables.ToggleMissingUI(8, true);
                            gameController.playerHasReadCarNote = true;
                            gameController.HandleNextObjective();
                            DisablePlayerMovement(true);
                            break;
                        case "Drinks":
                            interactables.ToggleDrinksUI(true);
                            DisablePlayerMovement(true);
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
                        case "Arcade":
                            StartCoroutine(interactables.ToggleArcade(true));
                            interactables.PlayingArcadeGame = true;
                            DisablePlayerMovement(true);
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
                            if(gameController.playerNeedsLighterFluid) {
                                if(gameController.playerNeedsZippo) {
                                    StartCoroutine(gameController.DisplayPopupMessage(needsZippoAndLighterFluidString));
                                } else {
                                    StartCoroutine(gameController.DisplayPopupMessage(needsLighterFluidString));
                                }
                            } else if (gameController.playerNeedsZippo) {
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
                        case "Escape":
                            StartCoroutine(gameController.HandleEscapeCutscene());
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
                            StartCoroutine(HandleZoomByProximity(hitObj, true));
                            break;
                        case "AJ":
                            var ajTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                            ajTrigger.TriggerDialogue();
                            var ajCharacter = hitObj.GetComponent<AICharacter>();
                            ajCharacter.RotateAndStartTalking();
                            StartCoroutine(HandleZoomByProximity(hitObj, true));
                            break;
                        case "David":
                            var davidTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                            davidTrigger.TriggerDialogue();
                            var davidCharacter = hitObj.GetComponent<AICharacter>();
                            davidCharacter.RotateAndStartTalking();
                            StartCoroutine(HandleZoomByProximity(hitObj, true));
                            break;
                        case "Hunter":
                            var hunterTrigger = hitObj.GetComponentInChildren<DialogueTrigger>();
                            hunterTrigger.TriggerDialogue();
                            var hunterCharacter = hitObj.GetComponent<AICharacter>();
                            hunterCharacter.RotateAndStartTalking();
                            StartCoroutine(HandleZoomByProximity(hitObj, true));
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
    
    
    void HandleJump() {
        if(shouldJump) {
            currentMovement.y = jumpForce;
        }
    }
}