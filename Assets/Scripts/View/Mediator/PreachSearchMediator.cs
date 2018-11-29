using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachSearchMediator : EventMediator {

    [Inject]
    public PreachSearchView PreachSearchView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private PostModel _mPost;

    public override void OnRegister()
    {
       dispatcher.AddListener(CmdEvent.ViewEvent.ReqPreachSearchSucc,ReqPreachSearchSucc);
       PreachSearchView.SearchInputField.onValueChanged.AddListener(SearchInputFieldListener);
       PreachSearchView.BackBut.onClick.AddListener(BackClick);
       PreachSearchView.PreachSearchFiler.OnCellClick = OnCellClick;
       PreachSearchView.PreachSearchFiler.ClickTypeCallBack = ClickTypeCallBack;
       PreachSearchView.IsVisibleScrollRect(false);
    }

    private void ClickTypeCallBack(ClickType clickType,int index, PostModel postModel)
    {
        switch (clickType)
        {
            case ClickType.Comment:
                OpenView(postModel, UiId.PreachPost, CmdEvent.ViewEvent.OpenPreachPostView);
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
                //postModel.FromType = FromViewType.FromPreachSearchView;
                //UserModel.PostModel = postModel;
                //iocViewManager.DestroyAndOpenNew(PreachSearchView.GetUiId(),(int)UiId.Personal);
                break;
        }
    }

    private void OnCellClick(int index, PostModel postModel)
    {
        OpenView(postModel, UiId.PreachPost, CmdEvent.ViewEvent.OpenPreachPostView);
    }

    private void OpenView(PostModel model, UiId id, CmdEvent.ViewEvent eViewEvent)
    {
        _mPost = model;
        if (_mPost == null) return;
        _mPost.FromType = FromViewType.FromPreachSearchView;
        UserModel.PostDetailModel = _mPost;
        iocViewManager.CloseCurrentOpenNew((int)id);
    }
    private void BackClick()
    {
        if (PreachSearchView.PreachSearchFiler.DataSource != null)
        {
            PreachSearchView.PreachSearchFiler.DataSource.Clear();
            PreachSearchView.PreachSearchFiler.DataSource = null;
        }
        switch (UserModel.PostSearchType)
        {
            case PostSearchType.PearchBrowse:
                iocViewManager.DestroyAndOpenNew(PreachSearchView.GetUiId(),(int)UiId.Preach);
                break;
            case PostSearchType.Personal:
                iocViewManager.DestroyAndOpenNew(PreachSearchView.GetUiId(),(int)UiId.Personal);
                break;
        }
    }

    private void SearchInputFieldListener(string arg0)
    {
        if (!string.IsNullOrEmpty(PreachSearchView.SearchInputField.text))
        {
            dispatcher.Dispatch(CmdEvent.Command.ReqPreach, new PreachPostInfo
            {
                Type = PostType.Search,
                Skip = 0,
                Limit = 20,
                SearchContent = PreachSearchView.SearchInputField.text
            });
        }
        else
        {
            PreachSearchView.IsVisibleScrollRect(false);
            PreachSearchView.PreachSearchFiler.DataSource.Clear();
            PreachSearchView.PreachSearchFiler.Refresh();
        }
    }

    private void ReqPreachSearchSucc(IEvent eEvent)
    {
        var postData = eEvent.data as PreachPostInfo;
        if(postData==null) return;
        PreachSearchView.PreachSearchFiler.DataSource.Clear();
        if (postData.PostModels == null || postData.PostModels.Count < 1)
        {
            PreachSearchView.IsVisibleScrollRect(false);
        }
        else
        {
            PreachSearchView.IsVisibleScrollRect(true);
        }
        PreachSearchView.PreachSearchFiler.DataSource = postData.PostModels;
        PreachSearchView.PreachSearchFiler.Refresh();
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqPreachSearchSucc, ReqPreachSearchSucc);
        PreachSearchView.BackBut.onClick.RemoveListener(BackClick);
        PreachSearchView.PreachSearchFiler.OnCellClick -= OnCellClick;
        PreachSearchView.PreachSearchFiler.ClickTypeCallBack -= ClickTypeCallBack;
    }

    private void OnDestroy()
    {   
        OnRemove();
    }


}
