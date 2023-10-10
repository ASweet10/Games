using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cashier : MonoBehaviour 
{
    [SerializeField] Transform playerTF; 
    [SerializeField] Animator anim;
    Transform tf;

    public void LookAtPlayer() {
        tf.LookAt(playerTF.position);
    }
}