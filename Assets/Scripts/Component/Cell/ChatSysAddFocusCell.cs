using System;
using System.Collections;
using System.Collections.Generic;
using NIM.SysMessage;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSysAddFocusCell : BaseCell<SysTemModel>
{
    private Text _mTime;
    private Text _mSendName;
    private Text _mMessage;
    private CircleRawImage _mHeadImage;
    private Button _mFocusBut;
    private Transform _mPassedTransform;
    private Transform _mTimeBodyTransform;
    private bool _mAgreeSelected = true;

    protected override void OnAwake()
    {
        base.OnAwake();
        _mTime = transform.Find("TimeContainer/Text").GetComponent<Text>();
        _mSendName = transform.Find("MsContainer/Bg/DisPlayName").GetComponent<Text>();
        _mMessage = transform.Find("MsContainer/Bg/Message").GetComponent<Text>();
        _mHeadImage = transform.Find("MsContainer/Bg/HeadImage").GetComponent<CircleRawImage>();
        _mFocusBut = transform.Find("MsContainer/Bg/AddFocus").GetComponent<Button>();
        _mPassedTransform = transform.Find("MsContainer/Bg/HasFocus").GetComponent<Transform>();
        _mTimeBodyTransform = transform.Find("TimeContainer").GetComponent<Transform>();

        _mFocusBut.onClick.AddListener(AddFocusClick);

    }

   

    public override void InitUi(int index, SysTemModel t, Action<int, SysTemModel> onCellClickCallback = null, Action<int, SysTemModel> onCellLongPressCallback = null,
        bool isShowTime = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isShowTime);
        _mSendName.text = !string.IsNullOrEmpty(t.SenderName) ? t.SenderName : t.SenderUserName;
        _mTime.text = t.ShowTime;
        _mTimeBodyTransform.gameObject.SetActive(isShowTime);
        _mHeadImage.texture = t.HeadTexture2D;
        _mMessage.text = t.MsgType == NIMSysMsgType.kNIMSysMsgTypeFriendAdd ? "关注了你" : "取消关注了你";
        if (t.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.SendAvatarUrl))
            {
                HttpManager.RequestImage(t.SendAvatarUrl + LoadPicStyle.ThumbnailHead, d =>
                {
                    if (d)
                    {
                        t.HeadTexture2D = d;
                        _mHeadImage.texture = d;
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
            _mHeadImage.texture = t.HeadTexture2D;
        }
            if (t.Status == NIMSysMsgStatus.kNIMSysMsgStatusPass)
            {
                _mPassedTransform.gameObject.SetActive(true);
                _mFocusBut.gameObject.SetActive(false);
            }
            else
            {
                _mPassedTransform.gameObject.SetActive(false);
                _mFocusBut.gameObject.SetActive(true);
            }
    }

    private void AddFocusClick()
    {
        _mAgreeSelected = !_mAgreeSelected;

        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.SysAddFocus, this, new ArgSelectedMember()
        {
            IsSelected = _mAgreeSelected,
            MemberUid = t.SenderId
        });
        _mPassedTransform.gameObject.SetActive(true);
        _mFocusBut.gameObject.SetActive(false);
    }
}
