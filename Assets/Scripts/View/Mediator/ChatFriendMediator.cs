using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Session;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatFriendMediator : EventMediator
{
    [Inject]
    public ChatFriendView ChatFriendView { get; set; }

    [Inject]
    public IImService ImService {get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    public override void OnRegister()
    {
        ChatFriendView.BackButton.onClick.AddListener(OnBackBtnClick);
        ChatFriendView.SearchButton.onClick.AddListener(SearchClick);
        ChatFriendView.Filler.OnCellClick += OnCellClick;
        LoadFrineds();
    }

    private void SearchClick()
    {
        UserModel.SearchUserType = SearchUserType.Friendlist;
        iocViewManager.DestroyAndOpenNew(ChatFriendView.GetUiId(),(int)UiId.ChatSerach);
    }

    public override void OnRemove()
    {
        ChatFriendView.BackButton.onClick.RemoveListener(OnBackBtnClick);
        ChatFriendView.SearchButton.onClick.RemoveListener(SearchClick);
        ChatFriendView.Filler.OnCellClick -= OnCellClick;
    }

    private void OnDestroy()
    {
        if (ChatFriendView.Filler.DataSource!=null)
        {
            ChatFriendView.Filler.DataSource.Clear();
            ChatFriendView.Filler.DataSource = null;
        }
        OnRemove();
    }

    #region 私有方法
    private void LoadFrineds()
    {
        var dict = UserModel.Friends;
        if (dict != null && dict.Count > 0)
        {
            var list = new List<ChatFrinedModel>();
            foreach (var followInfo in dict)
            {
                ChatFrinedModel model = new ChatFrinedModel()
                {
                    Uid = followInfo.Value.Owner.Id,
                    DisplayName = followInfo.Value.Owner.DisplayName,
                    UserName = followInfo.Value.Owner.UserName,
                    HeadIconUrl = followInfo.Value.Owner.AvatarUrl,
                    Brief = followInfo.Value.Owner.Signature
                };
                list.Add(model);
            }
            ChatFriendView.Filler.DataSource = list;
            ChatFriendView.Filler.Refresh();
        }
    }

    private void OnCellClick(int index, ChatFrinedModel model)
    {
        UserModel.UserSelectedChatModel = NIMSessionType.kNIMSessionTypeP2P;
        UserModel.FromChatMainType = FromChatMainType.ChatSession;
        UserModel.ArgLoadChatRecord =   new ArgLoadChatRecord()
        {
            SessionId = model.Uid.ToString(),
            SessionType = NIMSessionType.kNIMSessionTypeP2P,
            DisplayName = model.DisplayName,
            TimeTag = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
            HeadIconTexture2D = model.HeadIconTexture2D,
            Count = 0
        };
        iocViewManager.DestroyAndOpenNew(ChatFriendView.GetUiId(),(int)UiId.ChatMain);
    }

    private void OnBackBtnClick()
    {
        if (ChatFriendView.Filler.DataSource != null)
        {
            ChatFriendView.Filler.DataSource.Clear();
        }
        iocViewManager.DestroyAndOpenNew(ChatFriendView.GetUiId(),(int)UiId.ChatSession);
    }    
    #endregion

}
