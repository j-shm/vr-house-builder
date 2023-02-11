using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class OpenObjectMenu : MonoBehaviour
{
    AdditionalRightController interactions;
    public GameObject ui;
    void Start()
    {
        interactions = new AdditionalRightController();
        interactions.Right.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(interactions.Right.UIOPEN.WasPressedThisFrame()) {
            ui.SetActive(!ui.activeSelf);
        }
    }
}
