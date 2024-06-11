using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Placed on dialogue response buttons
//  Enables image on hover
public class DialogueButtonHover: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image buttonImage;
    public void OnPointerEnter(PointerEventData pointerEventData) {
        buttonImage.enabled = true;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData) {
        buttonImage.enabled = false;
    }
}
