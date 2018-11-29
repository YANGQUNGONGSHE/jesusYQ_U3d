using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NIM;
using NIM.Session;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PersonalMediator : EventMediator {
    [Inject]
    public PersonalView PersonalView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private PostModel _postModel;
    private PersonalInfo _mPersonalInfo;
    private FromViewType _mFromViewType;
    private bool _mIsLive = false;
    private List<PostModel> _mPostModels;

    public override void OnRegister()
    {
        PersonalView.BackBut.onClick.AddListener(Back);
        PersonalView.ExpandBut.onClick.AddListener(ExpandTop);
        PersonalView.SerachBut.onClick.AddListener(SearchClick);
        PersonalView.CloseExpand.onClick.AddListener(CloseExpandClick);
        PersonalView.CancelExpand.onClick.AddListener(CloseExpandClick);
        PersonalView.AddFocusBut.onClick.AddListener(AddFocusClick);
        PersonalView.PersonalMsBut.onClick.AddListener(PersonalMsClick);
        PersonalView.EditorDataBut.onClick.AddListener(EditorDataClick);
        PersonalView.CancelFocusBut.onClick.AddListener(CancelFocusClick);
        PersonalView.ReportBut.onClick.AddListener(ReportClick);
        PersonalView.SetBlackBut.onClick.AddListener(SetBlackClick);
        PersonalView.PersonalPostFiler.OnCellClick = OnCellClick;
        PersonalView.PersonalPostFiler.ClickTypeCallBack = ClickTypeCallBack;
        //PersonalView.MainRect.onValueChanged.AddListener(MainScrollRectListener);
        //PersonalView.PersonalPostFiler.ScrollView.onValueChanged.AddListener(ListScrollRectListener);
        PersonalView.PersonalPostFiler.ScrollView.enabled = false;
        PersonalView.PersonalPostFiler.ScrollView.movementType = ScrollRect.MovementType.Clamped;
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqDataByPosterSucc,ReqDataByPosterSucc);
        dispatcher.AddListener(CmdEvent.ViewEvent.QueryUserReadRecordCoutFinish,QueryUserReadRecordCoutFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqSetBlackSucc,ReqSetBlackFinish);
        //dispatcher.AddListener(CmdEvent.ViewEvent.LoginImSucc,LoginImSuccCallBack);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.DeletePersonalPreachSucc,DeletePersonalPreachSucc);
        // dispatcher.AddListener(CmdEvent.ViewEvent.LoginImFail,LoginImFailCallBack);
        LoadData();
    }

    private void DeletePersonalPreachSucc(Notification notification)
    {
        RefreshData();
    }
    private void LoadData()
    {
        _mFromViewType = UserModel.PostModel.FromType;
        dispatcher.Dispatch(CmdEvent.Command.ReqPersonalData, new PersonalInfo()
        {
            UserId = UserModel.PostModel.Author.Id.ToString(),
            IsRequestAll = true,
            Skip = 0,
            Limit = 20,
            IsRefresh = false
        });
        dispatcher.Dispatch(CmdEvent.Command.QueryUserReadRecord, new ReadRecordData()
        {
            Type = QueryReadRecordType.QueryReadCount,
            ParentType = "节",
            ParentidPrefix = string.Empty,
            UserId = UserModel.PostModel.Author.Id.ToString()
        });
        PersonalView.SetDefaultUi(UserModel.PostModel);
        if (UserModel.PostModel.Author.Id == UserModel.User.Id)
        {
            PersonalView.IsVisibleSocialPart(false);
            PersonalView.IsVisibleEditorRect(true);
            PersonalView.IsVisibleCancelFocusRecTrans(false);
            PersonalView.DisVisibleTopAction();
        }
        else
        {
            PersonalView.IsVisibleSocialPart(true);
            PersonalView.IsVisibleEditorRect(false);
            if (UserModel.Follows.ContainsKey(UserModel.PostModel.Author.Id.ToString()))
            {
                PersonalView.IsFocus(true);
                PersonalView.IsVisibleCancelFocusRecTrans(true);
            }
            else
            {
                PersonalView.IsFocus(false);
                PersonalView.IsVisibleCancelFocusRecTrans(false);
            }
        }
        if (_mFromViewType == FromViewType.FromGroupMemberView)
        {
            iocViewManager.DestoryView((int)UiId.ChatMain);
        }
        PersonalView.BlackTextStaus(IsBlack());
    }

    #region dispatcher Event

    private void ReqDataByPosterSucc(IEvent eEvent)
    {
        _mPersonalInfo = (PersonalInfo)eEvent.data;
        if (_mPersonalInfo.IsRequestAll)
        {
            UIUtil.Instance.CloseWaiting();
            PersonalView.SetFansAndFollowerUi(_mPersonalInfo.FansCount,_mPersonalInfo.FollowerCount);
            if (_mPostModels != null)
            {
                _mPostModels.Clear();
            }

            if (_mPersonalInfo.PostModels == null || _mPersonalInfo.PostModels.Count < 1)
            {
                PersonalView.IsVisibleScrollContent(false);
            }
            else
            {
                PersonalView.IsVisibleScrollContent(true);
                if (_mPersonalInfo.PostModels.Count > 1)
                {
                    PersonalView.MainRect.onValueChanged.AddListener(MainScrollRectListener);
                    PersonalView.PersonalPostFiler.ScrollView.onValueChanged.AddListener(ListScrollRectListener);
                }
            }
        }
        if (_mPersonalInfo.IsRefresh)
        {
            if (_mPostModels != null)
            {
                _mPostModels.Clear();
            }
        }
        if (_mPostModels == null)
        {
            _mPostModels = new List<PostModel>();
        }
        if (_mPersonalInfo.PostModels != null)
            for (var i = 0; i < _mPersonalInfo.PostModels.Count; i++)
            {
                _mPostModels.Add(_mPersonalInfo.PostModels[i]);
            }
        PersonalView.PersonalPostFiler.DataSource = _mPostModels;
        PersonalView.PersonalPostFiler.Refresh();
    }

    private void QueryUserReadRecordCoutFinish(IEvent eEvent)
    {
        var recordCount = eEvent.data as ReadRecordCount;
        if (recordCount == null) return;
        PersonalView.SetReadDaysUi(recordCount.DaysCount.ToString());
    }

    //private void LoginImFailCallBack()
    //{
      
    //}
    private void LoginImSuccCallBack()
    {
       TurnToChatMain();
    }

    private void ReqSetBlackFinish()
    {
        PersonalView.BlackTextStaus(IsBlack());
    }
    #endregion

    #region Click Event
    private void SearchClick()
    {
        UserModel.PostSearchType = PostSearchType.Personal;
        iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.PreachSearch);
    }
    private void CancelFocusClick()
    {
       dispatcher.Dispatch(CmdEvent.Command.FocusOptions,new FocusOptionInfo()
       {
           Options = FocusOptions.DeleteFocus,
           Id = UserModel.PostModel.Author.Id.ToString()
       });
        PersonalView.IsFocus(false);
        PersonalView.IsVisibleCancelFocusRecTrans(false);
        PersonalView.IsVisibleExpandAction(false);

    }
    private void EditorDataClick()
    {
        UserModel.EditorUserDataType = EditorUserDataType.Personal;
        iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.EditorUserData);
    }
    private void Back()
    {
        //CleanCache();
        switch (_mFromViewType)
        {
            case FromViewType.FromPreachView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.Preach);
                break;
            case FromViewType.FromPersonalView:
                break;
            case FromViewType.FromPreachSearchView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.Preach);
                break;
            case FromViewType.FromLikePostView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.MyLikePosts);
                break;
            case FromViewType.FromAccountView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.Me);
                break;
            case FromViewType.FromFansAndFollowView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(),(int)UiId.FocusAndFans);
                break;
            case FromViewType.FromGroupMemberView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(), (int)UiId.ChatGroupMember);
                break;
            case FromViewType.FromReadRecordView:
                iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(), (int)UiId.ChatMain);
                break;
        }
    }
    private void ExpandTop()
    {
        PersonalView.IsVisibleExpandAction(true);
    }
    private void CloseExpandClick()
    {
        PersonalView.IsVisibleExpandAction(false);
    }
    private void OnCellClick(int index, PostModel postModel)
    {
        OpenPostView(postModel);
    }
    private void ClickTypeCallBack(ClickType type,int index, PostModel postModel)
    {
        switch (type)
        {
            case ClickType.Comment:
                OpenPostView(postModel);
                break;
            case ClickType.Like:
                dispatcher.Dispatch(CmdEvent.Command.PostInteraction, new InteractionInfo()
                {
                    Type = InteractionType.Like,
                    ParentType = "帖子",
                    ParentId = postModel.Id
                });
                break;
            case ClickType.DisLike:
                dispatcher.Dispatch(CmdEvent.Command.PostInteraction, new InteractionInfo()
                {
                    Type = InteractionType.DisLike,
                    ParentId = postModel.Id
                });
                break;
            case ClickType.Share:
                break;
            case ClickType.OpenPersonal:

                break;
        }
    }
    private void PersonalMsClick()
    {
        #region 正式代码

        //if (ClientAPI.GetLoginState() == NIMLoginState.kNIMLoginStateLogin)
        //{
        //    TurnToChatMain();
        //}
        //else
        //{
        //    dispatcher.Dispatch(CmdEvent.Command.LoginIm);
        //}

        #endregion

        #region 调试代码

        TurnToChatMain();

        #endregion

    }
    private void AddFocusClick()
    {
        if (IsBlack())
        {
            UIUtil.Instance.ShowTextToast("该用户已被拉黑！");
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.FocusOptions, new FocusOptionInfo()
            {
                Options = FocusOptions.AddFcous,
                Id = UserModel.PostModel.Author.Id.ToString()
            });
            PersonalView.IsFocus(true);
            PersonalView.IsVisibleCancelFocusRecTrans(true);
        }
    }
    private void ReportClick()
    {
        UserModel.ReportedUserModel = new ReportedUserModel()
        {
            Uid = UserModel.PostModel.Author.Id.ToString(),
            UserName = UserModel.PostModel.Author.UserName,
            DisplyName = UserModel.PostModel.Author.DisplayName,
            HeadTexture2D = UserModel.PostModel.HeadTexture2D,
            HeadUrl = UserModel.PostModel.Author.AvatarUrl,
            Signature = UserModel.PostModel.Author.Signature,
            ParentId = UserModel.PostModel.Author.Id.ToString(),
            ReportType = ReportType.User,
            FromReportViewType = FromReportViewType.Personal
        };
        iocViewManager.CloseCurrentOpenNew((int)UiId.Report);
        PersonalView.IsVisibleExpandAction(false);
    }
    private void SetBlackClick()
    {
        if (!IsBlack())
        {
            if (UserModel.Follows.ContainsKey(UserModel.PostModel.Author.Id.ToString()))
            {   //拉入之前先取消关注
                dispatcher.Dispatch(CmdEvent.Command.FocusOptions, new FocusOptionInfo()
                {
                    Options = FocusOptions.DeleteFocus,
                    Id = UserModel.PostModel.Author.Id.ToString()
                });
                PersonalView.IsFocus(false);
                PersonalView.IsVisibleCancelFocusRecTrans(false);
            }
        }
        dispatcher.Dispatch(CmdEvent.Command.ReqSetBlack,new ReqSetBlackInfo()
        {
            AccountId = UserModel.PostModel.Author.Id.ToString(),
            IsBlack = !IsBlack()
        });
        PersonalView.IsVisibleExpandAction(false);
    }
    #endregion

    private void TurnToChatMain()
    {
        if (_mFromViewType == FromViewType.FromReadRecordView)
        {
            UserModel.FromChatMainType = FromChatMainType.ChatSession;
        }
        else if (_mFromViewType == FromViewType.FromGroupMemberView)
        {
            iocViewManager.DestoryView((int)UiId.ChatGroupMember);
            iocViewManager.DestoryView((int)UiId.ChatGroupSetting);
            UserModel.FromChatMainType = FromChatMainType.ChatSession;
        }
        else
        {
            UserModel.FromChatMainType = FromChatMainType.Personal;
        }
        UserModel.UserSelectedChatModel = NIMSessionType.kNIMSessionTypeP2P;
        UserModel.ArgLoadChatRecord = new ArgLoadChatRecord()
        {
            SessionId = UserModel.PostModel.Author.Id.ToString(),
            SessionType = NIMSessionType.kNIMSessionTypeP2P,
            DisplayName = UserModel.PostModel.Author.DisplayName,
            TimeTag = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
            HeadIconTexture2D = PersonalView.HeadTexture2D,
            HeadUrl = UserModel.PostModel.Author.AvatarUrl,
            Signature = UserModel.PostModel.Author.Signature,
            Count = 0
        };
        iocViewManager.DestroyAndOpenNew(PersonalView.GetUiId(), (int)UiId.ChatMain);
    }
    private void MainScrollRectListener(Vector2 arg0)
    {
        if (PersonalView.MainRect.normalizedPosition.y < 0.0f)
        {
            if (!_mIsLive)
            {
                PersonalView.PersonalPostFiler.ScrollView.enabled = true;
            }
            else
            {
                _mIsLive = false;
            }
        }
        if (PersonalView.MainRect.normalizedPosition.y > 1.2f && Input.GetMouseButtonUp(0))
        {
            RefreshData();
        }
    }
    private void ListScrollRectListener(Vector2 arg0)
    {
        if (PersonalView.PersonalPostFiler.ScrollView.normalizedPosition.y >= 1.0f)
        {
            _mIsLive = true;
            PersonalView.PersonalPostFiler.ScrollView.enabled = false;
        }
        if (PersonalView.PersonalPostFiler.ScrollView.normalizedPosition.y <= 0.0f)
        {
            if (PersonalView.PersonalPostFiler.DataSource.Count < 2) return;
            dispatcher.Dispatch(CmdEvent.Command.ReqPersonalData, new PersonalInfo()
            {
                UserId = UserModel.PostModel.Author.Id.ToString(),
                Skip = PersonalView.PersonalPostFiler.DataSource.Count,
                Limit = 20,
                IsRequestAll = false,
                IsRefresh = false
            });
        }
    }
    private void OpenPostView(PostModel model)
    {
        UserModel.PostModel = model;
        if(UserModel.PostModel == null)return;
        UserModel.PostModel.FromType = FromViewType.FromPersonalView;
        UserModel.PostDetailModel = UserModel.PostModel;
        iocViewManager.CloseCurrentOpenNew((int) UiId.PreachPost);
    }
    private void RefreshData()
    {
        Log.I("RefreshData!!!!");
        dispatcher.Dispatch(CmdEvent.Command.ReqPersonalData, new PersonalInfo()
        {
            UserId = UserModel.PostModel.Author.Id.ToString(),
            Skip = 0,
            Limit = 20,
            IsRequestAll = false,
            IsRefresh = true
        });
        PersonalView.PersonalPostFiler.DataSource.Clear();
    }
    private void  CleanCache()
    {
        if (_mPostModels != null)
        {
            _mPostModels.Clear();
        }
        if (PersonalView.PersonalPostFiler.DataSource != null)
        {
            PersonalView.PersonalPostFiler.DataSource.Clear();
            PersonalView.PersonalPostFiler.DataSource = null;
        }
    }

    private bool IsBlack()
    {

        if (UserModel.BlackModels.Count < 1)
        {
            return false;
        }
        for (var i = 0; i < UserModel.BlackModels.Count; i++)
        {
            if (UserModel.BlackModels[i].Id == UserModel.PostModel.Author.Id)
            {
                return true;
            }
        }
        return false;
    }
    public override void OnRemove()
    {
        PersonalView.BackBut.onClick.RemoveListener(Back);
        PersonalView.ExpandBut.onClick.RemoveListener(ExpandTop);
        PersonalView.CloseExpand.onClick.RemoveListener(CloseExpandClick);
        PersonalView.CancelExpand.onClick.RemoveListener(CloseExpandClick);
        PersonalView.AddFocusBut.onClick.RemoveListener(AddFocusClick);
        PersonalView.PersonalMsBut.onClick.RemoveListener(PersonalMsClick);
        PersonalView.EditorDataBut.onClick.RemoveListener(EditorDataClick);
        PersonalView.CancelFocusBut.onClick.RemoveListener(CancelFocusClick);
        PersonalView.SerachBut.onClick.RemoveListener(SearchClick);
        PersonalView.ReportBut.onClick.RemoveListener(ReportClick);
        PersonalView.SetBlackBut.onClick.RemoveListener(SetBlackClick);
        PersonalView.PersonalPostFiler.OnCellClick -= OnCellClick;
        PersonalView.PersonalPostFiler.ClickTypeCallBack -= ClickTypeCallBack;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqDataByPosterSucc, ReqDataByPosterSucc);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.QueryUserReadRecordCoutFinish, QueryUserReadRecordCoutFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqSetBlackSucc, ReqSetBlackFinish);
        // dispatcher.RemoveListener(CmdEvent.ViewEvent.LoginImSucc, LoginImSuccCallBack);
        // dispatcher.RemoveListener(CmdEvent.ViewEvent.LoginImFail, LoginImFailCallBack);
        //PersonalView.MainRect.onValueChanged.RemoveListener(MainScrollRectListener);
        //PersonalView.PersonalPostFiler.ScrollView.onValueChanged.RemoveListener(ListScrollRectListener);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.DeletePersonalPreachSucc, DeletePersonalPreachSucc);
    }
    private void OnDestroy()
    {
        if (_mPostModels != null)
        {
            _mPostModels.Clear();
        }
        if (PersonalView.PersonalPostFiler.DataSource!=null)
        {
            if (PersonalView.PersonalPostFiler.DataSource.Count > 1)
            {
                PersonalView.MainRect.onValueChanged.RemoveListener(MainScrollRectListener);
                PersonalView.PersonalPostFiler.ScrollView.onValueChanged.RemoveListener(ListScrollRectListener);
            }
            PersonalView.PersonalPostFiler.DataSource.Clear();
            PersonalView.PersonalPostFiler.DataSource = null;
        }
        OnRemove();
    }
}
