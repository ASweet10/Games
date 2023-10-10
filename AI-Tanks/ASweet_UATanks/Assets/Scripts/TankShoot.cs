using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(TankData))]
public class TankShoot : MonoBehaviour
{
    [SerializeField] Transform firePointTF;
    [SerializeField] Rigidbody shellRound;
    AudioSource audioSource;
    TankData data;
    bool canFire;
    void Start() {
        canFire = true;
        audioSource = gameObject.GetComponent<AudioSource>();
        data = gameObject.GetComponent<TankData>();
    }
    void Update() {
        //Debug.Log(gameObject.name + canFire);
    }
    public void FireShell() {
        StartCoroutine(FireShellAndWait());
    }
    IEnumerator FireShellAndWait() {
        canFire = false;
        Rigidbody shell = Instantiate(shellRound, firePointTF.position, firePointTF.rotation);
        //  ForceMode.Impulse applies instant force using its mass
        shell.AddForce(firePointTF.forward + -(firePointTF.up * data.shellSpeed) / 3, ForceMode.Impulse);
        audioSource.Play();
        //yield return new WaitForSecondsRealtime(3f);
        canFire = true;
        yield return null;
    }
    public bool ReturnCanFireStatus() {
        return canFire;
    }
}