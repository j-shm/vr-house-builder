using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using GLTFast;
using Newtonsoft.Json.Linq;

public class Importer : MonoBehaviour
{
    private string dirPath = $"C:/Users/{Environment.UserName}/Downloads/stuff/";

    [SerializeField]
    private bool importEverything = true;

    [SerializeField]
    private string fileNames;
        
    [SerializeField]
    private bool forceDetails = true;

    void Start()
    {
        if(importEverything) {
            ImportAll();
        } else {
            var files = fileNames.Split(",");
            foreach(var fileName in files) {
                if(fileName != "" || fileName != null) {
                    Import(fileName);
                }
            }
        }
    }

    void ImportAll() {
        if(System.IO.Directory.Exists(dirPath)) {
            string[] files = System.IO.Directory.GetFiles(dirPath, "*.glb");
            foreach(var file in files) {
                Import(file.Split("/")[^1].Split(".")[0]);
            }
        }
    }

    async void Import(String file) {
        string title = "";
        string description= "";
        string type= "";

        if(System.IO.File.Exists($"{dirPath}{file}.json")) {
            
            try {
                JObject o1 = JObject.Parse(File.ReadAllText($"{dirPath}{file}.json"));
                title = (string)o1["catalog"]["name"];
                description = (string)o1["catalog"]["description"];
                type = (string)o1["infomation"]["type"]; //either window or object at the minute
            } catch(Exception e) {
                if(forceDetails) {
                    Debug.LogError(e);
                    return;
                }
            }
        } else if(forceDetails) {
            return;
        }
        

        byte[] data = File.ReadAllBytes($"{dirPath}{file}.glb");

        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success) {
            var placedModel = new GameObject(file).transform;
            success = await gltf.InstantiateMainSceneAsync( placedModel );
            if(success) {
                var invis = Instantiate(placedModel.gameObject,placedModel.gameObject.transform);
                invis.gameObject.name = "Invis";
                if(type == "window") {
                    var comp = placedModel.gameObject.AddComponent<Window>();
                    comp.SetDetails(new ObjectDetails(name,description,type));
                    comp.SetInvis(invis);
                } else {
                    var comp = placedModel.gameObject.AddComponent<Object>();
                    comp.SetDetails(new ObjectDetails(name,description,type));
                    comp.SetInvis(invis);
                }

            }
        }
    }
}
