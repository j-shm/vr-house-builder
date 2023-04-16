using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class SaveLevel : MonoBehaviour
{
    public ObjectManager man;
    public bool doSave;

    void Update()
    {
        if(doSave) {
            doSave = false;
            Save($"save_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.json");
        }
    }

    public bool Save(string saveName) {
        if(this.man == null) {
            Debug.LogError("no object man");
            return false;
        }
        if(this.man.GetObjects().Count == 0) {
            Debug.LogError("nothing to save");
            return false;
        }
        string savePath = Application.persistentDataPath + "/saves/";
        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }
        if(File.Exists(savePath+saveName)) {
            Debug.Log("file already exists");
            return false;
        }

        GameObject[] objects = this.man.GetObjects().ToArray();
        List<SerialObject> serialObjectsList = new List<SerialObject>();

        foreach(var obj in objects) {
            serialObjectsList.Add(ObjectToSerialObject(obj));
        }
        SerialObjects serialObjects = new SerialObjects(serialObjectsList.ToArray()); 
        string jsonoutput = JsonConvert.SerializeObject(serialObjects);

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