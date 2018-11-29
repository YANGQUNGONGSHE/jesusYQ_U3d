using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupAnnEditorMediator : EventMediator {

    [Inject]
    public ChatGroupAnnEditorView ChatGroupAnnEditorView { get; set; }

    private string _mGroupId;
    private NIMTeamInfo _mInfo;
    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.OpenAnnouncementEditorView,Callback);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSingleTeamFinish, LoadSingleTeamInfo);
        ChatGroupAnnEditorView.BackBut.onClick.AddListener(BackClick);
        ChatGroupAnnEditorView.PushBut.onClick.AddListener(PublishClick);
    }

    private void LoadSingleTeamInfo(IEvent eEvent)
    {
        _mInfo = eEvent.data as NIMTeamInfo;
    }


    private void Callback(IEvent eEvent)
    {
        _mGroupId = (string)eEvent.data;
        if(_mGroupId==null)return;
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.QuerySingleTeam,
            GroupId = _mGroupId
        });
    }

    private void PublishClick()
    {
        if (string.IsNullOrEmpty(ChatGroupAnnEditorView.InputField.text)) return;
        _mInfo.Announcement = ChatGroupAnnEditorView.InputField.text;
        StartCoroutine(SendInfo());
    }

    IEnumerator SendInfo()
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.UpdateAnnounce,
            GroupId = _mGroupId,
            TeamInfo = _mInfo
        });
        iocViewManager.DestroyAndOpenNew(ChatGroupAnnEditorView.GetUiId(),(int)UiId.ChatGroupAnnouncement);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(ChatGroupAnnEditorView.GetUiId(), (int)UiId.ChatGroupAnnouncement);
    }


    public override void OnRemove()
    {
        ChatGroupAnnEditorView.BackBut.onClick.RemoveListener(BackClick);
        ChatGroupAnnEditorView.PushBut.onClick.RemoveListener(PublishClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSingleTeamFinish, LoadSingleTeamInfo);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenAnnouncementEditorView, Callback);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
