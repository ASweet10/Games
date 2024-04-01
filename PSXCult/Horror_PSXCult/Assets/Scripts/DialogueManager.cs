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

    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float turnSpeed = 2f;

    List<dialogueString> dialogueList;

    [Header("Player")]
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] MouseLook mouseLook;

    int currentDialogueIndex = 0;
    bool optionSelected = false;

    void Start() {
        dialogueParent.SetActive(false); // Hide dialogue by default
    }
    void Update() {
        //Debug.Log("dialogue index: " + currentDialogueIndex);
    }

    public void DialogueStart(List<dialogueString> textToPrint, string speakerName) {
        dialogueParent.SetActive(true);
        speakerText.text = speakerName;

        firstPersonController.enabled = false;
        mouseLook.canRotateMouseRef = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        dialogueList = textToPrint;
        currentDialogueIndex = 0;

        DisableButtons();
        StartCoroutine(PrintDialogue());
    }

    IEnumerator PrintDialogue() {
        while (currentDialogueIndex < dialogueList.Count) {
            dialogueString line = dialogueList[currentDialogueIndex];

            line.startDialogueEvent?.Invoke();

            if (line.isQuestion) {
                yield return StartCoroutine(TypeText(line.text));

                option1Button.interactable = true;
                option2Button.interactable = true;
                option3Button.interactable = true;

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
    }

    void DialogueStop() {
        StopAllCoroutines();
        Debug.Log("stop");
        dialogueText.text = "";
        dialogueParent.SetActive(false);

        firstPersonController.enabled = true;
        mouseLook.canRotateMouseRef = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
