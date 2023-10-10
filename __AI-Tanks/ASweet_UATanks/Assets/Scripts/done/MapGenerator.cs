using System.Collections;
using System.Collections.Generic;
using System; // Using DateTime
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int rows, columns;
    float roomWidth = 40f, roomHeight = 40f; // Was 50; Currently 40 because rooms are 0.8 x 0.8
    Game_Manager gameManager;
    [SerializeField] GameObject tankSpawnPoint;
    [SerializeField] GameObject powerupSpawner;
    [SerializeField] GameObject[] gridPrefabArray;
    Room[,] grid; // Grid used by MapGenerator; Store in [X,Y]; Call with grid[3,5]
    public enum RandomSpawnType{random, presetSeed, mapOfDay};
    public RandomSpawnType randomSpawnType = RandomSpawnType.random;
    int mapSeed;
    int presetSeed = 10;
    public bool gridGenerated = false;

    void Start() {
        gameManager = gameObject.GetComponent<Game_Manager>();
        if(randomSpawnType == RandomSpawnType.mapOfDay) {
            mapSeed = DateToInt(DateTime.Now.Date); // DateTime.Now.Date: year/month/date
        }
        else if(randomSpawnType == RandomSpawnType.random) {
            mapSeed = DateToInt(DateTime.Now); // DateTime.Now: current time
        }
        else if(randomSpawnType == RandomSpawnType.presetSeed) {
            mapSeed = presetSeed; // Preset seed from inspector
        }
    }

    public void GenerateGrid() { // Cycle through cols/rows; create room at each position
        gridGenerated = false;
        UnityEngine.Random.InitState(mapSeed); // UnityEngine.Random because using System, also has random
        grid = new Room[columns, rows]; // Clear previous grid

        for(int i = 0; i < rows; i++) { // Rows
            for(int j = 0; j < columns; j++) { // Columns
                float xPosition = roomWidth * j; // Multiply width by column number
                float yPosition = roomHeight * i; // Multiply height by row number
                Vector3 newPosition = new Vector3(xPosition, 0f, yPosition);

                //Create grid object at newPosition
                GameObject tempRoomObject = Instantiate(ReturnRandomPrefab(), newPosition, Quaternion.identity) as GameObject;

                tempRoomObject.transform.parent = this.transform; // Set object's parent
                tempRoomObject.name = "Room_#"+ j + i; // Name for inspector
                Room tempRoom = tempRoomObject.GetComponent<Room>();

                Instantiate(tankSpawnPoint, newPosition, Quaternion.identity); // Tank spawn point at (0,0,0) of each room
                Instantiate(powerupSpawner, newPosition + Vector3.forward * 3f, Quaternion.identity); // Powerup spawn point (0,0,3) units ahead of tank spawn point

                if(i == 0) { // Open doors depending on location (row)
                    tempRoom.doorNorth.SetActive(false); // Bottom row (i=0); open North
                }
                else if(i == (rows - 1)) {
                    tempRoom.doorSouth.SetActive(false); // Top row (i=rows.length-1); open South
                }
                else {
                    tempRoom.doorSouth.SetActive(false); // Middle of grid; open both
                    tempRoom.doorNorth.SetActive(false); // Middle of grid; open both
                }

                if(j == 0) { // Open doors depending on location (columns)
                    tempRoom.doorEast.SetActive(false); // First column (j=0); open East
                }
                else if(j == (columns - 1)) {
                    tempRoom.doorWest.SetActive(false); // Last column (j=rows.length-1); open West
                }
                else {
                    tempRoom.doorEast.SetActive(false); // Middle of grid; open both
                    tempRoom.doorWest.SetActive(false); // Middle of grid; open both
                }
                grid[j, i] = tempRoom; // Save current room in loop to array
            }
        }
        gridGenerated = true;
    }
    public void DestroyGrid() {
        foreach (GameObject powerupSpawner in gameManager.powerupSpawnerArray) {
            Destroy(powerupSpawner);
        }
        GameObject[] TankSpawnPoints = GameObject.FindGameObjectsWithTag("TankSpawnPoint");
        foreach (GameObject spawnPoint in TankSpawnPoints) {
            Destroy(spawnPoint);
        }
        GameObject[] roomArray = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in roomArray) {
            Destroy(room);
        }
        GameObject[] activePowerupArray = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject powerup in activePowerupArray) {
            Destroy(powerup);
        }
    }
    public int DateToInt(DateTime dateUsed) {
        //Add up current date and return as integer
        int date = (dateUsed.Year + dateUsed.Month + dateUsed.Day + dateUsed.Hour + 
            dateUsed.Minute + dateUsed.Second + dateUsed.Millisecond);
        return date;
    }
    public GameObject ReturnRandomPrefab() { // Random prefab room from list
        return gridPrefabArray[UnityEngine.Random.Range(0, gridPrefabArray.Length)];
    }
}