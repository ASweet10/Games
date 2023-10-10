using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{    
    [Header("Controls")]
    [SerializeField] KeyCode abilityOneKey = KeyCode.Alpha1;
    [SerializeField] KeyCode abilityTwoKey = KeyCode.Alpha2;
    [SerializeField] KeyCode abilityThreeKey = KeyCode.Alpha3;

    [Header("Cooldowns")]
    [SerializeField] float lightningCooldown = 3f;
    [SerializeField] float chainLightningCooldown = 5f;

    [Header("Abilities")]
    [SerializeField] GameObject lightningBolt;
    [SerializeField] GameObject chainLightningBolt;
    [SerializeField] GameObject chainLightningSphere;
    [SerializeField] GameObject explosionOne;

    public Color abilityColorOne {get; private set;}
    Vector3 mousePosition; // World position of mouse
    [SerializeField] GameObject castPoint;
    bool canUseLightning = true;
    bool canUseChainLightning = true;
    bool canCreateExplosion = true;

    private void Update() {
        if(canUseLightning){
            if(Input.GetKeyDown(abilityOneKey)){
                Debug.Log("boom");
                mousePosition = ReturnMousePosition();
                CastLightningBolt(mousePosition);
            }
        }
        if(canUseChainLightning){
            if(Input.GetKeyDown(abilityTwoKey)){
                Debug.Log("chain lightning");
                mousePosition = ReturnMousePosition();
                CastChainLightning(mousePosition);
            }
        }
        if(canCreateExplosion){
            if(Input.GetKeyDown(abilityThreeKey)){
                Debug.Log("explosion");
                mousePosition = ReturnMousePosition();
                CreateExplosion(mousePosition);
            }
        }
    }
    public void CastLightningBolt(Vector3 castPosition){
        Instantiate(lightningBolt, castPoint.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        StartCoroutine(WaitForAbilityCooldown(canUseLightning, lightningCooldown));
    }

    public void CastChainLightning(Vector3 castPosition){
        Instantiate(chainLightningBolt, castPosition + new Vector3(0, 1, 0), Quaternion.identity);
        Instantiate(chainLightningSphere, castPosition, Quaternion.identity);
        StartCoroutine(WaitForAbilityCooldown(canUseChainLightning, chainLightningCooldown));
    }

    public void CreateExplosion(Vector3 castPosition){
        Instantiate(explosionOne, castPosition, Quaternion.identity);
    }

    public void SetAbilityColorOne(Color newColor){
        abilityColorOne = newColor;
    }

    Vector3 ReturnMousePosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

        return mousePosition;
    }

    IEnumerator WaitForAbilityCooldown(bool abilityReady, float abilityCD){
        abilityReady = false;
        yield return new WaitForSeconds(abilityCD);
        abilityReady = true;
    }

}
