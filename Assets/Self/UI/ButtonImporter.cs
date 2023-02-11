using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class ButtonImporter : MonoBehaviour
{
    Importer importer;
    string model;
    void Start()
    {
        PressableButton buttonScript = gameObject.GetComponent<PressableButton>();
        buttonScript.selectEntered.AddListener(OnSelectEntered);
    }
    public void Setup(Importer importer, string model) {
        this.importer = importer;
        this.model = model;
        GameObject text = this.gameObject.transform.Find("Frontplate/AnimatedContent/Text").gameObject;
        TMP_Text textComp = text.GetComponent<TMP_Text>();
        textComp.text = model;
        textComp.enableAutoSizing = true;
        text.SetActive(true);
    }

    void Import() {
        importer.Import(model);
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args) => Import();

}
