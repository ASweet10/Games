using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusBall : MonoBehaviour
{
    public RectTransform bonusBall1SpawnArea;    
    public RectTransform bonusBall2SpawnArea;
    public GameObject bonusBall1;
    public GameObject bonusBall2;

    public GameController gameContScript;

    public bool bonus1CanSpawn;
    public bool bonus2CanSpawn;
    public bool bonusBall1Active;
    public bool bonusBall2Active;

    void Update()
    {
        SpawnBonusBall();
    }
    //-Check score from gamecontroller
    //-Spawn bonus ball at random point in rectTransform spawn area
    public void SpawnBonusBall()
    {
        if(gameContScript.tapCount % 10 == 0 && gameContScript.tapCount != 0)
        {
            if(!bonusBall1.activeInHierarchy && bonus1CanSpawn == true)
            {
                //Randomly change ball 1 position within the spawn area, then set active
                bonusBall1.transform.position = new Vector3(Random.Range(bonusBall1SpawnArea.rect.xMin, bonusBall1SpawnArea.rect.xMax), 
                                            Random.Range(bonusBall1SpawnArea.rect.yMin, bonusBall1SpawnArea.rect.yMax), 0);
                bonusBall1.SetActive(true);
                StartCoroutine(WaitForBonus1Respawn());
            }
        }
        if (gameContScript.tapCount % 20 == 0 && gameContScript.tapCount != 0)
        {
            if(!bonusBall2.activeInHierarchy && bonus2CanSpawn == true)
            {
                //Randomly change ball 2 position within the spawn area, then set active
                bonusBall2.transform.position = new Vector3(Random.Range(bonusBall2SpawnArea.rect.xMin, bonusBall2SpawnArea.rect.xMax), 
                                            Random.Range(bonusBall2SpawnArea.rect.yMin, bonusBall2SpawnArea.rect.yMax), 0);
                bonusBall2.SetActive(true);
                StartCoroutine(WaitForBonus2Respawn());
            }
        }
    }
    IEnumerator WaitForBonus1Respawn()
    {
        bonus1CanSpawn = false;
        yield return new WaitForSeconds(3f);
        bonus1CanSpawn = true;
    }
    IEnumerator WaitForBonus2Respawn()
    {
        bonus2CanSpawn = false;
        yield return new WaitForSeconds(5f);
        bonus2CanSpawn = true;
    }
}
