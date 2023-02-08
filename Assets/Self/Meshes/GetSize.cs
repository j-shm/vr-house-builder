using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSize : MonoBehaviour
{
    [SerializeField]
    public Vector3 size;

    [SerializeField]
    private bool update;
    private Collider col;
    void Start()
    {
        col = GetComponent<Collider>();
        size = col.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if(update) {
            size = col.bounds.size;
        }
    }
}
