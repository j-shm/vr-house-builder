using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
public class LoadLevel : MonoBehaviour
{
    public Importer importer;
    public ObjectManager man;
    public void Import(string file) {
        if(System.IO.File.Exists(file)) {
            var objectsInScene = man.GetObjects();
            for (int i = objectsInScene.Count - 1; i >= 0; i--) {
                objectsInScene[i].GetComponent<Object>().Kill();
            }
            try {
                JObject o1 = JObject.Parse(File.ReadAllText(file));
                var objs = o1["objects"];
                foreach(var obj in objs) {
                    string name = (string)obj["name"];
                    float[] objPos = ((JArray)obj["position"]).ToObject<float[]>();
                    float[] objRot = ((JArray)obj["rotation"]).ToObject<float[]>();
                    float[] objScale = ((JArray)obj["scale"]).ToObject<float[]>();
                    if(name != null && objPos != null && objRot != null && objScale != null) {
                        importer.Import(name,objPos, objRot,objScale);
                    }

                }
            } catch(Exception e) {
                Debug.LogError(e);
                return;
            }
        }
        
    }

}
