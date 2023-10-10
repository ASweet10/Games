using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    GameController gameController;
    public bool doneSaving;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
    }
    //-Check for file, if none exists then create save data file
    public void SaveRecordToFile(int record)
    {
        if(File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            FileStream recordFileExists = File.OpenWrite(Application.persistentDataPath + "/SaveData.dat");
            GameData data = new GameData(record);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(recordFileExists, data);
            recordFileExists.Close();
            doneSaving = true;
        }
        else
        {
            FileStream recordFile = File.Create(Application.persistentDataPath + "/SaveData.dat");
            GameData data = new GameData(record);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(recordFile, data);
            recordFile.Close();
            doneSaving = true;
        }        
    }
    //-Attempt to read save data file from persistent data path
    //-If none exists, return tap record = 0
    //-Called in GameController start function
    public int ReadRecordFromFile()
    {
        if(File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream recordFileExists = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);
            GameData data = (GameData) bf.Deserialize(recordFileExists);
            recordFileExists.Close();
            int tapRecord = data.record;
            return tapRecord;    
        }
        else
        {
            return 0;
        }
    }
    //-Create serializable data class to save game record
    //-Serializable attribute indicates class can be serialized,
    //  or stored in a new format that can be reconstructed later
    //-In this case, binary format
    
    [System.Serializable]
    public class GameData
    {
        public int record;
        public GameData(int recordInt)
        {
            record = recordInt;
        }
    }
}
