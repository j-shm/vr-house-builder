using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{
    public Material material;
    
    [SerializeField]
    public float scaleFactor = 1f;
    public float cubeLength = 1f;
    public float cubeHeight = 1f;
    public float cubeWidth = 1f;
    public float windowHeight = 0.25f;
    public float windowWidth = 0.25f;

    //basic cube

    void Start()
    {
        makeCube();
    }
    void makeCube() {
    Vector3[] verts = {
        //base 
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(cubeLength, 0.0f, 0.0f),
        new Vector3(cubeLength, 0.0f, cubeWidth),
        new Vector3(0.0f, 0.0f, cubeWidth),

        new Vector3(0.0f, cubeHeight, 0.0f),
        new Vector3(cubeLength, cubeHeight, 0.0f),
        new Vector3(cubeLength, cubeHeight, cubeWidth),
        new Vector3(0.0f, cubeHeight, cubeWidth),
    };
 
     int[] tris = {
        //base
        3,0,1, 2,3,1, 
        
        //front face
        1,0,4, 1,4,5,

        //left face
        0,3,7, 7,4,0,

        //right face
        2,1,5, 5,6,2,

        //back face
        2,6,7, 7,3,2,

        //top 
        5,4,7, 5,7,6, 
     };
             MeshFilter mF = gameObject.AddComponent<MeshFilter> (); // as MeshFilter;
        MeshRenderer render = gameObject.AddComponent<MeshRenderer> () as MeshRenderer;
        render.material = material;
        Mesh msh = new Mesh ();
        msh.vertices = verts;
        msh.triangles = tris;
        msh.RecalculateNormals ();
        mF.mesh = msh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
