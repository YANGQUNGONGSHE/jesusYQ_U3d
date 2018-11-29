using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSearchMediator : EventMediator 
{
	[Inject]
	public ChatSearchView ChatSearchView{ get; set;}
    [Inject]
    public UserModel UserModel { get; set; }
	private int _mSearchType = 0;
    private bool isOwn = false;
    private List<NIMTeamInfo> _mMyAllGroups;
    private User _mUser;

	public override void OnRegister()
	{
		ChatSearchView.SearchP2P.onValueChanged.AddListener(OnSearchP2P);

		ChatSearchView.SearchGroup.onValueChanged.AddListener(OnSearchGroup);

		ChatSearchView.SerachInput.onValueChanged.AddListener(OnSearchInputValueChanged);

		ChatSearchView.BackButton.onClick.AddListener(OnBackBtnClick);

		dispatcher.AddListener(CmdEvent.ViewEvent.LoadSingleGroupFinish, OnLoadSingleGroupFinish);
	    dispatcher.AddListener(CmdEvent.ViewEvent.LoadMyAllGroupsFinish, OnLoadMyGroupsFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqQueryUserInfoFinish,ReqQueryUserInfoFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqQueryUserInfoFail,ReqQueryUserInfoFail);
        dispatcher.Dispatch(CmdEvent.Command.LoadMyAllGroups);
	    NotificationCenter.DefaultCenter().AddObserver(NotifiyName.AddGroup, OnSelecGrouprCallback);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.AddFocus,AddFocusCallBack);
	}

    private void OnSelecGrouprCallback(Notification notification)
    {
        var param = (ArgSelectedMember)notification.Content;
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.ApplyJoinTeam,
            GroupId = param.MemberUid,
            ApplyReson = "您好，希望加入多多交流！"
        });
        UIUtil.Instance.ShowTextToast("已申请");
    }

    private void AddFocusCallBack(Notification notification)
    {
        var param = (ArgSelectedMember) notification.Content;
        dispatcher.Dispatch(CmdEvent.Command.FocusOptions, new FocusOptionInfo()
        {
            Options = FocusOptions.AddFcous,
            Id = param.MemberUid
        });
        UIUtil.Instance.ShowTextToast("已关注");
    }

    public override void OnRemove()
	{
		ChatSearchView.SearchP2P.onValueChanged.RemoveListener(OnSearchGroup);

		ChatSearchView.SearchGroup.onValueChanged.RemoveListener(OnSearchP2P);

		ChatSearchView.SerachInput.onValueChanged.RemoveListener(OnSearchInputValueChanged);

		dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSingleGroupFinish, OnLoadSingleGroupFinish);
	    dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadMyAllGroupsFinish, OnLoadMyGroupsFinish);
	    NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.AddGroup, OnSelecGrouprCallback);
	    NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.AddFocus, AddFocusCallBack);

        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqQueryUserInfoFinish, ReqQueryUserInfoFinish);
	    dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqQueryUserInfoFail, ReqQueryUserInfoFail);
	  
    }

	private void OnDestroy()
    {
        OnRemove();
    }

    private void OnSearchInputValueChanged(string text)
    {
		Log.I("当前搜索类型：" + _mSearchType);
        SearchInfo(text);
    }

    private void OnSearchP2P(bool isOn)
    {
        if (isOn)
        {
            ChatSearchView.Filler.DataSource.Clear();
            ChatSearchView.Filler.Refresh();
            ChatSearchView.IsVisibleNoResultGo(true);
            ChatSearchView.IsVisibleScrollRect(false);
            _mSearchType = 0;
            ChatSearchView.ChangeToggleTextColor(_mSearchType);
            SearchInfo(ChatSearchView.SerachInput.text);
        }
    }
    private void OnSearchGroup(bool isOn)
    {
        if (isOn)
        {
            ChatSearchView.Filler.DataSource.Clear();
            ChatSearchView.Filler.Refresh();
            ChatSearchView.IsVisibleNoResultGo(true);
            ChatSearchView.IsVisibleScrollRect(false);
            _mSearchType = 1;
            ChatSearchView.ChangeToggleTextColor(_mSearchType);
            SearchInfo(ChatSearchView.SerachInput.text);
        }
    }
	private void OnLoadSingleGroupFinish(IEvent evt)
	{
	    var tmInfo = evt.data as NIMTeamInfo;
	    if (tmInfo != null)
	    {
	        isOwn = false;
	        foreach (var info in _mMyAllGroups)
	        {

	            if (info.TeamId == tmInfo.TeamId)
	            {
	                isOwn = true;
	            }
	        }

	        var model = new ChatSearchModel()
	        {
	            Type = _mSearchType,
	            DisplayName = tmInfo.Name,
	            HeadIconUrl = tmInfo.TeamIcon,
	            Brief = tmInfo.Introduce,
	            Ext = tmInfo.MembersCount.ToString(),
	            IsOwn = isOwn,
	            Id = tmInfo.TeamId
            };

	        ChatSearchView.Filler.DataSource.Clear();
	        ChatSearchView.Filler.DataSource.Add(model);
	        ChatSearchView.Filler.Refresh();
	        ChatSearchView.IsVisibleNoResultGo(false);
	        ChatSearchView.IsVisibleScrollRect(true);

        }
	}

    private void OnLoadMyGroupsFinish(IEvent eEvent)
    {
        _mMyAllGroups = eEvent.data as List<NIMTeamInfo>;
    }

    private void ReqQueryUserInfoFail(IEvent eEvent)
    {
        Log.I("搜索用户失败！！！");
        ChatSearchView.IsVisibleNoResultGo(true);
        ChatSearchView.IsVisibleScrollRect(false);
    }

    private void ReqQueryUserInfoFinish(IEvent eEvent)
    {
        _mUser = eEvent.data as User;
        if (_mUser != null)
        {
            isOwn = false;
            foreach (var userInfo in UserModel.Follows)
            {
                if (userInfo.Value.Owner.Id == _mUser.Id)
                {
                    isOwn = true;
                }
            }
            if (_mUser.Id == UserModel.User.Id)
            {
                isOwn = true;
            }

            var model = new ChatSearchModel()
            {
                Type = _mSearchType,
                UserName = _mUser.UserName,
                DisplayName = _mUser.DisplayName,
                HeadIconUrl = _mUser.AvatarUrl,
                Brief = _mUser.Signature,
                Ext = "20",
                IsOwn = isOwn,
                Id = _mUser.Id.ToString()
            };

            ChatSearchView.Filler.DataSource.Clear();
            ChatSearchView.Filler.DataSource.Add(model);
            ChatSearchView.Filler.Refresh();
            ChatSearchView.IsVisibleNoResultGo(false);
            ChatSearchView.IsVisibleScrollRect(true);
        }

    }

    private void OnBackBtnClick()
    {

        if (ChatSearchView.Filler.DataSource != null)
        {
            ChatSearchView.Filler.DataSource.Clear();
            ChatSearchView.Filler.DataSource = null;
        }
        switch (UserModel.SearchUserType)
        {
            case SearchUserType.ChatSession:
                iocViewManager.DestroyAndOpenNew(ChatSearchView.GetUiId(),(int)UiId.ChatSession);
                break;
            case SearchUserType.FansAndFollow:
                iocViewManager.DestroyAndOpenNew(ChatSearchView.GetUiId(),(int)UiId.FocusAndFans);
                break;
            case SearchUserType.GroupMember:
                iocViewManager.DestroyAndOpenNew(ChatSearchView.GetUiId(),(int)UiId.ChatGroupMember);
                break;
            case SearchUserType.GroupList:
                iocViewManager.DestroyAndOpenNew(ChatSearchView.GetUiId(),(int)UiId.ChatGroup);
                break;
            case SearchUserType.Friendlist:
                iocViewManager.DestroyAndOpenNew(ChatSearchView.GetUiId(), (int)UiId.ChatFriend);
                break;
        }
    }

    private void SearchInfo(string text)
    {
        if (_mSearchType == 0)
        {
            if (string.IsNullOrEmpty(text))
            {
                ChatSearchView.Filler.DataSource.Clear();
                ChatSearchView.Filler.Refresh();
                ChatSearchView.IsVisibleNoResultGo(true);
                ChatSearchView.IsVisibleScrollRect(false);
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.Command.ReqQueryUserInfo, text);
            }
        }
        else
        {
            if (string.IsNullOrEmpty(text))
            {
                ChatSearchView.Filler.DataSource.Clear();
                ChatSearchView.Filler.Refresh();
                ChatSearchView.IsVisibleNoResultGo(true);
                ChatSearchView.IsVisibleScrollRect(false);
            }
            else
            {
                Log.I("搜索群组" + text);
                dispatcher.Dispatch(CmdEvent.Command.LoadGroupOption, new LoadGroupedInfo()
                {
                    Type = LoadGroupType.LoadSingleGroup,
                    Id = text
                });
            }
        }
    }
}
