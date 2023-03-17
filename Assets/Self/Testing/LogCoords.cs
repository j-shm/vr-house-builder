using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogCoords : MonoBehaviour
{   
    //quick temp debug class for me to check what height the player is 
    public bool constantLog;
    AdditionalRightController interactions;
    void Start() {
        interactions = new AdditionalRightController();
        interactions.Right.Enable();
    }

    void Update() {
        if(constantLog || interactions.Right.UIOPEN.WasPressedThisFrame()) {
            Debug.Log($"pos of {gameObject.name}: {gameObject.transform.position}");
        }
    }
}
