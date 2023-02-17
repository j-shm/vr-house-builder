using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetails {
    private string name;
    private string description;
    private string type; 
    public ObjectDetails(string name, string description, string type) {
        SetName(name);
        SetDescription(description);
        SetTypeOfObject(type);
    }
    public string GetName() {
        return name;
    }
    public string GetDescription() {
        return description;
    }
    public string GetTypeOfObject() {
        return type;
    }
    public void SetName(string name) {
        if(name != null) {
            this.name = name;
        }
        name = "no name for object";
    }
    public void SetDescription(string description) {
        if(description != null) {
            this.description = description;
        }
        description = "no description for object";
    }
    public void SetTypeOfObject(string type) {
        if(type != null) {
            this.type = type;
        }
        type = "no type for object";
    }
}
