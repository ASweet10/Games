using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject gameWonUI;
    [SerializeField] private GameObject gameLostUI;

    private void Start() {
        Time.timeScale = 1f;
    }
    public void EnableGameWon() {
        Time.timeScale = 0f;
        gameLostUI.SetActive(false);
        gameWonUI.SetActive(true);
    }
    public void EnableGameLost() {
        Time.timeScale = 0f;
        gameWonUI.SetActive(false);
        gameLostUI.SetActive(true);
    }
}
