using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] bool ikActive = false;
    [SerializeField] float lookWeight;
    [SerializeField] float desireDist;
    [SerializeField] Transform targetObj;
    GameObject objPivot;
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        objPivot = new GameObject("DummyPivot");
        objPivot.transform.SetParent(gameObject.transform);
        objPivot.transform.localPosition = new Vector3(0, 4.5f, 0);
    }

    void Update() {
        objPivot.transform.LookAt(targetObj);
        float pivotRotationY = objPivot.transform.localRotation.y;
        //Debug.Log(pivotRotationY);

        float dist = Vector3.Distance(objPivot.transform.position, targetObj.position);
        Debug.Log(dist);

        if(pivotRotationY < 0.65f && pivotRotationY > -0.65f && dist < desireDist) {
            lookWeight = Mathf.Lerp(lookWeight, 1, Time.deltaTime * 2.5f);
            //lookWeight = 1;
            ikActive = true;
        } else {
            lookWeight = Mathf.Lerp(lookWeight, 0, Time.deltaTime * 2.5f);
            //lookWeight = 0;
            ikActive = false;
        }
        animator.SetLookAtWeight(lookWeight);
        Debug.Log(lookWeight);
    }

    void OnAnimatorIK() {
        if(animator) {
            if(ikActive) {
                if(targetObj != null ) {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(targetObj.position);
                }
            }
            else {
                animator.SetLookAtWeight(0);
            }
        }
    }
}
