using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : MonoBehaviour
{
    Transform knifeTransform;
    public float rotationSpeed = 2f;
    bool knifeRotated = true;
    bool canPickKnifeUp = false;
    public float pickupDelay = 0.5f;
    void Start()
    {
        knifeTransform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        //WaitForKnifePickup();
        if(knifeRotated)
        {
            RotateKnife();
        }
        if(!knifeRotated)
        {
            canPickKnifeUp = true;
        }
    }
    private void RotateKnife()
    {
        knifeTransform.Rotate(0, 0, 360 * Time.deltaTime * rotationSpeed);
    }
    public void StopRotation()
    {
        knifeRotated = false;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            //If object is not spinning / stopped on wall
            if(canPickKnifeUp)
            {
                var knifeSystemRef = col.gameObject.GetComponent<KnifeSystem>();
                knifeSystemRef.ReplenishDagger();
                Destroy(gameObject);
            }
        }
    }
    private void WaitForKnifePickup()
    {
        if(pickupDelay > 0)
        {
            pickupDelay -= Time.deltaTime;
        }
        else
        {
            canPickKnifeUp = true;
        }
    }
}
