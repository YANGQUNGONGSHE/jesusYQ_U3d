using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class GroupReadRecordCell : BaseCell<GroupMemberReadRecordModel>
{
    public CircleRawImage HeadImage;
    public Text DisplayNameText;

    public override void InitUi(int index, GroupMemberReadRecordModel t, Action<int, GroupMemberReadRecordModel> onCellClickCallback = null,
        Action<int, GroupMemberReadRecordModel> onCellLongPressCallback = null, bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        DisplayNameText.text = t.DisplayName;
        if (t.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.AvatarUrl))
            {
                HttpManager.RequestImage(t.AvatarUrl+LoadPicStyle.ThumbnailHead, texture2D =>
                {
                    if (texture2D)
                    {
                        HeadImage.texture = texture2D;
                        t.HeadTexture2D = texture2D;
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
