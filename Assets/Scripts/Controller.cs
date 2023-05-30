using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SCRAPPED HUMAN CONTROLLER

public class Controller : MonoBehaviour
{
    public GameObject spiderPrefab;

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

    Cinemachine.CinemachineFreeLook freeLookCam;


    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Camera").gameObject;
        freeLookCam = camera.GetComponent<Cinemachine.CinemachineFreeLook>();
        freeLookCam.m_LookAt = transform;
        freeLookCam.m_Follow = transform;

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

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

        playerInput.x =  Input.GetAxis("Horizontal");
        playerInput.y =  Input.GetAxis("Vertical");

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        Vector3 cameraRight = cam.transform.right;
        cameraRight.y = 0;
        cameraRight = cameraRight.normalized;

        //playerInput.x *= cameraRight;
        //playerInput.y *= cameraForward;

        Vector3 playerInputDirection = (playerInput.x * cameraRight) + (playerInput.y * cameraForward);

        if (playerInputDirection == Vector3.zero) //if player isn't moving
        {
            animator.SetBool("IsMoving", false);
        }
        else
        {
            animator.SetBool("IsMoving", true);

            Quaternion toRotate = Quaternion.LookRotation(playerInputDirection, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }


        Vector3 velocity = (playerInput.x * cameraRight) + (playerInput.y * cameraForward);
        // Vector3 velocity = (cameraForward * playerInput.y + cameraRight * playerInput.x);

        float mag = velocity.magnitude;
        velocity.Normalize();
        velocity *= speed * Mathf.Clamp01(mag);

        Vector3 displacement = velocity * Time.deltaTime;

        //displacement = transform.TransformDirection(displacement);
        rigidBody.MovePosition(transform.localPosition + displacement);
        rigidBody.AddForce(Vector3.down * gravityScale);
      

    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);//*

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            print("jumped");
            rigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
        if (Input.GetButtonDown("Switch") && isGrounded)
        {
            print("Switching");
            Instantiate(spiderPrefab, transform.position, transform.rotation);
            //GameObject real = transform.Find("PlayerSpider(Clone)/spider/SpiderAnim").gameObject;
            //real.transform.rotation = transform.rotation;
            //GameObject.Find("PlayerSpider(Clone)/spider/SpiderAnim").gameObject.transform.rotation = transform.rotation;
            Destroy(gameObject);

        }


    }
}
