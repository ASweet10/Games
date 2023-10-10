using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D playerRB;
    private Vector2 currentInput;

    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerRB.velocity = Vector2.zero;
        MovePlayer();
    }
    //1. Get player input
    //2. Determine movement vector
    public void MovePlayer()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 moveVector = new Vector2(horizontal, vertical);
        currentInput = moveVector;
    }
    //Fixed update for physics calculations (MovePosition moving rigidbody)
    void FixedUpdate()
    {
        Vector2 position = playerRB.position;
        position = position + currentInput * speed * Time.deltaTime;
        playerRB.MovePosition(position);
    }
}
