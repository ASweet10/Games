using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Snowball : MonoBehaviour
{
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private float speed;

    private Rigidbody playerRB;

    private void Awake() {
        playerRB = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        HandleInput();
    }

    private void HandleInput() {
        if(Input.GetKey(moveLeftKey)) {
            playerRB.AddForce(Vector3.back * speed, ForceMode.Force);
        } else if (Input.GetKey(moveRightKey)) {
            playerRB.AddForce(Vector3.forward * speed, ForceMode.Force);
        }
    }
}
