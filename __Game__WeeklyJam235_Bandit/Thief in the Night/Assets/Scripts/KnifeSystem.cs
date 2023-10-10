using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSystem : MonoBehaviour
{
    public GameObject knifeUIOne;
    public GameObject knifeUITwo;
    public GameObject knifeUIThree;
    public GameObject knifeUIFour;

    public Transform shootPointTF;
    public GameObject throwingKnife;
    private Vector2 shootDirection;
    public MouseLook mouseLook;
    public float knifeSpeed;
    public int maxKnives = 4;
    int currentKnives;
    void Awake()
    {
        mouseLook = gameObject.GetComponent<MouseLook>();
    }
    void Start()
    {
        currentKnives = maxKnives;
        knifeUIOne.SetActive(true);
        knifeUITwo.SetActive(true);
        knifeUIThree.SetActive(true);
        knifeUIFour.SetActive(true);
    }
    void Update()
    {
        if(currentKnives > maxKnives)
        {
            currentKnives = maxKnives;
        }
        shootDirection = mouseLook.GetMousePosition();
        //GetKeyDown for one shot per click, not continuous
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(currentKnives > 0)
            {
                ThrowKnife();
            }
        }
        ChangeKnivesUI();
    }
    public void ReplenishDagger()
    {
        currentKnives ++;
    }
    private void ThrowKnife()
    {
        var knifeObj = Instantiate(throwingKnife, shootPointTF.position, Quaternion.identity);
        var knifeRB = knifeObj.GetComponent<Rigidbody2D>();
        knifeRB.velocity = shootDirection * knifeSpeed;
        currentKnives --;
    }
    private void ChangeKnivesUI()
    {
        if(currentKnives == 4)
        {
            knifeUIOne.SetActive(true);
            knifeUITwo.SetActive(true);
            knifeUIThree.SetActive(true);
            knifeUIFour.SetActive(true);            
        }
        else if(currentKnives == 3)
        {
            knifeUIOne.SetActive(true);
            knifeUITwo.SetActive(true);
            knifeUIThree.SetActive(true);
            knifeUIFour.SetActive(false); 
        }
        else if(currentKnives == 2)
        {
            knifeUIOne.SetActive(true);
            knifeUITwo.SetActive(true);
            knifeUIThree.SetActive(false);
            knifeUIFour.SetActive(false);          
        }
        else if(currentKnives == 1)
        {
            knifeUIOne.SetActive(true);
            knifeUITwo.SetActive(false);
            knifeUIThree.SetActive(false);
            knifeUIFour.SetActive(false); 
        }
        else if(currentKnives == 0)
        {
            knifeUIOne.SetActive(false);
            knifeUITwo.SetActive(false);
            knifeUIThree.SetActive(false);
            knifeUIFour.SetActive(false);             
        }                
    }
}
