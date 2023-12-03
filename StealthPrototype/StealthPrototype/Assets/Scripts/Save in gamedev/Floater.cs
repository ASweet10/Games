using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floatercount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floatercount, transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

        //If below the surface of the water...
        if (transform.position.y < waveHeight){
            //Invert y position to make it positive value
            // Clamp between 0 & 1 because once submerged, buoyancy will be the same regardless of object depth
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;

            //Add upwards force
            // Use Acceleration ForceMode because buoyancy force not affected by object mass
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);

            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
