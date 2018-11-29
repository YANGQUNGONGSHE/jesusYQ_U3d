using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;


public class LocalCell : BaseCell<LocalModel>
{
    private Text mName;

    protected override void OnAwake()
    {
        base.OnAwake();
        mName = transform.Find("Text").GetComponent<Text>();
    }

    public override void InitUi(int index, LocalModel t, Action<int, LocalModel> onCellClickCallback = null, Action<int, LocalModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        mName.text = t.Name;
    }
}
