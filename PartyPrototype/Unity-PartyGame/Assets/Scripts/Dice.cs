using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UIElements;

public class Dice : MonoBehaviour
{
    [SerializeField] TMP_Text diceText;
    [SerializeField] KeyCode rollDiceKey = KeyCode.Space;
    Transform tf;
    bool canRoll = true;
    bool isRolling = false;
    int diceValue = 5;
    Quaternion originalRotation;
    Vector3[] dieRotations;


    void Start() {
        tf = gameObject.GetComponent<Transform>();
        originalRotation = tf.localRotation;
        // die rotations 1-6
        // Make die finish on these using this: https://discussions.unity.com/t/how-to-rotate-a-dice-to-a-final-angle/71233/2
        dieRotations[0] = new Vector3(0, 270f, 0);
        dieRotations[1] = new Vector3(0, 180f, 0);
        dieRotations[2] = new Vector3(0, 270f, 270f);
        dieRotations[3] = new Vector3(0, 90f, 0);
        dieRotations[4] = new Vector3(0, 0, 270f);
        dieRotations[5] = new Vector3(90f, 0, 270f);
    }

    void Update() {
        HandleDiceRoll();
    }
    public void HandleDiceRoll() {
        if(canRoll) {
            //tf.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            tf.rotation = Quaternion.Slerp(tf.rotation, UnityEngine.Random.rotation, Mathf.Clamp01(Time.deltaTime * 50));
            isRolling = true;
        }
        if(isRolling && Input.GetKeyDown(rollDiceKey)){
            canRoll = false;
            isRolling = false;
            diceValue = Random.Range(1, 10);
            //wantedRotation = 

            //player jumps into air
            // collides with block
            // block explodes
            // ui number of dice roll appears in its place for few seconds
        }
    }

    private void Reset() {

    }
}
