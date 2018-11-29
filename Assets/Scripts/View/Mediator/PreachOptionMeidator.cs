using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachOptionMeidator : EventMediator {

    [Inject]
    public PreachOptionView PreachOptionView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    private bool _mIsHasTips = false;

    public override void OnRegister()
    {
        PreachOptionView.ArticleButton.onClick.AddListener(ArticleOption);
        PreachOptionView.VideoButton.onClick.AddListener(VideoOption);
        PreachOptionView.CancelButton.onClick.AddListener(CancelPublish);
    }

    private void CancelPublish()
    {
        //iocViewManager.DestroyAndOpenNew(PreachOptionView.GetUiId(),(int)UiId.Preach);
        iocViewManager.DestroyAndOpenNew(PreachOptionView.GetUiId(),UserModel.AboutEditorUid);
    }
    
    /// <summary>
    /// 视频编辑
    /// </summary>
    private void VideoOption()
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishVideo, this);
        iocViewManager.DestroyAndOpenNew(PreachOptionView.GetUiId(), (int)UiId.PreachEditorVideo);
    }
    /// <summary>
    /// 图文编辑
    /// </summary>
    private void ArticleOption()
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishTextImage, this);
        iocViewManager.DestroyAndOpenNew(PreachOptionView.GetUiId(),(int)UiId.PreachEditor);
    }

    public override void OnRemove()
    {
        PreachOptionView.ArticleButton.onClick.RemoveListener(ArticleOption);
        PreachOptionView.VideoButton.onClick.RemoveListener(VideoOption);
        PreachOptionView.CancelButton.onClick.RemoveListener(CancelPublish);
    }
    
    private void OnDestroy()
    {
        OnRemove();
    }

}
