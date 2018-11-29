using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupMemberMediator : EventMediator {


    [Inject]
    public ChatGroupMemberView ChatGroupMemberView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private GroupMembersInfo _groupAndManagers;
    private HashSet<string> _mSelectedKeys;
    private bool _mIsShow = false;
   
    public override void OnRegister()
    {
        ChatGroupMemberView.BackBut.onClick.AddListener(BackClick);
        ChatGroupMemberView.ManageBut.onClick.AddListener(ManageClick);
        ChatGroupMemberView.TakeOutBut.onClick.AddListener(TakeOutClick);
        ChatGroupMemberView.CancelBut.onClick.AddListener(CancelClick);
        ChatGroupMemberView.SearchBut.onClick.AddListener(SearchClick);
        ChatGroupMemberView.ChatGroupMemberFiler.OnCellClick = OnCellClick;
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupMembersFinish,GroupDataCallBack);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.GroupMember, OnSelectMemberCallback);
    }

    private void OnCellClick(int i, GroupMeberInfoModel groupMeberInfoModel)
    {
        UserModel.PostModel = new PostModel()
        {
            FromType = FromViewType.FromGroupMemberView,
            HeadTexture2D = groupMeberInfoModel.HeadIconTexture2D,
            Author = new User()
            {
                Id = groupMeberInfoModel.Uid,
                UserName = groupMeberInfoModel.UserName,
                DisplayName = groupMeberInfoModel.Displayname,
                AvatarUrl = groupMeberInfoModel.HeadIconUrl,
                Signature = groupMeberInfoModel.Signature
            }
        };
        iocViewManager.CloseCurrentOpenNew((int)UiId.Personal);
    }
    private void SearchClick()
    {
        UserModel.SearchUserType = SearchUserType.GroupMember;
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSerach);
    }

    private void CancelClick()
    {
        Reset();
    }

    private void TakeOutClick()
    {

       dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
       {
           Option = EGroupOption.Kick,
           GroupId = _groupAndManagers.ArgLoadGroupInfo.Tid,
           KickMembersId = _mSelectedKeys
       });

        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupSetting);
        Reset();
    }

    private void GroupDataCallBack(IEvent eEvent)
    {
        if (ChatGroupMemberView.ManageBut.gameObject.activeSelf==false)
        {
            ChatGroupMemberView.ManageBut.gameObject.SetActive(true);
        }
        ChatGroupMemberView.ChatGroupMemberFiler.DataSource.Clear();
        ChatGroupMemberView.ChatGroupMemberFiler.IsManage(false);
       _groupAndManagers = eEvent.data as GroupMembersInfo;

        if (_groupAndManagers == null)return;
        Log.I("群成员信息界面群人数： "+_groupAndManagers.ArgLoadGroupInfo.GroupMeberInfoModels.Count);

        ChatGroupMemberView.MemberCout.text = "公社成员" + " " + "(" + _groupAndManagers.ArgLoadGroupInfo.GroupMeberInfoModels.Count + ")";
        if (_groupAndManagers.CreaterInfo != null)
        {
            ChatGroupMemberView.ChatGroupMemberFiler.DataSource.Add(_groupAndManagers.CreaterInfo);
        }
        foreach (var t in _groupAndManagers.ManagersList)
        {
            ChatGroupMemberView.ChatGroupMemberFiler.DataSource.Add(t);
            if (UserModel.User.Id == t.Uid)
            {
                ChatGroupMemberView.ChatGroupMemberFiler.IsManage(true);
            }
        }
        foreach (var t in _groupAndManagers.NormalMemberList)
        {
            ChatGroupMemberView.ChatGroupMemberFiler.DataSource.Add(t);
            if (UserModel.User.Id == t.Uid)
             ChatGroupMemberView.ManageBut.gameObject.SetActive(false);            
        }
        ChatGroupMemberView.ChatGroupMemberFiler.Refresh();
            
    }

    private void ManageClick()
    {
        if (_mIsShow)
        {
            if(_mSelectedKeys==null||_mSelectedKeys.Count<1)return;
            ChatGroupMemberView.BottomBarRectTransform.gameObject.SetActive(true);
        }
        else
        {
            _mIsShow = true;
            ChatGroupMemberView.ManageText.text = "踢出";
            ChatGroupMemberView.ChatGroupMemberFiler.IsShow = _mIsShow;
            ChatGroupMemberView.ChatGroupMemberFiler.Refresh();
        }
       
    }

    private void BackClick()
    {
        if (_mIsShow == true)
        {
          Reset();
        }
        else
        {
            if (_mSelectedKeys != null)
                _mSelectedKeys.Clear();
            iocViewManager.DestroyAndOpenNew(ChatGroupMemberView.GetUiId(),(int)UiId.ChatGroupSetting);   
        }  
    }

    private void Reset()
    {
        _mIsShow = false;
        ChatGroupMemberView.BottomBarRectTransform.gameObject.SetActive(false);
        ChatGroupMemberView.ChatGroupMemberFiler.IsShow = _mIsShow;
        ChatGroupMemberView.ChatGroupMemberFiler.Refresh();
        ChatGroupMemberView.ManageText.text = "管理";
        if(_mSelectedKeys!=null)
        _mSelectedKeys.Clear();
    }

    private void OnSelectMemberCallback(Notification notification)
    {

        var param = (ArgSelectedMember)notification.Content;

        if (_mSelectedKeys == null)
            _mSelectedKeys = new HashSet<string>();
        if (param.IsSelected)
            _mSelectedKeys.Add(param.MemberUid);
        else
            _mSelectedKeys.Remove(param.MemberUid);

    }

    public override void OnRemove()
    {
        ChatGroupMemberView.BackBut.onClick.RemoveListener(BackClick);
        ChatGroupMemberView.ManageBut.onClick.RemoveListener(ManageClick);
        ChatGroupMemberView.TakeOutBut.onClick.RemoveListener(TakeOutClick);
        ChatGroupMemberView.CancelBut.onClick.RemoveListener(CancelClick);
        ChatGroupMemberView.SearchBut.onClick.RemoveListener(SearchClick);
        ChatGroupMemberView.ChatGroupMemberFiler.OnCellClick -= OnCellClick;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupMembersFinish, GroupDataCallBack);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.GroupMember, OnSelectMemberCallback);
    }

    private void OnDestroy()
    {
        OnRemove();
    }



}
