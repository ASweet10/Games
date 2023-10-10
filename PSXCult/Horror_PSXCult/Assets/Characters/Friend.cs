using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
    [SerializeField] float stabRange = 3f;
    bool playerInRange = false;
    bool injured = false;

    void Start () {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update() {
        playerInRange = CheckIfPlayerInRange();
        if(!injured) {
            if(playerInRange) {
                
            }
        }
    }
    bool CheckIfPlayerInRange() {
        if(Vector3.Distance(transform.position, playerTF.position) < stabRange) {
            return true;
        } else {
            return false;
        }
    }
}
