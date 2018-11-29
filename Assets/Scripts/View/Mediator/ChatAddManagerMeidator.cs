using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatAddManagerMeidator : EventMediator {

    [Inject]
    public ChatAddManagerView ChatAddManagerView { get; set; }

    private GroupAndManagers _mGroupAndManagers;
    private HashSet<string> _mSelectedKeys;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.OpenAddManagerView,DataCallBack);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.AddManager, OnSelectMemberCallback);
        ChatAddManagerView.BackBut.onClick.AddListener(BackListener);
        ChatAddManagerView.AddManagerBut.onClick.AddListener(AddManagerListener);
    }

    private void AddManagerListener()
    {
        if(_mSelectedKeys == null || _mSelectedKeys.Count<1)return;
        if (_mGroupAndManagers.ManagersList.Count + _mSelectedKeys.Count > 5)
        {
            UIUtil.Instance.ShowTextToast("最多设置五位管理员");
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
            {
                Option = EGroupOption.AddTeamManager,
                GroupId = _mGroupAndManagers.ArgLoadGroupInfo.Tid,
                AddMembersId = _mSelectedKeys
            });
            iocViewManager.DestroyAndOpenNew(ChatAddManagerView.GetUiId(), (int)UiId.ChatGroupSetManager);
        }
    }

    private void BackListener()
    {
        if (_mSelectedKeys != null)
            _mSelectedKeys.Clear();
        iocViewManager.DestroyAndOpenNew(ChatAddManagerView.GetUiId(),(int)UiId.ChatGroupSetManager); 
    }

    private void DataCallBack(IEvent eEvent)
    {
        _mGroupAndManagers = eEvent.data as GroupAndManagers;
        if(_mGroupAndManagers==null)return;
        Log.I("普通成员，数量： "+ _mGroupAndManagers.NormalMemberList.Count);
        ChatAddManagerView.ChatAddGroupManagerFiler.DataSource =
            _mGroupAndManagers.NormalMemberList;

        ChatAddManagerView.ChatAddGroupManagerFiler.Refresh();

        if (_mSelectedKeys != null)
            _mSelectedKeys.Clear();
    }


    private void OnSelectMemberCallback(Notification notification)
    {

        var param = (ArgSelectedMember)notification.Content;
        var s = param.IsSelected ? "Selected" : "diSelected";

        if (_mSelectedKeys == null)
            _mSelectedKeys = new HashSet<string>();
        if (param.IsSelected)
            _mSelectedKeys.Add(param.MemberUid);
        else
            _mSelectedKeys.Remove(param.MemberUid);

    }


    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenAddManagerView, DataCallBack);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.AddManager, OnSelectMemberCallback);
        ChatAddManagerView.BackBut.onClick.RemoveListener(BackListener);
        ChatAddManagerView.AddManagerBut.onClick.RemoveListener(AddManagerListener);
    }

    private void OnDestroy()
    {
        OnRemove();
    }



}
