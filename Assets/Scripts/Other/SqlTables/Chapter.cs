using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class Chapter : ISqlTable<Chapter>
{
    /// <summary>
    /// 主键；
    /// </summary>
    [ProtoMember(1)]
    public int Id { get; set; }

    /// <summary>
    /// 记ID；
    /// </summary>
    [ProtoMember(2)]
    public int BiographyId { get; set; }

    /// <summary>
    /// 序号；
    /// </summary>
    [ProtoMember(3)]
    public string Number { get; set; }

    /// <summary>
    /// 名称；
    /// </summary>
    [ProtoMember(4)]
    public string Name { get; set; }

    /// <summary>
    /// 全名；
    /// </summary>
    [ProtoMember(5)]
    public string FullName { get; set; }

    public override string DataBasePath()
    {
        return DBPATH.BIBLE;
    }
}
