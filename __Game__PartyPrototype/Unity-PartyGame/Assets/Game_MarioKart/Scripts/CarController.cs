using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{

    [Header("Movement Controls")]
    [SerializeField] KeyCode accelerateKey = KeyCode.W;
    [SerializeField] KeyCode reverseKey = KeyCode.S;
    [SerializeField] KeyCode turnLeftKey = KeyCode.A;
    [SerializeField] KeyCode turnRightKey = KeyCode.D;
    [SerializeField] KeyCode brakeKey = KeyCode.Space;


    [Header("Wheels")]
    [SerializeField] WheelCollider frontLeftWheelCollider;
    [SerializeField] WheelCollider frontRightWheelCollider;
    [SerializeField] WheelCollider rearLeftWheelCollider;
    [SerializeField] WheelCollider rearRightWheelCollider;

    [SerializeField] Transform frontLeftWheelTransform;
    [SerializeField] Transform frontRightWheelTransform;
    [SerializeField] Transform rearLeftWheelTransform;
    [SerializeField] Transform rearRightWheelTransform;

    [SerializeField] float maxAcceleration = 1000f;
    [SerializeField] float motorForce = 600f;
    [SerializeField] float brakeForce = 3000f;
    [SerializeField] float maxSteerAngle = 30f;


    [SerializeField] Rigidbody rb;

    private float verticalInput;
    private float horizontalInput;
    private float currentBrakeForce;
    private float currentSteerAngle;

    private bool canMove = true;
    private bool isBraking;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.9f, 0);
    }

    private void Update()
    {
        if(canMove)
        {
            GetMovementInput();
            UpdateWheelVisuals();
        }
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
    }

    private void GetMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(brakeKey); 
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce * maxAcceleration * Time.deltaTime;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce * maxAcceleration * Time.deltaTime;

        if(isBraking) {
            currentBrakeForce = brakeForce;
        } else {
            currentBrakeForce = 0f;
        }

        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void HandleBraking()
    {
        
    }

    private void UpdateWheelVisuals()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }

    public void ToggleCarMovement(bool letMeMove)
    {
        if(letMeMove) {
            canMove = true;
        } else {
            canMove = false;
        }
    }
}
