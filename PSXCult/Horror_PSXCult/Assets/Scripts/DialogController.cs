using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] FirstPersonController fpController;
    [SerializeField] MouseLook mouseLook;

    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TextMeshProUGUI speakerText;
    [SerializeField] TextMeshProUGUI dialogText;
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
    [SerializeField] string[][] cashierDialogueWithHeldItem = {
         new string [] {"Cashier", "You sure you want that? All sales are final." }
    };
    [SerializeField] string[][] cashierDialogueWithNoHeldItem = {
        new string[] { 
            "Cashier",
            "I'm watching you so don't try to steal anything.",
            "I can't wait to get off and go bowling.",
            "Where did I leave those damn keys... Huh? What do you want?",
        }
    };
    //string[][] initialGasDialog = new string[5][];
    [SerializeField] string[][] initialGasDialog = {
        new string[] { "Player", "Can I get $30 on pump 4?" },
        new string[] { "Cashier", "Sure thing." }, // break here. Player pays, fade to black, fade back in & ask next question
        new string[] { "Cashier", "Where are you headed?" },
        new string[] { "Player", "Why do you want to know?", "Mind your business old man", "I'm going camping at Hyde Lake" },
    };

    [SerializeField] string[] gasOptionsOne = {
        "Player",
        "Why do you want to know?",
        "Mind your business old man",
        "I'm going camping at Hyde Lake"
    };
    [SerializeField] string[] cashierStealDialogue = { "Cashier", "Hey! You have to pay for that!" };

    // How to move control of conversation back and forth?
    // 1. Array of string arrays (All cashier & player dialog for this conversation); start index 0, 

    int i = 0;
    bool canReply = false;
    bool gasConversationOneDone =  false; // put this in game controller? check if this is done before user can interact with shelf items. buy gas first.
    
    void Start() {
        /*
        initialGasDialog[0] = playerBuyGasDialogOne;
        initialGasDialog[1] = cashierBuyGasDialogOne;
        initialGasDialog[2] = cashierBuyGasDialogTwo;
        */
    }
    
    void Update() {
        if(pressAnyButtonText.enabled){
            if (Input.anyKeyDown) {
                HandleNextDialogue();
            }
        }
    }

    public void OpenCashierDialog() { 
        // Currently can still move if holding key while press e. 
        // ice cream script has something like that
        fpController.canMoveRef = false;
        mouseLook.canRotateMouseRef = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        dialogueUI.SetActive(true);

        if(gasConversationOneDone) {
            if(fpController.playerHoldingGasItem) {
                // player tries to buy item; what about stealing?
                StartDialog(cashierDialogueWithHeldItem, false, true);
            } else {
                StartDialog(cashierDialogueWithNoHeldItem, true, false); // normal dialog, wish i could go bowling
            }
        } else {
            StartDialog(initialGasDialog, false, true);
        }
    }

    void StartDialog(string[][] dialog, bool random, bool conversation ) {
        // If random, choose one random option from index 1 -> length - 1
        // -Example: used when player pesters shop keeper without any item in hand
        
        // If conversation, more than 1 turn of dialog so control shifts
        // If not...
        // & random: Choose one random option to play then close dialog when done
        // if not...: Exhaust all dialog options then close dialog window on next button press

        // If requires input, must pass parameter as well
        // -Example: price of item player is holding and whether or not they can afford it

        //speakerText.text = speakerName;
        int dialogIndex = 0;
        if(i <= dialog.Length - 1 ) {
            dialogText.text = dialog[dialogIndex][dialogIndex];
            StartCoroutine(ScrollTextLetterByLetter());
        }
    }

    IEnumerator ScrollTextLetterByLetter() {
        dialogText.ForceMeshUpdate();
        int totalVisibleCharacters = dialogText.textInfo.characterCount;
        int counter = 0;

        while (true) {
            int visiblecount = counter % (totalVisibleCharacters + 1);
            dialogText.maxVisibleCharacters = visiblecount;

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
        //dialogIndex ++;
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

}
