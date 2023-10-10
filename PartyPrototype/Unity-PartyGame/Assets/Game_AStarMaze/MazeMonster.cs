using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Concat: combine arrays

public class MazeMonster : MonoBehaviour
{
    public Transform target;
    Vector3 endPosition;
    float moveSpeed = 1f;
    float rotationSpeed = 1f;
    float checkRadius = 5f;
    Vector3[] path;
    Vector3 currentWaypoint;
    int targetIndex;
    int isWalkingHash;
    bool isMoving;
    CharacterController characterController;
    Animator anim;
    [SerializeField] AudioSource caughtAudio;
    AudioSource footstepAudio;
    void Awake(){
        characterController = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
        footstepAudio = gameObject.GetComponent<AudioSource>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }
    void Start(){
        endPosition = target.position;
        PathRequestManager.RequestPath(transform.position, endPosition, OnPathFound);
    }

    void Update(){
        if(target.position != endPosition){
            Debug.Log("plotting new path...");
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            endPosition = target.position;
        }
        if(Vector3.Distance(transform.position, endPosition) <= 3f){
            isMoving = false;
            StopCoroutine("FollowPath");
            CatchPlayer();
        }
        HandleAnimation();
    }
    
    void HandleAnimation(){
        if(isMoving){
            anim.SetBool(isWalkingHash, true);
            if(!footstepAudio.isPlaying){
                //footstepAudio.Play();
            }
        } else{
            anim.SetBool(isWalkingHash, false);
            //footstepAudio.Pause();
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful){
        if(pathSuccessful){
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath(){
        if(path.Length != 0){
            currentWaypoint = path[0];
        }

        while (true) {
            Vector3 offset = currentWaypoint - transform.position;
            if(transform.position == currentWaypoint){
                targetIndex++;
                if(targetIndex >= path.Length){
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, offset, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            
            if(transform.rotation == Quaternion.LookRotation(newDirection)){
                gameObject.transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed);
                //characterController.Move(offset * moveSpeed * Time.deltaTime);
                isMoving = true;
            }
            yield return null;
        }
    }

    void CatchPlayer() {
        footstepAudio.Stop();
        target.gameObject.SetActive(false);
        caughtAudio.Play();
        Debug.Log("caught");
    }


    //Called when trap triggered instead of continuously
    public void CheckForNewTarget(){
        //Cycle through all active players, get health script
        //Find lowest value and set maze target to that player
    }

    
    public void OnDrawGizmos() {
        if(path != null){
            for(int i = targetIndex; i < path.Length; i++){
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if(i == targetIndex){
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
