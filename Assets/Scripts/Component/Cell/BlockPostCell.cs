using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class BlockPostCell : BaseCell<PostModel> {

    public CircleRawImage HeadImage;
    public Text DisplayName;
    public Text Summary;

    public override void InitUi(int index, PostModel t, Action<int, PostModel> onCellClickCallback = null, Action<int, PostModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        DisplayName.text = t.Author.DisplayName;
        Summary.text = t.Title;
        if (t.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.Author.AvatarUrl))
            {
                HttpManager.RequestImage(t.Author.AvatarUrl + LoadPicStyle.ThumbnailHead, d =>
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
                });
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
