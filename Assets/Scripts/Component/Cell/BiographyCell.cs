using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class BiographyCell : BaseCell<Biography>
{


    [HideInInspector] public Text BiographyName;

    protected override void OnAwake()
    {
        BiographyName = transform.Find("Text").GetComponent<Text>();
    }


    public override void InitUi(int index, Biography t, Action<int, Biography> onCellClickCallback = null, Action<int, Biography> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        BiographyName.color = isSelected ? Color.red : Color.black;
        SetDefaultUi();
    }

    private void SetDefaultUi()
    {
        BiographyName.text = t.Name;
    }
}
