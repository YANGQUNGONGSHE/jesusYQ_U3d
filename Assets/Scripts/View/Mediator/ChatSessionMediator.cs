using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;
using NIM.Message;
using NIM;
using NIM.Session;
using NIM.SysMessage;

public class ChatSessionMediator : EventMediator
{
    [Inject]
    public ChatSessionView ChatSessionView { get; set; }

    [Inject]
    public IImService ImService{ get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    private List<ChatSessionCellModel> _mP2pSessionCache;
    private ChatSessionCellModel _sessionCellModel;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSessionFinish, OnLoadSessionFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReceiveImMsg, OnReceiveImMsg);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReceiveSysImMsg, OnReceiveSystemImMsg);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReceiveSysCustomLikeImMsg,ReceiveSysCustomLikeImMsg);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReceiveSysCustomCommentImMsg,ReceiveSysCustomCommentImMsg);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqDeleteSessionFinish,ReqDeleteSessionFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqDeleteSessionFail,ReqDeleteSessionFail);
        //dispatcher.AddListener(CmdEvent.ViewEvent.ReqCleanRecordFinish, CleanRecordFinish);
        //dispatcher.AddListener(CmdEvent.ViewEvent.ReqCleanRecordFail, CleanRecordFail);

        ChatSessionView.Filler.OnCellClick += OnCellClick;
        ChatSessionView.Filler.OnCellLongPress += OnCellLongPress;
        ChatSessionView.CreateP2PChat.onClick.AddListener(OnCreateP2PChatBtnClick);
        ChatSessionView.CreateGroupChat.onClick.AddListener(OnCreateGroupChatBtnClick);
        ChatSessionView.CreateGroup.onClick.AddListener(OnCreateGroupBtnClick);
        ChatSessionView.SerachButton.onClick.AddListener(OnSearchBtnClick);
        ChatSessionView.DeleteSessionBut.onClick.AddListener(DeleteSessionClick);
        ChatSessionView.CancelDeSessionBut.onClick.AddListener(CancelDeSessionClick);

        #region 正式代码

        //JudgeLoginState();

        #endregion

        #region 调试代码

        dispatcher.Dispatch(CmdEvent.Command.LoadSession);

        #endregion
    }

    private void JudgeLoginState()
    {
        Log.I("IM登录状态：：："+ ClientAPI.GetLoginState());
        dispatcher.Dispatch(ClientAPI.GetLoginState() == NIMLoginState.kNIMLoginStateLogin
            ? CmdEvent.Command.LoadSession
            : CmdEvent.Command.LoginIm);
    }
    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSessionFinish, OnLoadSessionFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReceiveImMsg, OnReceiveImMsg);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReceiveSysImMsg, OnReceiveSystemImMsg);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReceiveSysCustomLikeImMsg, ReceiveSysCustomLikeImMsg);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReceiveSysCustomCommentImMsg, ReceiveSysCustomCommentImMsg);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqDeleteSessionFinish, ReqDeleteSessionFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqDeleteSessionFail, ReqDeleteSessionFail);
        //dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqCleanRecordFinish, CleanRecordFinish);
        //dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqCleanRecordFail, CleanRecordFail);

        ChatSessionView.Filler.OnCellClick -= OnCellClick;
        ChatSessionView.Filler.OnCellLongPress -= OnCellLongPress;
        ChatSessionView.CreateP2PChat.onClick.RemoveListener(OnCreateP2PChatBtnClick);
        ChatSessionView.CreateGroup.onClick.RemoveListener(OnCreateGroupBtnClick);
        ChatSessionView.SerachButton.onClick.RemoveListener(OnSearchBtnClick);
        ChatSessionView.DeleteSessionBut.onClick.RemoveListener(DeleteSessionClick);
        ChatSessionView.CancelDeSessionBut.onClick.RemoveListener(CancelDeSessionClick);
    }

    private void OnDestroy()
    {
        if (ChatSessionView.Filler.DataSource!=null)
        {
            ChatSessionView.Filler.DataSource.Clear();
            ChatSessionView.Filler.DataSource = null;
        }
        OnRemove();
    }

    #region 私有方法
    /// <summary>
    /// 会话记录加载完成
    /// </summary>
    /// <param name="evt"></param>
    private void OnLoadSessionFinish(IEvent evt)
    {
        if (_mP2pSessionCache != null)
        {
            _mP2pSessionCache.Clear();
        }
        ChatSessionView.Filler.DataSource.Clear();
        _mP2pSessionCache = evt.data as List<ChatSessionCellModel>;
        if (_mP2pSessionCache == null || _mP2pSessionCache.Count < 1)
        {
           Dispatcher.InvokeAsync(ChatSessionView.IsVisibleScrollRect,false);
        }
        else
        {
            Dispatcher.InvokeAsync(ChatSessionView.IsVisibleScrollRect, true);
        }
        ChatSessionView.Filler.DataSource = _mP2pSessionCache;
        Dispatcher.InvokeAsync(ChatSessionView.Filler.Refresh);
    }
    /// <summary>
    /// 接收到新消息后该页面的操作
    /// </summary>
    /// <param name="evt"></param>
    private void OnReceiveImMsg(IEvent evt)
    {
        var msg = evt.data as NIMIMMessage;
        if(msg==null)return;
        dispatcher.Dispatch(CmdEvent.Command.LoadSession); //更新
    }
    /// <summary>
    /// 接受到系统新消息该页面的操作
    /// </summary>
    /// <param name="evt"></param>
    private void OnReceiveSystemImMsg(IEvent evt)
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadSession);
    }
    /// <summary>
    /// 接受系统评论新消息该页面的操作
    /// </summary>
    private void ReceiveSysCustomCommentImMsg()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadSession);
    }
    /// <summary>
    /// 接受系统点赞新消息该页面的操作
    /// </summary>
    private void ReceiveSysCustomLikeImMsg()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadSession);
    }
    /// <summary>
    /// cell的点击事件
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="model">数据模型</param>
    private void OnCellClick(int index, ChatSessionCellModel model)
    {
        _sessionCellModel = model;
        Log.I("点击事件的类型:::"+ _sessionCellModel.ChatSessionType);
        if (_sessionCellModel.ChatSessionType == ChatSessionType.SystemMsg)
        { 
            iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSysTemMs);
            dispatcher.Dispatch(CmdEvent.Command.LoadSysMs, _sessionCellModel.SortTime);
        }
        else if(_sessionCellModel.ChatSessionType == ChatSessionType.Like)
        {
            iocViewManager.DestroyAndOpenNew(ChatSessionView.GetUiId(), (int)UiId.SysCustomLike);
        }
        else if(_sessionCellModel.ChatSessionType == ChatSessionType.Comment)
        {
            iocViewManager.DestroyAndOpenNew(ChatSessionView.GetUiId(), (int)UiId.SysCustomComment);
        }
        else
        {
            UserModel.UserSelectedChatModel = model.SessionInfo.SessionType;
            UserModel.FromChatMainType = FromChatMainType.ChatSession;
            UserModel.ArgLoadChatRecord = new ArgLoadChatRecord()
            {
                SessionId = _sessionCellModel.SessionInfo.Id,
                SessionType = _sessionCellModel.SessionInfo.SessionType,
                DisplayName = _sessionCellModel.DisplayName,
                UserName = _sessionCellModel.UserName,
                Signature = _sessionCellModel.Signature,
                HeadUrl = _sessionCellModel.HeadIconUrl,
                TimeTag = _sessionCellModel.SortTime,
                HeadIconTexture2D = _sessionCellModel.HeadIconTexture2D,
                Count = 0
            };
            iocViewManager.DestroyAndOpenNew(ChatSessionView.GetUiId(),(int)UiId.ChatMain);
        } 
    }
    /// <summary>
    /// cell的长按事件
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="chatSessionCellModel">数据模型</param>
    private void OnCellLongPress(int index, ChatSessionCellModel chatSessionCellModel)
    {
       // Log.I("会话记录cell长按事件："+index+"  "+chatSessionCellModel.SessionInfo.SessionType+" "+chatSessionCellModel.SessionInfo.Id);
        _sessionCellModel = chatSessionCellModel;

        if (_sessionCellModel.ChatSessionType == ChatSessionType.SystemMsg|| _sessionCellModel.ChatSessionType == ChatSessionType.Comment || _sessionCellModel.ChatSessionType == ChatSessionType.Like)return;
            ChatSessionView.IsVisibleDeleteSessionBg(true);
        
    }
    /// <summary>
    /// 取消删除会话点击事件
    /// </summary>
    private void CancelDeSessionClick()
    {
        ChatSessionView.IsVisibleDeleteSessionBg(false);
    }
    /// <summary>
    /// 删除会话点击事件
    /// </summary>
    private void DeleteSessionClick()
    {
       Log.I("删除会话点击事件");
        dispatcher.Dispatch(CmdEvent.Command.ReqDeleteSession, new DeletedInfo()
        {
            DeleteType = DeleteType.DeleteSession,
            SessionType = _sessionCellModel.SessionInfo.SessionType,
            Id = _sessionCellModel.SessionInfo.Id
        });
        ChatSessionView.IsVisibleDeleteSessionBg(false);
    }
    /// <summary>
    /// 删除会话记录失败回调
    /// </summary>
    private void ReqDeleteSessionFail()
    {
        UIUtil.Instance.ShowFailToast("删除会话失败");
    }
    /// <summary>
    /// 删除会话记录成功回调
    /// </summary>
    private void ReqDeleteSessionFinish(IEvent evt)
    {
        var info = evt.data as SessionInfo;
        if (info != null)
           Dispatcher.InvokeAsync(dispatcher.Dispatch,(object)CmdEvent.Command.LoadSession);
        
    }
    /// <summary>
    /// 点击->创建组群
    /// </summary>
    private void OnCreateGroupBtnClick()
    {
        //UserModel.EditorGroupType = EditorGroupType.CreateGroup;
        //ChatSessionView.IsVisibleAction(false);
        //iocViewManager.DestroyAndOpenNew(ChatSessionView.GetUiId(),(int)UiId.ChatEditorGroupView);

        UserModel.SearchUserType = SearchUserType.ChatSession;
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSerach, new Vector2(Screen.width, 0));
        ChatSessionView.IsVisibleAction(false);
    }
    /// <summary>
    /// 点击->发起聊天
    /// </summary>
    private void OnCreateP2PChatBtnClick()
    {
        UserModel.UserSelectedChatModel = NIMSessionType.kNIMSessionTypeP2P;
        ChatSessionView.IsVisibleAction(false);
        iocViewManager.DestroyAndOpenNew(ChatSessionView.GetUiId(),(int)UiId.ChatFriend);
    }
    /// <summary>
    /// 点击->发起群聊
    /// </summary>
    private void OnCreateGroupChatBtnClick()
    {
        UserModel.UserSelectedChatModel = NIMSessionType.kNIMSessionTypeTeam;
        iocViewManager.DestroyAndOpenNew(ChatSessionView.GetUiId(),(int)UiId.ChatGroup);
        ChatSessionView.IsVisibleAction(false);
    }
    private void OnSearchBtnClick()
    {
        UserModel.SearchUserType = SearchUserType.ChatSession;
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatSerach, new Vector2(Screen.width, 0));
        ChatSessionView.IsVisibleAction(false);
    }
    /// <summary>
    /// 清空聊天记录失败回调
    /// </summary>
    private void CleanRecordFail()
    {
        UIUtil.Instance.ShowFailToast("清空聊天记录失败");
    }
    /// <summary>
    /// 清空聊天记录成功回调
    /// </summary>
    /// <param name="eEvent"></param>
    private void CleanRecordFinish(IEvent eEvent)
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadChatRecord, new ArgLoadChatRecord()
        {
            SessionId = _sessionCellModel.SessionInfo.Id,
            SessionType = _sessionCellModel.SessionInfo.SessionType,
            Signature = _sessionCellModel.Signature,
            HeadUrl = _sessionCellModel.HeadIconUrl,
            DisplayName = _sessionCellModel.DisplayName,
            TimeTag = _sessionCellModel.SortTime,
            HeadIconTexture2D = _sessionCellModel.HeadIconTexture2D,
            Count = 0
        });
    }
    #endregion
}
