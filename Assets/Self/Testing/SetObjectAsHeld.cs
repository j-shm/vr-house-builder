using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectAsHeld : MonoBehaviour
{
    //Window script;
    CSGWindow script2;
    public bool isHeld;
    public bool done;

    void Start() {
        //script = this.gameObject.GetComponent<Window>();
        script2 = this.gameObject.GetComponent<CSGWindow>();
    }
    void Update()
    {
        if(!done && isHeld) {
            //script.SetHeld();
            script2.SetHeld();
            done = true;
        }
        if(done && !isHeld) {
            //script.SetHeld();
            script2.SetHeld();
            done = false;
        }
    }
}
