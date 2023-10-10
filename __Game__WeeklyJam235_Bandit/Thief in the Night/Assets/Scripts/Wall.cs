using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.gameObject.tag == "ThrowingKnife")
        {
            var knifeRB = col.gameObject.GetComponent<Rigidbody2D>();
            var knifeScript = col.gameObject.GetComponent<ThrowingKnife>();
            knifeRB.velocity = Vector2.zero;
            knifeScript.StopRotation();
        }
    }
}
