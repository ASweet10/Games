using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fade object alpha based on proximity
public class FadeObjectAlpha : MonoBehaviour
{
    [SerializeField] GameObject objectToFade;
    [SerializeField] GameObject player;
    FadeObjectAlpha fadeObjectAlpha;
 
    [SerializeField] float objectProximity = 15f;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] Material materialToFade;
    Vector3 playerPosition;

    void Start()
    {
        meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        fadeObjectAlpha = gameObject.GetComponent<FadeObjectAlpha>();
    }

    void Update ()
    {
        playerPosition = player.transform.position;

        if(Vector3.Distance(playerPosition, objectToFade.transform.position) <= 2)
        {
            fadeObjectAlpha.enabled = false;
        }
        FadeInAlphaByDistance();

    }
    void FadeOutAlphaByDistance()
    {
        Color tempColor = materialToFade.color;
        tempColor.a = 1 - (Mathf.InverseLerp (objectProximity, 0.1f,
        Vector3.Distance (playerPosition, objectToFade.transform.position)));
        FadeMultipleObjects(tempColor);
    }
    void FadeInAlphaByDistance()
    {
        Color tempColor = materialToFade.color;
        float distance = Vector3.Distance(objectToFade.transform.position, playerPosition);
        tempColor.a = objectProximity / distance;
        FadeMultipleObjects(tempColor);
    }

    //Change all mesh renderer object alphas at once
    void FadeMultipleObjects(Color newColor)
    {
        foreach (MeshRenderer meshRend in meshRenderers)
        {
            meshRend.material.color = newColor;
        }
    }
}
