using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobEffect : MonoBehaviour
{
    [SerializeField] CharacterController playerController;
    [SerializeField] Animation anim;
    bool left = true;
    bool right = false;

    void Update()
    {
        CheckForMovementInput();
    }

    void CheckForMovementInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if(inputX != 0 || inputY != 0)
        {
            EnableHeadBobEffect();
        }
    }

    void EnableHeadBobEffect()
    {
        if(playerController.isGrounded)
        {
            if(left == true)
            {
                if(!anim.isPlaying)
                {
                    anim.Play("WalkLeft");
                    left = false;
                    right = true;
                }
            }
            if(right == true)
            {
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
