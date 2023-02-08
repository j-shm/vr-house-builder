using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using GLTFast;

public class Importer : MonoBehaviour
{
    [SerializeField]
    private string filename; 
    async void Start()
    {
        Import();
    }
    async void Import() {
<<<<<<< HEAD
        var filePath = $"C:/Users/{Environment.UserName}/Downloads/{filename}.glb";
=======
        var filePath = $"C:/Users/C4xy/Downloads/{filename}.glb";
>>>>>>> cc683b13af6d26e8acb2a0bc2b6d8710f5ffba71
        byte[] data = File.ReadAllBytes(filePath);
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success) {
            var placedModel = new GameObject(filename).transform;
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
