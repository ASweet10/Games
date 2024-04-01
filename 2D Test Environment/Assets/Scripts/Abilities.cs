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
    [SerializeField] Transform castPoint;
    bool canUseLightning = true;
    bool canUseChainLightning = true;
    bool canCreateExplosion = true;

    private void Update() {
        if(canUseLightning){
            if(Input.GetKeyDown(abilityOneKey)){
                mousePosition = ReturnMousePosition();
                CastLightningBolt(mousePosition);
            }
        }
        if(canUseChainLightning){
            if(Input.GetKeyDown(abilityTwoKey)){
                mousePosition = ReturnMousePosition();
                CastChainLightning(mousePosition);
            }
        }
        if(canCreateExplosion){
            if(Input.GetKeyDown(abilityThreeKey)){
                mousePosition = ReturnMousePosition();
                CreateExplosion(mousePosition);
            }
        }
    }
    public void CastLightningBolt(Vector3 mousePosition){
        Vector3 rotationVector = mousePosition - castPoint.position;
        Debug.Log(rotationVector);
        //GameObject bolt = Instantiate(lightningBolt, castPoint.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(5, 25, 125)));
        GameObject bolt = Instantiate(lightningBolt, castPoint.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(0, 0, 90)));

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
