using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachEditorVideoMediator : EventMediator
{
    [Inject]
    public PreachEditorVideoView PreachEditorVideoView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    public override void OnRegister()
    {
        PreachEditorVideoView.BackBut.onClick.AddListener(BackButClick);
        PreachEditorVideoView.PublishBut.onClick.AddListener(PublishButClick);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.PublishVideo, OnClickPublishVideo);
        WebController.Instance.OnMessageReceived += OnWebMessageReceived;
        CreateWebView();
    }

    private void OnClickPublishVideo(Notification notification)
    {
        CreateWebView();
    }

    private void CreateWebView()
    {
        //var oUrl = UniWebViewHelper.StreamingAssetURLForPath("www/videoTextEleditor.html");
        var oUrl = Web.Url + "/videoTextEleditor.html";
        /*WebController.Instance.CreateWeb(PreachEditorVideoView.WebRect, oUrl, false, (web, code, url) =>
        {
            if (code == 200)
            {
                var ssid = UserModel.SessionId.Ssid;
                string jsCode = String.Format("getUnityData('{0}');", ssid);
                WebController.Instance.RunJavaScript(jsCode,
                    delegate(UniWebViewNativeResultPayload payload) { Log.I(payload.resultCode); });
                //WebController.Instance.Show();
            }
        });*/
    }

    private void OnWebMessageReceived(UniWebView view, UniWebViewMessage msg)
    {
        if (msg.Path.Equals("preach_publish"))
        {
            var publishState = msg.Args["succ"].ToInt();
            if (publishState == 0)
            {
                string errMsg = msg.Args["msg"];
                UIUtil.Instance.ShowFailToast(errMsg);
            }
            else
            {
                WebController.Instance.CloseWeb();
                UIUtil.Instance.ShowSuccToast("发布成功!");
                NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishPreachSucc, this);
                iocViewManager.CloseCurrentOpenNew((int) UiId.Preach);
            }
        }
    }

    #region Click Event

    private void PublishButClick()
    {
        WebController.Instance.RunJavaScript("getEditorHtml();", null);
    }

    private void BackButClick()
    {
        WebController.Instance.CloseWeb();
        iocViewManager.DestroyAndOpenNew(PreachEditorVideoView.GetUiId(), (int) UiId.PreachEditorOption);
    }

    #endregion

    public override void OnRemove()
    {
        PreachEditorVideoView.BackBut.onClick.RemoveListener(BackButClick);
        PreachEditorVideoView.PublishBut.onClick.RemoveListener(PublishButClick);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.PublishTextImage, OnClickPublishVideo);
        WebController.Instance.OnMessageReceived -= OnWebMessageReceived;
    }

    private void OnDestroy()
    {
        OnRemove();
    }
}