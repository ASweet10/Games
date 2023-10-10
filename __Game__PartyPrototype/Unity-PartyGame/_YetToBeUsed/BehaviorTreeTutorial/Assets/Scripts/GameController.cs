using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Text popupText;
    [SerializeField] Text newObjectiveText;
    [SerializeField] Text objectiveOneText;
    [SerializeField] Text objectiveTwoText;
    [SerializeField] Text objectiveThreeText;
    [SerializeField] Text objectiveFourText;
    [SerializeField] Text endingUIText;
    [SerializeField] Text taskListText;

    AudioSource audioSource;
    public int currentCheckpoint;
    public bool canLeaveByCar = false;
    public bool playerNeedsKey = true;
    public bool playerHasFuse = false;
    
    public bool fuseLightsOff = false;

    public bool fuseBoxFixed = false;

    public bool engagedInTask = false;

    public bool holdingTrash = false;
    public bool sinkCompleted = false;
    public bool lightCompleted = false;
    public bool trashCompleted = false;

    /*
    public enum Checkpoint {MainPaper, Toolbox, FixProblems}
    public Checkpoint checkPoint; 
    */
    string closeUpForNightString = "Turn off breaker and lock up before you leave";

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    void Start()
    {
        currentCheckpoint = 0;
    }

    void Update()
    {
        DetermineObjectiveText();
        Debug.Log("chkpt: " + currentCheckpoint);
    }

    public void DetermineObjectiveText()
    {
        if(currentCheckpoint == 0)
        {
            objectiveOneText.text = "Get task list from head office";
        }
        else if(currentCheckpoint == 1)
        {
            objectiveOneText.text = "Grab toolbox left for you in office";
        }
        else if(currentCheckpoint == 2)
        {
            objectiveOneText.text = "Fix problems in apartment 3B";
        }
        else if(currentCheckpoint == 3)
        {
            objectiveOneText.text = "Take trash to garbage chute down the hall";
        }
        else if(currentCheckpoint == 4)
        {
            objectiveOneText.text = "Find ladder to reach light";
        }
        else if(currentCheckpoint == 5)
        {
            objectiveOneText.text = "Replace flickering bulb in bedroom";
        }
        else if(currentCheckpoint == 6)
        {
            objectiveOneText.text = "Fix leaky sink in bathroom";
        }
        else if(currentCheckpoint == 7)
        {
            objectiveOneText.text = "Find replacement fuse and restore power";
        }

        else if(currentCheckpoint == 8)
        {
            ShowPopupMessage(closeUpForNightString, 3.5f, true);
            objectiveOneText.text = "Turn off breaker and lock up";
        }
        else if(currentCheckpoint == 9)
        {
            objectiveOneText.text = "Escape!";
        }
    }

    public void ShowPopupMessage(string message, float delay, bool showObjectiveText)
    {
        StartCoroutine(DisplayMessage(message, delay, showObjectiveText));
    }
    IEnumerator DisplayMessage(string message, float delay, bool showObjectiveText)
    {
        popupText.text = message;
        popupText.enabled = true;
        if(showObjectiveText == true)
        {
            newObjectiveText.enabled = true;
        }
        yield return new WaitForSeconds(delay);
        if(showObjectiveText == true)
        {
            newObjectiveText.enabled = false;
        }
        popupText.enabled = false;
    }
    /*
    public void ChangeCheckpointEnum(string newCheckpoint)
    {

    }
    public string ReturnCurrentCheckpoint()
    {
        return 
    }
    */
}
