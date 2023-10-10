using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attached to shell fired by tanks
public class ShellScript : MonoBehaviour
{
    float startTime;
    [SerializeField] ParticleSystem roundExplodeVFX;
    [SerializeField] AudioClip tankHitSFX;

    void Start() {
        startTime = Time.time;
    }

    void Update() {
        if(Time.time - startTime > 3f) {
            Destroy(gameObject); // Destroy shell if not collided / already exploded
        }
    }

    public void OnCollisionEnter(Collision col) {
        if(col.gameObject.CompareTag("PlayerOneTank") || col.gameObject.CompareTag("PatrolAI")) {
            TankHealth tankHealth = col.gameObject.GetComponent<TankHealth>();
            tankHealth.DamageTank();
        }
        AudioSource.PlayClipAtPoint(tankHitSFX, col.gameObject.transform.position, 0.5f);
        Instantiate(roundExplodeVFX, col.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}