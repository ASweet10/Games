using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] GameObject blackFadeGO;
    [SerializeField] Image blackFadeImage;
    FirstPersonController fpsController;
    Color noAlpha = new Color(0, 0, 0, 0);
    Color fullAlpha = new Color(0, 0, 0, 255);
    public bool isFading;

    void Awake () {
        if(SceneManager.GetActiveScene().buildIndex != 0) {  // If main menu
            fpsController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        }
    }

    public IEnumerator FadeInFromBlack() {
        blackFadeGO.SetActive(true);
        yield return new WaitForSeconds(4f);
        blackFadeGO.SetActive(false);
    }
    public IEnumerator FadeOutToBlack() {
        blackFadeGO.SetActive(true);
        fpsController.canMove = false;

        yield return new WaitForSeconds(4f);

        blackFadeGO.SetActive(false);
        fpsController.canMove = true;

    }

    public IEnumerator FadeOutThenFadeIn(float delay, int optionalSceneIndex) {
        blackFadeGO.SetActive(true);
        blackFadeImage.enabled = true;

        fpsController.canMove = false;
        float time = 0;

        while(time < 4) {
            blackFadeImage.color = Color.Lerp(noAlpha, fullAlpha, time / 4);
            time += Time.deltaTime;
        }
        yield return new WaitForSeconds(delay);
        while(time < 4) {
            blackFadeImage.color = Color.Lerp(fullAlpha, noAlpha, time / 4);
            time += Time.deltaTime;
        }
        blackFadeImage.enabled = false;
        blackFadeGO.SetActive(false);

        fpsController.canMove = true;

        if (optionalSceneIndex == 0) {
            SceneManager.LoadScene(0);
        } else if (optionalSceneIndex == 1) {
            SceneManager.LoadScene(1);
        }
    }

}