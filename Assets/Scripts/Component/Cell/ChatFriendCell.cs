using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatFriendCell : BaseCell<ChatFrinedModel>
{
    [SerializeField]
    private Text _mDisplayNameText;

    [SerializeField]
    private Text _mBriefText;

    [SerializeField]
    private CircleRawImage _mHeadIcon;

    public override void InitUi(int index, ChatFrinedModel t, Action<int, ChatFrinedModel> onCellClickCallback = null, Action<int, ChatFrinedModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        _mDisplayNameText.text = !string.IsNullOrEmpty(t.DisplayName) ? t.DisplayName : t.UserName;
       
        _mBriefText.text = t.Brief;
        if (t.HeadIconTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.HeadIconUrl))
            {
                HttpManager.RequestImage(t.HeadIconUrl + LoadPicStyle.ThumbnailHead, (texture2D) =>
                {
                    if (texture2D)
                    {
                        _mHeadIcon.texture = texture2D;
                        t.HeadIconTexture2D = texture2D;
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
            _mHeadIcon.texture = t.HeadIconTexture2D;
        }
    }
}
