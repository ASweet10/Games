using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerper : MonoBehaviour
{
    public float lerpSpeed = 2f;
    public Color startColor;
    public Color endColor = Color.red;
    Color lerpedColor;
    public SpriteRenderer ringRenderer;

    private void Start()
    {
        if(startColor == null)
        {
            startColor = Color.green;
        }
    }

    void Update()
    {
        LerpRingColor();
    }
    public void LerpRingColor()
    {
        //Lerp color between values on timer using PingPong
        lerpedColor = Color.Lerp(endColor, startColor, Mathf.PingPong(Time.time, lerpSpeed));
        ringRenderer.color = lerpedColor;
    }
}