using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollyBallSimple : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;


    [SerializeField] private float maxSpeed = 75f;
    [SerializeField] private float maxWheelTurn = 50f;
    [SerializeField] private float turnStrength = 210f;
    [SerializeField] private float accelerationForce = 8f;
    [SerializeField] private float brakeForce = 7f;
    [SerializeField] private float gravityForce = -9.8f;
    [SerializeField] private float dragOnGround = 3f;

    [SerializeField] private float lateralFriction, forwardFriction;
    [SerializeField] private Transform visual, leftFrontWheel, rightFrontWheel;

    private SphereCollider sphereCollider;

    private float speedInput;
    private float turnInput;


    private bool isGrounded;
    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint; //Point ray appears from

    private void Start() {
        rb.transform.parent = null;
    }
    private void Awake() {
        sphereCollider = rb.GetComponent<SphereCollider>();
    }
    
    private void Update() {

        HandleInput();

        //Debug.Log(isGrounded);



        //Can only turn while grounded
        if(isGrounded)
        {
            //Can only turn while moving / receiving vertical input, otherwise multiply by 0
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);



        transform.position = rb.transform.position;
        
    }
    private void FixedUpdate() {

        isGrounded = false;
        RaycastHit hit;

        //If ray shoots downward and hits ground...
        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            isGrounded = true;

            //hit.normal is angle of surface hit
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        /*
        //1. Origin: shifted up to register ground collider properly
        //3. Direction: Cast directly downwards
        //5. Max Distance: Limited to 0.2 or 20cm to get proper ground checks when airborne
        isGrounded = Physics.SphereCast(rb.position + transform.up * 0.1f, sphereCollider.radius, -transform.up, out var hit, 0.2f);
        //Used to limit acceleration/turning in midair, can find other uses
        */
        if(isGrounded) {
            rb.drag = dragOnGround;
            if(Mathf.Abs(speedInput) > 0)
            {
                rb.AddForce(transform.forward * speedInput);
            }

            /*
            rb.AddForce(transform.forward * accelerationForce * horizontalInput);
            //Brake not clamped so that "brake" becomes "reverse" when fully stopped
            //rb.AddForce(-transform.forward * brakeForce * carActions.Brake.ReadValue<float>());

            rb.AddForce(-transform.right * Mathf.Min(Mathf.Abs(speedLateral) * rb.mass, lateralFriction) * Mathf.Sign(speedLateral));
            rb.AddForce(-transform.forward * Mathf.Min(Mathf.Abs(speedForward) * rb.mass, forwardFriction) * Mathf.Sign(speedForward));

            var forward = Vector3.Cross(transform.right, hit.normal);
            visual.rotation = Quaternion.LookRotation(forward, hit.normal);
            */
        } else {
            rb.drag = 0.1f;
            rb.AddForce(Vector3.up * gravityForce * 100f);
        }
    }

    private void HandleInput() {
        
        //Forward input
        if(Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * accelerationForce * 1000f;
        } else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * brakeForce * 500f;
        }
        
        turnInput = Input.GetAxis("Horizontal");
    }
}
