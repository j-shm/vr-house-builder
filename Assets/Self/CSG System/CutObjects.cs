using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

//used to cut objects with the CSG system mostly to make doors
public class CutObjects : MonoBehaviour
{
    public bool cut;
    public bool deleteAfterCut;
    public GameObject objectA;
    public GameObject objectB;
    public GameObject objectResult;


    void Update()
    {
        if(cut) {
            cut = false;
            if(objectA == null && objectB == null) {
                Debug.LogError("must have an object A and object B");
                return;
            }
            Model result = CSG.Subtract(objectA, objectB);
            var composite = new GameObject();
            composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
            composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

            objectResult = composite;
            if(deleteAfterCut) {
                Destroy(objectA);
                Destroy(objectB);
            }

        }
    }
}
