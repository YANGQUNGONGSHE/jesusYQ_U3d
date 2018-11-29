using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupAnnouncementMeidator : EventMediator {

    [Inject] 
    public ChatGroupAnnouncemnetView ChatGroupAnnouncemnetView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    private ArgLoadGroupInfo _mArgLoadGroupInfo;

    public override void OnRegister()
    {

       dispatcher.AddListener(CmdEvent.ViewEvent.OpenAnnouncementView,DataCallBack);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpdateAnnouncementFinish,UpdateAnnouncementListenr);
       ChatGroupAnnouncemnetView.BackBut.onClick.AddListener(BackClick);
       ChatGroupAnnouncemnetView.UpdateBut.onClick.AddListener(UpdateAnnClick);
    }

    private void UpdateAnnouncementListenr(IEvent eEvent)
    {
        var announcement = (string) eEvent.data;
        if(string.IsNullOrEmpty(announcement))return;

        Dispatcher.InvokeAsync(UpdateAnnouncement, announcement);
    }

    private void UpdateAnnouncement(string ann)
    {
        ChatGroupAnnouncemnetView.AnnouncemenText.text = ann;
    }

    private void DataCallBack(IEvent eEvent)
    {
        if (ChatGroupAnnouncemnetView.UpdatGameobject.activeSelf)
        {
            ChatGroupAnnouncemnetView.UpdatGameobject.SetActive(false);
        }

        _mArgLoadGroupInfo = eEvent.data as ArgLoadGroupInfo;
        SetUi(_mArgLoadGroupInfo);
        if (_mArgLoadGroupInfo == null) return;
        foreach (var t in _mArgLoadGroupInfo.GroupMeberInfoModels)
        {
            if (UserModel.User.Id != t.Uid) continue;
            if (t.UserType == NIMTeamUserType.kNIMTeamUserTypeCreator ||
                t.UserType == NIMTeamUserType.kNIMTeamUserTypeManager)
            {
                ChatGroupAnnouncemnetView.UpdatGameobject.SetActive(true);
            }
        }
    }

    private void SetUi(ArgLoadGroupInfo info)
    {
        ChatGroupAnnouncemnetView.AnnouncemenText.text = info.Announcement;
    }


    private void UpdateAnnClick()
    {
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGrouAnnEditor);
        StartCoroutine(SendInfo());

    }

    IEnumerator SendInfo()
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(CmdEvent.ViewEvent.OpenAnnouncementEditorView, _mArgLoadGroupInfo.Tid);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(ChatGroupAnnouncemnetView.GetUiId(),(int)UiId.ChatGroupSetting);
    }


    public override void OnRemove()
    {
        ChatGroupAnnouncemnetView.BackBut.onClick.RemoveListener(BackClick);
        ChatGroupAnnouncemnetView.UpdateBut.onClick.RemoveListener(UpdateAnnClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenAnnouncementView, DataCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.UpdateAnnouncementFinish, UpdateAnnouncementListenr);
    }

    private void OnDestroy()
    {
        OnRemove();
    }



}
