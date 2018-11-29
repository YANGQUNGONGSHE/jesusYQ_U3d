using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Team;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatTransfermemberCell : BaseCell<GroupMeberInfoModel> {

    private CircleRawImage _mHeadImage;
    private Text _mDisplayName;
    private Text _mBrief;
    private Text _mUserType;


    protected override void OnAwake()
    {
        base.OnAwake();
        _mHeadImage = transform.Find("HeadIcon").GetComponent<CircleRawImage>();
        _mBrief = transform.Find("Brief").GetComponent<Text>();
        _mDisplayName = transform.Find("Displayname/disPlayName").GetComponent<Text>();
        _mUserType = transform.Find("Displayname/userType").GetComponent<Text>();
    }

    public override void InitUi(int index, GroupMeberInfoModel t, Action<int, GroupMeberInfoModel> onCellClickCallback = null, Action<int, GroupMeberInfoModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

        _mDisplayName.text = t.Displayname;
        _mBrief.text = t.Signature;
     
        if (t.UserType == NIMTeamUserType.kNIMTeamUserTypeCreator)
        {
            _mUserType.text = "群主";
           
        }
        else if (t.UserType == NIMTeamUserType.kNIMTeamUserTypeManager)
        {
            _mUserType.text = "管理员";
        }
        else
        {
            _mUserType.text = "";
        }
        if (t.HeadIconTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.HeadIconUrl))
            {
                HttpManager.RequestImage(t.HeadIconUrl + LoadPicStyle.ThumbnailHead, texture2D =>
                {
                    if (texture2D)
                    {
                        _mHeadImage.texture = texture2D;
                        t.HeadIconTexture2D = texture2D;
                    }
                    else
                    {
                        _mHeadImage.texture = DefaultImage.Head;
                    }
                });
            }
            else
            {
                _mHeadImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mHeadImage.texture = t.HeadIconTexture2D;
        }
    }
}
