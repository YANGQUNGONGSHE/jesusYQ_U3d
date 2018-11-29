using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupTransferMembersMediator : EventMediator
{

    [Inject]
    public ChatGroupTransferMembersView ChatGroupTransferMembersView { get; set; }
    private GroupMembersInfo _groupMembersInfo;


    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupTransferMemersFinish,GroupDtaCallBack);
        dispatcher.AddListener(CmdEvent.ViewEvent.TransferGroupFinish, TransferCallBack);
        ChatGroupTransferMembersView.ChatGroupTransferfiler.OnCellClick = OnCellClick;
        ChatGroupTransferMembersView.BackBut.onClick.AddListener(BackClick);
    }

    private void BackClick()
    {
       iocViewManager.DestroyAndOpenNew(ChatGroupTransferMembersView.GetUiId(),(int)UiId.ChatGroupSetting);
    }

    private void TransferCallBack()
    {
      //  iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSession);
    }

    private void OnCellClick(int index, GroupMeberInfoModel groupMeberInfoModel)
    {
        dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
        {
            Option = EGroupOption.TransferTeam,
            GroupId = _groupMembersInfo.ArgLoadGroupInfo.Tid,
            NewOwnId = groupMeberInfoModel.Uid.ToString(),
            IsLeave = false
        });
        iocViewManager.DestroyAndOpenNew(ChatGroupTransferMembersView.GetUiId(),(int)UiId.ChatSession);
    }

    private void GroupDtaCallBack(IEvent eEvent)
    {
        _groupMembersInfo = eEvent.data as GroupMembersInfo;
        if(_groupMembersInfo==null) return;

        ChatGroupTransferMembersView.ChatGroupTransferfiler.DataSource.Clear();

        foreach (var t in _groupMembersInfo.ManagersList )
        {
            ChatGroupTransferMembersView.ChatGroupTransferfiler.DataSource.Add(t);
        }

        foreach (var t in _groupMembersInfo.NormalMemberList)
        {
            ChatGroupTransferMembersView.ChatGroupTransferfiler.DataSource.Add(t);
        }

        ChatGroupTransferMembersView.ChatGroupTransferfiler.Refresh();
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupTransferMemersFinish, GroupDtaCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.TransferGroupFinish, TransferCallBack);
        ChatGroupTransferMembersView.ChatGroupTransferfiler.OnCellClick -= OnCellClick;
        ChatGroupTransferMembersView.BackBut.onClick.RemoveListener(BackClick);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
