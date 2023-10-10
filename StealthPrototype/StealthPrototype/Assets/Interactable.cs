using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract allows incomplete data; Child classes define their behavior
public abstract class Interactable : MonoBehaviour
{
    //Virtual allows child classes to override this method
    public virtual void Awake(){
        gameObject.layer = 9;
    }

    public abstract void OnInteract();
    public abstract void OnFocus();
    public abstract void OnLoseFocus();
}
