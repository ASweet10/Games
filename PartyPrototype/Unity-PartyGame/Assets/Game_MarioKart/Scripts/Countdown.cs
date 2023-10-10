using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshPro countdownText;
    public UnityEvent StartCountdown;
    public UnityEvent StartRace;
    private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

    private void Awake() {
        if(countdownText == null) {
            countdownText = gameObject.GetComponent<TextMeshPro>();
        }
    }
    private IEnumerator Start()
    {
        StartCountdown.Invoke();
        countdownText.SetText("3");
        yield return waitOneSecond;
        countdownText.SetText("2");
        yield return waitOneSecond;
        countdownText.SetText("1");
        yield return waitOneSecond;
        countdownText.SetText("GO!");
        StartRace.Invoke();
        yield return waitOneSecond;
        countdownText.gameObject.SetActive(false);
    }

}
