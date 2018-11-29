using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class BookCell : BaseCell<BookModel>
{

    //public Text MBookName;
    public Image MBookCover;

    public override void InitUi(int index, BookModel t, Action<int, BookModel> onCellClickCallback = null, Action<int, BookModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        SetDefaultUi();
    }

    private void SetDefaultUi()
    {
        MBookCover.sprite = t.BookCover;
    }
}
