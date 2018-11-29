using System;
using System.Collections;
using System.Collections.Generic;
using SimpleSQL;
using UnityEngine;

public class BibleCollect : ISqlTable<BibleCollect>
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int BiographyId { get; set; }

    public int ChapterId { get; set; }

    public int SegmentId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }


    public override string DataBasePath()
    {
        return DBPATH.USERRECORD;
    }
}
