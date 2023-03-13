using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImportUI : MonoBehaviour
{
    [SerializeField]
    private GameObject actionButton;  
    private Importer importer;

    void Start() {
        importer = GameObject.Find("importer").GetComponent<Importer>();
        Populate();
    }  
    void Populate() {
        if(importer == null) {
            Debug.LogError("no importer found.");
            return;
        }
        if(actionButton == null) {
            Debug.LogError("no actionButton found.");
            return;
        }

        foreach(string model in importer.GetAllFiles()) {
            GameObject button = Instantiate(actionButton,gameObject.transform);
            ButtonImporter buttonScript = button.AddComponent<ButtonImporter>();
            buttonScript.Setup(importer,model);
            
        }

    }
}
