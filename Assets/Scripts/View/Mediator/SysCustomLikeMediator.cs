using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class SysCustomLikeMediator : EventMediator {

    [Inject]public SysCutomLikeView SysCutomLikeView { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private List<SysCustomLikeModel> _mList;
    public override void OnRegister()
    {
        SysCutomLikeView.BackBut.onClick.AddListener(BackClick);
        SysCutomLikeView.SysLikeFiler.ScrollView.onValueChanged.AddListener(ScrollViewListener);
        SysCutomLikeView.SysLikeFiler.OnCellClick = OnCellClick;
        dispatcher.Dispatch(CmdEvent.Command.LoadSysCustomMs,new ReqSysCustomInfo
        {
            Type = SysCustomType.Like,
            Limt = 20,
            Skip = 0
        });
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSysLikeFinish,LoadSysLikeFinish);
        DeleteRecord();
    }

    private void OnCellClick(int index, SysCustomLikeModel sysCustomLikeModel)
    {
        var model = new PostModel()
        {
            FromType = FromViewType.FromSysCustomLikeView,
            Author = new User()
            {
                Id = sysCustomLikeModel.UserId.ToInt(),
                DisplayName = sysCustomLikeModel.UserDisplayName,
                UserName = sysCustomLikeModel.UserName,
                Signature = sysCustomLikeModel.Signature,
                AvatarUrl = sysCustomLikeModel.UserAvatarUrl,
            },
            Id = sysCustomLikeModel.PostId,
            PictureUrl = sysCustomLikeModel.PostPictureUrl,
            ContentType = sysCustomLikeModel.PostContentType,
            IsLike = true,
            Title = sysCustomLikeModel.PostTitle
        };
        UserModel.PostDetailModel = model;
        iocViewManager.CloseCurrentOpenNew((int)UiId.PreachPost);
    }

    private void DeleteRecord()
    {
        string sysLikePath = Application.persistentDataPath + "/" + LocalDataObjKey.SysLastLikeMsg;
        if (System.IO.File.Exists(sysLikePath))
        {
            System.IO.File.Delete(sysLikePath);
        }
        if (UserModel.LastSysCustomLikeMsg != null)
        {
            UserModel.LastSysCustomLikeMsg = null;
        }
    }
    private void ScrollViewListener(Vector2 arg0)
    {
        if (SysCutomLikeView.SysLikeFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
        {
            dispatcher.Dispatch(CmdEvent.Command.LoadSysCustomMs, new ReqSysCustomInfo
            {
                Type = SysCustomType.Like,
                Limt = 20,
                Skip = SysCutomLikeView.SysLikeFiler.DataSource.Count
            });
        }
    }

    private void LoadSysLikeFinish(IEvent eEvent)
    {
        var list = (List<SysCustomLikeModel>)eEvent.data;
        if (_mList == null)
        {
            _mList = new List<SysCustomLikeModel>();
        }
        for (var i = 0; i < list.Count; i++)
        {
            _mList.Add(list[i]);
        }
        SysCutomLikeView.SysLikeFiler.DataSource = _mList;
        SysCutomLikeView.SysLikeFiler.Refresh();
        SysCutomLikeView.IsVisible(_mList.Count > 0);
    }
    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(SysCutomLikeView.GetUiId(),(int)UiId.ChatSession);
    }
    public override void OnRemove()
    {
        SysCutomLikeView.BackBut.onClick.RemoveListener(BackClick);
        SysCutomLikeView.SysLikeFiler.OnCellClick -= OnCellClick;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSysLikeFinish, LoadSysLikeFinish);
        SysCutomLikeView.SysLikeFiler.ScrollView.onValueChanged.RemoveListener(ScrollViewListener);
        if (_mList != null)
        {
            _mList.Clear();
            _mList = null;
        }
        if (SysCutomLikeView.SysLikeFiler.DataSource != null)
        {
            SysCutomLikeView.SysLikeFiler.DataSource.Clear();
            SysCutomLikeView.SysLikeFiler.DataSource = null;
        }
    }

    private void OnDestroy()
    {
        OnRemove();
    }

}
