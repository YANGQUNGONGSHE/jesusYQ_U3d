using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSetGroupManagerCell : BaseCell<GroupMeberInfoModel>
{

    private CircleRawImage _mHeadImage;
    private Text _mDisplayName;
    private Text _mBrief;
    private Button _mSelectBut;
    private RectTransform _mIconSelected;
    private bool _mIsSelected = false;

    protected override void OnAwake()
    {
        _mHeadImage = transform.Find("HeadIcon").GetComponent<CircleRawImage>();
        _mBrief = transform.Find("Brief").GetComponent<Text>();
        _mDisplayName = transform.Find("Displayname").GetComponent<Text>();
        _mSelectBut = transform.Find("SelectBtn").GetComponent<Button>();
        _mIconSelected = transform.Find("SelectBtn/icon_selected").GetComponent<RectTransform>();

        _mSelectBut.onClick.AddListener(OnSelectBtnClick);
    }

    


    public override void InitUi(int index, GroupMeberInfoModel t, Action<int, GroupMeberInfoModel> onCellClickCallback = null, Action<int, GroupMeberInfoModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        _mDisplayName.text = t.Displayname;
        _mBrief.text = t.Signature;
        _mIconSelected.gameObject.SetActive(false);
        _mIsSelected = false;
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

    private void OnSelectBtnClick()
    {
        _mIsSelected = !_mIsSelected;
        _mIconSelected.gameObject.SetActive(_mIsSelected);
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.SelectedManager, this, new ArgSelectedMember()
        {
            IsSelected = _mIsSelected,
            MemberUid = t.Uid.ToString()
        });

    }
}
