using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        var filePath = $"C:/Users/C4xy/Downloads/{filename}.glb";
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
