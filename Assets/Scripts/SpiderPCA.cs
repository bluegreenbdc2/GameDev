using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPCA : MonoBehaviour
{
    public bool bodyStuff;
    public float legStepHeight = 0.2f;
    public float stepSpeed = 8f;
    public Transform[] legs;
    public float maxDistanceFromTarget;
    public float maxTargetSurfaceOffset;
    private int noLegs;
    private Vector3[] defaultLocalPos;
    private Vector3[] localTargetPos;
    private Vector3[] legsPos;
    public Transform[] legsStartPos;
    
    private bool[] moving;
    
    
    private Vector3 lastBodyUp;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastBodyUp = transform.up;
        maxDistanceFromTarget = 1f;
        maxTargetSurfaceOffset = 1f;
        noLegs = legs.Length;
        defaultLocalPos = new Vector3[noLegs];
        localTargetPos = new Vector3[noLegs]; 
        legsPos = new Vector3[noLegs]; 
        moving = new bool[noLegs];
        for (int i = 0; i < noLegs; i++)
        {
            moving[i] = false;
            print(i);
            
            defaultLocalPos[i] = legs[i].localPosition;
            localTargetPos[i] = legs[i].localPosition;
            
            legs[i].position = legsStartPos[i].position;
            legsPos[i] = legs[i].position;
            //legMoving[i] = false;
            print(legsPos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Vector3 lastPos;
    Vector3 surfaceNormal;
    void FixedUpdate()
    {
        Vector3 velocity = transform.forward;
        //Vector3 velocity = transform.position - lastPos;
        //lastPos = transform.position;
        Debug.DrawRay(transform.position, velocity, Color.red);
        //print(noLegs);
        //print(targetPos[0].x);
        //print(targetPos[0].y);
        //print(targetPos[0].z);
        
        
        //stick targets to ground
        
        for (int i = noLegs - 1; i > -1; i--)
        {
            //localTargetPos[i] = targets[i].localPosition;
            if (!moving[i]) legs[i].position = legsPos[i];
            //legMoving[i] = false;
            
            //Gizmos.color = new Color(1, 0, 0, 0.5f);
            //Gizmos.DrawCube(transform.TransformPoint(localTargetPos[i]), new Vector3(0.2f, 0.2f, 0.2f));
            
            //raycast down from Max leg step height?
            if (surfaceNormal == Vector3.zero) surfaceNormal = -transform.up;
            
            if (Physics.Raycast( transform.TransformPoint(defaultLocalPos[i]) + transform.up , -transform.up, out RaycastHit hit, 10f))
            {
                Debug.DrawRay(transform.TransformPoint(defaultLocalPos[i]) + transform.up, -transform.up, Color.green);
                surfaceNormal = hit.normal;
                Vector3 hitPoint = hit.point;
                Vector3 newPos = transform.InverseTransformPoint(hitPoint);
                
                //raycast in moving direction
                if (Physics.Raycast(transform.TransformPoint(defaultLocalPos[i]) + transform.up, velocity, out RaycastHit fHit, 1.5f))
                {
                    Debug.DrawRay(transform.TransformPoint(defaultLocalPos[i]) + transform.up, velocity, Color.green);
                    //Debug.DrawRay(transform.position, velocity, Color.green);
                    surfaceNormal += fHit.normal;
                    //hitPoint = fHit.point;
                    if (Physics.Raycast(transform.TransformPoint(defaultLocalPos[i]) + transform.up, -surfaceNormal, out RaycastHit avgHit, 1.5f))
                    {
                        Debug.Log("wtf");
                        hitPoint = avgHit.point;
                        Debug.DrawRay(transform.TransformPoint(defaultLocalPos[i]) + transform.up, -surfaceNormal, Color.white);
                        newPos = transform.InverseTransformPoint(hitPoint);
                    }
                    
                }
                else{
                    
                }
                
                
                
                
                //stop target getting too far away due to raycast
                if ((defaultLocalPos[i] - newPos).magnitude < maxTargetSurfaceOffset)
                {
                
                    localTargetPos[i] = transform.InverseTransformPoint(hitPoint);
               //}
                }
                
                
            }
            //on the edge of a plane
            else{
                Debug.DrawRay(transform.TransformPoint(localTargetPos[i]) + transform.up, -transform.up, Color.red);
                Vector3 edgeVec = transform.TransformPoint(localTargetPos[i]) - (transform.up * 0.2f);
                if (Physics.Raycast(edgeVec, -velocity, out RaycastHit ehit, 3f))
                {
                    Debug.DrawRay(edgeVec, -velocity, Color.green);
                    //Debug.DrawRay(edgeVec, -velocity, Color.green);
                    //normal = ehit.normal;
                    Vector3 newPos = transform.InverseTransformPoint(ehit.point);
                    
                    //stop target getting too far away due to raycast
                    if ((defaultLocalPos[i] - newPos).magnitude < maxTargetSurfaceOffset)
                    {

                        localTargetPos[i] = transform.InverseTransformPoint(ehit.point);
                   //}
                    }
                }
                else{
                    Debug.DrawRay(edgeVec, -velocity, Color.red);
                }
                
            }
            
            //get distance between current leg pos and target point
            if ( (legs[i].position - transform.TransformPoint(localTargetPos[i])).magnitude > maxDistanceFromTarget)
            {
                
                //spiders altenate 2 pairs of legs
                if (!moving[i])
                {
                    StartCoroutine(moveLeg(i, transform.TransformPoint(localTargetPos[i]), stepSpeed, legStepHeight));
                    /*
                    if (i == 0 || i == 3 || i == 4 || i == 7)
                    {
                        //legs[0].position = transform.TransformPoint(localTargetPos[0]);

                        StartCoroutine(moveLeg(0, transform.TransformPoint(localTargetPos[0]), stepSpeed, legStepHeight));


                        StartCoroutine(moveLeg(3, transform.TransformPoint(localTargetPos[3]), stepSpeed, legStepHeight));
                       // legsPos[3] = legs[3].position;

                        StartCoroutine(moveLeg(4, transform.TransformPoint(localTargetPos[4]), stepSpeed, legStepHeight));
                       // legsPos[4] = legs[4].position;

                        StartCoroutine(moveLeg(7, transform.TransformPoint(localTargetPos[7]), stepSpeed, legStepHeight));
                       // legsPos[7] = legs[7].position;
                    }
                    else
                    {
                        StartCoroutine(moveLeg(1, transform.TransformPoint(localTargetPos[1]), stepSpeed, legStepHeight));
                      //  legsPos[1] = legs[1].position;

                        StartCoroutine(moveLeg(2, transform.TransformPoint(localTargetPos[2]), stepSpeed, legStepHeight));
                      //  legsPos[2] = legs[2].position;

                        StartCoroutine(moveLeg(5, transform.TransformPoint(localTargetPos[5]), stepSpeed, legStepHeight));
                       // legsPos[5] = legs[5].position;

                        StartCoroutine(moveLeg(6, transform.TransformPoint(localTargetPos[6]), stepSpeed, legStepHeight));
                      //  legsPos[6] = legs[6].position;

                    }
                    */
                    
                }
                
            }
            
            


        }
    }
    
    IEnumerator moveLeg(int leg, Vector3 newPosition, float speed, float height)
    {
        moving[leg] = true;
        for(int i = 1; i <= speed; i++)
        {
            
            Vector3 legMovement =  Vector3.Lerp(legs[leg].position, newPosition, i / speed);
            Vector3 legHeight = Mathf.Sin(i / speed * Mathf.PI) * height * transform.up;
            
            legs[leg].position = legMovement + legHeight;  
            yield return new WaitForFixedUpdate();
        }
        legsPos[leg] = legs[leg].position;
        moving[leg] = false;
    }
    
    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < noLegs; i++)
        {
            //localTargetPos[i] = targets[i].localPosition;

            //legMoving[i] = false;
            //Debug.Log("HAAA");
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            //Gizmos.DrawWireSphere(transform.TransformPoint(localTargetPos[i]), maxDistanceFromTarget);
            Gizmos.color = new Color(1, 1, 0, 0.5f);
            Gizmos.DrawCube(transform.TransformPoint(localTargetPos[i]/* + transform.up*/), new Vector3(0.2f, 0.2f, 0.2f));

        }
    }
}
