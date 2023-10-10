using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snowball : MonoBehaviour
{
    [SerializeField] private TMP_Text sizeText;
    [SerializeField] private Vector3 targetScale = new Vector3(125f, 125f, 125f);
    [SerializeField] private LayerMask groundLayer;
    private Vector3 scaleChange = new Vector3(25f, 25f, 25f);
    private float distanceToGround;
    private bool grounded;
    private float size;

    void Awake() {

    }
    void Update() {
        /*
        Debug.Log(grounded);
        if(grounded) {
            HandleSizeChange();
        }
        */
        HandleSizeChange();
        HandleSnowballUI();
        Debug.Log(transform.localScale.x);
    }
    private void OnCollisionEnter(Collision col) {
        if(col.collider.gameObject.layer == groundLayer) {
            grounded = true;
        }
    }

    private bool CheckGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
    }

    private void HandleSizeChange() {
        if(transform.localScale.x <= targetScale.x) {
            transform.localScale += scaleChange * Time.deltaTime;
        }
    }
    private void HandleSnowballUI() {
        size = Mathf.RoundToInt(transform.localScale.x);
        sizeText.text = size.ToString() + "m";
    }
}
