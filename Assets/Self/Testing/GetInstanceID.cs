using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInstanceID : MonoBehaviour
{
    public int InstanceID;
    void Start()
    {
        InstanceID = GetInstanceID();
    }
}
