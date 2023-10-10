using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public static MazeManager MazeManagerInstance { get; private set; }
    [SerializeField] GameObject gameWonUI;
    [SerializeField] GameObject gameLostUI;
    [SerializeField] AudioSource audioSource;

    void Awake() {

        if(MazeManagerInstance == null){
            MazeManagerInstance = this;
        } else {
            Debug.Log("Can only have 1 instance of maze controller");
            Destroy(gameObject);
        }

    }
    void Start() {
        Time.timeScale = 1f;
    }

    public void EnableGameWon() {
        Time.timeScale = 0f;
        gameLostUI.SetActive(false);
        gameWonUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void EnableGameLost() {
        Time.timeScale = 0f;
        gameWonUI.SetActive(false);
        gameLostUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
