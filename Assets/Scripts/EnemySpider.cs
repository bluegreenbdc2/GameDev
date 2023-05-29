using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour
{
    private bool isGrounded;

    [SerializeField, Range(0f, 100f)]
    float speed = 10f;

    Rigidbody rigidBody;
    
    // Start is called before the first frame update
    void Start()
    {

        rigidBody = GetComponent<Rigidbody>();

        PhysicMaterial material = new PhysicMaterial();
        material.dynamicFriction = 0f;
        material.staticFriction = 0f;
        material.bounciness = 0f;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        material.bounceCombine = PhysicMaterialCombine.Minimum;

       
        Collider collider = this.gameObject.transform.GetComponent<Collider>();
       
        gravityDirection = -transform.up;
        
        /*
        //has to be below camera assignment (dunno why)
        GameObject real = GameObject.Find("PlayerSpider(Clone)/spider/SpiderAnim").gameObject;
        real.transform.localRotation = transform.rotation;
        */
    }

    
    Vector3 gravityDirection;
    
    void FixedUpdate()
    {

       
        Vector3 playerInputDirection = Vector3.zero;

        if ((GameObject.Find("PlayerSpider").transform.position - transform.position).magnitude < 10f)
        {
            playerInputDirection = GameObject.Find("PlayerSpider").transform.position - transform.position;
        }

        Vector3 velocity = playerInputDirection;
        
        if (playerInputDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(velocity, transform.up);

        }

        
        velocity = transform.TransformDirection(velocity);

        float mag = velocity.magnitude;
        velocity.Normalize();
        velocity *= speed * Mathf.Clamp01(mag);
        //Debug.DrawRay(transform.position, velocity, Color.green);
        Vector3 displacement = velocity * Time.deltaTime;
        

        rigidBody.MovePosition(transform.position + displacement);

        Vector3 normal = -gravityDirection;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f) && hit.transform.tag != "NoClimb")
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
            if (Physics.Raycast(edgeVec, -velocity, out RaycastHit ehit, 1f) && ehit.transform.tag != "NoClimb")
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
                    
        if (Physics.Raycast(transform.position, velocity, out RaycastHit vHit, 1.5f) && vHit.transform.tag != "NoClimb")
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);

    }

    void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(" collision!!!");
        if (collision.gameObject.transform.root.name.Contains("Spike"))
        {
            Debug.Log("spike collision!!!");

            ContactPoint contact = collision.contacts[0];
            rigidBody.AddForce(contact.normal * 0.5f, ForceMode.Impulse);

        }
        if (collision.gameObject.transform.root.name.Contains("Player"))
        {
            Debug.Log("enemyaaa collision!!!");

            ContactPoint contact = collision.contacts[0];

            //attack animations
            gameObject.transform.GetChild(0).GetChild(1).GetComponent<SpiderPCA>().grabItem(contact.point);

            //rigidBody.AddForce(contact.normal * 0.5f, ForceMode.Impulse);

        }

        if (collision.gameObject.tag == "NoClimb")
        {
            //Debug.Log("noclimb collision!!!");
        }

    }


}
