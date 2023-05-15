using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public GameObject humanPrefab;

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

    public Transform camer;
    private Animator animator;
    private Camera cam;

    Cinemachine.CinemachineFreeLook freeLookCam;


    private Vector3 getGroundNormal() {
        Vector3 normal = transform.up;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit1, 1f))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.green);
            normal = hit1.normal;
            
        }
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1f))
        {
            Debug.DrawRay(transform.position, -transform.up, Color.green);
            normal += hit.normal;
        }
        return normal.normalized;
        //else { return Vector3.up; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Camera").gameObject;
        freeLookCam = camera.GetComponent<Cinemachine.CinemachineFreeLook>();
        freeLookCam.m_LookAt = transform;
        freeLookCam.m_Follow = transform;

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        PhysicMaterial material = new PhysicMaterial();
        material.dynamicFriction = 0f;
        material.staticFriction = 0f;
        material.bounciness = 0f;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        material.bounceCombine = PhysicMaterialCombine.Minimum;

        Collider collider = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        //collider.material = material;
        gravityDirection = -transform.up;
        cam = Camera.main;
        //rigidBody.mass = 0;
        
        //has to be below camera assignment (dunno why)
        GameObject real = GameObject.Find("PlayerSpider(Clone)/spider/SpiderAnim").gameObject;
        real.transform.localRotation = transform.rotation;
    }

    // Update is called once per frame
    
    Vector3 gravityDirection;
    
    void FixedUpdate()
    {

        Vector2 playerInput;

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;
        
        Vector3 cameraRight = cam.transform.right;
        cameraRight.y = 0;
        cameraRight = cameraRight.normalized;
        
        //Vector3 playerInputDir = new Vector3(playerInput.x, 0f, playerInput.y);
        Vector3 playerInputDirection = (playerInput.x * cameraRight) + (playerInput.y * cameraForward);
        
        Vector3 velocity = (cameraForward * playerInput.y + cameraRight * playerInput.x);
        
        Quaternion toRotate = Quaternion.LookRotation(velocity, transform.up);
        if (playerInputDirection != Vector3.zero)
        {
            //transform.rotation = toRotate;//Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.fixedDeltaTime);
            //Quaternion rotationY = Quaternion.Euler(transform.rotation.eulerAngles.x, toRotate.eulerAngles.y, transform.rotation.eulerAngles.z);
            //transform.rotation = rotationY;
        }

        //velocity = Quaternion.LookRotation(velocity, transform.up) * velocity;
        
       // velocity = Vector3.ProjectOnPlane(velocity, transform.up);
        //Debug.DrawRay(transform.position, transform.up, Color.blue);
        
        velocity = transform.TransformDirection(velocity);
        //Debug.DrawRay(transform.position, velocity, Color.magenta);
        //velocity = Quaternion.Inverse(toRotate) * velocity;
        //Debug.DrawRay(transform.position, velocity, Color.white);
        
        float mag = velocity.magnitude;
        velocity.Normalize();
        velocity *= speed * Mathf.Clamp01(mag);
        //Debug.DrawRay(transform.position, velocity, Color.green);
        Vector3 displacement = velocity * Time.deltaTime;
        
        
        
        //displacement = transform.TransformDirection(displacement);
        
        //Debug.DrawRay(transform.position, velocity, Color.green);
        rigidBody.MovePosition(transform.position + displacement);

        Vector3 normal = -gravityDirection;

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1.5f))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
            
            Debug.DrawRay(transform.position, -transform.up, Color.green);
            
            
            normal = hit.normal;

        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up, Color.red);
            Debug.Log("Down Raycast didn't hit anything!");
            
            Vector3 edgeVec = transform.localPosition - transform.up;
            if (Physics.Raycast(edgeVec, -velocity, out RaycastHit ehit, 1f))
            {
                Debug.DrawRay(edgeVec, -velocity, Color.green);
                normal = ehit.normal;
                
                
            }
            else
            {
                Debug.DrawRay(edgeVec, -velocity, Color.red);
            }
            
            //-transform.up is down in relation to objects rotation
        }
                    
        if (Physics.Raycast(transform.position, velocity, out RaycastHit vHit, 1.5f))
        {
            Debug.DrawRay(transform.position, velocity, Color.green);
            normal += vHit.normal;
        }
        else{
            Debug.DrawRay(transform.position, velocity, Color.red);
        }
        

        //Debug.DrawRay(transform.position, normal, Color.yellow);
        gravityDirection = -normal;
        
        //animator.SetBool("IsMoving", true); 
        if (playerInputDirection != Vector3.zero)
        {
            //animator.SetBool("IsMoving", false);
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);

        }
        
        //apply gravity
        rigidBody.AddForce(gravityDirection * Physics.gravity.magnitude, ForceMode.Acceleration);


    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);//*

        /*if (Input.GetButtonDown("Jump") && isGrounded)
        {
            print("jumped");
            //change to orientation
            rigidBody.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
            //Debug.DrawRay(Vector3.up * jumpSpeed, green)
        }*/

        if (Input.GetButtonDown("Switch") && isGrounded)
        {
            print("Switching");
            Instantiate(humanPrefab, transform.position, GameObject.Find("PlayerSpider(Clone)/spider/SpiderAnim").gameObject.transform.rotation);
            
            Destroy(gameObject);

        }



    }
}
