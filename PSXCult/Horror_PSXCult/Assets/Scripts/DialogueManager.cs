using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueParent;
    [SerializeField] TMP_Text speakerText;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Button option1Button;
    [SerializeField] Button option2Button;
    [SerializeField] Button option3Button;
    [SerializeField] Image option1Image;
    [SerializeField] Image option2Image;
    [SerializeField] Image option3Image;

    [SerializeField] float typingSpeed = 0.05f;

    List<dialogueString> dialogueList;

    [Header("Player")]
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] MouseLook mouseLook;

    int currentDialogueIndex = 0;
    bool optionSelected = false;

    bool hasPurchasedGas = false;
    bool hunterWarningComplete = false;
    bool holdingGasItem = false;
    bool playerCaughtStealing = false;

    void Start() {
        dialogueParent.SetActive(false); // Hide dialogue by default
    }
    public void DialogueStart(List<dialogueString> textToPrint, string speakerName) {
        dialogueParent.SetActive(true);
        speakerText.text = speakerName;

        firstPersonController.enabled = false;
        mouseLook.CanRotateMouse = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        dialogueList = textToPrint;
        
        switch(speakerName) {
            case "Cashier":
                if(hasPurchasedGas) {
                    if(holdingGasItem) {
                        if(playerCaughtStealing) {
                            currentDialogueIndex = 9; // Steal options
                        } else {
                            currentDialogueIndex = 11; // Buy item options
                        }
                    } else {
                        currentDialogueIndex = 10; // No item options
                    }
                } else {
                    currentDialogueIndex = 0; // Can I help you?
                }
                break;
            case "AJ":
                break;
            case "David":
                break;
            case "Hunter":
                if(hunterWarningComplete) {
                    currentDialogueIndex = 11; // Random hunter options
                    Debug.Log(11);
                } else {
                    currentDialogueIndex = 0; // Hunter warning start
                    Debug.Log(0);
                }
                break;
        }

        DisableButtons();
        StartCoroutine(PrintDialogue());
    }

    IEnumerator PrintDialogue() {
        while (currentDialogueIndex < dialogueList.Count) {
            dialogueString line = dialogueList[currentDialogueIndex];

            line.startDialogueEvent?.Invoke();

            if(line.isRandomOption) {
                yield return StartCoroutine(TypeText(line.randomOptions[Random.Range(0, line.randomOptions.Length)]));
            }

            if (line.isQuestion) {
                yield return StartCoroutine(TypeText(line.text));

                // check number of answers? (put in array instead of 3 strings)
                // Only one box appears / is interactable if only one answer etc.
                option1Button.interactable = true;
                option2Button.interactable = true;
                option3Button.interactable = true;

                option1Button.GetComponentInChildren<DialogueButtonHover>().enabled = true;
                option2Button.GetComponentInChildren<DialogueButtonHover>().enabled = true;
                option3Button.GetComponentInChildren<DialogueButtonHover>().enabled = true;

                option1Button.GetComponentInChildren<TMP_Text>().text = line.answerOption1;
                option2Button.GetComponentInChildren<TMP_Text>().text = line.answerOption2;
                option3Button.GetComponentInChildren<TMP_Text>().text = line.answerOption3;

                option1Button.onClick.AddListener(() => HandleOptionSelected(line.option1IndexJump));
                option2Button.onClick.AddListener(() => HandleOptionSelected(line.option2IndexJump));
                option3Button.onClick.AddListener(() => HandleOptionSelected(line.option3IndexJump));

                yield return new WaitUntil(() => optionSelected);
            
            } else {
                yield return StartCoroutine(TypeText(line.text));
            }

            line.endDialogueEvent?.Invoke();
            optionSelected = false;
        }

        DialogueStop();
    }
    private IEnumerator TypeText(string text) {
        dialogueText.text = "";
        foreach(char letter in text.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        if(!dialogueList[currentDialogueIndex].isQuestion) {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        if(dialogueList[currentDialogueIndex].isEnd) {
            DialogueStop();
        }

        currentDialogueIndex++;
    }
    void HandleOptionSelected(int indexJump) {
        optionSelected = true;
        DisableButtons();

        currentDialogueIndex = indexJump;
    }
    void DisableButtons() {
        option1Button.interactable = false;
        option2Button.interactable = false;
        option3Button.interactable = false;

        option1Button.GetComponentInChildren<TMP_Text>().text = "";
        option2Button.GetComponentInChildren<TMP_Text>().text = "";
        option3Button.GetComponentInChildren<TMP_Text>().text = "";

        option1Button.GetComponentInChildren<DialogueButtonHover>().enabled = false;
        option2Button.GetComponentInChildren<DialogueButtonHover>().enabled = false;
        option3Button.GetComponentInChildren<DialogueButtonHover>().enabled = false;

        option1Image.enabled = false;
        option2Image.enabled = false; 
        option3Image.enabled = false;     
    }

    public void DialogueStop() {
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueParent.SetActive(false);

        firstPersonController.enabled = true;
        mouseLook.CanRotateMouse = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PurchaseGas() {
        hasPurchasedGas = true;
    }
    public void FinishHunterWarning() {
        hunterWarningComplete = true;
    }
}
