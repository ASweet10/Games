using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterObject : MonoBehaviour
{
    [SerializeField] private GameObject shatteredObject;
    [SerializeField] private AudioClip explodeSFX;
    [SerializeField] private GameController gameController;

    private void Awake() {
        if(gameController == null) {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }
    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.tag == "Obstacle"){
            GameObject shatter = Instantiate(shatteredObject, transform.position, transform.rotation);
            Vector3 tempVector = gameObject.transform.localScale;
            //Divide by 100 because snowball is scaled 100/100/100 to start
            tempVector.x /= 100;
            tempVector.y /= 100;
            tempVector.z /= 100;
            shatter.transform.localScale = tempVector;
            shatter.transform.GetChild(0).GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 5f);
            //AudioSource.PlayClipAtPoint(explodeSFX, transform.position);
            gameController.EnableGameLost();
            Destroy(gameObject);
        }
    }
}
