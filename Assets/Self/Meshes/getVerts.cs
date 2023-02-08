using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getVerts : MonoBehaviour
{
    private MeshFilter fil;
    private MeshRenderer mshRenderer;
    private Mesh msh;
    [SerializeField]
    private bool update;

    [SerializeField]
    private Vector3[] verts;
    [SerializeField]
    private int[] triangles;

    [SerializeField]
    private float lowHeight;
    [SerializeField]
    private float highHeight;
    [SerializeField]
    private float lowLength;
    [SerializeField]
    private float highLength;
    [SerializeField]
    private float lowWidth;
    [SerializeField]
    private float highWidth;
    [SerializeField]
    private float height;
    [SerializeField]
    private float length;
    [SerializeField]
    private float width;
    [SerializeField]
    private Vector3 actualHeight;

    void Start()
    {
        Display();
    }
    void Update() {
        if(update) {
            Display();
        }
    }
    private void Display() {
        fil = GetComponent<MeshFilter>();  
        msh = fil.mesh;
        mshRenderer = GetComponent<MeshRenderer>();
        actualHeight = mshRenderer.bounds.size;
        verts = msh.vertices;
        triangles = msh.triangles;
        actualHeight = msh.bounds.size;
        foreach(var vert in verts) {
            if(lowHeight > vert.y) {
                lowHeight = vert.y;
            }
            if(highHeight < vert.y) {
                highHeight = vert.y;
            }
            
            if(lowLength > vert.x) {
                lowLength = vert.x;
            }
            if(highLength < vert.x) {
                highLength = vert.x;
            }

            if(lowWidth > vert.z) {
                lowWidth = vert.z;
            }
            if(lowWidth < vert.z) {
                lowWidth = vert.z;
            }
        }
        height = Mathf.Abs(lowHeight) + Mathf.Abs(highHeight);
        length = Mathf.Abs(lowLength) + Mathf.Abs(highLength);
        width = Mathf.Abs(lowWidth) + Mathf.Abs(highWidth);
    }
}
