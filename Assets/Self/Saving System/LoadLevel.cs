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
            Import("save");
        }
    }

    void Import(string file) {
        string savePath = Application.persistentDataPath + "/saves/";
        if(System.IO.File.Exists($"{savePath}{file}.json")) {
            try {
                JObject o1 = JObject.Parse(File.ReadAllText($"{savePath}{file}.json"));
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
    private float[] StringToFloatArray(string input) {
        string[] stringArray = input.Split(",");
        float[] floatArray = new float[3];
        floatArray[0] = float.Parse(stringArray[0]);
        floatArray[1] = float.Parse(stringArray[1]);
        floatArray[2] = float.Parse(stringArray[2]);
        return floatArray;
    }
}
