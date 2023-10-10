using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningField : MonoBehaviour
{
    [SerializeField] float chainLightningRange = 5f;
    [SerializeField] LayerMask dummyMask;
    CircleCollider2D circleCollider;
    Collider2D nextTargetCollider;
    Vector2 startPoint;
    bool hasHitNewEnemy = false;

    private void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Start() {
        StartCoroutine(ScaleSizeUntilHitTarget());
    }

    IEnumerator ScaleSizeUntilHitTarget(){
        //Scale collider size up until target is hit or max range reached
        //Check if enemies in range
        //If so, calculate distance to closest enemy
        //Scale lightning size to match this distance

        while(!hasHitNewEnemy && circleCollider.radius < chainLightningRange){
            circleCollider.radius += Time.deltaTime;
            Debug.Log(circleCollider.radius);
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            List<Collider2D> results = new List<Collider2D>();
            int colliderCount = circleCollider.OverlapCollider(filter, results);
            
            Collider2D closestCollider = null;
            if(colliderCount > 0){
                float maxDistance = Mathf.Infinity;

                foreach(Collider2D col2d in results){
                    float dist = Vector3.Distance(circleCollider.bounds.max, col2d.bounds.max);
                    if(dist < maxDistance){
                        maxDistance = dist;
                        closestCollider = col2d;
                        nextTargetCollider = closestCollider;
                        hasHitNewEnemy = true;
                        yield return null;
                    }
                }
            }
            if(closestCollider != null){
                Debug.Log("closest: " + closestCollider.gameObject.name);
                HandleLightningJump(closestCollider.gameObject);
                yield return null;
                Destroy(gameObject);
            }
            /*
            else{
                Debug.Log("No enemy in range to chain to");
                yield return null;
                Destroy(gameObject);
            }
            */
        }
    }

    void HandleLightningJump(GameObject nextTarget){
        //calculate distance to target
        //scale lightning bolt to match this size
        //bolt should jump from previous target to next
    }
}