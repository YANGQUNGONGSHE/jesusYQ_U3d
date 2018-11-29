using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachMeidator : EventMediator {

    [Inject]
    public PreachView PreachView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private List<PostModel> _mNewModels;
    private PostModel _mPost;
    private PreachPostInfo _mPreachPostInfo;
    private PostType _mPostType;
    private int _mIndex = 0;

    public override void OnRegister()
    {

#if !EDITOR_MAC
		Log.I("开始请求好友");
        dispatcher.Dispatch(CmdEvent.Command.ReqFriends);
        dispatcher.Dispatch(CmdEvent.Command.ReqBlackData);
#endif
       dispatcher.Dispatch(CmdEvent.Command.LoadAccountHeadT2D, UserModel.User.AvatarUrl);
	   Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
        _mPostType = PostType.New;
        dispatcher.Dispatch(CmdEvent.Command.ReqPreach,new PreachPostInfo
        {
            Type  = PostType.New,
            Skip = 0,
            Limit = 20
        });
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqPreachSucc, PreachData);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqPreachFail,ReqPreachFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.BlockPostFinish,BlockPostFinish);
        PreachView.HotPostFiler.ScrollView.onValueChanged.AddListener(HotScrollRectListener);
        PreachView.HotPostFiler.OnCellClick = OnCellClick;
        PreachView.HotPostFiler.ClickTypeCallBack = ClickTypeCallBack;
        PreachView.PublishOption.onClick.AddListener(SearchClick);
        PreachView.BlockPostBut.onClick.AddListener(BlockPostClick);
        PreachView.ReportBut.onClick.AddListener(ReportPostClick);
        PreachView.BlockUserBut.onClick.AddListener(BlockAuthorClick);
        PreachView.ActionBut.onClick.AddListener(ActionBgClick);
        PreachView.HotPostFiler.ScrollView.movementType = ScrollRect.MovementType.Clamped;
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.PublishPreachSucc, OnPublishPreachSucc);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.DeletePreachSucc,DeletePreachSucc);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.RefreshPostData,RefreshPostEvent);
    }

    /// <summary>
    /// 请求刷新数据
    /// </summary>
    /// <param name="notification"></param>
    private void RefreshPostEvent(Notification notification)
    {
        PreachView.HotPostFiler.ScrollToTop(false);
        UIUtil.Instance.ShowWaiting();
        CoroutineController.Instance.StartCoroutine(ForceRefresh());
    }

    private void OnPublishPreachSucc(Notification notification)
    {
        CoroutineController.Instance.StartCoroutine(ForceRefresh());
    }

    private void DeletePreachSucc(Notification notification)
    {
        CoroutineController.Instance.StartCoroutine(ForceRefresh());
    }

    IEnumerator ForceRefresh()
    {
        yield return new WaitForSeconds(1f);
        ReqRefreshData();
    }

    private void ReqPreachFail(IEvent evt)
    {
        UIUtil.Instance.CloseWaiting();
        UIUtil.Instance.ShowFailToast(evt.data as string);
    }

    private void PreachData(IEvent evt)
    {
        _mPreachPostInfo = evt.data as PreachPostInfo;
        if (_mPreachPostInfo == null) return;
        if (_mPreachPostInfo.PostModels.Count <= 0)
        {
            PreachView.LoadStatus(true);
        }
        if (_mPreachPostInfo.IsRefresh)
        {
            Log.I("刷新的数据返回！！！！！");
            UIUtil.Instance.CloseWaiting();
            PreachView.LoadStatus(false);
          
            if (_mNewModels != null)
            {
                _mNewModels.Clear();
                _mNewModels = null;
            }
            PreachView.HotPostFiler.DataSource.Clear();
            PreachView.HotPostFiler.DataSource = null;
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        if (_mNewModels == null)
        {
            _mNewModels = new List<PostModel>();
        }
        for (var i = 0; i < _mPreachPostInfo.PostModels.Count; i++)
        {
            _mNewModels.Add(_mPreachPostInfo.PostModels[i]);
        }
        PreachView.HotPostFiler.DataSource = _mNewModels;
     
        PreachView.HotPostFiler.Refresh();
    }
    /// <summary>
    /// 屏蔽帖子成功回调
    /// </summary>
    private void BlockPostFinish()
    {
        PreachView.HotPostFiler.DataSource.RemoveAt(_mIndex);
        PreachView.HotPostFiler.Refresh();
    }

    /// <summary>
    /// 论道浏览帖子列表OnCellClick
    /// </summary>
    /// <param name="clickIndex"></param>
    /// <param name="postModel">帖子Model</param>
    private void OnCellClick(int clickIndex, PostModel postModel)
    {
        //点击跳转到论道正文界面 PreachPostMedaitor
        OpenView(postModel, UiId.PreachPost, CmdEvent.ViewEvent.OpenPreachPostView);
    }
    /// <summary>
    /// 举报帖子
    /// </summary>
    private void ReportPostClick()
    {
        UserModel.ReportedUserModel = new ReportedUserModel()
        {
            Uid = _mPost.Author.Id.ToString(),
            UserName = _mPost.Author.UserName,
            DisplyName = _mPost.Author.DisplayName,
            HeadTexture2D = _mPost.HeadTexture2D,
            HeadUrl = _mPost.Author.AvatarUrl,
            Signature = _mPost.Author.Signature,
            ParentId = _mPost.Id,
            ReportType = ReportType.Post,
            FromReportViewType = FromReportViewType.Preach
        };
        iocViewManager.CloseCurrentOpenNew((int)UiId.Report);
        PreachView.IsVisibleAction(false);
    }
    /// <summary>
    /// 屏蔽帖子
    /// </summary>
    private void BlockPostClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.BlockPostOptions,new BlockPostOptionInfo()
        {
            Options = BlockPostOptions.SetBlockPost,
            PostId = _mPost.Id
        });
        PreachView.IsVisibleAction(false);
    }
    /// <summary>
    /// 拉黑作者
    /// </summary>
    private void BlockAuthorClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqSetBlack, new ReqSetBlackInfo()
        {
            AccountId = _mPost.Author.Id.ToString(),
            IsBlack = true
        });
        PreachView.IsVisibleAction(false);
        PreachView.HotPostFiler.DataSource.RemoveAt(_mIndex);
        PreachView.HotPostFiler.Refresh();
    }

    private void ActionBgClick()
    {
        PreachView.IsVisibleAction(false);
    }

    private void ClickTypeCallBack(ClickType type,int index, PostModel postModel)
    {
        switch (type)
        {
            case ClickType.Comment:
                OpenView(postModel, UiId.PreachPost, CmdEvent.ViewEvent.OpenPreachPostView);
                break;
            case ClickType.Like:
                dispatcher.Dispatch(CmdEvent.Command.PostInteraction,new InteractionInfo()
                {
                    Type = InteractionType.Like,
                    ParentType = "帖子",
                    ParentId = postModel.Id
                });
                break;

            case ClickType.DisLike:
                dispatcher.Dispatch(CmdEvent.Command.PostInteraction,new InteractionInfo()
                {
                    Type = InteractionType.DisLike,
                    ParentId = postModel.Id
                });
                break;

            case ClickType.Share:
               
                break;
            case ClickType.OpenPersonal:
                postModel.FromType = FromViewType.FromPreachView;
                UserModel.PostModel = postModel;
                iocViewManager.CloseCurrentOpenNew((int)UiId.Personal);
                break;
            case ClickType.Block:
                PreachView.IsVisibleAction(true);
                PreachView.SetBlockAuthorName(postModel.Author.DisplayName);
                _mPost = postModel;
                _mIndex = index;
                break;
        }
    }

    private void OpenView(PostModel model,UiId id, CmdEvent.ViewEvent eViewEvent)
    {
        _mPost = model;
        if(_mPost==null)return;
        _mPost.FromType = FromViewType.FromPreachView;
        UserModel.PostDetailModel = _mPost;
        iocViewManager.CloseCurrentOpenNew((int)id);
    }

    private void HotScrollRectListener(Vector2 arg0)
    {
        if (PreachView.HotPostFiler.ScrollView.normalizedPosition.y <=0.0f)
        {
           Log.I("！！！！！！！加载更多帖子数据！！！！！！！！");
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                UIUtil.Instance.ShowTextToast("当前网络不可用");
                return;
            }
            if (PreachView.HotPostFiler.DataSource.Count < 2) return;
            dispatcher.Dispatch(CmdEvent.Command.ReqPreach, new PreachPostInfo()
            {
                Type = _mPostType,
                Skip = PreachView.HotPostFiler.DataSource.Count,
                Limit = 20
            });
        }
    }

    private void ReqRefreshData()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqPreach, new PreachPostInfo()
        {
            Type = _mPostType,
            Skip = 0,
            Limit = 20,
            IsRefresh = true
        });
        PreachView.HotPostFiler.DataSource.Clear();
    }

    private void SearchClick()
    {
        //搜索内容
        UserModel.PostSearchType = PostSearchType.PearchBrowse;
        iocViewManager.CloseCurrentOpenNew((int)UiId.PreachSearch);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqPreachSucc, PreachData);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqPreachFail, ReqPreachFail);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.BlockPostFinish, BlockPostFinish);
        PreachView.HotPostFiler.ScrollView.onValueChanged.RemoveListener(HotScrollRectListener);
        PreachView.HotPostFiler.OnCellClick -= OnCellClick;
        PreachView.HotPostFiler.ClickTypeCallBack -= ClickTypeCallBack;
        PreachView.PublishOption.onClick.RemoveListener(SearchClick);
        PreachView.BlockPostBut.onClick.RemoveListener(BlockPostClick);
        PreachView.ReportBut.onClick.RemoveListener(ReportPostClick);
        PreachView.BlockUserBut.onClick.RemoveListener(BlockAuthorClick);
        PreachView.ActionBut.onClick.RemoveListener(ActionBgClick);
        //家俊哥加的↓↓↓↓↓↓↓↓↓↓↓↓↓
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.PublishPreachSucc, OnPublishPreachSucc);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.DeletePreachSucc, DeletePreachSucc);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.RefreshPostData, RefreshPostEvent);
        _mPost = null;
    }

    private void OnDestroy()
    {
        OnRemove();
    }

}
