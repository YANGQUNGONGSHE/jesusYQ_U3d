using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachEditorCommantMediator : EventMediator
{
    [Inject]
    public PreachEditorCommentView PreachEditorCommentView { get; set; }

    private PostModel _postModel;
    private string _webKey = "comment";
    private bool _mIsCheckedWebViewClose = false;

    void Update()
    {
        if (!_mIsCheckedWebViewClose)
        {
            if (!WebController.Instance.hasExist(_webKey))
            {
                _mIsCheckedWebViewClose = true;
                BackClick();
            }
        }
    }

    public override void OnRegister()
    {
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.PublishComment, OnPublishComment);
        PreachEditorCommentView.BackBut.onClick.AddListener(BackClick);
        PreachEditorCommentView.SendBut.onClick.AddListener(SendButClick);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqCommentPreachSucc, OnSendCommentSucc);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqCommentPreachFail, OnSendCommentFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.OpenPreachCommentView, DataCallback);
        WebController.Instance.OnMessageReceived += OnWebMessageReceived;
        CreateWebView();
    }

    private void OnSendCommentFail(IEvent payload)
    {
        UIUtil.Instance.ShowFailToast("评论失败,请重试");
    }

    private void OnSendCommentSucc(IEvent evt)
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.ReloadIndexandScroll, this);
        BackClick();
        UIUtil.Instance.ShowSuccToast("评论成功");
    }

    private void OnPublishComment(Notification notification)
    {
        Log.I("OnPublishComment");
        CreateWebView();
    }

    private void DataCallback(IEvent eEvent)
    {
        _postModel = eEvent.data as PostModel;
    }

    private void CreateWebView()
    {
        //var oUrl = UniWebViewHelper.StreamingAssetURLForPath("www/comment.html");
        var oUrl = Web.Url + "/comment.html";
        WebController.Instance.CreateWeb(_webKey,PreachEditorCommentView.WebRect, oUrl, false, (web, code, url) =>
        {
            if (code == 200)
            {
                //WebController.Instance.Show(_webKey);
            }
        });
    }

    private void OnWebMessageReceived(UniWebView view, UniWebViewMessage msg)
    {
        if (msg.Path.Equals("preach_comment"))
        {
            var content = msg.Args["msg"];
            if (string.IsNullOrEmpty(content))
                return;
            dispatcher.Dispatch(CmdEvent.Command.PostInteraction, new InteractionInfo()
            {
                Type = InteractionType.Comment,
                ParentType = "帖子",
                ParentId = _postModel.Id,
                Content = content,
            });
        }
    }

    #region Click Event

    private void SendButClick()
    {
        WebController.Instance.RunJavaScript(_webKey, "submit();", delegate(UniWebViewNativeResultPayload payload)
        {
            Log.I(payload.resultCode);
        });
    }

    private void BackClick()
    {
        WebController.Instance.CloseWeb(_webKey);
        iocViewManager.DestroyAndOpenNew(PreachEditorCommentView.GetUiId(),(int) UiId.PreachPost);
    }

    #endregion

    public override void OnRemove()
    {
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.PublishComment, OnPublishComment);
        PreachEditorCommentView.BackBut.onClick.RemoveListener(BackClick);
        PreachEditorCommentView.SendBut.onClick.RemoveListener(SendButClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqCommentPreachSucc, OnSendCommentSucc);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqCommentPreachFail, OnSendCommentFail);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenPreachCommentView, DataCallback);
        WebController.Instance.OnMessageReceived -= OnWebMessageReceived;
    }

    private void OnDestroy()
    {
        OnRemove();
    }
}