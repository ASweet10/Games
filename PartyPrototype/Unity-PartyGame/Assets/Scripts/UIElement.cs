using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum GameType {snowball, stealth, mariokart};
    public GameType gameType;
    [SerializeField] private Image gameImage;
    [Header("Game Pictures")]
    [SerializeField] private Sprite placeholderPicture;
    [SerializeField] private Sprite snowballGamePicture;

    private bool mouseOver = false;
    void Update() {
        if (mouseOver) {
            switch (gameType) {
                case GameType.snowball:
                    gameImage.sprite = snowballGamePicture;
                    break;
            }
        } 
        else {
            gameImage.sprite = placeholderPicture;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        Debug.Log("Mouse exit");
    }
}
