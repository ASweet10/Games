using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class placed on PowerupSpawner objects; Determines if powerup exists in this location & spawns if not
public class PowerupGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] powerupArray;
    [SerializeField] float spawnDelay;
    float nextSpawnTime;
    GameObject spawnedPickup;

    void Start() {
        nextSpawnTime = Time.time + spawnDelay; // Spawn after one delay cycle
    }

    void Update() {
        if(spawnedPickup == null) { // If nothing spawned in this location...
            if(Time.time > nextSpawnTime) {  // If time to spawn a new powerup...
                spawnedPickup = Instantiate(powerupArray[Random.Range(0, powerupArray.Length)], transform.position + Vector3.up, Quaternion.identity);
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else {
            nextSpawnTime = Time.time + spawnDelay; // Object still exists; postpone spawn
        }
    }
}