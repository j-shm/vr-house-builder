using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    [SerializeField]
    public TouchScreenKeyboard keyboard;

    public void ShowKeyboard()
    {
        Debug.Log("ShowKeyboard");
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    public void HideKeyboard()
    {
        Debug.Log("HideKeyboard");
        keyboard.active = false;
    }
}
