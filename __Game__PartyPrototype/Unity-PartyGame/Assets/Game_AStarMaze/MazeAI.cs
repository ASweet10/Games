using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeAI : MonoBehaviour
{
    Animator anim;
    [SerializeField] float speed = 0.05f;
    int isWalkingHash;
    bool readyToExecute = true;
    bool wallToRight = false;
    bool inFront = false;
    bool toRight = false;

    void Awake() {
        anim = gameObject.GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }
    void FixedUpdate() {
        //NavigateMaze();
        if(!inFront){
            StartCoroutine(MoveForward());
        } else {
            StopCoroutine(MoveForward());
        }

    }

    void NavigateMaze() {
        if(readyToExecute) {
            CheckSensors();
            readyToExecute = false;
        }
    }

    void CheckSensors() {

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 0.7f)){
            inFront = true;
            Debug.Log("Wall in front");
        }
        if(Physics.Raycast(transform.position, transform.right, out hit, 0.7f)){
            toRight = true;
            Debug.Log("wall to right");
        }

        if(!inFront && !toRight){
            if(!wallToRight){
                StartCoroutine(MoveForward());
            } else {
                StartCoroutine(TurnClockwise());
                wallToRight = false;
            }
        } else if(inFront){ //Wall in front
            StartCoroutine(TurnCounterClockwise());
            wallToRight = false;
        } else if(!inFront && toRight){ //Wall to right
            StartCoroutine(MoveForward());
            wallToRight = true;
        }
    }

    IEnumerator MoveForward(){
        transform.position = Vector3.MoveTowards(transform.position, transform.forward, speed);
        anim.SetBool(isWalkingHash, true);
        yield return null;
        SetReady();
    }
    IEnumerator TurnClockwise(){
        transform.Rotate(new Vector3(0f, 90f, 0f));
        anim.SetBool(isWalkingHash, false);
        yield return null;
        SetReady();
    }
    IEnumerator TurnCounterClockwise(){
        transform.Rotate(new Vector3(0f, -90f, 0f));
        anim.SetBool(isWalkingHash, false);
        yield return null;
        SetReady();
    }

    void SetReady(){
        if(!readyToExecute){
            readyToExecute = true;
        }
    }
}
