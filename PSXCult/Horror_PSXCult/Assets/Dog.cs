using System.Collections;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] Animator anim;
    Transform playerTF;
    Transform tf;
    AudioSource dogAudio;

    public enum State{ idle, barking, eating, sitting, walkingToFood, walkingToIdleSpot, followingPlayer };
    public State state = State.idle;
    void Start() {
        playerTF = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        tf = GetComponent<Transform>();
        dogAudio = GetComponent<AudioSource>();
    }

    void Update() {
        HandleDogAIBehavior();
    }

    void HandleDogAIBehavior() {
        switch (state) {
            case State.idle:
                HandleIdle();
                //StartCoroutine(TransitionAfterDelay(State.idle, 5));
                break;
            case State.barking:
                HandleBarking();
                //StartCoroutine(TransitionAfterDelay(State.barking, 2));
                break;
            case State.eating:
                HandleEating();
                //StartCoroutine(TransitionAfterDelay(State.eating, 3));
                break;
            case State.sitting:
                HandleSitting();
                //StartCoroutine(TransitionAfterDelay(State.sitting, 10));
                break;
            case State.walkingToFood:
                break;
            case State.walkingToIdleSpot:
                break;
            case State.followingPlayer:
                if (Vector3.Distance(playerTF.position, tf.position) > 2f) {

                }
                break;
            default:
                break;
        }
    }

    void HandleIdle() {
        anim.SetBool("eating", false);
        anim.SetBool("walking", false);
        anim.SetBool("barking", false);
        anim.SetBool("sitting", false);

        anim.SetBool("idle", true);
        StartCoroutine(WaitForDelay(10, State.sitting));
    }
    void HandleBarking() {
        anim.SetBool("eating", false);
        anim.SetBool("walking", false);
        anim.SetBool("sitting", false);
        anim.SetBool("idle", false);
        
        anim.SetBool("barking", true);
        tf.LookAt(playerTF.position);
        tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

        if (!dogAudio.isPlaying) {
            dogAudio.Play();
        }
        StartCoroutine(WaitForDelay(3, State.idle));
    }
    void HandleEating() {
        anim.SetBool("sitting", false);
        anim.SetBool("walking", false);
        anim.SetBool("barking", false);
        anim.SetBool("idle", false);

        anim.SetBool("eating", true);
        StartCoroutine(WaitForDelay(4, State.idle));
    }
    void HandleSitting() {
        anim.SetBool("eating", false);
        anim.SetBool("walking", false);
        anim.SetBool("barking", false);
        anim.SetBool("idle", false);

        anim.SetBool("sitting", true);
        StartCoroutine(WaitForDelay(10, State.idle));
    }
    IEnumerator WaitForDelay(float delay, State newState) {
        yield return new WaitForSeconds(delay);
        state = newState;
        /*
        if(currentState == State.eating) {
            anim.SetBool("idle", false);
            anim.SetBool("walking", false);
            anim.SetBool("eating", true);

            yield return new WaitForSeconds(delay);
            state = State.idle;
        } else if(currentState == State.barking) {
            tf.LookAt(playerTF.position);
            tf.localEulerAngles = new Vector3(0f, tf.localEulerAngles.y, tf.localEulerAngles.z);

            anim.SetBool("idle", false);
            anim.SetBool("eating", false);
            anim.SetBool("walking", false);
            anim.SetBool("barking", true);

            dogAudio.Play();
            yield return new WaitForSeconds(delay);
            state = State.idle;
        } else if(currentState == State.idle) {
            yield return new WaitForSeconds(delay);
            state = State.sitting;
        } else if(currentState == State.sitting) {
            anim.SetBool("eating", false);
            anim.SetBool("walking", false);
            anim.SetBool("idle", false);
            anim.SetBool("barking", false);
            anim.SetBool("sitting", true);

            yield return new WaitForSeconds(delay);
            state = State.idle;
        }
        */
    }
}