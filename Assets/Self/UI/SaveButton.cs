using UnityEngine;
using System;
using TMPro;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;
public class SaveButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textcomp;

    [SerializeField]
    private SaveLevel saveLevel;

    private string saveName;

    private float yieldTime;

    void Setup() {
        saveName = GetSaveName();
        this.textcomp.text = $@"<size=8>Save Game</size><size=6>
<alpha=#88>Game will be saved as:</size>
<size=6>{saveName}</size>";
    }

    void Start() {
        PressableButton buttonScript = gameObject.GetComponent<PressableButton>();
        buttonScript.selectEntered.AddListener(OnSelectEntered);
        textcomp = this.gameObject.transform.Find("Frontplate/AnimatedContent/Text").gameObject.GetComponent<TMP_Text>();
        Setup();
    }
    void Update() {
        if(yieldTime > 0) {
            yieldTime -= Time.deltaTime;
            return;
        }
        Setup();
    }
    private string GetSaveName() {
        return $"save_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.json";
    }

    private void SaveHandler() {
        Debug.Log("saving.....");
        if(this.textcomp == null) {
            Debug.LogError("no text comp found");
            return;
        }
        this.textcomp.text = Save();
        yieldTime = 1.5f;
    }

    private string Save() {
        if(this.saveLevel == null) {
            return "SAVE IMPORTER NOT FOUND";
        }
        if(saveLevel.Save(saveName)) {
            return $@"<size=8>Saved as:</size><size=6>
<alpha=#88>{saveName}</size>";
        }
        return "SAVE FAILED CHECK LOG";
    }

    protected virtual void OnSelectEntered(SelectEnterEventArgs args) => SaveHandler();

}
