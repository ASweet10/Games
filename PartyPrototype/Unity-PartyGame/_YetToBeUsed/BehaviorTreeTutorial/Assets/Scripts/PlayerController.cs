﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    AudioSource footstepAudioSource;
    [SerializeField] CharacterController controller;
    [SerializeField] Camera mainCamera;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; 


    [Header("Movement Parameters")]
    [SerializeField, Range(1, 10)] float walkSpeed = 5f;
    [SerializeField, Range(1, 20)] float sprintSpeed = 10f;
    Vector3 move;
    float currentSpeed;
    float gravityValue = 9.8f;
    float verticalSpeed;
    bool canMove = true;
    float moveForward;
    float moveSide;
    bool canRun = true;

    [Header("Stamina")]
    [SerializeField, Range(1, 20)] float maxStamina = 15f;
    [SerializeField] AudioSource windedAudioSource;
    float currentStamina;
    
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

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;
    private bool canJump = true;

    private void Awake()
    {
        footstepAudioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Start()
    {
        currentStamina = maxStamina;
    }
    private void Update()
    {
        if(canMove)
        {
            if(canRun)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    currentSpeed = sprintSpeed;
                }
                else
                {
                    currentSpeed = walkSpeed;
                }
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            HandleJump();

            moveSide = Input.GetAxis("Horizontal") * currentSpeed;
            moveForward = Input.GetAxis("Vertical") * currentSpeed;

            if(moveSide != 0 || moveForward != 0)
            {
                MovePlayer();
            }
            else
            {
                PauseFootstepAudio();
            }
        }
        else
        {
            PauseFootstepAudio();
        }

    }   

    private void MovePlayer()
    {
        move = transform.right * moveSide + transform.forward * moveForward;
        //Apply gravity to handle stairs and not float
        verticalSpeed -= gravityValue * Time.deltaTime;
        move.y = verticalSpeed;
        
        controller.Move(move * Time.deltaTime);
        PlayFootstepAudio();
        UpdateStamina();
    }

    private void PlayFootstepAudio()
    {
        if(!footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
    }

    public void PauseFootstepAudio()
    {
        footstepAudioSource.Pause();
    }

    private void UpdateStamina()
    {
        if(currentStamina <= 0)
        {
            windedAudioSource.Play();
            canRun = false;
        }
        else if(currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            canRun = true;
        }
        //If player moving
        if(moveSide != 0 || moveForward != 0)
        {
            //Sprinting, lose stamina
            if(currentSpeed == sprintSpeed)
            {
                currentStamina -= 1 * Time.deltaTime;
            }
            //Walking, repair stamina
            else
            {
                currentStamina += 1.5f * Time.deltaTime;
            }
        }
    }

    public void ToggleMovement(bool choice)
    {
        canMove = choice;
    }

    private void HandleJump()
    {
        if(controller.isGrounded)
        {
            if(Input.GetKeyDown(jumpKey))
            {
                move.y = jumpForce;
            }
        }
    }

    /*
    private void AttemptToCrouch()
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


