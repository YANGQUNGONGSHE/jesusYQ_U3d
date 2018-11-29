using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class Segment : ISqlTable<Segment>
{
    /// <summary>
    /// 主键；
    /// </summary>
    [ProtoMember(1)]
    public int Id { get; set; }

    /// <summary>
    /// 章ID；
    /// </summary>
    [ProtoMember(2)]
    public int ChapterId { get; set; }

    [ProtoMember(3)]
    public int SectionId { get; set; }

    /// <summary>
    /// 序号；
    /// </summary>
    [ProtoMember(4)]
    public string Number { get; set; }

    /// <summary>
    /// 节内容；
    /// </summary>
    [ProtoMember(5)]
    public string Sentence { get; set; }

    /// <summary>
    /// 文意注解；
    /// </summary>
    [ProtoMember(6)]
    public string Annotation { get; set; }

    public override string DataBasePath()
    {
        return DBPATH.BIBLE;
    }
}
