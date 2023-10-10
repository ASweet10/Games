using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text livesText;
    public Text banditsCaughtText;
    public Text gameOverCaughtText;
    public GameObject gameOverUI;
    
    AudioSource audioSource;
    public AudioClip catchBanditAudioOne;
    public AudioSource gameMusic;
    bool audioResumed = false;


    int currentRound;
    public int lootLives = 3;
    //Number of bandits to spawn
    int numBandits = 1;
    //Number of bandits caught in game
    public int banditCount;

    public Sprite[] lootSprites;
    public GameObject loot;
    public GameObject[] lootArray;
    public Transform lootSpawnPosition;
    public RectTransform lootSpawnArea;

    
    public Transform banditSpawnPosition;
    GameObject[] banditArray;
    public GameObject bandit;
    public GameObject[] banditSpawnPoints;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        Time.timeScale = 1;
        gameMusic.enabled = true;
        currentRound = 1;
        banditCount = 0;
        lootLives = 3;
        Application.targetFrameRate = -1;
        SpawnObjects();
    }

    void Update()
    {   
        banditArray = GameObject.FindGameObjectsWithTag("Bandit");

        CheckGameCounts();
        livesText.text = lootLives.ToString();
        banditsCaughtText.text = banditCount.ToString();
    }
    void CheckGameCounts()
    {
        if(lootLives <= 0)
        {
            EnableGameOverScreen();
        }
        if(banditArray.Length <= 0)
        {
            StartNextRound();
        }
    }
    void StartNextRound()
    {
        currentRound += 1;
        foreach(GameObject bandit in banditArray)
        {
            bandit.SetActive(false);
        }
        StartCoroutine(SpawnBanditsAndWait());
    }
    public void EnableGameOverScreen()
    {
        Time.timeScale = 0;
        gameMusic.enabled = false;
        gameOverUI.SetActive(true);
        gameOverCaughtText.text = banditCount.ToString();
    }
    public void DisableGameOverScreen()
    {
        gameOverUI.SetActive(false);
        SceneManager.LoadScene(1);
    }
    void SpawnObjects()
    {
        currentRound = 1;
        StartCoroutine(SpawnBanditsAndWait());
        SpawnLoot();
    }
    void SpawnLoot()
    {
        for(int i = 0; i < lootArray.Length; i++)
        {
            //spawn in random area inside RectTransform
            lootSpawnPosition.position = new Vector3(Random.Range(lootSpawnArea.rect.xMin, lootSpawnArea.rect.xMax),
                Random.Range(lootSpawnArea.rect.yMin, lootSpawnArea.rect.yMax), 0);
            
            foreach(GameObject loot in lootArray)
            {
                loot.SetActive(true);
                //Randomly set sprite from array
                SpriteRenderer lootObjectSR = loot.GetComponent<SpriteRenderer>();
                int lootObjectIndex = Random.Range(0, lootSprites.Length);
                lootObjectSR.sprite = lootSprites[lootObjectIndex];
            }
        }
    }
    public void RespawnLootAfterBanditCaught()
    {
        lootSpawnPosition.position = new Vector3(Random.Range(lootSpawnArea.rect.xMin, lootSpawnArea.rect.xMax),
            Random.Range(lootSpawnArea.rect.yMin, lootSpawnArea.rect.yMax), 0);
        
        GameObject respawnedLoot = Instantiate(loot, lootSpawnPosition.position, Quaternion.identity);
        
        SpriteRenderer lootObjectSR = respawnedLoot.GetComponent<SpriteRenderer>();
        int lootObjectIndex = Random.Range(0, lootSprites.Length);
        lootObjectSR.sprite = lootSprites[lootObjectIndex];
    }
    IEnumerator SpawnBanditsAndWait()
    {
        numBandits = currentRound - 1;
        for(int i = 0; i < numBandits; i++)
        {
            int spawnIndex = Random.Range(0, banditSpawnPoints.Length);
            banditSpawnPosition.position = new Vector3(banditSpawnPoints[spawnIndex].transform.position.x,
                                            banditSpawnPoints[spawnIndex].transform.position.y, 0);
            Instantiate(bandit, banditSpawnPosition.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            numBandits --;
        }
        //Stop spawning once number is reached
        if(numBandits == 0)
        {
            //Breaks out of coroutine
            yield break;
        }
    }
    public void IncrementBanditCount()
    {
        banditCount++;
    }
    public void PlayBanditCaughtAudio()
    {
        AudioSource.PlayClipAtPoint(catchBanditAudioOne, transform.position, 0.7f);
    }
}
