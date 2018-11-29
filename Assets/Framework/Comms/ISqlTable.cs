using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System;
using WongJJ.Game.Core;

/**
 * @Summary:ISqlTable
 * @Author：WongJJ
 * @Date: 2017-07-19 14:52:03
 * @Remark: 数据库表结构抽象类，表实体类集成该类可以拥有基础CRUD功能
 */
public abstract class ISqlTable<T> where T : class, new()
{
    /// <summary>
    /// The sql manager.
    /// </summary>
    private SimpleSQLManager m_sqlManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:ISqlTable`1"/> class.
    /// </summary>
    public ISqlTable()
    {
        string databaseName = DataBasePath().Substring(DataBasePath().LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) + 1);
        if (!LocalDataManager.SqlManagerDict.ContainsKey(databaseName))
        {
            m_sqlManager = LocalDataManager.Instance.gameObject.AddComponent<SimpleSQLManager>();
            //#if ASSETBUNDLE_MODEL
            //            m_sqlManager.databaseFile = LoadAssetBundleDBSource(DataBasePath(), DataBasePath().Substring(DataBasePath().LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) + 1));
            //#else
            //            m_sqlManager.databaseFile = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(string.Format("Assets/{0}", DataBasePath().Replace("assetbundle", "bytes")));
            //#endif
            m_sqlManager.databaseFile = Resources.Load<TextAsset>(DataBasePath());
            LocalDataManager.SqlManagerDict.Add(databaseName, m_sqlManager);
        }
        else
        {
            m_sqlManager = LocalDataManager.SqlManagerDict[databaseName];
        }
    }

    /// <summary>
    /// Loads the asset bundle DB Source.
    /// </summary>
    /// <returns>The asset bundle DBS ource.</returns>
    /// <param name="path">Path.</param>
    /// <param name="AbName">Ab name.</param>
    private TextAsset LoadAssetBundleDBSource(string path, string AbName)
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<TextAsset>(AbName);
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <returns>The create.</returns>
    public virtual bool Insert()
    {
        if (m_sqlManager != null)
        {
            int isSucc = m_sqlManager.Insert(this as T);
            if (isSucc > 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    /// <summary>
    /// 建议使用SaveManager LoadTable进行查询,最好不要使用改具体类查询。代码没有问题，只是不好看。
    /// </summary>
    /// <returns>The retrieve.</returns>
    /// <param name="query">查询语句SQL</param>
    /// <param name="args">参数.</param>
    public virtual List<T> Retrieve(string query, params object[] args)
    {
        List<T> list = new List<T>();
        if (m_sqlManager == null)
            return null;
        return m_sqlManager.Query<T>(query, args);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <returns>The update.</returns>
    protected virtual bool Update() { return false; }

    /// <summary>
    /// 删除
    /// </summary>
    /// <returns>The delete.</returns>
    protected virtual bool Delete() { return false; }

    /// <summary>
    /// 数据库文件路径
    /// </summary>
    /// <returns>The base path.</returns>
    public abstract string DataBasePath();
}
