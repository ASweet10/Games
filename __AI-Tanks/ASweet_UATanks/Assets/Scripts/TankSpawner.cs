using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // DateTime for RNG

public class TankSpawner : MonoBehaviour
{
    GameObject playerTank;
    [SerializeField] GameObject waypointPrefab;
    [SerializeField] Transform[] EnemyTanks;
    [SerializeField] List<GameObject> SpawnPoints;
    [SerializeField] List<Transform> GeneratedWaypoints;
    [SerializeField] float playerRespawnTime = 3f;

    [Header("Start Menu Scene")]
    [SerializeField] GameObject menuTankA;
    [SerializeField] GameObject menuTankB;
    [SerializeField] Transform menuTankASpawnPoint;
    [SerializeField] Transform menuTankBSpawnPoint;

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
        foreach(Transform enemy in EnemyTanks) {
            randomSpawnIndex = UnityEngine.Random.Range(0, SpawnPoints.Count);
            Transform enemySpawn = SpawnPoints[randomSpawnIndex].transform;
            enemy.transform.position = enemySpawn.position;
            SpawnWaypoints(enemy.transform.position);
            enemy.gameObject.SetActive(true);
            Debug.Log(enemy.gameObject.name);
            SpawnPoints.Remove(SpawnPoints[randomSpawnIndex]);
        }
    }

    public void RespawnPlayer() {
        StartCoroutine(WaitForRespawnTimer());
        SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("TankSpawnPoint"));
        Transform playerSpawn = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].transform;
        playerTank.transform.position = playerSpawn.position + new Vector3(0, 0.5f, 0);
        playerTank.SetActive(true);
        //TankHealth health = playerTank.GetComponent<TankHealth>();
        //health.ResetHealth();
    }

    public void HandleEnemyRespawn(GameObject tankToRespawn) {
        StartCoroutine(WaitForRespawnTimer());
        SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("TankSpawnPoint"));
        Transform enemySpawn = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Count)].transform; // Randomize spawn point

        tankToRespawn.transform.position = enemySpawn.position + new Vector3(0, 0.5f, 0);

        // Reset waypoints before respawning
        GameObject[] waypointArray = GameObject.FindGameObjectsWithTag("PatrolWaypoint");
        foreach (GameObject waypoint in waypointArray) {
            Destroy(waypoint);
        }

        SpawnWaypoints(tankToRespawn.transform.position);
        tankToRespawn.SetActive(true);
    }
    IEnumerator WaitForRespawnTimer() {
        yield return new WaitForSecondsRealtime(playerRespawnTime); 
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