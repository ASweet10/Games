using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private float missileSpeed = 45f;
    [SerializeField] private float searchRadius = 25f;

    [SerializeField] private GameObject target;
    bool foundTarget = false;

    private void Awake() {
        if(rb == null) {
            rb = gameObject.GetComponent<Rigidbody>();
        }
    }

    private void Start() {
        //shoot up function
        target = FindClosestTarget();
    }

    private void Update() {
        if (target != null) {
            foundTarget = true;
        }
        if(foundTarget) {
            gameObject.transform.LookAt(target.transform);
            StartCoroutine(SendHoming(target.transform));
        }
    }

    //1. spawn rocket
    //1.5 -Rockets shoot up to start while looking for target
    //2. If car is locked onto target, use that target
    //2b. - Otherwise, missile does check of all enemy vehicles in range
    //3. Find closest vehicle and supply as target

    private IEnumerator SendHoming(Transform target) {
    while(Vector3.Distance(target.transform.position, gameObject.transform.position) > 0.3f) {
        gameObject.transform.position += (target.transform.position - gameObject.transform.position).normalized * missileSpeed * Time.deltaTime;
        //gameObject.transform.LookAt(target.transform);

        Debug.Log("Homing...");
        yield return null;
    }
    Destroy(gameObject);

    Debug.Log("boom");
    }

    private GameObject FindClosestTarget() {
        GameObject target = null;
       float current = Mathf.Infinity;
       float dist = 0f;
       while(dist < current) {
        //Check each one in loop is within searchRadius
        //--If not, don't count its distance and move to next in queue
        //Run through all enemy vehicles within radius or FOV cone
        //If Vector3.Distance(target.transform.position - gameObject.transform.position)
        // is less than the current distance, that target is the new target
        current = dist;
       }

       return target;
    }


    private void FixedUpdate()
    {
        //rb.velocity = transform.up * missileSpeed * Time.deltaTime;
        //rb.AddForce(-transform.up * missileSpeed);
    }

    private void OnCollisionEnter(Collision other) {
        //Instantiate explosion at collision point
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        //
        //Damage, score, physics calculations, etc. here
        //

        Destroy(gameObject);
    }
}
