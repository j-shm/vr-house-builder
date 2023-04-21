using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private List<GameObject> objects = new List<GameObject>();

    public void AddObject(GameObject obj) {
        objects.Add(obj);
    }

    public bool RemoveObject(GameObject obj) {
        if(objects.Contains(obj)) {
            objects.Remove(obj);
            return true;
        }
        return false;
    }
    public List<GameObject> GetObjects() {
        return objects;
    }
}
