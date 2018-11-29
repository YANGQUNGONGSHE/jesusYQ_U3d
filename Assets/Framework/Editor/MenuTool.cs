//===================================================
//作    者：王家俊  http://www.unity3d.com  QQ：394916173
//创建时间：2016-08-30 17:00:55
//备    注：
//===================================================


using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace WongJJ.Game.Core
{
	public class MenuTool
	{
		[MenuItem ("WongJJ/AssetBundle/一键打包")]
		public static void CreateAssetBundlePath ()
		{
			AssetBundleWindow window = EditorWindow.GetWindow<AssetBundleWindow> ();
			window.titleContent = new GUIContent ("打包资源");
			window.Show ();
		}

        [MenuItem ("WongJJ/AssetBundle/拷贝初始资源到StreamingAsset")]
        public static void CopyAssetBundleToFile ()
        {
            string path = Application.streamingAssetsPath + "/AssetBundle/";
            if (System.IO.Directory.Exists(path))
            {
                Directory.Delete(path,true);
            }
            Directory.CreateDirectory(path);
            Util.CopyDirectory(Application.persistentDataPath,path);
            AssetDatabase.Refresh();
            Debug.Log("Copy Finish !!"); 
        }

        [MenuItem ("WongJJ/调试设置")]
        public static void Setting()
        {
            SettingWindow window = EditorWindow.GetWindow<SettingWindow> ();
            window.titleContent = new GUIContent ("调试设置");
            window.Show ();
        }

		[MenuItem("WongJJ/本地化语言")]
		public static void SetLanguage()
		{
            SettingLanguageWindow window = EditorWindow.GetWindow<SettingLanguageWindow>();
			window.titleContent = new GUIContent("本地化语言设置");
			window.Show();
		}
	}
}