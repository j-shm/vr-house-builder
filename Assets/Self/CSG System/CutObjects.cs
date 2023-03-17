using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class CutObjects : MonoBehaviour
{
    public bool cut;
    public GameObject objectA;
    public GameObject objectB;
    public GameObject objectResult;

    // Update is called once per frame
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

        }
    }
}
