using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRotation : MonoBehaviour
{
    [SerializeField]
    Quaternion rotation;
    [SerializeField]
    Vector3 rotationIndiv;
    void Update()
    {
        rotation = this.gameObject.transform.rotation;
        rotationIndiv = new Vector3(this.gameObject.transform.rotation.x,this.gameObject.transform.rotation.y,this.gameObject.transform.rotation.z);
    }
}
