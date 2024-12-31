using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Chest")]
    [SerializeField] BoxCollider chestCollider;
    [SerializeField] Animator chestAnimator;
    [SerializeField] AudioSource chestAudio;
    bool canOpenChest = true;
    

    [Header("Coins")]
    [SerializeField] GameObject coins;
    [SerializeField] AudioSource coinAudio;

    public void OpenChest() {
        if (canOpenChest) {
            canOpenChest = false;
            chestCollider.enabled = false;
            chestAnimator.Play("OpenChest");
            if(!chestAudio.isPlaying) {
                chestAudio.Play();
            }
        }
    }

    public void StealCoins() {
        chestCollider.enabled = true;
        coins.SetActive(false);
        coinAudio.Play();
    }
}
