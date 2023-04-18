using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOpenMenu : MonoBehaviour
{
    AdditionalRightController interactions;
    public GameObject ui;
    void Start()
    {
        interactions = new AdditionalRightController();
        interactions.Right.Enable();
    }

    void Update()
    {
        if(interactions.Right.UIOPEN.WasPressedThisFrame()) {
            foreach (Transform trans in transform.GetComponentsInChildren<Transform>())
            {
                if(trans.gameObject.activeSelf && trans.gameObject != this.gameObject) {
                    trans.gameObject.SetActive(false);
                    Debug.Log(trans.gameObject.name);
                    return;
                }
            }
            ui.SetActive(true);
        }
    }
}
