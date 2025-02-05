using UnityEngine;

public class Sword : MonoBehaviour
{
    PlayerController playerController;
    void Awake() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerController.TakeSwordHit();
        }
    }
}