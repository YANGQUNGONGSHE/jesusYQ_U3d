using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class FansAndFocusCell : BaseCell<FansAndFocusModel>
{

    private CircleRawImage mHeadImage;
    private Text mDisplayName;
    private Text mSignature;
    //private Transform mAddFocusTransform;
    //private Transform mHasFriendTransform;
    //private Transform mHasFocusTransform;
    private Button mAddFocusBut;
    private bool _mIsSelected = false;
    private Image _mTipsImage;
    /// <summary>
    /// 0:关注 1：friend 2:已关注
    /// </summary>
    public Sprite[] _MTipSprites;

    protected override void OnAwake()
    {
        base.OnAwake();
        mHeadImage = transform.Find("HeadIcon").GetComponent<CircleRawImage>();
        mDisplayName = transform.Find("Displayname").GetComponent<Text>();
        mSignature = transform.Find("Brief").GetComponent<Text>();
        //mAddFocusTransform = transform.Find("AddFocusBut").GetComponent<Transform>();
        //mHasFriendTransform = transform.Find("Friended").GetComponent<Transform>();
        //mHasFocusTransform = transform.Find("HasFocused").GetComponent<Transform>();
        mAddFocusBut = transform.Find("AddFocusBut").GetComponent<Button>();
        _mTipsImage = transform.Find("AddFocusBut/Image").GetComponent<Image>();
        mAddFocusBut.onClick.AddListener(AddFocusClick);
    }

    public override void InitUi(int index, FansAndFocusModel t, Action<int, FansAndFocusModel> onCellClickCallback = null, Action<int, FansAndFocusModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        SetUi();
    }
    /// <summary>
    /// 设置Ui
    /// </summary>
    private void SetUi()
    {
        mDisplayName.text = !string.IsNullOrEmpty(t.DisPlayName) ? t.DisPlayName : t.UserName;
        mSignature.text = t.Signature;
        if (t.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.HeadUrl))
            {
                HttpManager.RequestImage(t.HeadUrl + LoadPicStyle.ThumbnailHead, textrue2D =>
                {
                    if (textrue2D)
                    {
                        mHeadImage.texture = textrue2D;
                        t.HeadTexture2D = textrue2D;
                    }
                    else
                    {
                        t.HeadTexture2D = DefaultImage.Head;
                    }
                });
            }
            else
            {
                mHeadImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            mHeadImage.texture = t.HeadTexture2D;
        }

        //Log.I("状态：：："+t.IsBidirectional+"  "+t.IsFansOrFocus+t.DisPlayName);
        if (t.IsFansOrFocus)
        {
            if (t.IsBidirectional)
            {
                _mTipsImage.sprite = _MTipSprites[1];
                mAddFocusBut.interactable = false;
            }
            else
            {
                _mTipsImage.sprite = _MTipSprites[0];
                mAddFocusBut.interactable = true;
            }
        }
        else
        {
            if (t.IsBidirectional)
            {
                _mTipsImage.sprite = _MTipSprites[1];
                mAddFocusBut.interactable = false;
            }
            else
            {
                _mTipsImage.sprite = _MTipSprites[2];
                mAddFocusBut.interactable = false;
            }
        }
    }

    /// <summary>
    /// 关注点击事件
    /// </summary>
    private void AddFocusClick()
    {
        _mIsSelected = !_mIsSelected;

        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.FansAddFocus, this, new ArgSelectedMember()
        {
            IsSelected = _mIsSelected,
            MemberUid = t.Id.ToString()
        });
        _mTipsImage.sprite = _MTipSprites[1];
        mAddFocusBut.interactable = false;
    }
}
