using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    private Camera cam;
    public int turnSpeed;
    bool turning;

    public GameObject webPrefab;

    // Start is called before the first frame update
    void Start()
    {
        turnSpeed = 200;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ShootWeb") && !turning)
        {
            
            StartCoroutine(turnRound(15f, true));
            //Instantiate(humanPrefab, transform.position, transform.Find("spider/SpiderAnim").gameObject.transform.rotation);

            
        
        }
        if (Input.GetButtonUp("ShootWeb") && !turning)
        {
            StartCoroutine(turnRound(15f, false));
            //Instantiate(humanPrefab, transform.position, transform.Find("spider/SpiderAnim").gameObject.transform.rotation);
        }
    }

    IEnumerator turnRound(float speed, bool shootWeb)
    {
        turning = true;
        Quaternion initialRotation = transform.localRotation;
        Quaternion toRotate = Quaternion.LookRotation(-transform.forward, transform.up);
        Quaternion rotationY = Quaternion.Euler(0, toRotate.eulerAngles.y, 0);
        //moving[leg] = true;
        float webPosY = transform.position.y;
        for (int i = 1; i <= speed; i++)
        {
            
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, 999 * Time.fixedDeltaTime);
            
            //transform.rotation = rotationY;
            transform.localRotation = Quaternion.Lerp(initialRotation, rotationY, i / speed);
            
            if (shootWeb)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, -0.8f * (i / speed), transform.localPosition.z);
                //GetComponent<SpiderPCA>().grabItem(collision.gameObject.transform.position);

            }
            //Vector3 legMovement = Vector3.Lerp(legs[leg].position, newPosition, i / speed);
            //Vector3 legHeight = Mathf.Sin(i / speed * Mathf.PI) * height * transform.up;

            //legs[leg].position = legMovement + legHeight;
            yield return new WaitForFixedUpdate();
        }
        //legsPos[leg] = legs[leg].position;
        //moving[leg] = false;
        turning = false;
        Vector3 webPos = new Vector3(transform.position.x, webPosY - 0.2f, transform.position.z);
        webPos = webPos + -transform.forward;
        if (shootWeb)
        {
            Instantiate(webPrefab, webPos, initialRotation);

            for (int i = 1; i <= speed; i++)
            {
                
                //transform.localPosition = new Vector3(transform.localPosition.x, -0.8f * (i / speed), transform.localPosition.z);
                //yield return new WaitForFixedUpdate();
            }
            for (int i = 1; i <= speed; i++)
            {

                transform.localPosition = new Vector3(transform.localPosition.x, -0.8f - (-0.8f* (i / speed)), transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }

        }
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
        if (velocity != Vector3.zero && playerInputDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(velocity, transform.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, 999 * Time.fixedDeltaTime);
            Quaternion rotationY = Quaternion.Euler(0, toRotate.eulerAngles.y, 0);
            //transform.rotation = rotationY;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotationY, turnSpeed * Time.fixedDeltaTime);
        }
        
       

    }
}
