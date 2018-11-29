using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using SimpleSQL;
using UnityEngine;
using WongJJ.Game.Core;
using Object = System.Object;

public class LocalDataManager : KeepSingletion<LocalDataManager>
{
    private string _mFilePath;

    /// <summary>
    /// 全局数据库管理器
    /// </summary>
    static public Dictionary<string, SimpleSQLManager> SqlManagerDict = new Dictionary<string, SimpleSQLManager>();

    void Awake()
    {
        _mFilePath = Application.persistentDataPath + "/";
    }

    /// <summary>
    /// 查询SQL数据
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="sql">查询语句.</param>
    /// <param name="param">参数.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    
    static public List<T> LoadSQLTable<T>(string sql, params object[] param) where T : ISqlTable<T>, new()
    {
        T t = new T();
        //            SimpleSQLManager m_sqlManager;
        //            if (!SaveManager.SqlManagerDict.ContainsKey(typeof(T).Name))
        //            {
        //                T t = new T();
        //                m_sqlManager = SaveManager.Instance.gameObject.AddComponent<SimpleSQLManager>();
        //#if ASSETBUNDLE_MODEL
        //            m_sqlManager.databaseFile = LoadAssetBundleDBSource(t.DataBasePath(), t.DataBaseFullName());
        //#else
        //                m_sqlManager.databaseFile = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(string.Format("Assets/{0}", t.DataBasePath().Replace("assetbundle", "bytes")));
        //#endif
        //    SaveManager.SqlManagerDict.Add(typeof(T).Name, m_sqlManager);
        //}
        //else
        //{
        //    m_sqlManager = SaveManager.SqlManagerDict[typeof(T).Name];
        //}
        return t.Retrieve(sql, param);
    }


    /// <summary>
    /// 读取本地二进制Protobuf文件
    /// </summary>
    /// <returns>The file object.</returns>
    /// <param name="key">Key.</param>
    /// <param name="type">Type.</param>
     public System.Object LoadFileObj(string key, Type type)
    {
        if (!System.IO.File.Exists(_mFilePath + key))
        {
            return null;
        }
        return ProtoBufUtils.Deserialize(_mFilePath + key, type);
    }

    /// <summary>
    /// 以Protobuf二进制方式存储内容到本地文件
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="obj">Object.</param>
     public void SaveFileObj(string key, System.Object obj)
    {
        ProtoBufUtils.Serialize(obj, _mFilePath + key);
    }

    /// <summary>
    /// 读取一个Int类型数据
    /// </summary>
    /// <returns>The int.</returns>
    /// <param name="key">Key.</param>
    public int LoadInt(string key)
    {
        return LoadInt(key, 0);
    }

    /// <summary>
    /// 读取一个Int类型数据
    /// </summary>
    /// <returns>The int.</returns>
    /// <param name="key">Key.</param>
    /// <param name="defaultValue">Default value.</param>
    public int LoadInt(string key, int defaultValue)
    {
        int @int = PlayerPrefs.GetInt(key);
        return @int;
    }

    /// <summary>
    /// 读取一个String类型数据
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="key">Key.</param>
    public string LoadStr(string key)
    {
        string @string = PlayerPrefs.GetString(key);
        return @string;
    }

    /// <summary>
    /// 存储一个Int类型数据
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    /// <summary>
    /// 存储一个String类型数据
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void SaveStr(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// 根据key删除一个本地数据
    /// </summary>
    /// <param name="key"></param>
    public void DeleteByKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public void SaveJsonObj(string key, System.Object obj)
    {
        if (obj == null) throw new ArgumentNullException("obj");
        var data = JsonMapper.ToJson(obj);
        byte[] dataByte = System.Text.Encoding.UTF8.GetBytes(data);
        using (var fs = new FileStream(_mFilePath + key, FileMode.Create))
        {
            fs.Write(dataByte, 0, dataByte.Length);
        }
    }

    public System.Object LoadJsonObj(string key)
    {
        if (!File.Exists(_mFilePath + key)) return null;
        using (var fs = new FileStream(_mFilePath + key, FileMode.Open))
        {
            int fsLen = (int)fs.Length;
            byte[] heByte = new byte[fsLen];
            int r = fs.Read(heByte, 0, heByte.Length);
            string s = System.Text.Encoding.UTF8.GetString(heByte);
            return s;
        }
    }
}

