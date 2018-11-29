using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class AllMyRankGroupCell : BaseCell<RankGroupModel> {

    [SerializeField] private Text _mGroupName;
    [SerializeField] private Text _mGroupRankNumber;
    [SerializeField] private CircleRawImage _mGroupRawImage;
    [SerializeField] private Text _mChangeRankNumber;


    public override void InitUi(int index, RankGroupModel t, Action<int, RankGroupModel> onCellClickCallback = null, Action<int, RankGroupModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        _mGroupName.text = t.GroupName;
        _mGroupRankNumber.text = t.RankNumber.ToString();
        var count = t.LastRankNumber - t.RankNumber;
        _mChangeRankNumber.text = count >= 0 ? string.Format("{0}{1}", "+", count) : count.ToString();
        if (t.GroupTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.GroupHeadUrl))
            {
                HttpManager.RequestImage(t.GroupHeadUrl + LoadPicStyle.ThumbnailHead, d =>
                {
                    if (d)
                    {
                        t.GroupTexture2D = d;
                        _mGroupRawImage.texture = d;
                    }
                    else
                    {
                        _mGroupRawImage.texture = DefaultImage.Head;
                    }
                });
            }
            else
            {
                _mGroupRawImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mGroupRawImage.texture = t.GroupTexture2D;
        }
    }
}
