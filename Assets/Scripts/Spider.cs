using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{

    public float rotationSpeed;

    private bool isGrounded;

    [SerializeField, Range(0f, 100f)]
    public float jumpSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    public float gravityScale = 1.5f;

    [SerializeField, Range(0f, 100f)]
    float speed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 40f;

    Vector3 velocity;
    Rigidbody rigidBody;


    private Animator animator;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        PhysicMaterial material = new PhysicMaterial();
        material.dynamicFriction = 0f;
        material.staticFriction = 0f;
        material.bounciness = 0f;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        material.bounceCombine = PhysicMaterialCombine.Minimum;

        Collider collider = GetComponent<Collider>();
        //collider.material = material;

        cam = Camera.main;
        //rigidBody.mass = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 playerInput;

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;

        //playerInput.x *= cameraRight;
        //playerInput.y *= cameraForward;

        Vector3 playerInputDirection = new Vector3(playerInput.x, 0f, playerInput.y);

        if (playerInputDirection == Vector3.zero) //if player isn't moving
        {
            //animator.SetBool("IsMoving", false);
        }
        else
        {
            print("ok");
            //animator.SetBool("IsMoving", true);

            //Quaternion toRotate = Quaternion.LookRotation(playerInputDirection, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }


        Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y);
        // Vector3 velocity = (cameraForward * playerInput.y + cameraRight * playerInput.x);

        float mag = velocity.magnitude;
        velocity.Normalize();
        velocity *= speed * Mathf.Clamp01(mag);

        Vector3 displacement = velocity * Time.deltaTime;

        displacement = transform.TransformDirection(displacement);
        rigidBody.MovePosition(transform.localPosition + displacement);
        //rigidBody.AddForce(Vector3.down * gravityScale);
        //rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            //print("ok");
            Vector3 normal = hit.normal;
            Vector3 gravityDirection = -normal;
            rigidBody.AddForce(gravityDirection * Physics.gravity.magnitude, ForceMode.Acceleration);
            
            Debug.DrawLine(transform.position, hit.point, Color.green, 0.5f);
            //Debug.Log("transformposition: " + transform.position);
           // Debug.Log("Hit point: " + hit.point);
            //Debug.Log("Normal: " + normal);
            Debug.DrawLine(transform.position, transform.position + hit.normal * 2f, Color.red);
            
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);

        }
        else
        {
            Debug.Log("Raycast didn't hit anything!");
            rigidBody.AddForce(Vector3.down * Physics.gravity.magnitude, ForceMode.Acceleration);
        }


    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);//*

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            print("jumped");
            //change to orientation
            rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            //Debug.DrawRay(Vector3.up * jumpSpeed, green)
        }
    }
}
