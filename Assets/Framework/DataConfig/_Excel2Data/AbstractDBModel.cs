//===================================================
//作    者：王家俊  http://www.unity3d.com  QQ：394916173
//创建时间：2016-08-26 23:47:22
//备    注：数据管理基类-> 子类使用工具自动生成
//===================================================

using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDBModel<T,P> where T:class,new() where P:AbstractEntity
{
    private static T m_instance;
    protected List<P> m_List;
    private Dictionary<int, P> m_Dict;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new T();
            }
            return m_instance;
        }
    }

    public AbstractDBModel()
    {
        m_Dict = new Dictionary<int, P>();
        m_List = new List<P>();
        LoadData();
    }

    protected abstract string FileName
    {
        get;
    }

    protected abstract P MakeEntity(GameDataTableParser parse);

    public void LoadData()
    {
//        string path = Application.dataPath + "/Data/";
//        string path =  Application.dataPath + "/../WWW/";
        string path =  "/Users/bosshong/UnityProject_5.x/SG_20170629/Assets/Download/Data/";
//        using (GameDataTableParser parse = new GameDataTableParser(string.Format("E:\\Unity_Learn_test_projects\\JJMMORpg\\Assets\\www\\Data\\{0}", FileName)))
        using(GameDataTableParser parse = new GameDataTableParser(path + FileName))
        {
            while (!parse.Eof)
            {
                P p = MakeEntity(parse);
                m_List.Add(p);
                m_Dict[p.Id] = p;
                parse.Next();
            }
        }
    }

    public List<P> GetList()
    {
        return m_List;
    }

    public P Get(int id)
    {
        if (m_Dict.ContainsKey(id))
        {
            return m_Dict[id];
        }
        return null;
    }

}