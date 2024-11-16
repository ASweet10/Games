using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Transform tf;
    [SerializeField] Camera mainCamera;
    [SerializeField] MouseLook mouseLook;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 20f;
    KeyCode sprintKey = KeyCode.LeftShift;
    Vector2 currentInput;
    Vector3 currentMovement;
    bool isSprinting => canSprint && Input.GetKey(sprintKey);
    bool isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    bool canSprint = true;
    bool canMove = true;
    int currentHealth = 10;
    int swordHitValue = 2;

    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        tf = gameObject.GetComponent<Transform>();
    }

    void Update() {
        if(canMove) {
            HandleMovementInput();
            ApplyFinalMovement();
        }
    }

    public bool TakeSwordHit() {
        currentHealth -= swordHitValue;
        if(currentHealth > 0){
            return false; // Change UI
        } else {
            return true; // Kill player / handle game over
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
}