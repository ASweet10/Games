using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject creditsUI;
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void EnableCredits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }
    public void DisableCredits()
    {
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
