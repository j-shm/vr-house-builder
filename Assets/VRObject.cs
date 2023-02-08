using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRObject : MonoBehaviour
{
    public bool held;
    public bool freeRotate;

    public InputActionReference leftHand;
    public InputActionReference rightHand;
    private float _rotateSpeed = 50f;

    public void setHeld() {
        held = !held;
    }

    void Update()
    {
        if(!held || !freeRotate) return;
        float controllerValue = leftHand.action.ReadValue<Vector2>().x;
        transform.Rotate(Vector3.forward * Time.deltaTime * controllerValue * 1000);
    }
}
