using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private FindPathAStar pathfindScript;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image playerPortrait;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float trapDamage = 20f;

    private float currentHealth;
    public float CurrentHealth { 
        get { return currentHealth; }
    }

    private void Start() {
        currentHealth = maxHealth;
    }
    public void TakeTrapDamage()
    {
        currentHealth -= trapDamage;
        healthBar.fillAmount -= 0.2f;

        //Move this logic to a game manager, observer pattern
        pathfindScript.ChooseTargetByHealthPercentage();

        //Make screen ui bloodier as you get lower on HP
        // Maybe 3-4 progressively bloody outlines you can make in gimp etc.
        if(currentHealth <= 0)
        {
            EliminatePlayer();
        }
    }
    
    private void EliminatePlayer()
    {
        //Move this logic to a game manager, observer pattern
        //pathfindScript.activePlayers.Remove(gameObject);
        healthBar.enabled = false;
        playerPortrait.color = new Color(1, 1, 1, 0.1f);
    }
}
