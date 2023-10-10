using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is placed on PowerupSpawner prefab. PowerupSpawners are created in
// MapGenerator script upon game start. 
//  Tracks current powerups & duration timers
public class PowerupGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] pickupArray;
    [SerializeField] float spawnDelay;
    float nextSpawnTime;
    GameObject spawnedPickup;

    void Start() {
        nextSpawnTime = Time.time + spawnDelay; // Spawn after one delay cycle
    }

    private void Update() {
        // If nothing spawned in this location...
        if(spawnedPickup == null) {
            // If time to spawn a new powerup...
            if(Time.time > nextSpawnTime) {
                //Spawn pickup and set next SpawnTime
                spawnedPickup = Instantiate(pickupArray[Random.Range(0, pickupArray.Length)], transform.position + Vector3.up, Quaternion.identity);
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else {
            nextSpawnTime = Time.time + spawnDelay; // Object still exists; postpone spawn
        }
    }
}