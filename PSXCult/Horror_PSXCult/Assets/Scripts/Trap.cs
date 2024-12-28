using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource;
    [SerializeField] FirstPersonController fpController;

    void Awake() {
        fpController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            anim.SetBool("TrapTriggered", true);
            audioSource.Play();
            StartCoroutine(DisableMovementOnTimer());
        }
    }

    IEnumerator DisableMovementOnTimer() {
        fpController.DisablePlayerMovement(true, false);
        yield return new WaitForSeconds(2f);
        fpController.DisablePlayerMovement(false, false);
    }
}