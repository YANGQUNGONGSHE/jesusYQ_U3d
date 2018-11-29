using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class BibleShowFiler : BaseFiller<Segment> {
    

    public int FontSize = CommDefine.FontSize;
    public bool IsBeginReading = false;
    public Action<int> OnBeginReader;
    public Action<int> OnEndReader;
    public Dictionary<int, bool> SelectedIndexs;

    protected override void OnAwake()
    {
        SelectedIndexs = new Dictionary<int, bool>();
    }

    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("SegmentScrollRect").GetComponent<ListScrollRect>();
    }


    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
         if(DataSource==null||DataSource.Count<1)return;

         bool hasSelected;
         SelectedIndexs.TryGetValue(index, out hasSelected);

         obj.transform.GetComponent<BibleShowCell>().SegmentContent.fontSize = FontSize;
         obj.transform.GetComponent<BibleShowCell>().InitUi(index,DataSource[index], (i, segment) =>
        {
            if (SelectedIndexs.ContainsKey(i))
                SelectedIndexs.Remove(i);
            else
                SelectedIndexs.Add(i, true);

            OnCellClick(i, segment);
            Refresh();
        }, null, IsBeginReading,hasSelected);

        obj.transform.GetComponent<BibleShowCell>().OnBeginReaderClick = i =>
        {
            if (OnBeginReader != null)
            {
                OnBeginReader(i);
            }
        };

        obj.transform.GetComponent<BibleShowCell>().OnEndReaderClick = i =>
        {
            if (OnEndReader != null)
            {
                OnEndReader(i);
            }
        };

    }
}
