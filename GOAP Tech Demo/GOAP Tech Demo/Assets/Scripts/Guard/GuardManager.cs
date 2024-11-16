using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    int currentHealth;

    private void Start() {
        currentHealth = maxHealth;
    }
    public void TakeSwordHit(){
        currentHealth --;
        Debug.Log(currentHealth);
        if(currentHealth <= 0){
            // kill guard / death animation
        }
    }
    public int ReturnCurrentHP(){
        return currentHealth;
    }
}
