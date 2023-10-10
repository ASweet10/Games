using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Image blackFadeImage;
    [SerializeField] float fadeTime = 6f;
    Color noAlpha = new Color(0, 0, 0, 0);
    Color fullAlpha = new Color(0, 0, 0, 255);
    
    public IEnumerator FadeInFromBlack() {
        blackFadeImage.enabled = true;
        float time = 0;
        Color startColor = blackFadeImage.color;

        while(time < fadeTime) {
            blackFadeImage.color = Color.Lerp(startColor, noAlpha, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }
        blackFadeImage.enabled = false;
    }

    public IEnumerator FadeOutThenLoadScene(int sceneToLoad) {
        blackFadeImage.enabled = true;
        float time = 0;

        while(time < fadeTime){
            blackFadeImage.color = Color.Lerp(noAlpha, fullAlpha, time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        blackFadeImage.enabled = false;
        SceneManager.LoadScene(sceneToLoad);         
    }
}