using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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
        
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        Debug.DrawRay(transform.position, velocity, Color.yellow);
        velocity.y = 0;
        Quaternion toRotate = Quaternion.LookRotation(velocity, transform.up);
        if (playerInputDirection != Vector3.zero)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, 999 * Time.fixedDeltaTime);
            Quaternion rotationY = Quaternion.Euler(0, toRotate.eulerAngles.y, 0);
            //transform.rotation = rotationY;
            transform.localRotation = rotationY;
        }
        
       

    }
}
