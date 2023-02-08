using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWindowMesh : MonoBehaviour
{
    public Material material;
    
    public float wallHeight =1f;
    public float wallLength =1f;
    public float wallWidth =1f;
    public float scaleFactor =1f;

    public float windowHeight =0.25f;
    public float windowLength =0.25f;

    public float bottomRatio = 0.5f;
    public float leftRatio = 0.5f;

    private Mesh msh;
    private MeshFilter mF;
    private MeshRenderer render;
    private MeshCollider col;

    public GameObject center;

    public bool valid;
    
    void Start()
    {
        mF = gameObject.AddComponent<MeshFilter> (); // as MeshFilter;
        render = gameObject.AddComponent<MeshRenderer> () as MeshRenderer;
        col = gameObject.AddComponent<MeshCollider> () as MeshCollider;
        col.convex = true;
        render.material = material;
        msh = new Mesh ();
        MakeWall();

    }
    public void SetWindowSize(Vector3 dims,float bRatio = 0.5f, float lRatio=0.5f) {
        windowHeight = dims.y;
        windowLength = dims.x;

        bottomRatio = bRatio;
        leftRatio = lRatio;
        
    }
    public bool GetValidity(Vector3 size) {
        if(wallHeight < size.y || wallLength < size.x) {
            Debug.LogError("LARGER WINDOW THAN WALL");
            return false;
        }
        if(bottomRatio < 0 || bottomRatio > 1) {
            Debug.LogError("RATIO IS BIGGER THAN WALL");
            return false;
        }
        return true;
    }
    private bool GetValidity(float windowLength,float windowHeight) {
        return GetValidity(new Vector3(windowLength,windowHeight,0));
    }

    public List<Vector3> GetPoints() {
        List<Vector3> points = new List<Vector3>();
        for(float i=0f; i < 0.75f; i+=0.25f) {
            points.Add(new Vector3(transform.position.x,transform.position.y+i,transform.position.z));
        }
        return points;
    }


    public void MakeWall() {
        GetValidity(windowLength,windowHeight);


        float bottomWindowHeightOffset = (wallHeight - windowHeight) * bottomRatio;
        float topWindowHeightOffset    = windowHeight + bottomWindowHeightOffset;

        float leftWindowLengthOffset   = (wallLength - windowLength) * leftRatio;
        float rightWindowLengthOffset  = windowLength + leftWindowLengthOffset;


        //this can only handle one window 
        
        Vector3[] verts = {                                                                //vert number
        //wall points
        new Vector3(0.0f, 0.0f, 0.0f),                                                     //0
        new Vector3(wallLength, 0.0f, 0.0f),                                               //1
        new Vector3(wallLength, 0.0f, wallWidth),                                          //2
        new Vector3(0.0f, 0.0f, wallWidth),                                                //3

        new Vector3(0.0f, wallHeight, 0.0f),                                               //4
        new Vector3(wallLength, wallHeight, 0.0f),                                         //5
        new Vector3(wallLength, wallHeight, wallWidth),                                    //6
        new Vector3(0.0f, wallHeight, wallWidth),                                          //7

        //window points
        new Vector3(leftWindowLengthOffset, bottomWindowHeightOffset, 0f),                 //8
        new Vector3(rightWindowLengthOffset, bottomWindowHeightOffset, 0f),                //9
        new Vector3(leftWindowLengthOffset, topWindowHeightOffset, 0f),                    //10
        new Vector3(rightWindowLengthOffset, topWindowHeightOffset, 0f),                   //11

        new Vector3(leftWindowLengthOffset, bottomWindowHeightOffset, wallWidth),          //12
        new Vector3(rightWindowLengthOffset, bottomWindowHeightOffset, wallWidth),         //13
        new Vector3(leftWindowLengthOffset, topWindowHeightOffset, wallWidth),             //14
        new Vector3(rightWindowLengthOffset, topWindowHeightOffset, wallWidth),            //15
        };
 
        int[] tris = {
            //base
            3,0,1, 2,3,1, 

            //front face
            4,8,0, 4,10,8, 4,5,10, 5,11,10, 5,1,11, 1,9,11, 1,0,9, 9,1,0, 9,0,8,

            //left face
            0,3,7, 7,4,0,

            //right face
            2,1,5, 5,6,2,

            //back face
            3,12,7, 12,14,7, 14,6,7, 14,15,6, 15,2,6, 15,13,2, 13,3,2, 3,2,13, 12,3,13,

            //top 
            5,4,7, 5,7,6, 
        };
        msh.vertices = verts;
        msh.triangles = tris;
        msh.RecalculateNormals ();
        mF.mesh = msh;
        col.sharedMesh = msh;
        center.transform.localPosition = new Vector3(leftWindowLengthOffset+0.5f*windowLength,bottomWindowHeightOffset+0.5f*windowHeight,0f); 
    }

    public Vector3 GetCenter() {
        return center.transform.position;
    }
    public void OnValidate() {
        MakeWall();
    }
    // Update is called once per frame
}
