using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using System;
using NIM.Session;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;
using WongJJ.Game.Core;

public class ChatGroupSettingMediator : EventMediator 
{
	[Inject]
	public ChatGroupSettingView ChatGroupSettingView { get; set;}
    [Inject]
    public UserModel UserModel { get; set; }

    private ArgLoadGroupInfo _mGroupInfo;
    private GroupMeberInfoModel _meberInfoModel;


	public override void OnRegister()
	{
		dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupInfoByIdFinish, OnLoadGroupInfoFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpdateAnnouncementFinish,UpdateAnnouncementListenr);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpdateTeamInfoFinish,UpdateGroupData);
		ChatGroupSettingView.BackButton.onClick.AddListener(OnBackBtnClick);
        ChatGroupSettingView.AnnounceMentBut.onClick.AddListener(AnnouncementClick);
        ChatGroupSettingView.MgrBut.onClick.AddListener(MgrListenr);
        ChatGroupSettingView.MoreMemberBut.onClick.AddListener(MoreMemberClick);
        ChatGroupSettingView.QuitGroupButton.onClick.AddListener(QuitGroupClick);
        ChatGroupSettingView.LeaveBut.onClick.AddListener(LeavelClick);
	    ChatGroupSettingView.Cancelbut.onClick.AddListener(CancelClick);
	    ChatGroupSettingView.TransferBut.onClick.AddListener(TransferClick);
	    ChatGroupSettingView.DismissBut.onClick.AddListener(DismissClick);
	    ChatGroupSettingView.CreateCancelBut.onClick.AddListener(CreateCancelClick);
        ChatGroupSettingView.CleanRecordBut.onClick.AddListener(CleanRecordClick);
	    NotificationCenter.DefaultCenter().AddObserver(NotifiyName.AddMembers, AddMembersClick);
    }

    private void CleanRecordClick()
    {
       dispatcher.Dispatch(CmdEvent.Command.ReqDeleteSession,new DeletedInfo()
       {
           DeleteType = DeleteType.DeleteRecord,
           SessionType = NIMSessionType.kNIMSessionTypeTeam,
           Id = _mGroupInfo.Tid
       });
    }

    private void AddMembersClick(Notification notification)
    {
        Log.I("添加成员点击事件");
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupSelectMemberAdd);
        StartCoroutine(SendInfo(CmdEvent.Command.LoadGroupSelectMm));
    }
    private void CreateCancelClick()
    {
        ChatGroupSettingView.CreaterTool.gameObject.SetActive(false);
        ChatGroupSettingView.ToolBarRectTransform.gameObject.SetActive(false);
    }

    private void DismissClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
        {
            Option = EGroupOption.Dismiss,
            GroupId = _mGroupInfo.Tid
        });

        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSession);
        ChatGroupSettingView.CreaterTool.gameObject.SetActive(false);
    }

    private void TransferClick()
    {
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupTransMembers);
        _mGroupInfo.IsUpdateGroup = false;
        StartCoroutine(SendInfo(CmdEvent.Command.LoadGroupTransferMemers));
    }

    private void LeavelClick()
    {
        if(_meberInfoModel==null)return;
        if (_meberInfoModel.UserType == NIMTeamUserType.kNIMTeamUserTypeCreator)
        {
            ChatGroupSettingView.CreaterTool.gameObject.SetActive(true);
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
            {   Option = EGroupOption.Level,
                GroupId = _mGroupInfo.Tid
            });

            iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSession);
            ChatGroupSettingView.ToolBarRectTransform.gameObject.SetActive(false);
        }
    }

    private void CancelClick()
    {
        ChatGroupSettingView.ToolBarRectTransform.gameObject.SetActive(false);
    }

    private void QuitGroupClick()
    {
        ChatGroupSettingView.ToolBarRectTransform.gameObject.SetActive(true);
    }

    private void MoreMemberClick()
    {
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupMember);
        _mGroupInfo.IsUpdateGroup = false;
        StartCoroutine(SendInfo(CmdEvent.Command.LoadGroupMembers));
    }

    private void MgrListenr()
    {
        foreach (var t in _mGroupInfo.GroupMeberInfoModels)
        {

            if (UserModel.User.Id != t.Uid) continue;
            if (t.UserType == NIMTeamUserType.kNIMTeamUserTypeCreator ||
                t.UserType == NIMTeamUserType.kNIMTeamUserTypeManager)
            {
                Log.I("身份 ：" + t.UserType+ t.Displayname);
                iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupManage);
                _mGroupInfo.IsUpdateGroup = false;
                StartCoroutine(SendInfo(CmdEvent.Command.LoadGroupManagers));
            }
            else
            {
               Log.I("身份 ："+ t.UserType+t.Displayname);
                UIUtil.Instance.ShowTextToast("没有管理权限");
            }
        }
    }

    private void UpdateGroupData(IEvent eEvent)
    {
        var teamInfo = eEvent.data as NIMTeamInfo;
        if (teamInfo != null)
            Dispatcher.InvokeAsync(UpdateGroupName, teamInfo.Name);
            
    }

    private void UpdateGroupName(string groupName)
    {
        ChatGroupSettingView.GroupName.text = groupName;
    }

    private void UpdateAnnouncementListenr(IEvent eEvent)
    {
        var announcement = (string)eEvent.data;
        if(string.IsNullOrEmpty(announcement))return;
        Dispatcher.InvokeAsync(UpdateAnnouncement, announcement);
    }

    private void UpdateAnnouncement(string ann)
    {
        ChatGroupSettingView.Announcement.text = ann;
    }

    private void AnnouncementClick()
    {
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupAnnouncement);
        _mGroupInfo.IsUpdateGroup = false;
        StartCoroutine(SendInfo(CmdEvent.ViewEvent.OpenAnnouncementView));
    }

    IEnumerator SendInfo(Enum eEnum)
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(eEnum, _mGroupInfo);
    }

    public override void OnRemove()
	{
		dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupInfoByIdFinish,OnLoadGroupInfoFinish);
	    dispatcher.RemoveListener(CmdEvent.ViewEvent.UpdateAnnouncementFinish, UpdateAnnouncementListenr);
	    dispatcher.RemoveListener(CmdEvent.ViewEvent.UpdateTeamInfoFinish, UpdateGroupData);
        ChatGroupSettingView.BackButton.onClick.RemoveListener(OnBackBtnClick);
	    ChatGroupSettingView.AnnounceMentBut.onClick.RemoveListener(AnnouncementClick);
	    ChatGroupSettingView.MgrBut.onClick.RemoveListener(MgrListenr);
	    ChatGroupSettingView.MoreMemberBut.onClick.RemoveListener(MoreMemberClick);
	    ChatGroupSettingView.QuitGroupButton.onClick.RemoveListener(QuitGroupClick);
	    ChatGroupSettingView.LeaveBut.onClick.RemoveListener(LeavelClick);
	    ChatGroupSettingView.Cancelbut.onClick.RemoveListener(CancelClick);
	    ChatGroupSettingView.TransferBut.onClick.RemoveListener(TransferClick);
	    ChatGroupSettingView.DismissBut.onClick.RemoveListener(DismissClick);
	    ChatGroupSettingView.CreateCancelBut.onClick.RemoveListener(CreateCancelClick);
	    ChatGroupSettingView.CleanRecordBut.onClick.RemoveListener(CleanRecordClick);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.AddMembers, AddMembersClick);
    }

    private void OnDestroy()
    {
        OnRemove();
    }

    private void OnLoadGroupInfoFinish(IEvent evt)
    {
        _mGroupInfo = evt.data as ArgLoadGroupInfo;
		Dispatcher.InvokeAsync(SetUi, _mGroupInfo);
        if (_mGroupInfo.IsUpdateGroup == true)
        {
            dispatcher.Dispatch(CmdEvent.Command.LoadGroupManagers, _mGroupInfo);
        }
        for (var i = 0; i < _mGroupInfo.GroupMeberInfoModels.Count; i++)
        {
            if (UserModel.User.Id == _mGroupInfo.GroupMeberInfoModels[i].Uid)
                _meberInfoModel = _mGroupInfo.GroupMeberInfoModels[i];
        }
    }

    private void OnBackBtnClick()
    {
        iocViewManager.DestroyAndOpenNew(ChatGroupSettingView.GetUiId(),(int)UiId.ChatMain);
    }

	private void SetUi(ArgLoadGroupInfo info)
	{
        Log.I("设置界面群成员人数 ："+info.GroupMeberInfoModels.Count);
	    ChatGroupSettingView.GroupName.text = info.Name;
        ChatGroupSettingView.GroupId.text = info.Tid;
		ChatGroupSettingView.Announcement.text = info.Announcement;
	    ChatGroupSettingView.SetMember(info.GroupMeberInfoModels);

	}
}


