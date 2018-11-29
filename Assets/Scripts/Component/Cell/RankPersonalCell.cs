using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class RankPersonalCell : BaseCell<RankPersonalModel>
{

    [SerializeField]
    private Text _mDisplyName;
    [SerializeField]
    private Text _mRankNumber;
    [SerializeField]
    private CircleRawImage _mHeadRawImage;

    public override void InitUi(int index, RankPersonalModel t, Action<int, RankPersonalModel> onCellClickCallback = null, Action<int, RankPersonalModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        _mDisplyName.text = !string.IsNullOrEmpty(t.DisplayName) ? t.DisplayName : t.UserName;
        _mRankNumber.text = t.RankNumber.ToString();
        if (t.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.AvatarUrl))
            {
                HttpManager.RequestImage(t.AvatarUrl+LoadPicStyle.ThumbnailRankHead, d =>
                {
                    if (d)
                    {
                        t.HeadTexture2D = d;
                        _mHeadRawImage.texture = d;
                    }
                    else
                    {
                        _mHeadRawImage.texture = DefaultImage.Head;
                    }
                } );
            }
            else
            {
                _mHeadRawImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mHeadRawImage.texture = t.HeadTexture2D;
        }
    }
}
