using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    Animator animator;
    Scene currentScene;
    [SerializeField] Image blackFadeImage;


    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Scene1")
        {
            FadeInFromBlack();
        }
    }

    public void LoadLevel(int levelNumber)
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LoadLevelAfterDelay(levelNumber));
    }
    IEnumerator LoadLevelAfterDelay(int levelNumber)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelNumber);
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeScreenToBlack());
    }
    public void FadeInFromBlack()
    {
        StartCoroutine(FadeScreenFromBlack());
    }

    IEnumerator FadeScreenToBlack()
    {
        blackFadeImage.enabled = true;
        animator.Play("Fade_Out");

        Color noAlpha = new Color(0, 0, 0, 0);
        if(blackFadeImage.color == noAlpha)
        {
            yield return null;
        }

    }

    IEnumerator FadeScreenFromBlack()
    {
        blackFadeImage.enabled = true;
        animator.Play("Fade_In");

        Color fullAlpha = new Color(0, 0, 0, 1);
        if(blackFadeImage.color == fullAlpha)
        {
            blackFadeImage.enabled = false;
            yield return null;
        }
    }
}
