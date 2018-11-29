using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class Biography : ISqlTable<Biography>
{
    /// <summary>
    /// 主键；
    /// </summary>
    [ProtoMember(1)]
    public int Id { get; set; }

    /// <summary>
    /// 记的中文名称；
    /// </summary>
    [ProtoMember(2)]
    public string Name { get; set; }

    /// <summary>
    /// 中文缩写；
    /// </summary>
    [ProtoMember(3)]
    public string Abbreviation { get; set; }

    /// <summary>
    /// 记的英文名称；
    /// </summary>
    [ProtoMember(4)]
    public string EnglishName { get; set; }

    /// <summary>
    /// 该记下共有多少章；
    /// </summary>
    [ProtoMember(5)]
    public int ChaptersCount { get; set; }


    public override string DataBasePath()
    {
        return DBPATH.BIBLE;
    }
}
