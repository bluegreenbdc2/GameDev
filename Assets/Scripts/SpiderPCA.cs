using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPCA : MonoBehaviour
{
    public Transform[] legs;
    private int noLegs;
    private Vector3[] localTargetPos;
    private Vector3[] legsPos;
    // Start is called before the first frame update
    void Start()
    {
    
        noLegs = legs.Length;
        localTargetPos = new Vector3[noLegs]; 
        legsPos = new Vector3[noLegs]; 
        for (int i = 0; i < noLegs; i++)
        {
            
            print(i);
            
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
    
    void FixedUpdate()
    {
        //print(noLegs);
        //print(targetPos[0].x);
        //print(targetPos[0].y);
        //print(targetPos[0].z);
        
        
        //stick targets to ground
        
        for (int i = 0; i < noLegs; i++)
        {
            //localTargetPos[i] = targets[i].localPosition;
            legs[i].position = legsPos[i];
            //legMoving[i] = false;
            
            //Gizmos.color = new Color(1, 0, 0, 0.5f);
            //Gizmos.DrawCube(transform.TransformPoint(localTargetPos[i]), new Vector3(0.2f, 0.2f, 0.2f));
            
            //raycast down from Max leg step height?
            if (Physics.Raycast( transform.TransformPoint(localTargetPos[i] + transform.up ) , -transform.up, out RaycastHit hit, 1f))
            {
                Debug.DrawRay(transform.TransformPoint(localTargetPos[i] + transform.up), -transform.up, Color.green);
                Vector3 normal = hit.normal;
                Vector3 hitPoint = hit.point;
                localTargetPos[i] = transform.InverseTransformPoint(hitPoint);
                
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
            Gizmos.DrawCube(transform.TransformPoint(localTargetPos[i]), new Vector3(0.2f, 0.2f, 0.2f));
            
        }
    }
}
