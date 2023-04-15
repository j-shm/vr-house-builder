using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
public class LoadLevel : MonoBehaviour
{
    public bool import;
    public Importer importer;
    void Update()
    {
        if(import) {
            import = false;
            Import(Application.persistentDataPath + "/saves/save.json");
        }
    }

    public void Import(string file) {
        if(System.IO.File.Exists(file)) {
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