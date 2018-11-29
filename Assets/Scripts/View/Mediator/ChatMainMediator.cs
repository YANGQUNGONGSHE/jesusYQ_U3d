using System;
using System.Collections.Generic;
using System.IO;
using CameraShot;
using ImageAndVideoPicker;
using NIM;
using NIM.Session;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatMainMediator : EventMediator
{
    [Inject]
    public ChatMainView ChatMainView { get; set; }
    [Inject]
    public IImService ImService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    [Inject]
    public IFriendService FriendService { get; set; }

    /// <summary>
    /// 当前查询聊天记录的请求参数
    /// </summary>
    private ArgLoadChatRecord _mArg;

    /// <summary>
    /// Cell模型内存数据（缓存）
    /// </summary>
    private List<ChatBaseModel> _mCellLists;

    /// <summary>
    /// 消息等待返回结果队列
    /// </summary>
    private Dictionary<string, ChatBaseModel> _mSendMsgUnCheckedCache;

    private int _mChatRecordCount = 0;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadChatRecordFinish, OnLoadChatRecordFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSingleChatRecordFinish, OnLoadSingleChatRecordFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReceiveImMsg, OnReceiveImMsg);
        dispatcher.AddListener(CmdEvent.ViewEvent.SendImMsgSucc, OnSendMsgSucc);
        dispatcher.AddListener(CmdEvent.ViewEvent.SendImMsgFail, OnSendMsgFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.AutoDownloadImResSucc, OnAutoDownloadResSucc);
        dispatcher.AddListener(CmdEvent.ViewEvent.AddGroupManagerFinish, AddGroupManagerFinishListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.RemoveManagerFinish, RemoveGroupManagerListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpdateTeamInfoFinish, UpdateGroupDataListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.KickMemberSucc, KickGroupMemberListenr);
        dispatcher.AddListener(CmdEvent.ViewEvent.AddMemberSucc, AddMemberListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupMemberReadRecordFinish,LoadGroupMemberReadCallback);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqCleanRecordFinish, CleanRecordFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqSetBlackSucc,ReqSetBlackFinish);


        ChatMainView.TextInput.onValueChanged.AddListener(OnTextValuedChanged);
        ChatMainView.BackButton.onClick.AddListener(OnBackBtnClick);
        ChatMainView.OptionButton.onClick.AddListener(OnOptionBtnClick);
        ChatMainView.SendTextButton.onClick.AddListener(OnSendTextBtnClick);
        ChatMainView.GetPhotoButton.onClick.AddListener(OnGetPhotoBtnClick);
        ChatMainView.TakePhotoButton.onClick.AddListener(OnTakePhotoBtnClick);
        ChatMainView.KeyboardButton.onClick.AddListener(OnKeyboardBtnClick);
        ChatMainView.VoiceButton.onClick.AddListener(OnVoiceBtnClick);
        ChatMainView.HitHiddenBtn.onClick.AddListener(OnHiddenBtnClick);
        ChatMainView.CancelBut.onClick.AddListener(CancelClick);
        ChatMainView.BgCancelBut.onClick.AddListener(BgCancelClick);
        ChatMainView.CleanRecordBut.onClick.AddListener(CleanRecordClick);
        ChatMainView.ReportBut.onClick.AddListener(ReportClick);
        ChatMainView.BlockBut.onClick.AddListener(BlockClick);
        ChatMainView.ReadInfoCloseBut.onClick.AddListener(ReadInfoCloseClick);
        ChatMainView.ReadInfoShowBut.onClick.AddListener(ReadInfoShowClick);
        ChatMainView.GroupReadRecordFiler.OnCellClick = OnCellClick;
        ChatMainView.Filler.MScrollRect.onValueChanged.AddListener(MScrollRectListener);
        ChatMainView.Filler.MScrollRect.movementType = ScrollRect.MovementType.Clamped;

        ChatMainView.PressAudioBtn.OnPressAudio += OnPressAudito;
        ChatMainView.PressAudioBtn.OnEndPressAudio += OnEndPressAudio;
        ChatMainView.PressAudioBtn.OnCancelAudio += OnCancelPressAudio;
        ChatMainView.PressAudioBtn.OnTimeNotEnough += OnTimeNotEnough;

        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.OnStopCaptureCb, OnStopAudioCaptureCallback);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.PlayAuido, OnPlayAudioCallback);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.StopPlayAudio, OnStopPlayAudioCallback);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.ChatMainHeadClick,ChatMainHeadCallBack);
        
        PickerEventListener.onImageSelect += OnPickImageSelect;
        PickerEventListener.onImageLoad += OnPickImageLoad;
        PickerEventListener.onError += OnPickImagePickerError;
        PickerEventListener.onCancel += OnPickImagePickerCancel;

        CameraShotEventListener.onImageSaved += OnCameraImageSaved;
        CameraShotEventListener.onImageLoad += OnCameraImageLoad;
        CameraShotEventListener.onVideoSaved += OnCameraVideoSaved;
        CameraShotEventListener.onError += OnCameraError;
        CameraShotEventListener.onCancel += OnCameraCancel;
        LoadGroupReadRecord();
    }

    private void ChatMainHeadCallBack(Notification notification)
    {
        var param =(ChatBaseModel) notification.Content;
        dispatcher.Dispatch(CmdEvent.Command.ChatMainTurnPersonal,param.SenderId);
    }
    private void OnTextValuedChanged(string arg0)
    {
        if (arg0.Length > 2000)
        {
            UIUtil.Instance.ShowFailToast("最多输入2000个字符");
        }
    }
    private void OnCellClick(int index, GroupMemberReadRecordModel groupMemberReadRecordModel)
    {
        if(groupMemberReadRecordModel==null)return;
        UserModel.PostModel = new PostModel()
        {
            FromType = FromViewType.FromReadRecordView,
            HeadTexture2D = groupMemberReadRecordModel.HeadTexture2D,
            Author = new User()
            {
                Id = groupMemberReadRecordModel.Uid,
                DisplayName = groupMemberReadRecordModel.DisplayName,
                Signature = groupMemberReadRecordModel.Signature,
                AvatarUrl = groupMemberReadRecordModel.AvatarUrl
            }
        };
        iocViewManager.DestroyAndOpenNew(ChatMainView.GetUiId(),(int)UiId.Personal);
    }
    private void LoadGroupReadRecord()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadChatRecord, UserModel.ArgLoadChatRecord);
        if (UserModel.UserSelectedChatModel == NIMSessionType.kNIMSessionTypeTeam)
        {
            dispatcher.Dispatch(CmdEvent.Command.LoadGroupMemberReadRecord, UserModel.ArgLoadChatRecord.SessionId);
            ChatMainView.IsVisibleReadRecordShowBut(true);
            ChatMainView.IsVisibleReadRecordAction(true);
        }
        else
        {
            ChatMainView.BlockTextStatus(IsBlock());
        }

    }
    private void MScrollRectListener(Vector2 arg0)
    {
        if (ChatMainView.Filler.MScrollRect.normalizedPosition.y >= 1.0f && Input.GetMouseButtonUp(0))
        {
            Log.I("OriginalTime::"+ _mCellLists[0].OriginalTime);
            dispatcher.Dispatch(CmdEvent.Command.LoadChatRecord, new ArgLoadChatRecord()
            {
                SessionId = _mArg.SessionId,
                SessionType = _mArg.SessionType,
                DisplayName = _mArg.DisplayName,
                TimeTag = _mCellLists[_mCellLists.Count-1].OriginalTime,
                HeadIconTexture2D = _mArg.HeadIconTexture2D,
                Count = _mCellLists.Count,
                IsLoadMore = true
            });
        }
    }

    #region P2p Expand Event

    private void BgCancelClick()
    {
        ChatMainView.IsVisibleP2PExpandBg(false);
    }

    private void CancelClick()
    {
        ChatMainView.IsVisibleP2PExpandBg(false);
    }

    private void CleanRecordClick()
    {
       dispatcher.Dispatch(CmdEvent.Command.ReqDeleteSession, new DeletedInfo()
       {
           DeleteType = DeleteType.DeleteRecord,
           SessionType = NIMSessionType.kNIMSessionTypeP2P,
           Id = _mArg.SessionId
       });
        ChatMainView.IsVisibleP2PExpandBg(false);
    }

    private void ReportClick()
    {
        UserModel.ReportedUserModel = new ReportedUserModel()
        {
            Uid = _mArg.SessionId,
            DisplyName = _mArg.DisplayName,
            UserName = _mArg.UserName,
            HeadTexture2D = _mArg.HeadIconTexture2D,
            HeadUrl = _mArg.HeadUrl,
            Signature = _mArg.Signature,
            ParentId = _mArg.SessionId,
            ReportType = ReportType.User,
            FromReportViewType = FromReportViewType.ChatMain
        };
        iocViewManager.CloseCurrentOpenNew((int)UiId.Report);
        ChatMainView.IsVisibleP2PExpandBg(false);
    }

    private void BlockClick()
    {

        if (!IsBlock())
        {
            if (UserModel.Follows.ContainsKey(_mArg.SessionId))
            {   //拉入之前先取消关注
                dispatcher.Dispatch(CmdEvent.Command.FocusOptions, new FocusOptionInfo()
                {
                    Options = FocusOptions.DeleteFocus,
                    Id = _mArg.SessionId
                });
            }
        }

      dispatcher.Dispatch(CmdEvent.Command.ReqSetBlack,new ReqSetBlackInfo()
      {
           AccountId = _mArg.SessionId,
           IsBlack = !IsBlock()
       });
      ChatMainView.IsVisibleP2PExpandBg(false);
    }

    private void ReqSetBlackFinish()
    {
        ChatMainView.BlockTextStatus(IsBlock());
    }
    #endregion

    #region GroupNotification Listener

    private void UpdateGroupDataListener(IEvent eEvent)
    {
        var teamInfo = (NIMTeamInfo) eEvent.data;
        if(teamInfo!=null)
            Dispatcher.InvokeAsync(UpdateTitleName, teamInfo.Name);
    }

    private void UpdateTitleName(string displayName)
    {
        ChatMainView.TitleDisplayName.text = displayName;
    }

    private void RemoveGroupManagerListener()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadGroupInfoById, new LoadType() { SessionId = _mArg.SessionId, IsUpdateTeam = true });
    }

    private void AddGroupManagerFinishListener()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadGroupInfoById, new LoadType() { SessionId = _mArg.SessionId, IsUpdateTeam = true });
    }

    private void KickGroupMemberListenr()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadGroupInfoById, new LoadType() { SessionId = _mArg.SessionId, IsUpdateTeam = true });
    }

    private void AddMemberListener()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadGroupInfoById, new LoadType() { SessionId = _mArg.SessionId, IsUpdateTeam = true });
    }

    #endregion

    private bool IsBlock()
    {
        if (UserModel.BlackModels.Count < 1)
        {
            return false;
        }
        for (var i = 0; i < UserModel.BlackModels.Count; i++)
        {
            if (UserModel.BlackModels[i].Id == UserModel.ArgLoadChatRecord.SessionId.ToInt())
            {
                return true;
            }
        }
        return false;
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadChatRecordFinish, OnLoadChatRecordFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSingleChatRecordFinish, OnLoadSingleChatRecordFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReceiveImMsg, OnReceiveImMsg);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.SendImMsgSucc, OnSendMsgSucc);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.SendImMsgFail, OnSendMsgFail);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.AutoDownloadImResSucc, OnAutoDownloadResSucc);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.AddGroupManagerFinish, AddGroupManagerFinishListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.RemoveManagerFinish, RemoveGroupManagerListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.UpdateTeamInfoFinish, UpdateGroupDataListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.KickMemberSucc, KickGroupMemberListenr);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.AddMemberSucc, AddMemberListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupMemberReadRecordFinish, LoadGroupMemberReadCallback);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqCleanRecordFinish, CleanRecordFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqSetBlackSucc, ReqSetBlackFinish);


        ChatMainView.TextInput.onValueChanged.RemoveListener(OnTextValuedChanged);
        ChatMainView.BackButton.onClick.RemoveListener(OnBackBtnClick);
        ChatMainView.OptionButton.onClick.RemoveListener(OnOptionBtnClick);
        ChatMainView.SendTextButton.onClick.RemoveListener(OnSendTextBtnClick);
        ChatMainView.GetPhotoButton.onClick.RemoveListener(OnGetPhotoBtnClick);
        ChatMainView.TakePhotoButton.onClick.RemoveListener(OnTakePhotoBtnClick);
        ChatMainView.KeyboardButton.onClick.RemoveListener(OnKeyboardBtnClick);
        ChatMainView.VoiceButton.onClick.RemoveListener(OnVoiceBtnClick);
        ChatMainView.HitHiddenBtn.onClick.RemoveListener(OnHiddenBtnClick);
        ChatMainView.CancelBut.onClick.RemoveListener(CancelClick);
        ChatMainView.BgCancelBut.onClick.RemoveListener(BgCancelClick);
        ChatMainView.CleanRecordBut.onClick.RemoveListener(CleanRecordClick);
        ChatMainView.ReportBut.onClick.RemoveListener(ReportClick);
        ChatMainView.BlockBut.onClick.RemoveListener(BlockClick);
        ChatMainView.ReadInfoCloseBut.onClick.RemoveListener(ReadInfoCloseClick);
        ChatMainView.ReadInfoShowBut.onClick.RemoveListener(ReadInfoShowClick);
        ChatMainView.GroupReadRecordFiler.OnCellClick -= OnCellClick;
        ChatMainView.Filler.MScrollRect.onValueChanged.RemoveListener(MScrollRectListener);

        ChatMainView.PressAudioBtn.OnPressAudio -= OnPressAudito;
        ChatMainView.PressAudioBtn.OnEndPressAudio -= OnEndPressAudio;
        ChatMainView.PressAudioBtn.OnCancelAudio -= OnCancelPressAudio;
        ChatMainView.PressAudioBtn.OnTimeNotEnough -= OnTimeNotEnough;

        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.OnStopCaptureCb, OnStopAudioCaptureCallback);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.PlayAuido, OnPlayAudioCallback);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.StopPlayAudio, OnStopPlayAudioCallback);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.ChatMainHeadClick, ChatMainHeadCallBack);

        PickerEventListener.onImageSelect -= OnPickImageSelect;
        PickerEventListener.onImageLoad -= OnPickImageLoad;
        PickerEventListener.onError -= OnPickImagePickerError;
        PickerEventListener.onCancel -= OnPickImagePickerCancel;

        CameraShotEventListener.onImageSaved -= OnCameraImageSaved;
        CameraShotEventListener.onImageLoad -= OnCameraImageLoad;
        CameraShotEventListener.onVideoSaved -= OnCameraVideoSaved;
        CameraShotEventListener.onError -= OnCameraError;
        CameraShotEventListener.onCancel -= OnCameraCancel;
    }

    private void OnDestroy()
    {
        if (_mCellLists != null)
        {
            _mCellLists.Clear();
            _mCellLists = null;
        }
        
        if (ChatMainView.Filler.DataSource.Count > 0)
        {
            ChatMainView.Filler.DataSource.Clear();
            ChatMainView.Filler.DataSource = null;
        }

        Log.I("...==>>> ChatMainMediator... 销毁!");
        OnRemove();
    }

    #region Ui Event Callback & Control Ui

    private void ReadInfoShowClick()
    {
        ChatMainView.IsVisibleReadRecordAction(true);
    }
    private void ReadInfoCloseClick()
    {
        ChatMainView.IsVisibleReadRecordAction(false);
    }
    private void Refresh()
    {
        Dispatcher.InvokeAsync(ChatMainView.Filler.Refresh);
        Dispatcher.InvokeAsync(ChatMainView.Filler.GoToBottom);
    }

    private void OnHiddenBtnClick()
    {
        ChatMainView.ShowExtendBar(false);
    }

    private void SetDisplayName()
    {
        ChatMainView.SetTitleDisplayName(_mArg.DisplayName,_mArg.UserName);
    }

    private void OnBackBtnClick()
    {
        _mChatRecordCount = 0;
        dispatcher.Dispatch(CmdEvent.Command.SetMessageStatus, new MessagesTypeInfo()
        {
            Id = _mArg.SessionId,
            Type = _mArg.SessionType,
            MarkType = MarkType.MarkMessagesStaus
        });
        switch (UserModel.FromChatMainType)
        {
            case FromChatMainType.ChatSession:
                iocViewManager.DestroyAndOpenNew(ChatMainView.GetUiId(), (int)UiId.ChatSession);
                break;
            case FromChatMainType.Personal:
                iocViewManager.DestroyAndOpenNew(ChatMainView.GetUiId(), (int)UiId.Personal);
                break;
        }
    }

    private void OnOptionBtnClick()
    {
       if( _mArg.SessionType == NIMSessionType.kNIMSessionTypeP2P)
       {
          ChatMainView.IsVisibleP2PExpandBg(true);
       }
       else
       {
           UserModel.UserSelectedGroupId = _mArg.SessionId;
           Log.I("which id is my click ? -> :" + _mArg.SessionId);
           iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupSetting);
           dispatcher.Dispatch(CmdEvent.Command.LoadGroupInfoById, new LoadType() { SessionId = _mArg.SessionId, IsUpdateTeam = false });
        }
    }

    private void OnGetPhotoBtnClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidPicker.BrowseImage(false);
#elif UNITY_IPHONE && !UNITY_EDITOR
	    IOSPicker.BrowseImage(false);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        int random = UnityEngine.Random.Range(1, 7);
        string sendImagePath = @"D:\pic\" + random + ".jpg";
        int width = 0;
        int height = 0;
        switch (random)
        {
            case 1:
                width = 432;
                height = 668;
                break;
            case 2:
                width = 532;
                height = 710;
                break;
            case 3:
                width = 338;
                height = 600;
                break;
            case 4:
                width = 320;
                height = 480;
                break;
            case 5:
                width = 600;
                height = 329;
                break;
            case 6:
                width = 600;
                height = 315;
                break;
        }
        var tex = new Texture2D(width, height);
        tex.LoadImage(LoadImageFromLocalPath(sendImagePath));
        SendImageMsg(sendImagePath, tex);
#endif
    }

    private void OnTakePhotoBtnClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidCameraShot.GetTexture2DFromCamera(); 
#elif UNITY_IPHONE && !UNITY_EDITOR
		IOSCameraShot.GetTexture2DFromCamera(false);// capture and crop
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
    }

    private void OnVoiceBtnClick()
    {
        ChatMainView.ChangeAudioAndTextInput(true);
    }

    private void OnKeyboardBtnClick()
    {
        ChatMainView.ChangeAudioAndTextInput(false);
    }

    private void OnTimeNotEnough()
    {
        ChatMainView.CaptureAudioBox.gameObject.SetActive(false);
        UIUtil.Instance.ShowFailToast("语音时间过短");
    }

    private void OnCancelPressAudio()
    {
        ChatMainView.CaptureAudioBox.gameObject.SetActive(false);
        dispatcher.Dispatch(CmdEvent.Command.AudioOption, new ArgAudioOptionParam(){Option= EAudioOption.CancelRecord});
    }

    private void OnEndPressAudio()
    {
        ChatMainView.CaptureAudioBox.gameObject.SetActive(false);
        dispatcher.Dispatch(CmdEvent.Command.AudioOption, new ArgAudioOptionParam() { Option = EAudioOption.EndRecord });
    }

    private void OnPressAudito()
    {
        ChatMainView.CaptureAudioBox.gameObject.SetActive(true);
        dispatcher.Dispatch(CmdEvent.Command.AudioOption, new ArgAudioOptionParam() {Option = EAudioOption.BeginRecord});
    }

    private void OnPlayAudioCallback(Notification notification)
    {
        dispatcher.Dispatch(CmdEvent.Command.AudioOption, new ArgAudioOptionParam() { Option = EAudioOption.PlayAudio ,AudioPath = (notification.Content as ChatAudioModel).AuidoPath });
    }

    private void OnStopPlayAudioCallback(Notification notification)
    {
        dispatcher.Dispatch(CmdEvent.Command.AudioOption, new ArgAudioOptionParam() { Option = EAudioOption.StopPlayAudio });
    }
    #endregion

    #region Dispathcer Callback
    private void OnSendMsgFail(IEvent evt)
    {
         UIUtil.Instance.ShowFailToast(evt.data as string);
    }

    private void OnSendMsgSucc(IEvent evt)
    {
        var param = evt.data as MessageAck;
        if (param != null)
            dispatcher.Dispatch(CmdEvent.Command.LoadSingleChatRecord, new ArgLoadSingleChatRecord() { MsgId = param.MsgId });
    }

    private void OnReceiveImMsg(IEvent evt)
    {
        var arg = evt.data as NIMIMMessage;
        if (arg != null)
        {
            if(arg.TalkID!=_mArg.SessionId)return;

            switch (arg.MessageType)
            {
                case NIMMessageType.kNIMMessageTypeText:
                    CreateTextModelByReceive(arg);
                    break;

                case NIMMessageType.kNIMMessageTypeImage:
                    break;

                case NIMMessageType.kNIMMessageTypeAudio:
                    break;
                case NIMMessageType.kNIMMessageTypeNotification:
                    CreateNotifyModelReceive(arg);
                    break;
            }
          
        }
    }

    private void OnLoadChatRecordFinish(IEvent evt)
    {
        _mArg = (ArgLoadChatRecord)evt.data;

        Dispatcher.InvokeAsync(SetDisplayName);
        if (_mArg.RetChatRecordCellModels != null)
        {
            if (_mCellLists == null)
            {
                _mCellLists = new List<ChatBaseModel>();
            }
            for (int i = 0; i < _mArg.RetChatRecordCellModels.Count- _mChatRecordCount; i++)
            {
                _mCellLists.Insert(i, _mArg.RetChatRecordCellModels[i]);
            }
            ChatMainView.Filler.DataSource = _mCellLists;

            if (_mArg.IsLoadMore)
            {
                Log.I("记录数量：" + ChatMainView.Filler.DataSource.Count + "  " + _mChatRecordCount);
                Dispatcher.InvokeAsync(ChatMainView.Filler.Refresh);
                Dispatcher.InvokeAsync(ChatMainView.Filler.GoToTop, ChatMainView.Filler.DataSource.Count - _mChatRecordCount);
            }
            else
            {
                Refresh();
            }
            _mChatRecordCount = _mCellLists.Count;
        }
    }

    private void OnLoadSingleChatRecordFinish(IEvent evt)
    {
        var arg = (ArgLoadSingleChatRecord)evt.data;

        if (arg.RetMsg.ReceiverID == _mArg.SessionId)
        {
            ChangeSendMsgState2Succ(arg.RetMsg);
        }
        else
        {
            if (arg.RetMsg.MessageType == NIMMessageType.kNIMMessageTypeImage)
            {
                CreateImageModelByReceive(arg.RetMsg);
            }
            else if (arg.RetMsg.MessageType == NIMMessageType.kNIMMessageTypeAudio)
            {
                CreateAudioModelByReceive(arg.RetMsg);
            }
        }
       
    }

    private void OnAutoDownloadResSucc(IEvent evt)
    {
        var arg = (ArgAutoDownloadSucc) evt.data;
        if (arg.RetSessionId != _mArg.SessionId)
            return;
        dispatcher.Dispatch(CmdEvent.Command.LoadSingleChatRecord,new ArgLoadSingleChatRecord(){ MsgId = arg.RetMsgId});
    }

    private void LoadGroupMemberReadCallback(IEvent eEvent)
    {
        var record = (ReqGroupMemberReadInfo)eEvent.data;
        ChatMainView.SetAnnountUi(record.Annount);
        if (record.MemberReadRecordModels != null&& record.MemberReadRecordModels.Count >0)
        {
            ChatMainView.GroupReadRecordFiler.DataSource = record.MemberReadRecordModels;
            ChatMainView.GroupReadRecordFiler.Refresh();
        }
    }

    private void CleanRecordFinish()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadChatRecord, new ArgLoadChatRecord()
        {
            SessionId = _mArg.SessionId,
            SessionType = _mArg.SessionType,
            Signature = _mArg.Signature,
            HeadUrl = _mArg.HeadUrl,
            DisplayName = _mArg.DisplayName,
            TimeTag = _mCellLists[_mCellLists.Count - 1].OriginalTime,
            HeadIconTexture2D = _mArg.HeadIconTexture2D,
            Count = 0
        });
        _mCellLists.Clear();
        _mChatRecordCount = 0;
    }
    #endregion

    #region Type of Text Msg About
    private void OnSendTextBtnClick()
    {
        if (string.IsNullOrEmpty(ChatMainView.TextInput.text))
            return;

        string contentText = ChatMainView.TextInput.text;
        ChatMainView.TextInput.text = String.Empty;

        var ext = new ImLocalExtension()
        {
            MsgType = ImLocalExtension.EMsgType.Text,
            UniqueId = Util.Md5(contentText + DateTime.Now)
        };

        var arg = new ArgSendChatMsg()
        {
            Type = NIMMessageType.kNIMMessageTypeText,
            Text = contentText,
            ReceiverId = _mArg.SessionId,
            LocalExtension = CommUtil.Instance.BuildImLocalExtension(ext)
        };

        CreateTextModelBySend(contentText, ext.UniqueId);
        dispatcher.Dispatch(CmdEvent.Command.SendImMsg, arg);
    }

    private void CreateTextModelBySend(string text, string uniqueId)
    {
        var textModel = new ChatTextModel()
        {
            IsP2P = _mArg.SessionType == NIMSessionType.kNIMSessionTypeP2P,
            SenderId = UserModel.User.Id.ToString(),
            ChatMsgType = ChatMsgType.Text,
            ChatOwener = ChatOwener.Me,
            OriginalTime = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
            Context = text,
        };

        if (textModel.IsP2P)
        {
            textModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
        }

        if (_mSendMsgUnCheckedCache == null)
        {
            _mSendMsgUnCheckedCache = new Dictionary<string, ChatBaseModel>();
        }

        if (!_mSendMsgUnCheckedCache.ContainsKey(uniqueId))
        {
            _mSendMsgUnCheckedCache.Add(uniqueId, textModel);
           
          Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, textModel);
        }
     
    }

    private void CreateTextModelByReceive(NIMIMMessage message)
    {
        var msg = message as NIMTextMessage;
        if (msg != null)
        {
            ChatTextModel textModel = new ChatTextModel()
            {
                IsP2P = msg.SessionType == NIMSessionType.kNIMSessionTypeP2P,
                SenderId = msg.SenderID,
                MsgId = message.ClientMsgID,
                ChatMsgType = ChatMsgType.Text,
                ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
                OriginalTime = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
                Context = msg.TextContent
            };

            if (textModel.IsP2P)
            {
                textModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
            }
            Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, textModel);
        }
    }
    #endregion

    #region Type of Image Msg About
    private void SendImageMsg(string imagePath, Texture2D tex)
    {
        if (tex == null) return;
        int width = 0, height = 0;
        float aspectRatio = (float)tex.width / tex.height;
        if (aspectRatio > 1)
        {
            width = Const.ChatWidthImageMax;
            height = (int)(Const.ChatWidthImageMax / aspectRatio);
        }
        else
        {
            height = Const.ChatHeightImageMax;
            width = (int)(Const.ChatHeightImageMax * aspectRatio);
        }

        if (imagePath == null || tex == null)
            return;

        string attachmentMd5 = Util.Md5(imagePath + DateTime.Now);

        var ext = new ImLocalExtension()
        {
            MsgType = ImLocalExtension.EMsgType.Image,
            UniqueId = attachmentMd5
        };

        var arg = new ArgSendChatMsg()
        {
            Type = NIMMessageType.kNIMMessageTypeImage,
            ReceiverId = _mArg.SessionId,
            AttachmentPath = imagePath,
            AttachmentMd5 = attachmentMd5,
            Width = width,
            Height = height,
            LocalExtension = CommUtil.Instance.BuildImLocalExtension(ext)
        };

        CreateImageModelBySend(imagePath, ext.UniqueId, width, height);
        dispatcher.Dispatch(CmdEvent.Command.SendImMsg, arg);
    }

    private void CreateImageModelBySend(string imagePath, string uniqueId, int width, int height)
    {
        var imageModel = new ChatImageModel()
        {
            IsP2P = _mArg.SessionType == NIMSessionType.kNIMSessionTypeP2P,
            SenderId = UserModel.User.Id.ToString(),
            ChatMsgType = ChatMsgType.Image,
            ChatOwener = ChatOwener.Me,
            OriginalTime = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
            ImageBytes = LoadImageFromLocalPath(imagePath),
            Width = width,
            Height = height
        };


        if (imageModel.IsP2P)
        {
            imageModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
        }

        if (_mSendMsgUnCheckedCache == null)
        {
            _mSendMsgUnCheckedCache = new Dictionary<string, ChatBaseModel>();
        }

        if (!_mSendMsgUnCheckedCache.ContainsKey(uniqueId))
        {
            _mSendMsgUnCheckedCache.Add(uniqueId, imageModel);
        
            Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, imageModel);
        }
    }

    private void CreateImageModelByReceive(NIMIMMessage message)
    {
        NIMImageMessage msg = message as NIMImageMessage;

        if (msg == null)
            return;

        string uidMd5 = Util.Md5(UserModel.User.Id.ToString());

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        var imagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\image\" + msg.ImageAttachment.MD5;
        var resPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                      + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\res\" + Util.Md5(msg.ImageAttachment.RemoteUrl);
#elif UNITY_ANDROID
            var imagePath = ImService.AppDataPath + "/NIM/" + uidMd5 + "/image/" + msg.ImageAttachment.MD5;
            var resPath = ImService.AppDataPath + "/NIM/" + uidMd5 + "/res/" +
                            Util.Md5(msg.ImageAttachment.RemoteUrl);
#elif UNITY_IPHONE
            var imagePath = ImService.AppDataPath + "/NIM_Debug/" + uidMd5 + "/image/" + msg.ImageAttachment.MD5;
            var resPath = ImService.AppDataPath + "/NIM_Debug/" + uidMd5 + "/res/" +
                            Util.Md5(msg.ImageAttachment.RemoteUrl);
#endif
        ChatImageModel imageModel = new ChatImageModel()
        {
            IsP2P = msg.SessionType == NIMSessionType.kNIMSessionTypeP2P,
            SenderId = msg.SenderID,
            MsgId = message.ClientMsgID,
            ChatMsgType = ChatMsgType.Image,
            ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
            OriginalTime = msg.TimeStamp,
            ImageBytes = LoadImageFromAllPath(imagePath, resPath),
            Width = msg.ImageAttachment.Width,
            Height = msg.ImageAttachment.Height
        };
        if (imageModel.IsP2P)
        {
            imageModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
        }
        Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, imageModel);
    }
    #endregion

    #region Type of Audio Msg About

    private void OnStopAudioCaptureCallback(Notification notification)
    {
        StopCaptureCbParam sp = notification.Content as StopCaptureCbParam;
        if (sp.ResCode == 200)
        {
            string attachmentMd5 = Util.Md5(sp.FilePath + DateTime.Now);

            var ext = new ImLocalExtension()
            {
                MsgType = ImLocalExtension.EMsgType.Audio,
                UniqueId = attachmentMd5
            };
            
            var arg = new ArgSendChatMsg()
            {
                Type = NIMMessageType.kNIMMessageTypeAudio,
                ReceiverId = _mArg.SessionId,
                AttachmentPath = sp.FilePath,
                AttachmentMd5 = attachmentMd5,
                Duration = sp.AudioDuration,
                LocalExtension = CommUtil.Instance.BuildImLocalExtension(ext)
            };

            dispatcher.Dispatch(CmdEvent.Command.SendImMsg, arg);
            CreateAudioModelBySend(sp.FilePath, sp.AudioDuration, ext.UniqueId);
        }
    }

    private void CreateAudioModelBySend(string audioPath, int duration, string uniqueId)
    {
        var audioModel = new ChatAudioModel()
        {
            IsP2P = _mArg.SessionType == NIMSessionType.kNIMSessionTypeP2P,
            SenderId = UserModel.User.Id.ToString(),
            ChatMsgType = ChatMsgType.Audio,
            ChatOwener = ChatOwener.Me,
            OriginalTime = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
            AuidoPath = audioPath,
            IsRead = true,
            AudioDuration = duration
        };

        if (audioModel.IsP2P)
        {
            audioModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
        }
        if (_mSendMsgUnCheckedCache == null)
        {
            _mSendMsgUnCheckedCache = new Dictionary<string, ChatBaseModel>();
        }

        if (!_mSendMsgUnCheckedCache.ContainsKey(uniqueId))
        {
            _mSendMsgUnCheckedCache.Add(uniqueId, audioModel);
           
            Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, audioModel);
        }
    }

    private void CreateAudioModelByReceive(NIMIMMessage message)
    {
        NIMAudioMessage msg = message as NIMAudioMessage;

        if (msg == null)
            return;

        string uidMd5 = Util.Md5(UserModel.User.Id.ToString());

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        var audioPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\audio\" + msg.AudioAttachment.MD5;
        var resPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                      + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\res\" + Util.Md5(msg.AudioAttachment.RemoteUrl);
#elif UNITY_ANDROID
			var audioPath = ImService.AppDataPath + "/NIM/" + uidMd5 + "/audio/" + msg.AudioAttachment.MD5;
            var resPath = ImService.AppDataPath + "/NIM/" + uidMd5 + "/res/" +
				Util.Md5(msg.AudioAttachment.RemoteUrl);
#elif UNITY_IPHONE
			var audioPath = ImService.AppDataPath + "/NIM_Debug/" + uidMd5 + "/audio/" + msg.AudioAttachment.MD5;
            var resPath = ImService.AppDataPath + "/NIM_Debug/" + uidMd5 + "/res/" +
				Util.Md5(msg.AudioAttachment.RemoteUrl);
#endif
        ChatAudioModel audioModel = new ChatAudioModel
        {
            SenderId = message.SenderID,
            MsgId = message.ClientMsgID,
            ChatMsgType = ChatMsgType.Audio,
            ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
            OriginalTime = msg.TimeStamp,
            AuidoPath = LoadAudioPath(audioPath, resPath),
            IsP2P = message.SessionType == NIMSessionType.kNIMSessionTypeP2P,
            AudioDuration = msg.AudioAttachment.Duration,
        };
        if (audioModel.IsP2P)
        {
            audioModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
        }

        Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, audioModel);
    }

    #endregion

    #region Type of Notifition Msg About

    private void CreateNotifyModelReceive(NIMIMMessage message)
    {
        var msg = message as NIMTeamNotificationMessage;
        if (msg != null)
        {
            ChatNotificationModel notifyModel = new ChatNotificationModel()
            {
                IsP2P = message.SessionType == NIMSessionType.kNIMSessionTypeP2P,
                MsgId = msg.ClientMsgID,
                ReceiverId = msg.ReceiverID,
                SenderId = msg.SenderID,
                ChatMsgType = ChatMsgType.Notification,
                NotificationType = msg.NotifyMsgData.NotificationId,
                ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
                OriginalTime = msg.TimeStamp
            };

            Dispatcher.InvokeAsync(LoadSenderHeadTexture2D, notifyModel);
        }    
    }

    #endregion

    #region 私有方法
    private void ChangeSendMsgState2Succ(NIMIMMessage message)
    {
        if (message == null) return;

        var ext = CommUtil.Instance.ParseImLocalExtension(message.LocalExtension);
        if (_mSendMsgUnCheckedCache.ContainsKey(ext.UniqueId))
        {
            var chatModel = _mSendMsgUnCheckedCache[ext.UniqueId];
            chatModel.MsgId = message.ClientMsgID;
            chatModel.MsgSendState = MsgSendState.SendSucc;
            switch (ext.MsgType)
            {
                case ImLocalExtension.EMsgType.Text:
                    break;

                case ImLocalExtension.EMsgType.Image:
                    //自身发送图片成功后，需要去下载，因为本地记录路径没有
                    dispatcher.Dispatch(CmdEvent.Command.DownloadLostResChat, new ArgDownloadLostResChat() { Msgs = new List<NIMIMMessage>() { message }, IsForceUpdateCell = false });
                    break;

                case ImLocalExtension.EMsgType.Audio:
                    break;
            }
            _mSendMsgUnCheckedCache.Remove(ext.UniqueId);
            Log.I("发送成功,队列剩余数:" + _mSendMsgUnCheckedCache.Count);
        }
    }

    private byte[] LoadImageFromLocalPath(string path)
    {
        if (System.IO.File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            return bytes;
        }
        return null;
    }

    private byte[] LoadImageFromAllPath(string imagePath, string resPath)
    {
        byte[] b = LoadImageFromLocalPath(imagePath);
        if (b == null)
        {
            b = LoadImageFromLocalPath(resPath);
            if (b == null)
            {
                Log.I("找不到的Image路径: " + imagePath);
                Log.E("图片真心找不到");
                return null;
            }
            return b;
        }
        return b;
    }

    private string LoadAudioPath(string audioPath, string resPath)
    {
        if (System.IO.File.Exists(audioPath))
        {
            return audioPath;
        }
        else if (System.IO.File.Exists(resPath))
        {
            return resPath;
        }
        else
        {
            return null;
        }
    }

    private void LoadSenderHeadTexture2D(ChatBaseModel model)
    {
        if (_mCellLists == null)
        {
            _mCellLists = new List<ChatBaseModel>();
        }

        if (model.ChatMsgType != ChatMsgType.Notification && !model.IsP2P)
        {
            FriendService.RequestUserInfoById(model.SenderId, (i, user) =>
            {
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    model.HeadIconUrl = user.AvatarUrl;
                }
            } );
        }
        _mCellLists.Add(model);
        _mChatRecordCount = _mCellLists.Count;
        Refresh();
    }

    #endregion

    #region 图片选择器回调

    private void OnPickImageSelect(string imgPath, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("Image Location : " + imgPath + " Image Orientation" + imgOrientation);
    }

    private void OnPickImageLoad(string imgPath, Texture2D tex, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("准备发送OnImageLoad ： Image Location : " + imgPath);
        SendImageMsg(imgPath, tex);
    }

    private void OnPickImagePickerError(string errorMsg)
    {
        Log.I("Error : " + errorMsg);
    }

    private void OnPickImagePickerCancel()
    {
        Log.I("Cancel by user");
    }

    #endregion

    #region 照相机回调
    void OnCameraImageSaved(string path, CameraShot.ImageOrientation orientation)
    {
        Log.I("Image Saved to gallery, path : " + path + ", orientation : " + orientation);
    }

    void OnCameraImageLoad(string path, Texture2D tex, CameraShot.ImageOrientation orientation)
    {
        SendImageMsg(path, tex);
        Log.I("Image Saved to gallery, loaded: " + path + ", orientation: " + orientation);
    }

    void OnCameraVideoSaved(string path)
    {
        Log.I("Video Saved at path : " + path);
    }

    void OnCameraError(string errorMsg)
    {
        Log.I("Error : " + errorMsg);
    }

    void OnCameraCancel()
    {
        Log.I("OnCancel");
    }
    #endregion
}
