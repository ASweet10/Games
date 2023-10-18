using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // DateTime for RNG

public class TankSpawner : MonoBehaviour
{
    GameObject playerTank;
    [SerializeField] GameObject waypointPrefab;
    [SerializeField] GameObject BomberAI;
    [SerializeField] GameObject AggressiveAI;
    [SerializeField] GameObject HunterAI;
    [SerializeField] GameObject[] EnemyTanks;
    [SerializeField] List<GameObject> SpawnPoints;
    [SerializeField] List<Transform> GeneratedWaypoints;

    void Start() {
        playerTank = GameObject.FindGameObjectWithTag("PlayerOneTank");
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
    }
    public void HandleInitialTankSpawn() {
        SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("TankSpawnPoint"));
        foreach(GameObject spawnPoint in SpawnPoints) {
            Debug.Log(spawnPoint.name.ToString());
            Debug.Log(spawnPoint.transform.position);
        }
        int randomSpawnIndex = UnityEngine.Random.Range(0, SpawnPoints.Count);

        // Spawn player
        Transform playerSpawn = SpawnPoints[randomSpawnIndex].transform;
        playerTank.transform.position = playerSpawn.position + new Vector3(0, 0.5f, 0);
        SpawnPoints.Remove(SpawnPoints[randomSpawnIndex]); //Remove random index from list; avoid duplicate spawns
        playerTank.SetActive(true);

        // Spawn enemies
        foreach(GameObject enemy in EnemyTanks) {
            randomSpawnIndex = UnityEngine.Random.Range(0, SpawnPoints.Count);
            Transform enemySpawn = SpawnPoints[randomSpawnIndex].transform;
            enemy.transform.position = enemySpawn.position;
            SpawnWaypoints(enemy.transform.position);
            enemy.SetActive(true);
            Debug.Log(enemy.name);
            SpawnPoints.Remove(SpawnPoints[randomSpawnIndex]);
        }
    }

    public void HandleEnemyRespawn(string tankToRespawn) {
        SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("TankSpawnPoint"));
        Transform enemySpawn = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].transform; // Randomize spawn point
        SpawnWaypoints(enemySpawn.position);
        switch (tankToRespawn){
            case "BomberAI":
                BomberAI.SetActive(false);
                BomberAI.transform.position = enemySpawn.position + new Vector3(0, 0.5f, 0);
                BomberAI.SetActive(true);
                break;
            case "HunterAI":
                HunterAI.SetActive(false);
                HunterAI.transform.position = enemySpawn.position + new Vector3(0, 0.5f, 0);
                HunterAI.SetActive(true);
                break;
            case "AggressiveAI":
                AggressiveAI.SetActive(false);
                AggressiveAI.transform.position = enemySpawn.position + new Vector3(0, 0.5f, 0);
                AggressiveAI.SetActive(true);
                break;
            default:
                break;
        }
    }
    void SpawnWaypoints(Vector3 patrolSpawnpoint) {
        //Spawn waypoints in the corners of the room AI is spawned in
        GameObject waypointOne = Instantiate(waypointPrefab, patrolSpawnpoint + new Vector3(15, 0, 15), Quaternion.identity);
        GameObject waypointTwo = Instantiate(waypointPrefab, patrolSpawnpoint + new Vector3(15, 0, -15), Quaternion.identity);
        GameObject waypointThree = Instantiate(waypointPrefab, patrolSpawnpoint + new Vector3(-15, 0, -15), Quaternion.identity);
        GameObject waypointFour = Instantiate(waypointPrefab, patrolSpawnpoint + new Vector3(-15, 0, 15), Quaternion.identity);      
        
        GeneratedWaypoints.Add(waypointOne.transform);
        GeneratedWaypoints.Add(waypointTwo.transform);
        GeneratedWaypoints.Add(waypointThree.transform);
        GeneratedWaypoints.Add(waypointFour.transform);
    }
    public List<Transform> ReturnWaypoints() {
        return GeneratedWaypoints;
    }
}