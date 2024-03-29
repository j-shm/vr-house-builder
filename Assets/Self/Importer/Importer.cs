using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using GLTFast;
using Newtonsoft.Json.Linq;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

//used for importing gltf files into the game 
public class Importer : MonoBehaviour
{
    private string dirPath;

    [SerializeField]
    private bool importEverything = true;

    [SerializeField]
    private string fileNames;
        
    [SerializeField]
    private bool forceDetails = true;
    public InputActionReference leftHand;
    public InputActionReference deleteButton;
    public GameObject placedParticle;
    public ObjectManager man;
    void Start()
    {
        dirPath = $"{Application.persistentDataPath}/models/";
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
    public async void Import(String file, float[] objPos = null, float[] objRot = null, float[] objScale = null) {
        string title = "";
        string description= "";
        string type= "";
        Vector3 light = new Vector3(0,0,0);
        if(System.IO.File.Exists($"{dirPath}{file}.json")) {
            
            try {
                JObject o1 = JObject.Parse(File.ReadAllText($"{dirPath}{file}.json"));
                title = (string)o1["catalog"]["name"];
                description = (string)o1["catalog"]["description"];
                type = (string)o1["information"]["type"]; //either window or object at the minute
                
                if(o1["light"] != null) {
                    string temporaryTransform = (string)o1["light"]["transform"];
                    float scale = 10;
                    if(o1["light"]["scale"] != null) {
                        scale = float.Parse((string)o1["light"]["scale"]);
                    }
                    light.y = float.Parse(temporaryTransform.Split(",")[1])*scale;  //x10 to add the scale factor of unity
                } 
            } catch(Exception e) {
                if(forceDetails) {
                    Debug.LogError(e);
                    return;
                }
               
            }
        } else {
            Debug.LogError("no json file found for " + file);
            return;
        }
        
        if(!System.IO.File.Exists($"{dirPath}{file}.glb")) {
            Debug.LogError("no glb file found for " + file);
            return;
        }

        byte[] data = File.ReadAllBytes($"{dirPath}{file}.glb");

        GltfImport gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success) {
            Transform placedModel = new GameObject(file).transform;
            if(placedModel == null) {
                Debug.LogError("placedModel is null for " + file);
                return;
            }

            bool successofinstantiate = false;
            try {
                successofinstantiate = await gltf.InstantiateMainSceneAsync( placedModel );
            } catch(Exception e) {
                Debug.LogError("we didn't manage to instantiate: " + e);
            }
            
            if(successofinstantiate) {
                this.man.AddObject(placedModel.gameObject);
                if(!light.Equals(new Vector3(0f,0f,0f))) {
                    GameObject lightGameObject = new GameObject("light");
                    lightGameObject.transform.parent = placedModel.gameObject.transform;
                    Light lightComp = lightGameObject.AddComponent<Light>();
                    lightGameObject.transform.localPosition = light;
                }
                GameObject invis = Instantiate(placedModel.gameObject,placedModel.gameObject.transform);
                invis.gameObject.name = "Invis";
                if(type == "window") {
                    Window comp = placedModel.gameObject.AddComponent<Window>();
                    comp.SetDetails(new ObjectDetails(name,description,type));
                    comp.SetInvis(invis);
                    comp.deleteButton = deleteButton;
                    comp.SetMan(man); 
                    GameObject centerPoint = new GameObject("center");
                    centerPoint.transform.parent = placedModel.gameObject.transform;
                    if(objPos != null) { 
                        StartCoroutine(SetHeldOfObjectWithWait(comp));
                    }
                } else {
                    Object comp = placedModel.gameObject.AddComponent<Object>();
                    comp.SetDetails(new ObjectDetails(name,description,type));
                    comp.SetInvis(invis);
                    comp.leftHand = leftHand;
                    comp.deleteButton = deleteButton;
                    comp.SetParticleSystem(placedParticle);
                    comp.SetMan(man);
                }
                if(objPos != null) {
                    placedModel.gameObject.transform.position = ArrayToVector(objPos);
                    placedModel.gameObject.transform.eulerAngles = ArrayToVector(objRot);
                    placedModel.gameObject.transform.localScale = ArrayToVector(objScale);
                }
            } else {
                Debug.LogError("failed to load " + file);
                return;
            }
        } else {
            Debug.LogError("failed to load " + file);
            return;
        }
    }

    //if you do it instantly it wont cut the wall out.
    //this may be broken on less performant pcs im not sure.
    IEnumerator SetHeldOfObjectWithWait(Window comp) {
        yield return new WaitForSeconds(0.5f);
        comp.LoadWindow();
    }

    public Vector3 ArrayToVector(float[] vecArray) {
        return new Vector3(vecArray[0],vecArray[1],vecArray[2]);
    }
}
