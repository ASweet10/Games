using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cashier : MonoBehaviour 
{
    [SerializeField] Transform playerTF; 
    [SerializeField] Animator anim;
    [SerializeField] float cashierRadius = 15f;
    [SerializeField] float rotationSpeed = 60f;
    Transform tf;
    void Awake() {
        tf = gameObject.transform;
    }
    void Update() {
        if(Vector3.Distance(playerTF.position, tf.position) <= cashierRadius) {
            LookAtPlayer();
        }
    }

    public void LookAtPlayer() {
        //tf.LookAt(new Vector3(0f, playerTF.position.y, playerTF.position.z));
        /*
        Vector3 targetDirection = playerTF.position - tf.position;
        Vector3 newDirection = Vector3.RotateTowards(tf.forward, targetDirection, 1f * Time.deltaTime, 0.0f);
        Debug.DrawRay(tf.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);
        */
        Vector3 targetPosition = playerTF.position - tf.position;
        targetPosition.y = 0;
        var rotation = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Slerp(tf.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}