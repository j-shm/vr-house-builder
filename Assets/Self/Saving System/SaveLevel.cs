using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveLevel : MonoBehaviour
{
    public ObjectManager man;
    public bool doSave;

    void Update()
    {
        if(doSave) {
            doSave = false;
            Save();
        }
    }

    public bool Save() {
        if(this.man == null) {
            Debug.LogError("no object man");
            return false;
        }
        if(this.man.GetObjects().Count == 0) {
            Debug.LogError("nothing to save");
            return false;
        }


        GameObject[] objects = this.man.GetObjects().ToArray();
        List<SerialObject> serialObjectsList = new List<SerialObject>();

        foreach(var obj in objects) {
            serialObjectsList.Add(ObjectToSerialObject(obj));
        }
        SerialObjects serialObjects = new SerialObjects(serialObjectsList.ToArray()); 
        string jsonoutput = JsonConvert.SerializeObject(serialObjects);
        string savePath = Application.persistentDataPath + "/saves/";
        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }

        string saveName = "save.json"; // allow to be changed!

        File.WriteAllText(savePath+saveName, jsonoutput);
        
        return true;
    }

    private SerialObject ObjectToSerialObject(GameObject obj) {
        SerialObject serialObj = new SerialObject(
            obj.gameObject.name,
            VectorToArray(obj.transform.position),
            VectorToArray(obj.transform.rotation.eulerAngles),
            VectorToArray(obj.transform.localScale)
            );

        return serialObj;
    }

    public float[] VectorToArray(Vector3 vec) {
        float[] vecArray = {vec.x,vec.y,vec.z};
        return vecArray;
    }
}