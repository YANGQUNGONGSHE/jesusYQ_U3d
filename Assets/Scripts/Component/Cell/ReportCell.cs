using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ReportCell:BaseCell<string>
{

    public Text Content;
    public Image SelectIcon;
    public Sprite[] IconSprites;

    public override void InitUi(int index, string t, Action<int, string> onCellClickCallback = null, Action<int, string> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        Content.text = t;
        SelectIcon.sprite = isSelected ? IconSprites[0] : IconSprites[1];
    }
}
