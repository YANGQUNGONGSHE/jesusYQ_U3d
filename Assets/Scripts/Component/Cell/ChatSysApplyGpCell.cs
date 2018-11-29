using System;
using System.Collections;
using System.Collections.Generic;
using NIM.SysMessage;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSysApplyGpCell : BaseCell<SysTemModel>
{


    private Text _mTime;
    private Text _mSendName;
    private Text _mMessage;
    private Text _mGroupName;
    private CircleRawImage _mHeadImage;
    private Button _mApplyBut;
    private Transform _mPassedTransform;
    private Transform _mTimeBodyTransform;
    private Transform _mNopassTransform;
    private bool _mAgreeSelected = true;
    protected override void OnAwake()
    {
        base.OnAwake();
        _mTime = transform.Find("TimeContainer/Text").GetComponent<Text>();
        _mSendName = transform.Find("MsContainer/Bg/DisPlayName").GetComponent<Text>();
        _mMessage = transform.Find("MsContainer/Bg/ApplyReson").GetComponent<Text>();
        _mGroupName = transform.Find("MsContainer/Bg/Message").GetComponent<Text>();
        _mHeadImage = transform.Find("MsContainer/Bg/HeadImage").GetComponent<CircleRawImage>();
        _mApplyBut = transform.Find("MsContainer/Bg/ApplyDeal").GetComponent<Button>();
        _mPassedTransform = transform.Find("MsContainer/Bg/PassApplyDeal").GetComponent<Transform>();
        _mTimeBodyTransform = transform.Find("TimeContainer").GetComponent<Transform>();
        _mNopassTransform = transform.Find("MsContainer/Bg/NoPassApplyDeal").GetComponent<Transform>();
        _mApplyBut.onClick.AddListener(AgreeClick);
    }

    public override void InitUi(int index, SysTemModel t, Action<int, SysTemModel> onCellClickCallback = null, Action<int, SysTemModel> onCellLongPressCallback = null,
        bool isShowTime = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isShowTime);
        _mSendName.text = !string.IsNullOrEmpty(t.SenderName) ? t.SenderName : t.SenderUserName;
        _mTime.text = t.ShowTime;
        _mTimeBodyTransform.gameObject.SetActive(isShowTime);

        if (t.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.SendAvatarUrl))
            {
                HttpManager.RequestImage(t.SendAvatarUrl+LoadPicStyle.ThumbnailHead, d =>
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

                } );
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
       
        if (t.MsgType == NIMSysMsgType.kNIMSysMsgTypeTeamApply)
        {

            _mMessage.text = "申请理由："+t.Message;
            _mGroupName.text = "申请加入群组";

            if (t.Status == NIMSysMsgStatus.kNIMSysMsgStatusDecline)
            {
                _mPassedTransform.gameObject.SetActive(false);
                _mApplyBut.gameObject.SetActive(false);
                _mNopassTransform.gameObject.SetActive(true);
            }
            else if(t.Status == NIMSysMsgStatus.kNIMSysMsgStatusPass)
            {
                _mPassedTransform.gameObject.SetActive(true);
                _mApplyBut.gameObject.SetActive(false);
                _mNopassTransform.gameObject.SetActive(false);
            }
            else
            {
                _mPassedTransform.gameObject.SetActive(false);
                _mApplyBut.gameObject.SetActive(true);
                _mNopassTransform.gameObject.SetActive(false);
            }
            
        }

        if (t.MsgType == NIMSysMsgType.kNIMSysMsgTypeTeamReject)
        {
            _mMessage.text = "拒绝理由："+t.Message;
            _mGroupName.text = "拒绝您申请加入群组";
            _mPassedTransform.gameObject.SetActive(false);
            _mApplyBut.gameObject.SetActive(false);
            _mNopassTransform.gameObject.SetActive(true);
        }

    }

    private void AgreeClick()
    {
        _mAgreeSelected = !_mAgreeSelected;

        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.ApplyJoinTeamDeal, this, new ArgSelectedTeamMember()
        {
            SysTemModel = t,
            IsSelected = _mAgreeSelected,
            Tid = t.ReceiverId,
            Uid = t.SenderId
        });
    }
}
