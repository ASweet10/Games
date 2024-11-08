using UnityEngine;

public class WatcherTrigger : MonoBehaviour
{
    [SerializeField] WatcherOnLedge watcherScript;
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            watcherScript.state = WatcherOnLedge.State.leaving;
        }
    }
}