using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChapterCell : BaseCell<Chapter>
{

    private Text _mChapterId;

    protected override void OnAwake()
    {
        _mChapterId = transform.Find("Text").GetComponent<Text>();
    }

    public override void InitUi(int index, Chapter t, Action<int, Chapter> onCellClickCallback = null, Action<int, Chapter> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        _mChapterId.color = isSelected ? Color.red : Color.black;
        SetDefaultUi();
    }


    private void SetDefaultUi()
    {
        if (string.IsNullOrEmpty(t.Number))return;
        _mChapterId.text = t.Number;
      
    }
}
