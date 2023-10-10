using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class MonsterAI : MonoBehaviour
{
    [Header ("Distortion")]
    //From forum post
    //https://forum.unity.com/threads/access-renderer-feature-settings-at-runtime.770918/
    //[SerializeField] private ForwardRendererData rendererData;
    [SerializeField] ScriptableRendererFeature distortFeature;
    [SerializeField] Material distortMat;
    [SerializeField] float distortRange = 40f;
    float distortStrength = 0f;

    [Header ("General")]
    [SerializeField] GameObject player;
    [SerializeField] Transform[] waypoints;
    [SerializeField] LevelController levelController;
    [SerializeField] PlayerController playerController;
    [SerializeField] FlashlightToggle flashlightToggle;


    [SerializeField] float catchRange = 5f;
    [SerializeField] float awarenessRange = 10f;
    [SerializeField] float chaseSpeed = 4f;

    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource heartBeatAudioSource;
    [SerializeField] AudioClip catchAudio;

    public bool awareOfPlayer = false;
    public bool playerIsHiding = false;
    public bool playerCanHide = true;
    
    NavMeshAgent agent;
    float distance;
    float catchTimer;
    AudioSource audioSource;
    int destinationIndex = 0;
    bool monsterCanMove = true;
    bool isPatrolling = true;
    bool isChasing = false;
    bool playerCaught = false;


    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        audioSource = gameObject.GetComponent<AudioSource>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if(monsterCanMove)
        {
            HandleMonsterLogic();
        }
    }

    void HandleMonsterLogic() {        
        
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        
        //DistortScreenByProximity();
        CheckForFlashlight(distance);

        if(distance <= awarenessRange) {
            playerCanHide = false;
            awareOfPlayer = true;
        }

        if(playerIsHiding) {
            awareOfPlayer = false;
        }

        if(awareOfPlayer)
        {
            agent.speed = chaseSpeed;
            isChasing = true;
            isPatrolling = false;
            FollowPlayer();
            AttemptToCatchPlayer();
        }
        else
        {
            //Change state from chase to patrol
            if(isChasing)
            {
                isChasing = false;
                isPatrolling = true;
                destinationIndex = ChooseNextPatrolPointByProximity();
            }
            agent.speed = 2f;
            //Choose next point when agent gets close enough to current target
            if(!agent.pathPending && agent.remainingDistance <= 0.1f)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    void FollowPlayer()
    {
        agent.destination = player.transform.position;
        
        musicAudioSource.Pause();
        if(!heartBeatAudioSource.isPlaying)
        {
            heartBeatAudioSource.Play();
        }
    }

    private void AttemptToCatchPlayer() {
        if(distance <= catchRange)
        {
            catchTimer += Time.deltaTime;
            DistortScreenByCatchTime(catchTimer);
            if(catchTimer >= 2f)
            {
                if(!playerCaught)
                {
                    StartCoroutine(CatchPlayer());
                }

            }
        }
        else
        {
            distortFeature.SetActive(false);
            catchTimer = 0f;
        }
    }

    private void DistortScreenByCatchTime(float time) {
        if(distance <= distortRange)
        {
            distortFeature.SetActive(true);
            distortStrength = (Mathf.InverseLerp (0f, 0.1f, time));

            if(distortStrength >= 0.1f)
            {
                distortStrength = 0.03f;
            }
            distortMat.SetFloat("_DistortStrength", distortStrength);
        }
        else
        {
            distortFeature.SetActive(false);
        }
    }

    int ChooseNextPatrolPointByProximity()
    {
        float currentDistance = Mathf.Infinity;
        float distanceBetween;
        int closestWaypointIndex = 0;

        for(int i = 0; i < waypoints.Length; i++)
        {
            distanceBetween = Vector3.Distance(gameObject.transform.position, waypoints[i].position);
            if(distanceBetween <= currentDistance)
            {
                currentDistance = distanceBetween;
                closestWaypointIndex = i;
            }
        }
        return closestWaypointIndex;
    }

    //Get next waypoint in array and move agent there
    //From documentation: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
    void GoToNextPatrolPoint()
    {
        //heartBeatAudioSource.Stop();
        agent.destination = waypoints[destinationIndex].position;

        //Increment index after movement
        destinationIndex += 1;

        if(destinationIndex == waypoints.Length)
        {
            destinationIndex = 0;
        }
    }

    void CheckForFlashlight(float distance)
    {
        if(flashlightToggle.ReturnLightStatus() == true)
        {
            if(distance <= awarenessRange)
            {
                awareOfPlayer = true;
            }
        } 
    }

    //Change this to an event?
    IEnumerator CatchPlayer()
    {
        playerCaught = true;
        playerController.ToggleMovement(false);
        heartBeatAudioSource.Stop();

        audioSource.Stop();

        audioSource.clip = catchAudio;
        audioSource.volume = 0.7f;
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        levelController.FadeToBlack();
        yield return new WaitForSeconds(2f);
        levelController.LoadLevel(3);
    }
}
