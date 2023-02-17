using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBounds : MonoBehaviour
{    
    [SerializeField]
    public Vector3 size;
    [SerializeField]
    public Vector3 min;
    [SerializeField]
    public Vector3 max;
    [SerializeField]
    public Vector3 center;
    [SerializeField]
    public Vector3 extends;

    private Collider col;
    void Start()
    {
        col = GetComponent<Collider>();
    }
    private void stuff() {
        size = col.bounds.size;
        min = col.bounds.min;
        max = col.bounds.max;
        center = col.bounds.center;
        extends = col.bounds.extents;

    }
    void Update()
    {
        stuff();
    }
}
