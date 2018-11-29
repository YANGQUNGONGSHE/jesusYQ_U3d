using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using ProtoBuf;


#region Bible

[ProtoContract]
public class BibleRecord
{
    [ProtoMember(1)]
    public Biography biography { get; set; }

    [ProtoMember(2)]
    public Chapter chapter { get; set; }

    [ProtoMember(3)]
    public Segment segment { get; set; }
}

#endregion