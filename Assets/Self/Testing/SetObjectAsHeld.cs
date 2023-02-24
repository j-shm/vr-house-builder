using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectAsHeld : MonoBehaviour
{
    Object script;
    public bool isHeld;
    public bool done;

    void Start() {
        script = this.gameObject.GetComponent<Object>();
        if(script == null) {
            script = this.gameObject.GetComponent<Window>();
        }
    }
    void Update()
    {
        if(!done && isHeld) {
            script.SetHeld();
            done = true;
        }
        if(done && !isHeld) {
            script.SetHeld();
            done = false;
        }
    }
}
