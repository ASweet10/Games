using System.Collections;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] Animator anim;
    Transform playerTF;
    Transform tf;
    AudioSource dogAudio;
    Vector3 startRotation = new Vector3(0, 90, 0);

    public enum State{ barking, sitting };
    public State state = State.sitting;
    void Start() {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        tf = GetComponent<Transform>();
        dogAudio = GetComponent<AudioSource>();
    }

    void Update() {
        switch (state) {
            case State.barking:
                anim.SetBool("sitting", false);
                anim.SetBool("barking", true);
                tf.LookAt(playerTF.position);
                tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

                StartCoroutine(WaitToBark());

                StartCoroutine(WaitAndChangeState(3, State.sitting));
                break;
            case State.sitting:
                anim.SetBool("barking", false);
                anim.SetBool("sitting", true);
                                
                if(Vector3.Distance(tf.eulerAngles, startRotation) > 0.01f) {
                    tf.eulerAngles = Vector3.Lerp(tf.rotation.eulerAngles, startRotation, Time.deltaTime);
                } else {
                    tf.eulerAngles = startRotation;
                }
                break;
            default:
                break;
        }
    }
    IEnumerator WaitToBark() {
        yield return new WaitForSeconds(1.5f);
        if (!dogAudio.isPlaying) {
            dogAudio.Play();
        }
    }
    IEnumerator WaitAndChangeState(float delay, State newState) {
        yield return new WaitForSeconds(delay);
        state = newState;
    }
}