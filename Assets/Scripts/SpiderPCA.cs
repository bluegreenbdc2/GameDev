using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPCA : MonoBehaviour
{
    public Transform[] legs;
    public float maxDistanceFromTarget;
    public float maxTargetSurfaceOffset;
    private int noLegs;
    private Vector3[] defaultLocalPos;
    private Vector3[] localTargetPos;
    private Vector3[] legsPos;
    // Start is called before the first frame update
    void Start()
    {
        maxDistanceFromTarget = 1f;
        maxTargetSurfaceOffset = 1f;
        noLegs = legs.Length;
        defaultLocalPos = new Vector3[noLegs];
        localTargetPos = new Vector3[noLegs]; 
        legsPos = new Vector3[noLegs]; 
        for (int i = 0; i < noLegs; i++)
        {
            
            print(i);
            
            defaultLocalPos[i] = legs[i].localPosition;
            localTargetPos[i] = legs[i].localPosition;
            legsPos[i] = legs[i].position;
            //legMoving[i] = false;
            print(legsPos[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Vector3 surfaceNormal;
    void FixedUpdate()
    {
        //print(noLegs);
        //print(targetPos[0].x);
        //print(targetPos[0].y);
        //print(targetPos[0].z);
        
        
        //stick targets to ground
        
        for (int i = noLegs - 1; i > -1; i--)
        {
            //localTargetPos[i] = targets[i].localPosition;
            legs[i].position = legsPos[i];
            //legMoving[i] = false;
            
            //Gizmos.color = new Color(1, 0, 0, 0.5f);
            //Gizmos.DrawCube(transform.TransformPoint(localTargetPos[i]), new Vector3(0.2f, 0.2f, 0.2f));
            
            //raycast down from Max leg step height?
            if (surfaceNormal == Vector3.zero) surfaceNormal = -transform.up;
            if (Physics.Raycast( transform.TransformPoint(localTargetPos[i]) + transform.up , -transform.up, out RaycastHit hit, 10f))
            {
                Debug.DrawRay(transform.TransformPoint(localTargetPos[i]) + transform.up, -transform.up, Color.green);
                surfaceNormal = hit.normal;
                Vector3 hitPoint = hit.point;
                Vector3 newPos = transform.InverseTransformPoint(hitPoint);
                
                //stop target getting too far away due to raycast
                if ((defaultLocalPos[i] - newPos).magnitude < maxTargetSurfaceOffset)
                {
                
                    localTargetPos[i] = transform.InverseTransformPoint(hitPoint);
               //}
                }
                
                
            }
            
            //get distance between current leg pos and target point
            if ( (legs[i].position - transform.TransformPoint(localTargetPos[i])).magnitude > maxDistanceFromTarget)
            {
                
                //spiders altenate 2 pairs of legs
                if (i == 0 || i == 3 || i == 4 || i == 7)
                {
                    legs[0].position = transform.TransformPoint(localTargetPos[0]);
                    legsPos[0] = legs[0].position;
                    legs[3].position = transform.TransformPoint(localTargetPos[3]);
                    legsPos[3] = legs[3].position;
                    legs[4].position = transform.TransformPoint(localTargetPos[4]);
                    legsPos[4] = legs[4].position;
                    legs[7].position = transform.TransformPoint(localTargetPos[7]);
                    legsPos[7] = legs[7].position;
                }
                else
                {
                    legs[1].position = transform.TransformPoint(localTargetPos[1]);
                    legsPos[1] = legs[1].position;
                    legs[2].position = transform.TransformPoint(localTargetPos[2]);
                    legsPos[2] = legs[2].position;
                    legs[5].position = transform.TransformPoint(localTargetPos[5]);
                    legsPos[5] = legs[5].position;
                    legs[6].position = transform.TransformPoint(localTargetPos[6]);
                    legsPos[6] = legs[6].position;
                    
                }
                
            }
            
        }
    }
    
    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < noLegs; i++)
        {
            //localTargetPos[i] = targets[i].localPosition;
            
            //legMoving[i] = false;
            
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.TransformPoint(localTargetPos[i]), maxDistanceFromTarget);
            Gizmos.color = new Color(1, 1, 0, 0.5f);
            Gizmos.DrawCube(transform.TransformPoint(localTargetPos[i] + transform.up), new Vector3(0.2f, 0.2f, 0.2f));

        }
    }
}
