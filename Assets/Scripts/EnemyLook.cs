using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    public int turnSpeed;
    bool turning;

    // Start is called before the first frame update
    void Start()
    {
        turnSpeed = 200;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {

        Vector3 playerInputDirection = GameObject.Find("PlayerSpider").transform.position - transform.position;

        Vector3 velocity = playerInputDirection;

        Debug.DrawRay(transform.position, transform.up, Color.blue);
        Debug.DrawRay(transform.position, velocity, Color.yellow);
        velocity.y = 0;
        if (velocity != Vector3.zero && playerInputDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(velocity, transform.up);
            Quaternion rotationY = Quaternion.Euler(0, toRotate.eulerAngles.y, 0);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotationY, turnSpeed * Time.fixedDeltaTime);
        }
        
       

    }
}
