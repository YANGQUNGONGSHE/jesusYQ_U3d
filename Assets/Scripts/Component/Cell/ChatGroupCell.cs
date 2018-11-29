using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatGroupCell : BaseCell<ChatGroupModel>
{
    [SerializeField]
    private Text _mDisplayNameText;

    [SerializeField]
    private Text _mBriefText;

    [SerializeField]
    private CircleRawImage _mHeadIcon;

    public override void InitUi(int index, ChatGroupModel t, Action<int, ChatGroupModel> onCellClickCallback = null, Action<int, ChatGroupModel> onCellLongPressCallback = null, bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
         _mDisplayNameText.text = t.GroupName;
        _mBriefText.text = t.GroupBrief;
        if (t.GroupHeadIconTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.GroupHeadIconUrl))
            {
                HttpManager.RequestImage(t.GroupHeadIconUrl+LoadPicStyle.ThumbnailHead, (texture2D) =>
                {
                    if (texture2D)
                    {
                        _mHeadIcon.texture = texture2D;
                        t.GroupHeadIconTexture2D = texture2D;
                    }
                    else
                    {
                        _mHeadIcon.texture = DefaultImage.Head;
                    } 
                });
            }
            else
            {
                _mHeadIcon.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mHeadIcon.texture = t.GroupHeadIconTexture2D;
        }
    }
}
