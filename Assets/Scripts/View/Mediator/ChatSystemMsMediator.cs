using System;
using System.Collections;
using System.Collections.Generic;
using NIM.SysMessage;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSystemMsMediator : EventMediator {

    [Inject]
    public ChatSystemMsView  ChatSystemMsView{get;set; }

    private List<SysTemModel> _sysTemModels;

    private ArgSelectedTeamMember _selectedTeamMember;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSystemMsFinish,OnLoadSysTeamMsFinish);
        ChatSystemMsView.BackBut.onClick.AddListener(BackClick);
        ChatSystemMsView.AgreeApplyBut.onClick.AddListener(AgreeApplyClick);
        ChatSystemMsView.RejectApplyBut.onClick.AddListener(RejectApplyClick);
        ChatSystemMsView.OptionBut.onClick.AddListener(OptionClick);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.ApplyJoinTeamDeal, ApplyJoinTeamDealCallBack);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.SysAddFocus,SysAddFocusCallBack);
    }

    private void OptionClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.SetMessageStatus, new MessagesTypeInfo()
        {
            MarkType = MarkType.DeleteAllSystemMessages
        });
        iocViewManager.DestroyAndOpenNew(ChatSystemMsView.GetUiId(),(int)UiId.ChatSession);
    }

    private void SysAddFocusCallBack(Notification notification)
    {
        var param = (ArgSelectedMember)notification.Content;

        dispatcher.Dispatch(CmdEvent.Command.FocusOptions,new FocusOptionInfo()
       {
           Options = FocusOptions.AddFcous,
           Id = param.MemberUid,
           IsSysMsg = true,
           SysMsgStatus = NIMSysMsgStatus.kNIMSysMsgStatusPass
       });
    }

    private void ApplyJoinTeamDealCallBack(Notification notification)
    {
        _selectedTeamMember = (ArgSelectedTeamMember)notification.Content;

        ChatSystemMsView.IsvibileApplyDealWindow(true);

        ChatSystemMsView.SetApplyDealUi(_selectedTeamMember.SysTemModel.HeadTexture2D, _selectedTeamMember.SysTemModel.SenderName, _selectedTeamMember.SysTemModel.SenderSigure);

    }

    private void RejectApplyClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.RejectJoinTeam,
            GroupId = _selectedTeamMember.Tid,
            Uid = _selectedTeamMember.Uid,
            ApplyReson = "不好意思啦！",
            MsgId = _selectedTeamMember.SysTemModel.Id,
            SysMsgStatus = NIMSysMsgStatus.kNIMSysMsgStatusDecline
        });
        ChatSystemMsView.IsvibileApplyDealWindow(false);
        iocViewManager.DestroyAndOpenNew(ChatSystemMsView.GetUiId(),(int)UiId.ChatSession);
    }

    private void AgreeApplyClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.AgreeJoinTeam,
            GroupId = _selectedTeamMember.Tid,
            Uid = _selectedTeamMember.Uid,
            MsgId = _selectedTeamMember.SysTemModel.Id,
            SysMsgStatus = NIMSysMsgStatus.kNIMSysMsgStatusPass
        });
        ChatSystemMsView.IsvibileApplyDealWindow(false);
        iocViewManager.DestroyAndOpenNew(ChatSystemMsView.GetUiId(),(int)UiId.ChatSession);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(ChatSystemMsView.GetUiId(),(int)UiId.ChatSession);
    }

    private void OnLoadSysTeamMsFinish(IEvent eEvent)
    {
        _sysTemModels = eEvent.data as List<SysTemModel>;

        if (_sysTemModels != null)
        {
            Log.I("*********系统消息查询结果******* 条数：" + _sysTemModels.Count);
            ChatSystemMsView.ChatSystemFiler.DataSource = _sysTemModels;
            //Dispatcher.InvokeAsync(ChatSystemMsView.ChatSystemFiler.Refresh); 
            ChatSystemMsView.ChatSystemFiler.Refresh();
        }
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSystemMsFinish, OnLoadSysTeamMsFinish);
        ChatSystemMsView.BackBut.onClick.RemoveListener(BackClick);
        ChatSystemMsView.AgreeApplyBut.onClick.RemoveListener(AgreeApplyClick);
        ChatSystemMsView.RejectApplyBut.onClick.RemoveListener(RejectApplyClick);
        ChatSystemMsView.OptionBut.onClick.AddListener(OptionClick);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.ApplyJoinTeamDeal, ApplyJoinTeamDealCallBack);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.SysAddFocus, SysAddFocusCallBack);
    }

    private void OnDestroy()
    {
        OnRemove();
    }



}
