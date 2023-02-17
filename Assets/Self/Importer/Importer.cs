using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using GLTFast;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Importer : MonoBehaviour
{
    private string dirPath = $"C:/Users/{Environment.UserName}/Downloads/stuff/";

    [SerializeField]
    private bool importEverything = true;

    [SerializeField]
    private string fileNames;
        
    [SerializeField]
    private bool forceDetails = true;
    public InputActionReference leftHand;

    void Start()
    {
        if(importEverything) {
            ImportAll();
        } else {
            string[] files = fileNames.Split(",");
            foreach(string fileName in files) {
                if(fileName != "" || fileName != null) {
                    Import(fileName);
                }
            }
        }
    }
    public List<string> GetAllFiles() {
        List<string> models = new List<string>();
        if(System.IO.Directory.Exists(dirPath)) {
            string[] files = System.IO.Directory.GetFiles(dirPath, "*.glb");
            foreach(string file in files) {
                models.Add(file.Split("/")[^1].Split(".")[0]);
            }
        }
        return models;
    }
    public string GetDirPath() {
        return dirPath;
    }
    void ImportAll() {
        if(System.IO.Directory.Exists(dirPath)) {
            string[] files = System.IO.Directory.GetFiles(dirPath, "*.glb");
            foreach(string file in files) {
                Import(file.Split("/")[^1].Split(".")[0]);
            }
        }
    }
    void ImportAll(List<string> files) {
        if(System.IO.Directory.Exists(dirPath)) {
            foreach(string file in files) {
                Import(file);
            }
        }
    }
    public async void Import(String file) {
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

        GltfImport gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success) {
            Transform placedModel = new GameObject(file).transform;
            success = await gltf.InstantiateMainSceneAsync( placedModel );
            if(success) {
                GameObject invis = Instantiate(placedModel.gameObject,placedModel.gameObject.transform);
                invis.gameObject.name = "Invis";
                if(type == "window") {
                    Window comp = placedModel.gameObject.AddComponent<Window>();
                    comp.SetDetails(new ObjectDetails(name,description,type));
                    comp.SetInvis(invis);
                } else {
                    Object comp = placedModel.gameObject.AddComponent<Object>();
                    comp.SetDetails(new ObjectDetails(name,description,type));
                    comp.SetInvis(invis);
                    comp.leftHand = leftHand;
                }

            }
        }
    }
}
