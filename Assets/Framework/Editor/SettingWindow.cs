using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SettingWindow : EditorWindow
{
	private List<MacorItem> list = new List<MacorItem> ();
	private Dictionary<string,bool> dic = new Dictionary<string, bool> ();
	private string macor = string.Empty;

	void OnEnable()
	{
		macor = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS);
		list.Clear ();
		list.Add(new MacorItem(){Name = "PRINTLOG",DisplayName = "打印日志",IsDebug = true,IsRelease = false});
		list.Add(new MacorItem(){Name = "DEBUGPART",DisplayName = "调试模式",IsDebug = true,IsRelease = false});
		list.Add(new MacorItem(){Name = "REALMACHINE",DisplayName = "真机模式",IsDebug = false,IsRelease = true});
		list.Add(new MacorItem(){Name = "TESTPART",DisplayName = "测试模式",IsDebug = true,IsRelease = false});
		list.Add(new MacorItem(){Name = "TEST_FIGHT",DisplayName = "测试战斗",IsDebug = true,IsRelease = false});
		list.Add(new MacorItem(){Name = "EDITOR_MAC",DisplayName = "Mac编辑器环境",IsDebug = true,IsRelease = false});
		list.Add(new MacorItem(){Name = "ASSETBUNDLE_MODEL",DisplayName = "AssetBundle加载模式",IsDebug = true,IsRelease = false});

		for (int i = 0; i < list.Count; i++) 
		{
			if (!string.IsNullOrEmpty (macor) && macor.IndexOf (list [i].Name) != -1) 
			{
				dic [list[i].Name] = true;
			}  
			else
			{
				dic [list[i].Name] = false;
			}
		}
	}

	void OnGUI()
	{
		for (int i = 0; i < list.Count; i++) 
		{
			EditorGUILayout.BeginHorizontal ("box");
			dic[list[i].Name] = GUILayout.Toggle(dic[list[i].Name], list[i].DisplayName);
			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("保存", GUILayout.Width(100)))
		{
			Save();
		}

		if (GUILayout.Button("调试", GUILayout.Width(100)))
		{
			for(int i=0;i<list.Count;i++)
			{
				dic[list[i].Name] = list[i].IsDebug;
			}
			Save();
		}

		if (GUILayout.Button("发布", GUILayout.Width(100)))
		{
			for(int i=0;i<list.Count;i++)
			{
				dic[list[i].Name] = list[i].IsRelease;
			}
			Save();
		}
		EditorGUILayout.EndHorizontal();
	}

	void Save()
	{
		macor = string.Empty;
		foreach (var item in dic)
		{
			if (item.Value)
			{
				macor += string.Format("{0};", item.Key);
			}

			if (item.Key.Equals ("ASSETBUNDLE_MODEL", System.StringComparison.CurrentCultureIgnoreCase)) 
			{
				EditorBuildSettingsScene[] sceneArr = EditorBuildSettings.scenes;
				for (int i = 0; i < sceneArr.Length; i++) 
				{
					if (sceneArr[i].path.IndexOf ("Downloads", System.StringComparison.CurrentCultureIgnoreCase) > -1) 
					{
						sceneArr[i].enabled = !item.Value;
					}
				}
				EditorBuildSettings.scenes = sceneArr;
			}
		}

	    macor += "UNITY;"; //NIM需要
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone,macor);
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS,macor);
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android,macor);
	}
}

public class MacorItem
{
	public string Name;
	public string DisplayName;
	public bool IsDebug;
	public bool IsRelease;
}


