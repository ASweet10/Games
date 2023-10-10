using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //
    //Attach this script to camera, which should be child of FPS Character
    //

    [Header("Mouse Look")]
    [SerializeField]  private float mouseSensitivity = 100f;
    [SerializeField]  private Transform playerBody;
    private float xRotation = 0f;

    private void Update()
    {
        LookWithMouse();
    }

    private void LookWithMouse()
    {
        //Mouse Input * mouse sensitivity * delta time for same speed regardless of frame rate
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        //Clamp camera rotation so you can't look past a certain point 
        xRotation = Mathf.Clamp(xRotation, -55f, 55f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}