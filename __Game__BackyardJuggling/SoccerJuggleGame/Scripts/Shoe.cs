using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : MonoBehaviour
{
    public GameObject ShoeUI;
    public GameObject Shoe2UI;
    public GameObject Shoe3UI;
    public GameObject actualShoe;

    AudioSource audioSource;

    public GameController gameContScript;

    public int shoeCount;
    bool shoeCanSpawn = true;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        ShoeUI.SetActive(true);
        Shoe2UI.SetActive(true);
        Shoe3UI.SetActive(true);
        shoeCount = 3;
    }

    //Spawn shoe under ball if it enters the shoe area
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "GameBall")
        {
            if(shoeCount == 3)
            {
                if(shoeCanSpawn == true)
                {
                    ShoeUI.SetActive(false);
                    SpawnShoe(other);
                }
            }
            else if(shoeCount == 2)
            {
                if(shoeCanSpawn == true)
                {
                    Shoe2UI.SetActive(false);
                    SpawnShoe(other);
                }
            }
            else if(shoeCount == 1)
            {
                if(shoeCanSpawn == true)
                {
                    Shoe3UI.SetActive(false);
                    SpawnShoe(other);
                }
            }
        }
    }
    public void SpawnShoe(Collider2D otherCol)
    {
        shoeCount --;
        GameObject shoe = Instantiate(actualShoe, otherCol.gameObject.GetComponent<CircleCollider2D>().ClosestPoint(transform.position), 
                                                Quaternion.identity);
        gameContScript.EnableBallStasis();
        audioSource.Play();
        Destroy(shoe, 1f);
        StartCoroutine(WaitForShoeRespawn());
    }
    IEnumerator WaitForShoeRespawn()
    {
        shoeCanSpawn = false;
        yield return new WaitForSeconds(3);
        shoeCanSpawn = true;
    }
}
