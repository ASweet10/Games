using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTrap : MonoBehaviour
{
    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private AudioClip trapSFX;
    [SerializeField] MazeMonster monsterScript;
    [SerializeField] GameObject playerOne;
    private void Awake(){
        if(monsterScript == null){
            monsterScript = GameObject.FindGameObjectWithTag("MazeMonster").GetComponent<MazeMonster>();
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag != "MazeMonster")    
        {
            var health = other.gameObject.GetComponent<PlayerHealth>();
            health.TakeTrapDamage();
            Instantiate(bloodVFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(trapSFX, transform.position);

            monsterScript.CheckForNewTarget();
            gameObject.SetActive(false);
        }
    }
}
