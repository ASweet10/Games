using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    void Awake() {
        if(gameController == null) {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }
    private void OnTriggerEnter(Collider col) {
        if(col.tag == "Player") {
            gameController.EnableGameWon();
            //Make snowball explode into pieces?
            //Make it keep going until crashes into something?
        }
    }
}
