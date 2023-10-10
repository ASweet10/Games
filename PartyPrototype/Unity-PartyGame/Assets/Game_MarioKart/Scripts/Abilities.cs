using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] float pickupCooldown = 10f;
    [SerializeField] AudioClip pickupAudio;
    MeshRenderer mesh;
    Collider collider;
    void Awake() {
        mesh = gameObject.GetComponent<MeshRenderer>();
        collider = gameObject.GetComponent<SphereCollider>();
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            var abilityScript = other.gameObject.GetComponent<CarAbilities>();
            var carUIScript = other.gameObject.GetComponent<CarUI>();
            int index;
            
            AudioSource.PlayClipAtPoint(pickupAudio, transform.position);


            if(!abilityScript.abilityTwoReady) {
                if(!abilityScript.abilityOneReady) {
                    index = abilityScript.RollForAbility();
                    abilityScript.abilityOneIndex = index;

                    carUIScript.SetAbilityUI(1, index, true);
                    abilityScript.abilityOneReady = true;
                } else {
                    index = abilityScript.RollForAbility();
                    abilityScript.abilityTwoIndex = index;

                    carUIScript.SetAbilityUI(2, index, true);                    
                    abilityScript.abilityTwoReady = true;
                }
            }
        }
        StartCoroutine(WaitForPickupCooldown());
    }

    IEnumerator WaitForPickupCooldown() {
        mesh.enabled = false;
        collider.enabled = false;
        yield return new WaitForSeconds(pickupCooldown);
        mesh.enabled = true;
        collider.enabled = true;
        yield break;
    }


}
