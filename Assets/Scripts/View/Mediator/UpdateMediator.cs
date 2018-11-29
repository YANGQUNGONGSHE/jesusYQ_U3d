using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using UnityEngine.SceneManagement;
using WongJJ.Game.Core;

public class UpdateMediator : EventMediator
{
    [Inject]
    public UpdateView UpdateView{get;set;}

    public override void OnRegister()
    {

        NetStatus();
        CheckUpdate();
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqLoginImFail,LoginImFailListener);
        UpdateView.QuitAppBut.onClick.AddListener(QuitAppClick);
    }

    private void QuitAppClick()
    {
        Application.Quit();
    }

    private void LoginImFailListener()
    {
        UpdateView.IsVisibleErrorAction(true);
    }

    private void NetStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIUtil.Instance.ShowTextToast("当前网络连接不可用");
        }
    }
    public override void OnRemove()
    {
        DownloadManager.Instance.OnProgress -= OnCheckAssetProgressCallback;
        DownloadManager.Instance.OnNoAssetUpdate -= OnNoAssetUpdate;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqLoginImFail, LoginImFailListener);
        UpdateView.QuitAppBut.onClick.RemoveListener(QuitAppClick);
    }

    private void OnDestroy()
    {
        OnRemove();
    }

    private void CheckUpdate()
    {
        DownloadManager.Instance.OnProgress = OnCheckAssetProgressCallback;
        DownloadManager.Instance.OnNoAssetUpdate = OnNoAssetUpdate;
        DownloadManager.Instance.InitAssetBundle(() =>
        {
            dispatcher.Dispatch(CmdEvent.Command.UpdateFinish);
        });
    }

    private void OnCheckAssetProgressCallback(int finishCount, int totalCount)
    {
        UpdateView.SetProgress(finishCount / (float)totalCount);
    }

    private void OnNoAssetUpdate(bool yes)
    {
        if (yes)
        {
            dispatcher.Dispatch(CmdEvent.Command.UpdateFinish);
        }
    }
}
