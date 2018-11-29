using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class BlackCell : BaseCell<BlackModel>
{

    public CircleRawImage HeadImage;
    public Text DisplayName;
    public Text Signature;

    public override void InitUi(int index, BlackModel t, Action<int, BlackModel> onCellClickCallback = null, Action<int, BlackModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        DisplayName.text = !string.IsNullOrEmpty(t.DisPlayName) ? t.DisPlayName : t.UserName;
        Signature.text = t.Signature;

        if (t.HeadTexture2D==null)
        {
            if (!string.IsNullOrEmpty(t.HeadUrl))
            {
                HttpManager.RequestImage(t.HeadUrl+ LoadPicStyle.ThumbnailHead, d =>
                {
                    if (d)
                    {
                        HeadImage.texture = d;
                        t.HeadTexture2D = d;
                    }
                    else
                    {
                        HeadImage.texture = DefaultImage.Head;
                    }
                } );
            }
            else
            {
                HeadImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            HeadImage.texture = t.HeadTexture2D;
        }

    }
}
