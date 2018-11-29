using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class FeedBackMediator : EventMediator {

    [Inject]
    public FeedBackView FeedBackView { get; set; }
    public override void OnRegister()
    {
       FeedBackView.BackBut.onClick.AddListener(BackClick);
       FeedBackView.CommitBut.onClick.AddListener(CommitClick);
       
    }

    private void CommitClick()
    {
        if (!string.IsNullOrEmpty(FeedBackView.InputField.text))
        {
            dispatcher.Dispatch(CmdEvent.Command.ReqCreateFeedBack, FeedBackView.InputField.text);
            iocViewManager.DestroyAndOpenNew(FeedBackView.GetUiId(), (int)UiId.Setting);
        }
        else
        {
            UIUtil.Instance.ShowTextToast("反馈内容不能为空哟");
        }
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(FeedBackView.GetUiId(),(int)UiId.Setting);
    }

    public override void OnRemove()
    {
        FeedBackView.BackBut.onClick.RemoveListener(BackClick);
        FeedBackView.CommitBut.onClick.RemoveListener(CommitClick);
    }
    private void OnDestroy()
    {
        OnRemove();
    }


}
