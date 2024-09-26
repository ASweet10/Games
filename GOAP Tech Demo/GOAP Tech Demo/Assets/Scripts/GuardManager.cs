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
        if(currentHealth > 0){
            //change UI, lose heart, etc.
        } else {
            //kill guard, play death animation
        }
    }
    public void TakeKnockout(){
        //play knockout animation
        //Diable guard bt script
    }
    public int ReturnCurrentHP(){
        return currentHealth;
    }
}
