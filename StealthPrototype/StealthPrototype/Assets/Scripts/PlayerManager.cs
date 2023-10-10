using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    int currentHealth;

    private void Start() {
        currentHealth = maxHealth;
    }

    public bool TakeSwordHit() {
        currentHealth --;
        Debug.Log(currentHealth);
        if(currentHealth > 0){
            //change UI, lose heart, etc.
            return false;
        } else {
            //kill player
            return true;
        }
    }
}
