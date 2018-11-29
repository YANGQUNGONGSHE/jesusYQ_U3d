using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextLanguageComponent : MonoBehaviour 
{
    [HideInInspector]
    public string Module;
    [HideInInspector]
    public string Key;

    public void Refresh()
    {
        if (string.IsNullOrEmpty(Module) || string.IsNullOrEmpty(Key))
            return;
        Text text = GetComponent<Text>();

        text.text = LanguageDBModel.Instance.GetText(Module, Key);
    }
}
