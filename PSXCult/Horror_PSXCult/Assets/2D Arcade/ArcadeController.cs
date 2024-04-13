using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ArcadeController : MonoBehaviour
{
    [SerializeField] GameObject arcadeWolf;
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject arcadeLevelOne;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Animator anim;
    [SerializeField] Transform arcadePlayerTF;
    [SerializeField] Transform playerStartPosition;
    Vector2 movement;
    float moveSpeed = 1.5f;
    int playerHealth;
    int maxHealth = 3;
    bool canMove;
    public bool CanMove {
        get { return canMove; }
        set { canMove = value; }
    }
    bool wolfSpawned;
    bool facingRight;
    float startTime;

    void Start() {
        playerHealth = maxHealth;
        canMove = true;
        facingRight = true;
        startTime = Time.time;
        wolfSpawned = false;
    }
    void Update() {
        if(Time.time - startTime > 8f) {
            if(!wolfSpawned) {
                arcadeWolf.SetActive(true);
                wolfSpawned = true;
            }
        }
        MovePlayerOnInput();
    }
    void MovePlayerOnInput() {
        if(canMove) {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                playerRB.MovePosition(playerRB.position + new Vector2(0, 1) * moveSpeed * Time.deltaTime);
                anim.SetBool("isWalking", true);
            } else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                playerRB.MovePosition(playerRB.position + new Vector2(-1, 0) * moveSpeed * Time.deltaTime);
                anim.SetBool("isWalking", true);
                if(facingRight) {
                    spriteRenderer.flipX = true;
                    facingRight = false;
                }
            } else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                playerRB.MovePosition(playerRB.position + new Vector2(0, -1) * moveSpeed * Time.deltaTime);
                anim.SetBool("isWalking", true);
            } else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                playerRB.MovePosition(playerRB.position + new Vector2(1, 0) * moveSpeed * Time.deltaTime);
                anim.SetBool("isWalking", true);
                if(!facingRight) {
                    spriteRenderer.flipX = false;
                    facingRight = true;
                }
            } else { // No movement input
                anim.SetBool("isWalking", false);
            }
        }
    }
    // or no take damage, if caught by monster you are dead. restart level / lose life
    void TakeDamage() {
        playerHealth --;
        if(playerHealth <= 0) {
            canMove = false;
            deathUI.SetActive(true);
        } else {
            // screen gets bloodier? some feedback
        }
    }

    public void HandleArcadeGameOver() {
        canMove = false;
        deathUI.SetActive(true);
        arcadeLevelOne.SetActive(false);
    }

    public void ResetArcadePlayerPosition() {
        arcadePlayerTF.position = playerStartPosition.position;
    }
    public void ResetStartTime() {
        startTime = Time.time;
    }
}
