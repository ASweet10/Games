using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultMember : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTF;
    [SerializeField] float stabRange = 3f;
    bool playerInRange = false;
    bool isStabbing = false;

    void Start () {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update() {
        playerInRange = CheckIfPlayerInRange();
        Debug.Log("In range: " + playerInRange);
        if(playerInRange) {
            anim.SetBool("stabbing", true);
        } else {
            anim.SetBool("stabbing", false);
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