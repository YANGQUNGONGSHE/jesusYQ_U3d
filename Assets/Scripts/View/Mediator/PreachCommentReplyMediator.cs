using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachCommentReplyMediator : EventMediator {

    [Inject] public PreachCommentReplyView PreachCommentReplyView { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private PostModel _postModel;
    private string _webKey = "comment";
    
    public override void OnRegister()
    {
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.PublishReply, OnPublishReply);
        PreachCommentReplyView.BackBut.onClick.AddListener(BackClick);
        PreachCommentReplyView.SendBut.onClick.AddListener(SendClick);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqReplyCommentSucc, OnReplyCommentSucc);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqReplyCommentFail, OnReplyCommentFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.OpenPreachReplyCommentView, DataCallback);
        WebController.Instance.OnMessageReceived += OnWebMessageReceived;
        CreateWebView();
    }

    private void OnReplyCommentFail(IEvent evt)
    {
        UIUtil.Instance.ShowFailToast("评论失败,请重试");
    }

    private void OnReplyCommentSucc(IEvent evt)
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.ReloadIndexandScroll, this);
        BackClick();
        UIUtil.Instance.ShowSuccToast("评论成功");
    }

    private void DataCallback(IEvent eEvent)
    {
        _postModel = eEvent.data as PostModel;
    }
    
    private void OnPublishReply(Notification notification)
    {
        CreateWebView();
    }
    
    private void CreateWebView()
    {
        //var oUrl = UniWebViewHelper.StreamingAssetURLForPath("www/comment.html");
        var oUrl = Web.Url + "/comment.html";
        WebController.Instance.CreateWeb(_webKey, PreachCommentReplyView.WebRect, oUrl, false, (web, code, url) =>
        {
            if (code == 200)
            {
                string jsCode = String.Format("setPlaceholder('{0}');", UserModel.TempCommentName);
                WebController.Instance.RunJavaScript(_webKey, jsCode, delegate(UniWebViewNativeResultPayload payload)
                {
                    Log.I(payload.resultCode);
                });
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
                Type = InteractionType.ReplyComment,
                ParentType = "评论",
                ParentId = UserModel.TempCommentId,
                Content = content,
            });
        }
    }

    #region Event Click

    private void SendClick()
    {
        WebController.Instance.RunJavaScript(_webKey, "submit();", delegate(UniWebViewNativeResultPayload payload)
        {
            Log.I(payload.resultCode);
        });
    }

    private void BackClick()
    {
        WebController.Instance.CloseWeb(_webKey);
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.SetCurrenWebIndexIsContent,this);
        iocViewManager.DestroyAndOpenNew(PreachCommentReplyView.GetUiId(),(int)UiId.PreachPost);
    }

    #endregion

    public override void OnRemove()
    {
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.PublishReply, OnPublishReply);
        PreachCommentReplyView.BackBut.onClick.RemoveListener(BackClick);
        PreachCommentReplyView.SendBut.onClick.RemoveListener(SendClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqReplyCommentSucc, OnReplyCommentSucc);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqReplyCommentFail, OnReplyCommentFail);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenPreachReplyCommentView, DataCallback);
        WebController.Instance.OnMessageReceived -= OnWebMessageReceived;
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
