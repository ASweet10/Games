using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public GameObject gameController;
    public Rigidbody2D gameBallRB2D;
    AudioSource audioSource;
    GameController gameContScript;
    Rigidbody2D ballRB;
    Vector2 circleCenter;
    public float forceValue = 10f;
    
    void Start()
    {
        ballRB = gameObject.GetComponent<Rigidbody2D>();
        gameContScript = gameController.GetComponent<GameController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        circleCenter = transform.position;
        AddForceOnPlay();
    }
    
    //Give game ball upward vector when game starts  
    public void AddForceOnPlay()
    {
        ballRB.AddForce(Vector2.up * forceValue, ForceMode2D.Impulse);
    }  
    
    public void OnMouseDown()  //User touch input on mobile
    {
        //If ball is currently in stasis, disable it by re-enabling gravity
        if(gameContScript.stasisEnabled)
        {
            gameBallRB2D.gravityScale = 0.85f;
        }
        audioSource.Play();

        float ballRadius = 1;
        Vector3 ballCenter = transform.position;
        
        //Get random angle value in radians, from 2π/3 (60 deg) to π/3 (120 deg)
        float randAngle = Random.Range((2*(Mathf.PI)) / 3, Mathf.PI / 3);
        
        //Get X and Y values of point on circle, chosen from the random angle value
        float x = ballCenter.x + ballRadius * Mathf.Cos(randAngle);
        float y = ballCenter.y + ballRadius * Mathf.Sin(randAngle);
        Vector3 pointOnCircle = new Vector3(x, y, 0);

        //Get vector from center to pointoncircle
        Vector3 dirVector = pointOnCircle - ballCenter;
        
        //Normalize keeps direction but sets length to 1
        dirVector.Normalize();

        //Impulse adds instant forceValue in vector direction
        ballRB.AddForce(dirVector * forceValue, ForceMode2D.Impulse);

        if(gameObject.tag == "Bonus1")
        {
            gameContScript.tapCount += 2;
        }
        else if (gameObject.tag == "Bonus2")
        {
            gameContScript.tapCount += 5;
        }
        else
        {
            gameContScript.tapCount ++;
        }
    }
}
