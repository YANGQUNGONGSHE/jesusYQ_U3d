using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WongJJ.Game.Core.SimpleObjectPool
{
    public class ObjectPoolEditor
    {
        [MenuItem("WongJJ/点击生成对象池配置")]
        public static void CreatePoolList()
        {
			ObjectPoolConfig PoolList = ScriptableObject.CreateInstance<ObjectPoolConfig>();
			string path = "Assets/Resources/ObjectPoolConfig.asset";
			AssetDatabase.CreateAsset(PoolList,path);
			AssetDatabase.SaveAssets();
			EditorUtility.DisplayDialog("腻害了","创建成功，目录:" + path, "牛逼！");
        }
    }
}