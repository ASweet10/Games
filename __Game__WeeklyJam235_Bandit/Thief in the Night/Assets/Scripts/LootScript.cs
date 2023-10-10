using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LootScript : MonoBehaviour
{
    BanditBehavior banditBehavior;
    //When bandit collides with (this) loot, call function on bandit
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Bandit")
        {
            banditBehavior = col.gameObject.GetComponent<BanditBehavior>();
            banditBehavior.BanditPickUpLoot(gameObject);
        }
    }
}
