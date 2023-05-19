using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class Web : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject obj)
    {
        Debug.Log(obj.name);   
        if (obj.transform.root.name.Contains("Spider"))
        {
            
            Debug.Log("particle colls");
        }
        else
        {
            ParticleSystem part = GetComponent<ParticleSystem>();
            part.Pause(true);

            //ParticleSystem.Particle[] particles;
            //int noParts = ParticleSystem.GetParticles(particles);
            //foreach (ParticleSystem.Particle particle in particles)
            //{
                
            //}
            /*
            Mesh webMesh = new Mesh();
            

            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 7),
                new Vector3(4, 0, 7),
                new Vector3(4, 0, 0),
            };

            int[] triangles = new int[]
            {
                0, 1, 2,
                2, 1, 3
            };

            webMesh.vertices = vertices;
            webMesh.triangles = triangles;

            GetComponent<MeshFilter>().mesh = webMesh;
            */
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            BoxCollider box = GetComponent<BoxCollider>();
            box.center = new Vector3(box.center.x, -0.7f, 3f);
            box.size = new Vector3(1f, 1f, 8f);
        }
    }


}
