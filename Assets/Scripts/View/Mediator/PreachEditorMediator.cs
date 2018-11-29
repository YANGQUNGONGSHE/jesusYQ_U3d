using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachEditorMediator : EventMediator
{
    [Inject]
    public PreachEditorView PreachEditorView { get; set; }
    
    [Inject]
    public UserModel UserModel { get; set; }
    
    private string _webKey = "eleditor";

    public override void OnRegister()
    {
        PreachEditorView.BackBut.onClick.AddListener(BackButClick);
        PreachEditorView.PublishBut.onClick.AddListener(PublishButClick);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.PublishTextImage, OnClickPublishTextImage);
        WebController.Instance.OnMessageReceived += OnWebMessageReceived;
        CreateWebView();
    }
    
    private void OnClickPublishTextImage(Notification notification)
    {
        CreateWebView();
    }
    
    private void CreateWebView()
    {
        //var oUrl = UniWebViewHelper.StreamingAssetURLForPath("www/eleditor.html");
        var oUrl = Web.Url + "/eleditor.html";
        WebController.Instance.CreateWeb(_webKey, PreachEditorView.WebRect, oUrl, false, (web, code, url) =>
        {
            if (code == 200)
            {
#if !EDITOR_MAC
                var ssid = UserModel.SessionId.Ssid;
                string jsCode = String.Format("getUnityData('{0}');", ssid);
                WebController.Instance.RunJavaScript(_webKey, jsCode,
                    delegate(UniWebViewNativeResultPayload payload)
                    {
                        Log.I("调用getUnityData():" + payload.resultCode);
                    });
#endif
                //WebController.Instance.Show(_webKey);
            }
        });
    }
    
    private void OnWebMessageReceived(UniWebView view, UniWebViewMessage msg)
    {
        if (msg.Path.Equals("preach_publish"))
        {
            var publishState = msg.Args["succ"].ToInt();
            if (publishState == 0)
            {
                string errMsg = msg.Args["msg"];
                Log.E(errMsg);
            }
            else
            {
                WebController.Instance.CloseWeb(_webKey);
                Log.I("发布成功!");
                NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishPreachSucc, this);
                //iocViewManager.CloseCurrentOpenNew((int) UiId.Preach);
                iocViewManager.DestroyAndOpenNew(PreachEditorView.GetUiId(),UserModel.AboutEditorUid);
            }
        }
    }

    #region Click Event

    private bool _mPublishBtnClickDisable;
    private void PublishButClick()
    {
        if (!_mPublishBtnClickDisable)
        {
            _mPublishBtnClickDisable = true;
            WebController.Instance.RunJavaScript(_webKey, "send();", payload =>
            {
                Log.I("调用send():" + payload.resultCode);
            });
            StartCoroutine(SetPublishBtnClickable());
        }
    }

    IEnumerator SetPublishBtnClickable()
    {
        yield return new WaitForSeconds(0.5f);
        _mPublishBtnClickDisable = false;
    }

    private void BackButClick()
    {
        WebController.Instance.CloseWeb(_webKey);
        iocViewManager.DestroyAndOpenNew(PreachEditorView.GetUiId(), UserModel.AboutEditorUid);
    }

    #endregion

    public override void OnRemove()
    {
        PreachEditorView.BackBut.onClick.RemoveListener(BackButClick);
        PreachEditorView.PublishBut.onClick.RemoveListener(PublishButClick);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.PublishTextImage, OnClickPublishTextImage);
        WebController.Instance.OnMessageReceived -= OnWebMessageReceived;
    }

    private void OnDestroy()
    {
        OnRemove();
    }
}