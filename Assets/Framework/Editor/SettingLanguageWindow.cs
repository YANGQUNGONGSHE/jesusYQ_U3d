using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SettingLanguageWindow : EditorWindow 
{
    private List<string> m_List = new List<string>();
    int index = 0;

    public SettingLanguageWindow()
    {
        m_List.Add(Language.EN.ToString());
        m_List.Add(Language.ZH.ToString());
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("box");
        index = EditorGUILayout.Popup("语言", index, m_List.ToArray(), new GUILayoutOption[0]);
        EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("保存", GUILayout.Width(100)))
		{
            EditorApplication.delayCall = OnSelectLanguageCallback;
		}
        EditorGUILayout.EndHorizontal();
    }

    void OnSelectLanguageCallback()
    {
        string lan = m_List.ToArray()[index];
        LanguageDBModel.Instance.CurrentLanguage = (Language)Enum.Parse(typeof(Language), lan);
        TextLanguageComponent[] arr = UnityEngine.Object.FindObjectsOfType<TextLanguageComponent>();
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].Refresh();
        }
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }
}
