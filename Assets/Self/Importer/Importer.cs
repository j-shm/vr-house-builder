using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GLTFast;

//basic import example
public class Importer : MonoBehaviour
{
    async void Start()
    {
        var filePath = "";
        byte[] data = File.ReadAllBytes(filePath);
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success) {
            success = await gltf.InstantiateMainSceneAsync(transform);
        }
    }

}
