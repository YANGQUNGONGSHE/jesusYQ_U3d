using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupSelectMemberMediator : EventMediator
{
	[Inject]
	public ChatGroupSelectMemberAddView ChatGroupSelectMemberAddView {get; set;}
    [Inject]
    public UserModel UserModel { get; set; }
    private List<GroupSelectMemberModel> _list;

    private HashSet<string> _mSelectedKeys;

    public override void OnRegister()
    {
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.SelectedMember, OnSelectMemberCallback);
        ChatGroupSelectMemberAddView.AddButton.onClick.AddListener(OnAddBtnClick);
        ChatGroupSelectMemberAddView.BackButton.onClick.AddListener(BackClick);
        dispatcher.AddListener(CmdEvent.ViewEvent.AddMemberSucc, OnAddMemberSucc);
        dispatcher.AddListener(CmdEvent.ViewEvent.AddMemberFail, OnAddMemberFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupSelectMmFinish,OnLoadGroupSelectMmFinish);  
    }

    private void OnLoadGroupSelectMmFinish(IEvent eEvent)
    {
        _list = eEvent.data as List<GroupSelectMemberModel>;
        if(_list==null)return;

        ChatGroupSelectMemberAddView.Filler.DataSource = _list;
        Dispatcher.InvokeAsync(ChatGroupSelectMemberAddView.Filler.Refresh);
    }

    private void BackClick()
    {
        if (_mSelectedKeys != null)
            _mSelectedKeys.Clear();
        iocViewManager.DestroyAndOpenNew(ChatGroupSelectMemberAddView.GetUiId(),(int)UiId.ChatGroupSetting);
    }

    public override void OnRemove()
    {
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.SelectedMember, OnSelectMemberCallback);
        ChatGroupSelectMemberAddView.AddButton.onClick.RemoveListener(OnAddBtnClick);
        ChatGroupSelectMemberAddView.BackButton.onClick.AddListener(BackClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.AddMemberSucc, OnAddMemberSucc);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.AddMemberFail, OnAddMemberFail);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupSelectMmFinish, OnLoadGroupSelectMmFinish);
    }

    private void OnDestroy()
    {
        OnRemove();
    }
    
    private void OnSelectMemberCallback(Notification notification)
    {
        var param = (ArgSelectedMember)notification.Content;
        var s = param.IsSelected ? "Selected" : "diSelected";
        Log.I("user id = " + param.MemberUid + "," + s);
        if (_mSelectedKeys == null)
            _mSelectedKeys = new HashSet<string>();
        if (param.IsSelected)
            _mSelectedKeys.Add(param.MemberUid);
        else
            _mSelectedKeys.Remove(param.MemberUid);
    }

    private void OnAddBtnClick()
    {
        if (_mSelectedKeys == null || _mSelectedKeys.Count <= 0) return;
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.AddMember,
            GroupId = UserModel.UserSelectedGroupId,
            AddMembersId = _mSelectedKeys
        });
        if (_mSelectedKeys != null)
            _mSelectedKeys.Clear();
        iocViewManager.DestroyAndOpenNew(ChatGroupSelectMemberAddView.GetUiId(),(int)UiId.ChatGroupSetting);
    }

    private void OnAddMemberSucc(IEvent evt)
    {
        Log.I("******************^^^^^***>>>>>>>>>>>> add member succ !");
    }

    private void OnAddMemberFail(IEvent evt)
    {
        Log.I("******************^^^^^***>>>>>>>>>>>> add member fail !");
    }
}
