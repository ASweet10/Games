using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] int stabHealthLoss = 2;
    int currentHealth;
    [SerializeField] AudioSource bellAudioSource;

    private void Start() {
        currentHealth = maxHealth;
    }
    public void TakeSwordHit(){
        currentHealth -= stabHealthLoss;
        if(currentHealth <= 0){
            // guard death animation
        }
    }
    public int ReturnCurrentHP(){
        return currentHealth;
    }
    public void RingBell() {
        StartCoroutine(RingBellAndWait());
    }
    public IEnumerator RingBellAndWait() {
        bellAudioSource.PlayDelayed(0.5f);
        GuardBT.alarmRungRecently = true;
        yield return new WaitForSeconds(3f);
        GuardBT.alarmRungRecently = false;
    }
}