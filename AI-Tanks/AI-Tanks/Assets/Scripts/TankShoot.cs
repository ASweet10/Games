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
    float fireTime;
    void Start() {
        canFire = true;
        audioSource = gameObject.GetComponent<AudioSource>();
        data = gameObject.GetComponent<TankData>();
    }
    void Update() {
        if(Time.time - fireTime >= data.shootReloadTimer){
            canFire = true;
        }
    }
    public void FireShell() {
        canFire = false;
        fireTime = Time.time;
        Rigidbody shell = Instantiate(shellRound, firePointTF.position, firePointTF.rotation);
        shell.AddForce(firePointTF.forward * data.shellSpeed, ForceMode.Impulse); // Impulse: Apply instant force using mass
        audioSource.Play();
    }
    public bool ReturnCanFireStatus() {
        return canFire;
    }
}