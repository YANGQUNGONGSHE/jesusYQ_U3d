using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class MyLikePostsMediator : EventMediator {


    [Inject]
    public MyLikePostsView MyLikePostsView { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private List<PostModel> _postModels;
    private PostModel mPostModel;
    public override void OnRegister()
    {
        LoadData();
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadLikesPostsByUserFinish,LoadLikesPostsByUserFinish);
        MyLikePostsView.BackBut.onClick.AddListener(BackClick);
        MyLikePostsView.LikePostsFiler.ScrollView.onValueChanged.AddListener(ScrollViewListener);
        MyLikePostsView.LikePostsFiler.OnCellClick = OnCellClick;
        MyLikePostsView.LikePostsFiler.ClickTypeCallBack = ClickTypeCallBack;
    }

    private void LoadData()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadLikesPostsByUser, new ReqLikepostsInfo
        {
            Skip = 0,
            Limit = 20,
            IsRefresh = true
        });
    }
    #region Click Event

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
                postModel.FromType = FromViewType.FromLikePostView;
                UserModel.PostModel = postModel;
                iocViewManager.DestroyAndOpenNew(MyLikePostsView.GetUiId(),(int)UiId.Personal);
                break;
        }

    }
    private void OnCellClick(int index, PostModel postModel)
    {
        OpenView(postModel, UiId.PreachPost, CmdEvent.ViewEvent.OpenPreachPostView);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(MyLikePostsView.GetUiId(),(int)UiId.Me);
    }
    #endregion

    private void OpenView(PostModel model, UiId id, CmdEvent.ViewEvent eViewEvent)
    {
        mPostModel = model;
        if (mPostModel == null) return;
        mPostModel.FromType = FromViewType.FromLikePostView;
        UserModel.PostDetailModel = mPostModel;
        iocViewManager.CloseCurrentOpenNew((int)id);
    }

    private void ScrollViewListener(Vector2 arg0)
    {
        if (MyLikePostsView.LikePostsFiler.ScrollView.normalizedPosition.y >= 1.2f && Input.GetMouseButtonUp(0))
        {
            Log.I("刷新数据");
            RefreshData();
        }

        if (MyLikePostsView.LikePostsFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
        {
            if(MyLikePostsView.LikePostsFiler.DataSource.Count<2)return;
            Log.I("加载数据");
            dispatcher.Dispatch(CmdEvent.Command.LoadLikesPostsByUser, new ReqLikepostsInfo
            {
                Skip = MyLikePostsView.LikePostsFiler.DataSource.Count,
                Limit = 20,
                IsRefresh = false
            });
        }
    }

    private void RefreshData()
    {
        var limit = 20;
        if (MyLikePostsView.LikePostsFiler.DataSource.Count >= 20)
        {
            limit = MyLikePostsView.LikePostsFiler.DataSource.Count;
        }

        dispatcher.Dispatch(CmdEvent.Command.LoadLikesPostsByUser, new ReqLikepostsInfo
        {
            Skip = 0,
            Limit = limit,
            IsRefresh = true
        });
    }

    private void LoadLikesPostsByUserFinish(IEvent eEvent)
    {
        UIUtil.Instance.CloseWaiting();
        var postData = (ReqLikepostsInfo)eEvent.data;
        if (_postModels == null)
        {
            _postModels = new List<PostModel>();
        }
        if (postData.IsRefresh)
        {
            _postModels.Clear();
        }

        //if (postData.PostModels == null)
        //{
        //    MyLikePostsView.IsVisibleScrollRect(_postModels.Count >= 1);
        //}
        //else
        //{
        //    MyLikePostsView.IsVisibleScrollRect(true);
        //}
        if (postData.PostModels != null)
        {
            for (var i = 0; i < postData.PostModels.Count; i++)
            {
               _postModels.Add(postData.PostModels[i]);
            }
        }
        MyLikePostsView.IsVisibleScrollRect(_postModels.Count >= 1);
        MyLikePostsView.LikePostsFiler.DataSource = _postModels;
        MyLikePostsView.LikePostsFiler.Refresh();
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadLikesPostsByUserFinish, LoadLikesPostsByUserFinish);
        MyLikePostsView.BackBut.onClick.RemoveListener(BackClick);
        MyLikePostsView.LikePostsFiler.ScrollView.onValueChanged.RemoveListener(ScrollViewListener);
        MyLikePostsView.LikePostsFiler.OnCellClick -= OnCellClick;
        MyLikePostsView.LikePostsFiler.ClickTypeCallBack -= ClickTypeCallBack;
    }

    private void OnDestroy()
    {
        if (_postModels != null)
        {
            _postModels.Clear();
        }
        if (MyLikePostsView.LikePostsFiler.DataSource != null)
        {
            MyLikePostsView.LikePostsFiler.DataSource.Clear();
            MyLikePostsView.LikePostsFiler.DataSource = null;
        }
        OnRemove();
    }


}
