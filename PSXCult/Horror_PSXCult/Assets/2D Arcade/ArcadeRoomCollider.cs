using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeRoomCollider : MonoBehaviour
{
    ArcadeController arcadeController;
    [SerializeField] int leftRoomNumber;
    void Awake() {
        arcadeController = GameObject.FindGameObjectWithTag("Arcade").GetComponent<ArcadeController>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        StartCoroutine(arcadeController.DisableMovementAndTransitionScreen());
    }
}