using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    TankHealth tankHealth;
    [SerializeField] GameObject tankArmorVFX;
    [SerializeField] GameObject shieldUI;
    [SerializeField] float armorDuration;
    float armorDurationTime = 3f;
    bool canUseArmor;
    
    void Start() {
        tankHealth = gameObject.GetComponent<TankHealth>();
        canUseArmor = true;
    }
    void Update() {
        if(Time.time - armorDuration > armorDurationTime) {  //Has armor expired?
            tankHealth.ToggleCanTakeDamage(true);
            tankArmorVFX.SetActive(false);
        }
    }
    public void ActivateArmor() {
        tankHealth.ToggleCanTakeDamage(false);
        tankArmorVFX.SetActive(true);
        SetShieldUI(false);
        armorDuration = Time.time;
        canUseArmor = false;
    }
    public bool ReturnCanUseArmorStatus() {
        return canUseArmor;
    }
    public void SetCanUseArmor(bool choice) {
        canUseArmor = choice;
    }
    public void SetShieldUI(bool choice) {
        shieldUI.SetActive(choice);
    }
}