using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System.IO;

public class Simulator : MonoBehaviour
{


    public bool save;

    public bool playback; 
    private InputEventTrace inputEventTrace = new InputEventTrace();
    private MemoryStream ms = new MemoryStream();
    void Start()
    {
        inputEventTrace.Enable();
    }
    void Update() {
        if(save) {
            save = false;
            Save();
        }
        if(playback) {
            playback = false;
            PlayBack();
        }
    }
    void Save() {
        inputEventTrace.WriteTo(ms);
        ms.Close();
        inputEventTrace.Clear();
    }

    void PlayBack() {
        
        inputEventTrace.ReadFrom(ms);
    
        var replay = inputEventTrace.Replay().WithAllDevicesMappedToNewInstances();
                        
        replay.Rewind();
        replay.PlayAllEvents();
                        
        inputEventTrace.Clear();
        
    }
}
