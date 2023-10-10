using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is attached to "Game Over" object, which is placed beneath game area with collider
public class GameOver : MonoBehaviour
{
    public GameObject gameController;
    public GameObject shoeUI;
    public GameObject shoe2UI;
    public GameObject shoe3UI;

    GameController gameContScript;
    public Shoe shoeScript;

    void Start()
    {
        gameContScript = gameController.GetComponent<GameController>();
    }
    //-If game ball triggers game over collider,
    //--Set Game HUD inactive
    //--Stop music/ambiance
    //--Then enable GameOver UI menu
    //-If bonus ball 1 or 2 triggers, set inactive

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "GameBall")
        {
            if(shoeUI.activeInHierarchy || shoe2UI.activeInHierarchy || shoe3UI.activeInHierarchy)
            {
                shoeUI.SetActive(false);
                shoe2UI.SetActive(false);
                shoe3UI.SetActive(false);
                gameContScript.EnableGameOverMenu();              
            }
            else
            {
                gameContScript.EnableGameOverMenu();
            }

        }
        else if (other.tag == "Bonus1")
        {
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Bonus2")
        {
            other.gameObject.SetActive(false);
        }
    }
}
