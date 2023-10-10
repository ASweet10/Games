using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "PlayerOneTank" || col.gameObject.tag == "PatrolAI"){
            TankHealth tankHealth = col.gameObject.GetComponent<TankHealth>();
            tankHealth.DestroyTank();
        }
    }
}