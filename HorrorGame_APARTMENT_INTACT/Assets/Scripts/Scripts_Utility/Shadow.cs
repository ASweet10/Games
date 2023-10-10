using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public void DisableShadowAfterMovement()
    {
        gameObject.SetActive(false);
    }
}
