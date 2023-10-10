using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditBehavior : MonoBehaviour
{
    GameController gameController;
    public AudioClip banditStealSFX;
    public float banditSpeed = 1f;

    private bool holdingLoot;
    GameObject[] escapeHoles;
    GameObject chosenEscapeHole;

    //Array of treasure, populated at start of level
    GameObject[] availableLoot;
    GameObject closestLoot = null;
    private bool canGrabLoot;
    public GameObject lootOnBandit;
    public bool banditCanEscape;
    Vector3 banditPosition;

    void Start()
    {
        if(gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        banditPosition = transform.position;
        availableLoot = GameObject.FindGameObjectsWithTag("Loot");
        // Closest loot determined by FindNearestLoot and returned as GameObject
        closestLoot = FindNearestLoot();
        escapeHoles = GameObject.FindGameObjectsWithTag("EscapeHole");
        chosenEscapeHole = ChooseRandomEscapeHole();
    }
    void Update()
    {
        if(closestLoot == null)
        {
            closestLoot = FindNearestLoot();
        }
        if(!holdingLoot)
        {
            canGrabLoot = true;
        }
        else
        {
            canGrabLoot = false;
            MoveToEscapeHole(chosenEscapeHole);
        }
        MoveToChosenLoot(closestLoot);
    }
    private GameObject FindNearestLoot()
    {
        float lowDistance = Mathf.Infinity;
        for(int i = 0; i < availableLoot.Length; i++)
        {
            float distance = Vector3.Distance(availableLoot[i].transform.position, banditPosition);
            if(distance < lowDistance)
            {
                lowDistance = distance;
                closestLoot = availableLoot[i];
            }
        }
        return closestLoot;
    }
    void MoveToChosenLoot(GameObject closestObj)
    {
        if(!holdingLoot)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestObj.transform.position,
                                                banditSpeed * Time.deltaTime);
        }
        //If loot not found or taken while bandit moving to grab it, 
        //  assign new loot
        if(closestObj == null)
        {
            closestObj = FindNearestLoot();
        }
    }
    GameObject ChooseRandomEscapeHole()
    {
        int randInt = Random.Range(0, escapeHoles.Length);
        GameObject chosenEscapeHole = escapeHoles[randInt];
        return chosenEscapeHole;
    }
    void MoveToEscapeHole(GameObject chosenEscapeHole)
    {
        transform.position = Vector2.MoveTowards(transform.position, chosenEscapeHole.transform.position,
                                            banditSpeed * Time.deltaTime);
    }
    public void BanditPickUpLoot(GameObject pickedUpLoot)
    {
        if(canGrabLoot)
        {
            lootOnBandit.SetActive(true);
            holdingLoot = true;
            pickedUpLoot.gameObject.SetActive(false);
            banditCanEscape = true;
            banditSpeed *= 1.5f;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "ThrowingKnife")
        {
            //Get reference to knife RB & stop velocity. Also stop rotation.
            var knifeRB = col.gameObject.GetComponent<Rigidbody2D>();
            var knifeScript = col.gameObject.GetComponent<ThrowingKnife>();
            knifeRB.velocity = Vector2.zero;
            knifeScript.StopRotation();

            //Bandit count + SFX
            gameController.IncrementBanditCount();
            gameController.PlayBanditCaughtAudio();
            if(holdingLoot)
            {
                lootOnBandit.SetActive(false);
                holdingLoot = false;
                gameController.RespawnLootAfterBanditCaught();
            }
            Destroy(gameObject);
        }
        else if(col.gameObject.tag == "EscapeHole")
        {
            if(banditCanEscape)
            {
                AudioSource.PlayClipAtPoint(banditStealSFX, transform.position, 3f);
                gameController.lootLives --;
                Destroy(gameObject);
            }
        }
    }
}