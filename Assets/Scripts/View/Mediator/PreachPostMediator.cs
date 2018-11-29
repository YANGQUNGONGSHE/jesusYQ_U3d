using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.SimpleUIManager;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachPostMediator : EventMediator {
    
    [Inject]
    public PreachPostView PreachPostView { get; set; }
    
    [Inject]
    public UserModel UserModel { get; set; }
    private FromViewType _mFromViewType;
    private PostModel _postInfo;
    private string _webKey = "index";
    private bool _mIsFocus;
    private bool _mIsSelf;
    private bool _mIsLike;
    private bool _mIsCheckedWebViewClose;
    private bool _mStartCheckWebView;

    void Update()
    {
        if (_mStartCheckWebView)
        {
            if (!_mIsCheckedWebViewClose)
            {
                if (!WebController.Instance.hasExist(_webKey))
                {
                    _mIsCheckedWebViewClose = true;
                    Back();
                }
            }
        }
    }

    /// <summary>
    /// 路径地址 =>家俊哥加的
    /// </summary>
    private readonly List<string> _paths = new List<string>()
    {
        "index.html",
        "reply.html"
    };
    
    /// <summary>
    /// 家俊哥加的 => 你懂的
    /// </summary>
    private readonly List<string> _titleName = new List<string>()
    {
        "正文内容",
        "更多评论"
    };

    /// <summary>
    /// 页面枚举 =>家俊哥加的
    /// </summary>
    private enum WebPage
    {
        Content,
        Comment,
    }
    
    /// <summary>
    /// 当前页面 =>家俊哥加的
    /// </summary>
    private WebPage _mCurrentPage = WebPage.Content;

    public override void OnRegister()
    {
        LoadData();
        PreachPostView.BackButton.onClick.AddListener(Back);
        PreachPostView.ShareButton.onClick.AddListener(OpenShareBar);
        PreachPostView.CancelShareButton.onClick.AddListener(CancelShare);
        PreachPostView.CommentButton.onClick.AddListener(CommentClick);
        PreachPostView.TopExpandButton.onClick.AddListener(TopExpandClick);
        PreachPostView.FocusBut.onClick.AddListener(FocusClick);
        PreachPostView.DeOrRpBut.onClick.AddListener(DeletePostClick);
        PreachPostView.CancelActionBut.onClick.AddListener(CancelActionClick);
        PreachPostView.LikeButton.onClick.AddListener(LikeClick);
        dispatcher.AddListener(CmdEvent.ViewEvent.DeletePostFinish,DeletePostFinish);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.SetCurrenWebIndexIsContent, OnSetCurrentWeb);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.ReloadIndexandScroll,OnReloadPageAndScroll);
        WebController.Instance.OnMessageReceived += OnWebMessageReceived;
    }

    private void LoadData()
    {
        _postInfo = UserModel.PostDetailModel;
        _mFromViewType = _postInfo.FromType;
        OnLoadWebPage(_postInfo.Id);
        Judgestatus();
    }

    #region Event Click
    private void LikeClick()
    {
        if (_mIsLike)
        {
            dispatcher.Dispatch(CmdEvent.Command.PostInteraction, new InteractionInfo()
            {
                Type = InteractionType.DisLike,
                ParentId = _postInfo.Id
            });
            _mIsLike = false;
            PreachPostView.IsLike(false);
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.PostInteraction, new InteractionInfo()
            {
                Type = InteractionType.Like,
                ParentType = "帖子",
                ParentId = _postInfo.Id
            });
            _mIsLike = true;
            PreachPostView.IsLike(true);
        }
        UserModel.PostDetailModel.IsLike = _mIsLike;
    }
    private void CancelActionClick()
    {
       PreachPostView.IsVisibleActionRec(false);
    }

    private void DeletePostClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqPreach, new PreachPostInfo
        {
            Type = PostType.DeletePost,
            PostId = _postInfo.Id
        });
        PreachPostView.IsVisibleActionRec(false);
    }

    private void FocusClick()
    {
        if (_mIsSelf)
        {
            PreachPostView.IsVisibleActionRec(true);
        }
        else
        {
            if (_mIsFocus)
            {
               Log.I("已经关注过啦"); 
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.Command.FocusOptions, new FocusOptionInfo()
                {
                    Options = FocusOptions.AddFcous,
                    Id = _postInfo.Author.Id.ToString()
                });
                _mIsFocus = true;
                PreachPostView.IsFocused(true);
            }
        }
    }

    private void TopExpandClick()
    {
        UserModel.ReportedUserModel = new ReportedUserModel()
        {
            Uid = _postInfo.Author.Id.ToString(),
            UserName = _postInfo.Author.UserName,
            DisplyName = _postInfo.Author.DisplayName,
            HeadTexture2D = _postInfo.HeadTexture2D,
            HeadUrl = _postInfo.Author.AvatarUrl,
            Signature = _postInfo.Author.Signature,
            ParentId = _postInfo.Id,
            ReportType = ReportType.Post,
            FromReportViewType = FromReportViewType.PreachPost
        };
        WebController.Instance.CloseWeb(_webKey);
        iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(),(int)UiId.Report);
    }

    private void DeletePostFinish()
    {
        UIUtil.Instance.ShowTextToast("已删除成功");
        this.Back();
        NotificationCenter.DefaultCenter().PostNotification(
            _mFromViewType == FromViewType.FromPreachView
                ? NotifiyName.DeletePreachSucc
                : NotifiyName.DeletePersonalPreachSucc, this);
    }
    #endregion

    private void OnReloadPageAndScroll(Notification notification)
    {
        // 刷新
        WebController.Instance.Reload(_webKey);
        // 滚动到评论
        CoroutineController.Instance.StartCoroutine(ScrollerPage());
    }

    IEnumerator ScrollerPage()
    {
        yield return new  WaitForSeconds(1f);
        WebController.Instance.RunJavaScript(_webKey, "scrollToComment();", delegate(UniWebViewNativeResultPayload payload)
        {
            Log.I( payload.resultCode);
        });
    }

    private void OnSetCurrentWeb(Notification notification)
    {
        _mCurrentPage = WebPage.Content;
    }

    private void CommentClick()
    {
        iocViewManager.CloseCurrentOpenNew((int)UiId.PreachEditorCommnet);
        StartCoroutine(SendInfo(CmdEvent.ViewEvent.OpenPreachCommentView));
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishComment,this);
    }

    IEnumerator SendInfo(CmdEvent.ViewEvent tyEvent)
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(tyEvent, _postInfo);
    }

    // ********************************* 家俊哥加的 ***************************************
    private void OnLoadWebPage(string postId)
    {
        //var oUrl = UniWebViewHelper.StreamingAssetURLForPath("www/index.html"); 
        var oUrl = Web.Url + "/index.html";
        WebController.Instance.CreateWeb(_webKey, PreachPostView.WebRect, oUrl, false, (web, code, url)=>
        {
            if (!string.IsNullOrEmpty(url))
            {
                int index1 = url.LastIndexOf('/');
                string page;
                if (url.Contains("?"))
                {
                    var index2 = url.IndexOf('?') - 1;
                    var length = index2 - index1;
                    page = url.Substring(index1 + 1, length);
                }
                else
                {
                    page = url.Substring(index1 + 1);
                }
                int pageIndex = _paths.IndexOf(page);
                _mCurrentPage = (WebPage)pageIndex;
                PreachPostView.SetNavTitle(_titleName[pageIndex]);
            }
            
            if (code == 200)
            {
                if (_mCurrentPage == WebPage.Content)
                {
                    if (!string.IsNullOrEmpty(postId))
                    {
                        var ssid = UserModel.SessionId.Ssid;
                        string jsCode = String.Format("getUnityData('{0}','{1}');", ssid, postId);
                        WebController.Instance.RunJavaScript(_webKey, jsCode, delegate(UniWebViewNativeResultPayload payload)
                        {
                            Log.I(payload.resultCode);
                        });
                    }
                }
                else if (_mCurrentPage == WebPage.Comment)
                {
                    var ssid = UserModel.SessionId.Ssid;
                    string jsCodeSid = String.Format("getUnityData('{0}');", ssid);
                    WebController.Instance.RunJavaScript(_webKey, jsCodeSid, delegate(UniWebViewNativeResultPayload payload)
                    {
                        Log.I(payload.resultCode);
                    });
                }
            }
        });
        
//#if UNITY_ANDROID
//        CoroutineController.Instance.StartCoroutine(Android());
//#else
//        WebController.Instance.Show(_webKey);
//#endif
        WebController.Instance.Show(_webKey);
    }

    IEnumerator Android()
    {
        yield return new WaitForSeconds(0.1f);
        WebController.Instance.Reload(_webKey);
        _mStartCheckWebView = true;
    }

    private void OnWebMessageReceived(UniWebView view , UniWebViewMessage msg)
    {
        if (msg.Path.Equals("preach_changePage")) {
            var pageEnum = msg.Args["pageEnum"].ToInt();
            _mCurrentPage = (WebPage)pageEnum;
            Log.I("current page = " + _mCurrentPage);
            if (string.IsNullOrEmpty(msg.Args["commentId"]))
                return;
            UserModel.TempCommentId = msg.Args["commentId"];
            UserModel.TempCommentName = msg.Args["commentName"];
            iocViewManager.CloseCurrentOpenNew((int)UiId.PreachReplyComment);
            NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishReply,this);
        }
        else if (msg.Path.Equals("preach_debug_getUnityData"))
        {
            Log.I("收到web回调->收到unity传给web的SSID为：" + msg.Args["ssid"] + "POSTID为:" + msg.Args["postId"]);
        }
        else if (msg.Path.Equals("preach_debug_fetchPost"))
        {
            Log.I("Web加载Post->：" + msg.Args["fetchPost"]);
        }
        else if (msg.Path.Equals("preach_debug_fetchLikes"))
        {
            Log.I("Web加载Likes->：" + msg.Args["fetchLikes"]);
        }
        else if (msg.Path.Equals("preach_debug_fetchComments"))
        {
            Log.I("Web加载Comments->：" + msg.Args["fetchComments"]);
        }
    }

    private void Judgestatus()
    {
        if (UserModel.User.Id == _postInfo.Author.Id)
        {
            PreachPostView.IsSelf(true);
            _mIsSelf = true;
        }
        else
        {
            if (UserModel.Follows.ContainsKey(_postInfo.Author.Id.ToString()))
            {
                PreachPostView.IsFocused(true);
                _mIsFocus = true;
            }
            else
            {
                PreachPostView.IsFocused(false);
            }
        }
        if (_postInfo.IsLike)
        {
            Log.I("IsLikeIsLike");
            PreachPostView.IsLike(true);
            _mIsLike = true;
        }
        else
        {
            Log.I("DisLikeDisLike");
            PreachPostView.IsLike(false);
            _mIsLike = false;
        }
    }
    private void CancelShare()
    {
        PreachPostView.VisibleShareBar(false);
    }
    private void OpenShareBar()
    {
        PreachPostView.VisibleShareBar(true);
    }

    private void Back()
    {   
        //=>家俊哥加的,if里面是亮亮自己的
        Log.I("xxxxxxxxx=>" + _mCurrentPage);
        if (_mCurrentPage == WebPage.Content)
        {
            Log.I("准备销毁");
            Log.I("_webKey" + _webKey);
            Log.I("WebController" + WebController.Instance);
            WebController.Instance.CloseWeb(_webKey);
            switch (_mFromViewType)
            {
                case FromViewType.FromPreachView:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(),(int)UiId.Preach);
                    break;
                case FromViewType.FromPersonalView:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(),(int)UiId.Personal);
                    break;
                case FromViewType.FromPreachSearchView:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(),(int)UiId.PreachSearch);
                    break;
                case FromViewType.FromLikePostView:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(),(int)UiId.MyLikePosts);
                    break;
                case FromViewType.FromSysCustomLikeView:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(), (int)UiId.SysCustomLike);
                    break;
                case FromViewType.FromSysCutomCustomView:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(), (int)UiId.SysCustomComment);
                    break;
                case FromViewType.FromNotification:
                    iocViewManager.DestroyAndOpenNew(PreachPostView.GetUiId(), UserModel.NowUid);
                    break;
            }
        }
        else //=>家俊哥加的
        {
            WebController.Instance.GoBack(_webKey);
        }
    }

    public override void OnRemove()
    {
        PreachPostView.BackButton.onClick.RemoveListener(Back);
        PreachPostView.ShareButton.onClick.RemoveListener(OpenShareBar);
        PreachPostView.CancelShareButton.onClick.RemoveListener(CancelShare);
        PreachPostView.CommentButton.onClick.RemoveListener(CommentClick);
        PreachPostView.TopExpandButton.onClick.RemoveListener(TopExpandClick);
        PreachPostView.FocusBut.onClick.RemoveListener(FocusClick);
        PreachPostView.DeOrRpBut.onClick.RemoveListener(DeletePostClick);
        PreachPostView.CancelActionBut.onClick.RemoveListener(CancelActionClick);
        PreachPostView.LikeButton.onClick.RemoveListener(LikeClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.DeletePostFinish, DeletePostFinish);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.SetCurrenWebIndexIsContent, OnSetCurrentWeb);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.ReloadIndexandScroll,OnReloadPageAndScroll);
        WebController.Instance.OnMessageReceived -= OnWebMessageReceived;
    }

    private void OnDestroy()
    {
        OnRemove();
    }
}

