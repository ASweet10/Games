using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float explosionForce = 3000f;
    [SerializeField] private float explosionRadius = 50f;
    [SerializeField] private AudioClip explosionClip;

    private void Awake() {
        if(rb == null) {
            rb = gameObject.GetComponent<Rigidbody>();
        }
    }
    void Start()
    {
        Explode();
    }

    private void Explode()
    {
        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        AudioSource.PlayClipAtPoint(explosionClip, transform.position);
    }

}
