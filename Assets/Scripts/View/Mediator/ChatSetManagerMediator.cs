using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSetManagerMediator : EventMediator {


    [Inject]
    public ChatSetManagerView ChatSetManagerView { get; set; }

    private GroupAndManagers _groupAndManagers;

    private HashSet<string> _mSelectedKeys;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.OpenSetGroupManagerView,DataCallBack);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.SelectedManager, OnSelectMemberCallback);
        ChatSetManagerView.TakeOutBut.onClick.AddListener(TakeOutManagerListenr);
        ChatSetManagerView.AddManagerBut.onClick.AddListener(AddManagerListener);
        ChatSetManagerView.BackBut.onClick.AddListener(BackClick);
    }

    private void BackClick()
    {
        ChatSetManagerView.ManagerContent.SetActive(true);
        ChatSetManagerView.Tip.SetActive(true);
        if(_mSelectedKeys!=null)
        _mSelectedKeys.Clear();
        iocViewManager.DestroyAndOpenNew(ChatSetManagerView.GetUiId(),(int)UiId.ChatGroupManage);
    }

    private void AddManagerListener()
    {
        if (_groupAndManagers.ManagersList.Count < 5)
        {
            iocViewManager.CloseCurrentOpenNew((int)UiId.ChatAddManager);
            StartCoroutine(SendInfo());
        }
        else
        {
            UIUtil.Instance.ShowTextToast("最多设置五位管理员");
        }
        
    }

    IEnumerator SendInfo()
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(CmdEvent.ViewEvent.OpenAddManagerView, _groupAndManagers);
    }

    private void TakeOutManagerListenr()
    {

        if(_mSelectedKeys==null || _mSelectedKeys.Count<1)return;
        Log.I("要踢出管理员的数量："+ _mSelectedKeys.Count);
        dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
        {
            Option = EGroupOption.RemoveTeamManager,
            GroupId = _groupAndManagers.ArgLoadGroupInfo.Tid,
            KickMembersId = _mSelectedKeys
        });
    }

    private void DataCallBack(IEvent eEvent)
    {
        
        _groupAndManagers = eEvent.data as GroupAndManagers;
        if(_groupAndManagers==null) return;
        Log.I("是否有管理员   "+_groupAndManagers.ManagersList.Count);
        ChatSetManagerView.SetManagerCount(_groupAndManagers.ManagersList.Count);
        RefreshList();
    }

    private void RefreshList()
    {
        if (_groupAndManagers.ManagersList.Count < 1)
        {
            ChatSetManagerView.ManagerContent.SetActive(false);
            ChatSetManagerView.Tip.SetActive(true);
        }
        else
        {
            ChatSetManagerView.Tip.SetActive(false);
            ChatSetManagerView.ManagerContent.SetActive(true);
            ChatSetManagerView.ChatSetGroupManagerFiler.DataSource = _groupAndManagers.ManagersList;
            ChatSetManagerView.ChatSetGroupManagerFiler.Refresh();
        }
       
    }

    private void OnSelectMemberCallback(Notification notification)
    {

        var param = (ArgSelectedMember)notification.Content ;
        var s = param.IsSelected ? "Selected" : "diSelected";
        Log.I(s);
        if (_mSelectedKeys == null)
            _mSelectedKeys = new HashSet<string>();
        if (param.IsSelected)
            _mSelectedKeys.Add(param.MemberUid);
        else
            _mSelectedKeys.Remove(param.MemberUid);

    }
    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenSetGroupManagerView, DataCallBack);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.SelectedManager, OnSelectMemberCallback);
        ChatSetManagerView.TakeOutBut.onClick.RemoveListener(TakeOutManagerListenr);
        ChatSetManagerView.AddManagerBut.onClick.RemoveListener(AddManagerListener);
        ChatSetManagerView.BackBut.onClick.RemoveListener(BackClick);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
