using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestScript : MonoBehaviour
{
    [SerializeField] Animator anim;
    private void Awake() {
        anim = gameObject.GetComponent<Animator>();
    }
    public void ClickOnDummy(){
        anim.SetTrigger("Active");
    }
}
