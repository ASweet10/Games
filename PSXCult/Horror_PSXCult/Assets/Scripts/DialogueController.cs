using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] FirstPersonController fpController;
    [SerializeField] MouseLook mouseLook;

    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI speakerText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI pressAnyButtonText;

    [Header("Options")]
    [SerializeField] TextMeshProUGUI optionOneText;
    [SerializeField] TextMeshProUGUI optionTwoText;
    [SerializeField] TextMeshProUGUI optionThreeText;
    [SerializeField] TextMeshProUGUI optionOneBtn;
    [SerializeField] TextMeshProUGUI optionTwoBtn;
    [SerializeField] TextMeshProUGUI optionThreeBtn;

    [Header("Speed")]
    [SerializeField] float timeBetweenChars;
    [SerializeField] float timeBetweenWords;
    [SerializeField] string gasDialogueOne = "Can I get $30 on pump 4?";
    [SerializeField] string[] cashierDialogue = 
    { 
        "Sure thing.",
        "I'm watching you so don't try to steal anything.",
        "Where are you headed?", // Gas options 1 here
    };

    [SerializeField] string[] gasOptionsOne = 
    {
        "Why do you want to know?",
        "Mind your business old man",
        "I'm going camping at Hyde Lake"
    };
    [SerializeField] string cashierStealDialogue = "Hey! You have to pay for that!";

    int dialogueIndex = 0;
    int i = 0;

    void Update() {
        if(pressAnyButtonText.enabled){
            if (Input.anyKeyDown) {
                HandleNextDialogue();
            }
        }
    }

    public void OpenGasDialog() {
        fpController.canMoveRef = false;
        mouseLook.canRotateMouseRef = false;
        dialogueUI.SetActive(true);
        speakerText.text = "Cashier";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EndCheck();
    }

    void EndCheck() {
        if(i <= cashierDialogue.Length - 1 ) {
            dialogueText.text = cashierDialogue[dialogueIndex];
            StartCoroutine(ScrollTextLetterByLetter());
        }
    }

    IEnumerator ScrollTextLetterByLetter() {
        dialogueText.ForceMeshUpdate();
        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        int counter = 0;

        while (true) {
            int visiblecount = counter % (totalVisibleCharacters + 1);
            dialogueText.maxVisibleCharacters = visiblecount;

            if(visiblecount >= totalVisibleCharacters) {
                i += 1;
                Invoke("EndCheck", timeBetweenWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBetweenChars);
        }
        pressAnyButtonText.enabled = true;
    }

    public void HandleNextDialogue() {
        pressAnyButtonText.enabled = false;
        dialogueIndex ++;
        ScrollTextLetterByLetter();
    }

    public void CloseDialogue() {
        //speakerText.text = "";
        //dialogueText.text = "";
        dialogueUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        fpController.canMoveRef = true;
        mouseLook.canRotateMouseRef = true;
    }

    void DisplayOptions(string[] options) {
        optionOneText.text = options[0];
        optionTwoText.text = options[1];
        optionThreeText.text = options[2];
    }

    public void ZoomInOnSpeaker() {
        
    }
}
