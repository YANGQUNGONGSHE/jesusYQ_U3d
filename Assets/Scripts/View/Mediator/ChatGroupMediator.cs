using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Session;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupMediator : EventMediator
{
	[Inject]
    public ChatGroupView ChatGroupView { get; set; }

    [Inject]
    public IImService ImService {get; set; }

    [Inject]
    public UserModel UserModel {get; set;}
    
    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadMyAllGroupsFinish, OnLoadGroupsFinsh);
        ChatGroupView.BackButton.onClick.AddListener(OnBackBtnClick);
        ChatGroupView.SearchButton.onClick.AddListener(SearchClick);
        ChatGroupView.CreateGroupButton.onClick.AddListener(CreateGroupClick);
        ChatGroupView.Filler.OnCellClick += OnCellClick;
        dispatcher.Dispatch(CmdEvent.Command.LoadMyAllGroups);
    }

    private void CreateGroupClick()
    {
        UserModel.EditorGroupType = EditorGroupType.CreateGroup;
        iocViewManager.DestroyAndOpenNew(ChatGroupView.GetUiId(), (int)UiId.ChatEditorGroupView);
    }

    private void SearchClick()
    {
        UserModel.SearchUserType = SearchUserType.GroupList;
        // iocViewManager.DestroyAndOpenNew(ChatGroupView.GetUiId(),(int)UiId.ChatSerach);
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSerach);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadMyAllGroupsFinish, OnLoadGroupsFinsh);
        ChatGroupView.CreateGroupButton.onClick.RemoveListener(CreateGroupClick);
        ChatGroupView.BackButton.onClick.RemoveListener(OnBackBtnClick);
        ChatGroupView.SearchButton.onClick.RemoveListener(SearchClick);
        ChatGroupView.Filler.OnCellClick -= OnCellClick;
    }

    private void OnDestroy()
    {
        if (ChatGroupView.Filler.DataSource != null)
        {
            ChatGroupView.Filler.DataSource.Clear();
            ChatGroupView.Filler.DataSource = null;
        }
        OnRemove();
    }

    private void OnLoadGroupsFinsh(IEvent evt)
    {
        List<NIMTeamInfo> groups = evt.data as List<NIMTeamInfo>;
        if(groups != null && groups.Count > 0)
        {
            var list = new List<ChatGroupModel>();
            for(var i = 0; i<groups.Count; i++)
            {
                ChatGroupModel model = new ChatGroupModel()
                {
                    Tid = groups[i].TeamId,
                    GroupName = groups[i].Name,
                    GroupBrief = groups[i].Introduce,
                    GroupHeadIconUrl = groups[i].TeamIcon
                };
                list.Add(model);
            }
            ChatGroupView.Filler.DataSource = list;
            Dispatcher.InvokeAsync(ChatGroupView.Filler.Refresh);
        }
    }

	private void OnCellClick(int index, ChatGroupModel model)
    {
        UserModel.UserSelectedChatModel = NIMSessionType.kNIMSessionTypeTeam;
        UserModel.FromChatMainType = FromChatMainType.ChatSession;
        UserModel.ArgLoadChatRecord = new ArgLoadChatRecord()
        {
            SessionId = model.Tid,
            SessionType = NIMSessionType.kNIMSessionTypeTeam,
            DisplayName = model.GroupName,
            TimeTag = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
            HeadIconTexture2D = model.GroupHeadIconTexture2D,
            Count = 0
        };
        iocViewManager.DestroyAndOpenNew(ChatGroupView.GetUiId(),(int)UiId.ChatMain);
    }

    private void OnBackBtnClick()
    {
        if (ChatGroupView.Filler.DataSource != null)
        {
            ChatGroupView.Filler.DataSource.Clear();
        }
        iocViewManager.DestroyAndOpenNew(ChatGroupView.GetUiId(),(int)UiId.ChatSession);
    }
}
