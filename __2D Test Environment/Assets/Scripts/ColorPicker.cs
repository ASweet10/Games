using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour
{
    public UnityEvent<Color> ColorPickerEvent;

    public float currentHue, currentSat, currentVal;
    [SerializeField] RawImage hueImage, satImage, outputImage;
    [SerializeField] Slider hueSlider;
    [SerializeField] TMP_InputField hexInputField;

    private Texture2D hueTexture, satTexture, outputTexture;
    [SerializeField] SpriteRenderer changeThisColor;

    private void Start() {
        CreateHueImage();

        CreateSVImage();

        CreateOutputImage();

        UpdateOutputImage();
    }

    void CreateHueImage(){
        hueTexture = new Texture2D(1, 16);
        hueTexture.wrapMode = TextureWrapMode.Clamp;
        hueTexture.name = "HueTexture";

        for(int i = 0; i < hueTexture.height; i++){
            hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / hueTexture.height, 1, 1));
        }

        hueTexture.Apply();
        currentHue = 0;
        hueImage.texture = hueTexture;
    }

    void CreateSVImage(){
        satTexture = new Texture2D(16, 16);
        satTexture.wrapMode = TextureWrapMode.Clamp;
        satTexture.name = "SatValTexture";

        for(int y = 0; y < satTexture.height; y++){
            for(int x = 0; x < satTexture.width; x++){
                satTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, (float) x / satTexture.width, (float) y / satTexture.height));
            }
        }

        satTexture.Apply();
        currentSat = 0;
        currentVal = 0;

        satImage.texture = satTexture;
    }

    void CreateOutputImage(){
        outputTexture = new Texture2D(1, 16);
        outputTexture.wrapMode = TextureWrapMode.Clamp;
        outputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for(int i = 0; i < outputTexture.height; i++){
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        outputImage.texture = outputTexture;
    }

    public void UpdateOutputImage(){
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for(int i = 0; i < outputTexture.height; i++){
            outputTexture.SetPixel(0, i, currentColor);
        }

        outputTexture.Apply();

        hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);

        changeThisColor.GetComponent<SpriteRenderer>().material.color = currentColor;
    }

    public void SetSV(float s, float v){
        currentSat = s;
        currentVal = v;

        UpdateOutputImage();
    }

    public void UpdateSVImage(){
        currentHue = hueSlider.value;
        for(int y = 0; y < satTexture.height; y ++){
            for(int x = 0; x < satTexture.width; x ++){
                satTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, (float) x / satTexture.width, (float) y / satTexture.height));
            }
        }

        satTexture.Apply();

        UpdateOutputImage();
    }

    public void OnTextInput(){
        //Break out of function if less than 6 digits entered
        if(hexInputField.text.Length < 6) { return; }

        Color newColor;

        if(ColorUtility.TryParseHtmlString("#" + hexInputField.text, out newColor)){
            Color.RGBToHSV(newColor, out currentHue, out currentSat, out currentVal);
        }

        hueSlider.value = currentHue;

        hexInputField.text = "";
        
        UpdateOutputImage();
    }
}
