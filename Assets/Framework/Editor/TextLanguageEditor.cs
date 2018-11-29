using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TextLanguageComponent),true)]
public class TextLanguageEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TextLanguageComponent textComm = base.target as TextLanguageComponent;

        int valueIndex = 0, index = 0;

        valueIndex = LanguageDBModel.Instance.GetModules().IndexOf(textComm.Module);
        index = EditorGUILayout.Popup("模块", valueIndex, LanguageDBModel.Instance.GetModules().ToArray(), new GUILayoutOption[0]);
        if(valueIndex != index)
        {
            textComm.Module = LanguageDBModel.Instance.GetModules()[index];
            EditorUtility.SetDirty(base.target);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            textComm.Refresh();
        }

        valueIndex = LanguageDBModel.Instance.GetKeysByModule(textComm.Module).IndexOf(textComm.Key);
		index = EditorGUILayout.Popup("Key", valueIndex, LanguageDBModel.Instance.GetKeysByModule(textComm.Module).ToArray(), new GUILayoutOption[0]);
		if (valueIndex != index)
		{
            textComm.Key = LanguageDBModel.Instance.GetKeysByModule(textComm.Module)[index];
			EditorUtility.SetDirty(base.target);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			textComm.Refresh();
		}
    }
}
