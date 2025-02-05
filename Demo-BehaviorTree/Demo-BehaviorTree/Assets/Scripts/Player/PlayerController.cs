using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Manager manager;
    CharacterController controller;
    Transform tf;
    [SerializeField] Camera mainCamera;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 20f;
    KeyCode sprintKey = KeyCode.LeftShift;
    Vector2 currentInput;
    Vector3 currentMovement;
    bool isSprinting => canSprint && Input.GetKey(sprintKey);    
    bool canSprint = true;
    bool canMove = true;
    int currentHealth = 10;
    int swordHitValue = 2;

    [Header("Interactions")]
    public bool canInteract = true;
    GameObject lastHighlightedObject;
    [SerializeField] Sprite normalCursor;
    [SerializeField] Sprite interactCursor;
    [SerializeField] Image cursorImage;

    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        tf = gameObject.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if(canMove) {
            HandleMovementInput();
            ApplyFinalMovement();
        }
        if (canInteract) {
            HandleInteraction();
        }
    }

    public void TakeSwordHit() {
        currentHealth -= swordHitValue;
        if(currentHealth > 0){
            // Change UI
        } else {
            // Kill player / handle game over
        }
    }

    void HandleMovementInput() {
        currentInput.x = Input.GetAxis("Vertical") * (isSprinting ? sprintSpeed :  walkSpeed);
        currentInput.y = Input.GetAxis("Horizontal") * (isSprinting ? sprintSpeed : walkSpeed);

        float currentMovementY = currentMovement.y;
        currentMovement = (tf.forward * currentInput.x) + (transform.right * currentInput.y);
        currentMovement.y = currentMovementY;
    }

    void ApplyFinalMovement() {
        if(!controller.isGrounded){
            currentMovement.y -= 9.8f * Time.deltaTime; // Apply gravity
            if(controller.velocity.y < -1 && controller.isGrounded){  //Landing frame; reset y value to 0
                currentMovement.y = 0;
            }
        }
        controller.Move(currentMovement * Time.deltaTime);
    }

    void HandleInteraction() {
        float rayDistance = 50f;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Ray from center of the viewport
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit, rayDistance)) {
            GameObject hitObj = rayHit.collider.gameObject;  // Get object that was hit
            if(Vector3.Distance(gameObject.transform.position, hitObj.transform.position) < 6f) {
                HighlightObject(hitObj);
                if(Input.GetKeyDown(KeyCode.E)) {
                    switch(hitObj.tag) {
                        case "Chest":
                            manager.OpenChest();
                            break;
                        case "Coins":
                            manager.StealCoins();
                            break;
                        default:
                            ClearHighlighted();
                            break;
                    }
                }
            } else {
                ClearHighlighted();
            }
        }
    }
    public void ClearHighlighted() {
        if (lastHighlightedObject != null) {
            lastHighlightedObject = null;
            cursorImage.sprite = normalCursor;
        }
    }
    void HighlightObject(GameObject hitObj) {
        if (lastHighlightedObject != hitObj) {
            ClearHighlighted();
            lastHighlightedObject = hitObj;
        }
        if(hitObj.tag != "Untagged") {
            cursorImage.sprite = interactCursor;
        }
    }
}