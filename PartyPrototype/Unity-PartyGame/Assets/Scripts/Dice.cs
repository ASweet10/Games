using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 diceVelocity;
    [SerializeField] private KeyCode rollDiceKey = KeyCode.Space;
    private bool canRoll = true;
    private bool isRolling = false;
    [SerializeField] private Text diceText;
    private int diceValue = 5;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update() {
        HandleDiceRoll();
    }
    public void HandleDiceRoll() {
        if(canRoll && Input.GetKeyDown(rollDiceKey)){
            gameObject.transform.Rotate(new Vector3(50f, 125f, 75f));
            canRoll = false;
            isRolling = true;
        }
        if(isRolling && Input.GetKeyDown(rollDiceKey)){
            diceValue = Random.Range(1, 10);
            //player jumps into air
            // collides with block
            // block explodes
            // ui number of dice roll appears in its place for few seconds
        }
    }

    private void Reset() {

    }
}
