using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector3 worldMousePosition;
    Vector2 shootDirection;
    void Update()
    {
        GetMousePosition();
    }
    public Vector2 GetMousePosition()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shootDirection = (Vector2)(worldMousePosition - transform.position);
        shootDirection.Normalize();
        return shootDirection;
    }

}
