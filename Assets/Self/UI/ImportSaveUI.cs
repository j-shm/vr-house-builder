using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportSaveUI : MonoBehaviour
{
    [SerializeField]
    private GameObject actionButton;  
    string savePath = Application.persistentDataPath + "/saves";
    void Start() {
        Populate();
    }  
    void Populate() {
        if(actionButton == null) {
            Debug.LogError("no actionButton found.");
            return;
        }

        foreach(string file in GetAllFiles()) {
            GameObject button = Instantiate(actionButton,gameObject.transform);
            ButtonImporterSave buttonScript = button.AddComponent<ButtonImporterSave>();
            buttonScript.Setup(file);
        }

    }    
    public string[] GetAllFiles() {
        List<string> models = new List<string>();
        if(System.IO.Directory.Exists(savePath)) {
            string[] files = System.IO.Directory.GetFiles(savePath, "*.json");
            Debug.Log(files);
            return files;
        }
        Debug.Log(savePath);
        
        return null;
    }
}
