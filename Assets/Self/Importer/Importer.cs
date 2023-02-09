using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using GLTFast;

public class Importer : MonoBehaviour
{
    [SerializeField]
    private string dirPath = $"C:/Users/{Environment.UserName}/Downloads/";

    void Start()
    {
        ImportAll();
    }

    void ImportAll() {
        if(System.IO.Directory.Exists(dirPath)) {
            string[] files = System.IO.Directory.GetFiles(dirPath, "*.glb");
            foreach(var file in files) {
                Import(file);
            }
        }
    }

    async void Import(String file) {
        byte[] data = File.ReadAllBytes(file);
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success) {
            var placedModel = new GameObject(file.Split("/")[^1].Split(".")[0]).transform;
            success = await gltf.InstantiateMainSceneAsync( placedModel );
            if(success) {
                var invis = Instantiate(placedModel.gameObject,placedModel.gameObject.transform);
                invis.gameObject.name = "Invis";
                var comp = placedModel.gameObject.AddComponent<Object>();
                comp.SetInvis(invis);
            }
        }
    }

}
