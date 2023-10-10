using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            var uiScript = other.gameObject.GetComponent<CarUI>();
            uiScript.StartLap();
        }
    }
}
