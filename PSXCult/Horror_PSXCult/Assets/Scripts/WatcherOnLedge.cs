using System.Collections;
using UnityEngine;

public class WatcherOnLedge : MonoBehaviour
{
    [SerializeField] Transform exitPoint;
    Animator anim;
    Transform tf;
    CharacterController controller;

    public enum State{ watching, leaving };
    public State state = State.watching;
    void Start() {
        tf = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        switch (state) {
            case State.watching:
                anim.SetBool("waiting", true);
                break;
            case State.leaving:
                anim.SetBool("waiting", false);
                StartCoroutine(WaitAndMove());
                /*
                if(Vector3.Distance(tf.eulerAngles, startRotation) > 0.01f) {
                    tf.eulerAngles = Vector3.Lerp(tf.rotation.eulerAngles, startRotation, Time.deltaTime);
                } else {
                    tf.eulerAngles = startRotation;
                }
                */
                break;
            default:
                break;
        }
    }
    IEnumerator WaitAndMove() {
        yield return new WaitForSeconds(1f);
        Vector3 exitPos = exitPoint.position - tf.position;
        exitPos = exitPos.normalized;

        controller.Move(exitPos * 5f * Time.deltaTime);
        yield return new WaitForSeconds(2f);
        //gameObject.SetActive(false);
        Debug.Log("inactive");
    }
}