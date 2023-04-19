using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPCA : MonoBehaviour
{
    public Transform[] targets;
    private int noLegs;
    private Vector3[] localTargetPos;
    private Vector3[] targetPos;
    // Start is called before the first frame update
    void Start()
    {
    
        noLegs = targets.Length;
        localTargetPos = new Vector3[noLegs]; 
        targetPos = new Vector3[noLegs]; 
        for (int i = 0; i < noLegs; i++)
        {
            
            print(i);
            
            localTargetPos[i] = targets[i].localPosition;
            targetPos[i] = targets[i].position;
            //legMoving[i] = false;
            print(targetPos[i]);
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
        for (int i = 0; i < noLegs; i++)
        {
            //localTargetPos[i] = targets[i].localPosition;
            targets[i].position = targetPos[i];
            //legMoving[i] = false;
        }
    }
}
