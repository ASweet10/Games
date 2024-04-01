using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextColorLerper : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color startColor = new Color(1f, 1f, 1f, 1f);
    Color endColor = new Color(1f, 1f, 1f, 0.3f);
    float duration = 5f;
    float t = 0f;
    void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Update() {
        LerpColor();
    }

    void LerpColor() {
        Color newColor = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time, 1));
        spriteRenderer.material.color = newColor;
        Debug.Log(spriteRenderer.color.a);
    }
}
