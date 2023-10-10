using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaturationColorControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] Image cursorImage;
    RawImage svImage;
    ColorPicker colorPicker;
    RectTransform rectTransform, cursorTransform;

    private void Awake() {
        svImage = GetComponent<RawImage>();
        colorPicker = FindObjectOfType<ColorPicker>();
        rectTransform = GetComponent<RectTransform>();

        cursorTransform = cursorImage.GetComponent<RectTransform>();
        cursorTransform.position = new Vector2(-(rectTransform.sizeDelta.x * 0.5f), -(rectTransform.sizeDelta.y * 0.5f));
    }

    void UpdateColor(PointerEventData eventData){
        Vector3 position = rectTransform.InverseTransformPoint(eventData.position);

        float deltaX = rectTransform.sizeDelta.x * 0.5f;
        float deltaY = rectTransform.sizeDelta.y * 0.5f;

        if(position.x < -deltaX){
            position.x = -deltaX;
        }
        else if(position.x > deltaX){
            position.x = deltaX;
        }

        if(position.y < -deltaY){
            position.y = -deltaY;
        }
        else if(position.y > deltaY){
            position.y = deltaY;
        }

        float x = position.x + deltaX;
        float y = position.y + deltaY;

        float xNormal = x / rectTransform.sizeDelta.x;
        float yNormal = y / rectTransform.sizeDelta.y;

        cursorTransform.localPosition = position;
        cursorImage.color = Color.HSVToRGB(0, 0, 1 - yNormal);

        colorPicker.SetSV(xNormal, yNormal);
    }

    public void OnDrag(PointerEventData eventData){
        UpdateColor(eventData);
    }

    public void OnPointerClick(PointerEventData eventData){
        UpdateColor(eventData);
    }
}
