using UnityEngine;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject parentMenu;
    void Start()
    {
        PressableButton buttonScript = gameObject.GetComponent<PressableButton>();
        buttonScript.selectEntered.AddListener(OnSelectEntered);
    }
    void Open() {
        menu.SetActive(!menu.activeSelf);
        parentMenu.SetActive(false);
    }
    protected virtual void OnSelectEntered(SelectEnterEventArgs args) => Open();
}
