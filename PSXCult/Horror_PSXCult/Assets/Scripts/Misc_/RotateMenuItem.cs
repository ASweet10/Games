using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMenuItem : MonoBehaviour
{
    Transform tf;
    void Awake() {
        tf = gameObject.GetComponent<Transform>();
    }

    void Update() {
        RotateItem();
    }
    void RotateItem() {
        tf.Rotate(new Vector3(0f, 90f * Time.deltaTime * 0.5f, 0f));
    }
}
