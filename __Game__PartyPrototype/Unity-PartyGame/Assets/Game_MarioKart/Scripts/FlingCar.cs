using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlingCar : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float thrust = 100f;

    private void Awake() {
        if(rb == null) {
            rb = gameObject.GetComponent<Rigidbody>();
        }
    }

    public void LaunchCar()
    {
        rb.AddForce(transform.up * thrust);
        Debug.Log("flung");
    }



}
