using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObject : MonoBehaviour
{
    public Collider col;
    public bool make;

    public void Update() {
        if(col != null && make) {
            CreateCube();
            make = false;
        }
    }
    public void CreateCube() {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = col.bounds.size;
        cube.transform.position = col.gameObject.GetComponent<Window>().GetPos();
    }
}
