using System;
using System.Collections.Generic;
using System.IO;

public class GameDataTableParser : IDisposable
{
    #region xorScale 异或因子
    /// <summary>
    /// 异或因子
    /// </summary>
    private byte[] xorScale = new byte[] { 45, 66, 38, 55, 23, 254, 9, 165, 90, 19, 41, 45, 201, 58, 55, 37, 254, 185, 165, 169, 19, 171 };//.data文件的xor加解密因子
    #endregion

    #region GameDataTableParser 构造函数
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="path"></param>
    public GameDataTableParser(string path)
    {
        m_FieldNameDic = new Dictionary<string, int>();
        byte[] buffer = null;

        //------------------
        //第1步：读取文件
        //------------------
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
        }

        //------------------
        //第2步：解压缩
        //------------------
        buffer = ZlibHelper.DeCompressBytes(buffer);

        //------------------
        //第3步：xor解密
        //------------------
        int iScaleLen = xorScale.Length;
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(buffer[i] ^ xorScale[i % iScaleLen]);
        }

        //------------------
        //第4步：解析数据到数组
        //------------------
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            m_Row = ms.ReadInt();
            m_Column = ms.ReadInt();

            m_GameData = new String[m_Row, m_Column];
            m_FieldName = new string[m_Column];

            for (int i = 0; i < m_Row; i++)
            {
                for (int j = 0; j < m_Column; j++)
                {
                    string str = ms.ReadUTF8String();

                    if (i == 0)
                    { 
                        //表示读取的是字段
                        m_FieldName[j] = str;
                        m_FieldNameDic[str] = j;

                    }
                    else if (i > 2)
                    {
                        //表示读取的是内容
                        m_GameData[i, j] = str;
                    }
                }
            }
        }
    }
    #endregion

    #region 字段或属性
    /// <summary>
    /// 行数
    /// </summary>
    private int m_Row;

    /// <summary>
    /// 列数
    /// </summary>
    private int m_Column;

    /// <summary>
    /// 字段名称
    /// </summary>
    private string[] m_FieldName;

    /// <summary>
    /// 字段名称
    /// </summary>
    public string[] FieldName
    {
        get { return m_FieldName; }
    }

    /// <summary>
    /// 游戏数据
    /// </summary>
    private string[,] m_GameData;

    /// <summary>
    /// 当前行号
    /// </summary>
    private int m_CurRowNo = 3;

    /// <summary>
    /// 字段名称字典
    /// </summary>
    private Dictionary<string, int> m_FieldNameDic;

    /// <summary>
    /// 是否结束
    /// </summary>
    public bool Eof
    {
        get
        {
            return m_CurRowNo == m_Row;
        }
    }
    #endregion

    #region Next 转到下一条
    /// <summary>
    /// 转到下一条
    /// </summary>
    public void Next()
    {
        if (Eof) return;
        m_CurRowNo++;
    }
    #endregion

    #region GetFieldValue 获取字段值
    /// <summary>
    /// 获取字段值
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public string GetFieldValue(string fieldName)
    {
        try
        {
            if (m_CurRowNo < 3 || m_CurRowNo >= m_Row) return null;
            return m_GameData[m_CurRowNo, m_FieldNameDic[fieldName]];
        }
        catch { return null; }
    }
    #endregion

    #region Dispose 释放
    /// <summary>
    /// 释放
    /// </summary>
    void IDisposable.Dispose()
    {
        m_FieldNameDic.Clear();
        m_FieldNameDic = null;

        m_FieldName = null;
        m_GameData = null;
    }
    #endregion
}
