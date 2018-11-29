using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class SliderCell : BaseCell<RecommendationModel>
{
    [SerializeField]
    private RawImage _mImage;

    public override void InitUi(int index, RecommendationModel t, Action<int, RecommendationModel> onCellClickCallback = null, Action<int, RecommendationModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        if (t.PicTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.PictureUrl))
            {
                HttpManager.RequestImage(t.PictureUrl+LoadPicStyle.Cell, texture2D =>
                {
                    if (texture2D)
                    {
                        _mImage.texture = texture2D;
                        t.PicTexture2D = texture2D;
                    }
                    else
                    {
                        _mImage.texture = DefaultImage.Cover;
                    }   
                });
            }
            else
            {
                _mImage.texture = DefaultImage.Cover;
            }
        }
        else
        {
            _mImage.texture = t.PicTexture2D;
        }
    }
}
