using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
public class ButtonImporterSave : MonoBehaviour
{
    public LoadLevel levelLoader;
    string file;
    void Start()
    {
        PressableButton buttonScript = gameObject.GetComponent<PressableButton>();
        buttonScript.selectEntered.AddListener(OnSelectEntered);
    }
    public void Setup(string file, LoadLevel loader) {
        this.file = file;
        GameObject text = this.gameObject.transform.Find("Frontplate/AnimatedContent/Text").gameObject;
        TMP_Text textComp = text.GetComponent<TMP_Text>();
        textComp.text = file.Split("\\")[1];
        textComp.fontSize = 10; 
        //textComp.enableAutoSizing = true; //i think auto sizing looks weird
        text.SetActive(true);
        this.levelLoader = loader;
    }

    void Load() {
        levelLoader.Import(file);
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args) => Load();
}
