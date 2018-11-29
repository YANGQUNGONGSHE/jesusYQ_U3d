using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class SementCell : BaseCell<Segment>
{

    private Text _mSegmentId;


    protected override void OnAwake()
    {
        _mSegmentId = transform.Find("Text").GetComponent<Text>();
    }

    public override void InitUi(int index, Segment t, Action<int, Segment> onCellClickCallback = null, Action<int, Segment> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        
        SetDefaultUi();
    }


    private void SetDefaultUi()
    {
        if(string.IsNullOrEmpty(t.Number))return;
        _mSegmentId.text = t.Number;
    }
}
