using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class SysCustomCommentMediator : EventMediator {

    [Inject]
    public SysCustomCommentView SysCustomCommentView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private List<SysCustomCommentModel> _mList;
    public override void OnRegister()
    {
        SysCustomCommentView.BackBut.onClick.AddListener(BackClick);
        SysCustomCommentView.SysCommentFiler.OnCellClick = OnCellClick;
        SysCustomCommentView.SysCommentFiler.ScrollView.onValueChanged.AddListener(ScrollViewListener);
        dispatcher.Dispatch(CmdEvent.Command.LoadSysCustomMs, new ReqSysCustomInfo
        {
            Type = SysCustomType.Comment,
            Skip = 0,
            Limt = 20,
        });
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSysCommentFinish,LoadSysCommentFinish);
        DeleteRecord();
    }

    private void OnCellClick(int index, SysCustomCommentModel sysCustomCommentModel)
    {
        var model = new PostModel()
        {
            FromType = FromViewType.FromSysCutomCustomView,
            Author = new User()
            {
                Id = sysCustomCommentModel.UserId.ToInt(),
                UserName = sysCustomCommentModel.UserName,
                DisplayName = sysCustomCommentModel.UserDisplayName,
                Signature = sysCustomCommentModel.Signature,
                AvatarUrl = sysCustomCommentModel.UserAvatarUrl,
            },
            Id = sysCustomCommentModel.PostId,
            PictureUrl = sysCustomCommentModel.PostPictureUrl,
            ContentType = sysCustomCommentModel.PostContentType,
            IsLike = false,
            Title = sysCustomCommentModel.PostTitle
        };
        UserModel.PostDetailModel = model;
        iocViewManager.CloseCurrentOpenNew((int)UiId.PreachPost);
    }

    private void DeleteRecord()
    {
        string sysCommentPath = Application.persistentDataPath + "/" + LocalDataObjKey.SysLastCommentMsg;
        if (System.IO.File.Exists(sysCommentPath))
        {
            System.IO.File.Delete(sysCommentPath);
        }
        if (UserModel.LastSysCustomCommentMsg != null)
        {
            UserModel.LastSysCustomCommentMsg = null;
        }
    }
    private void ScrollViewListener(Vector2 arg0)
    {
        if (SysCustomCommentView.SysCommentFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
        {
            dispatcher.Dispatch(CmdEvent.Command.LoadSysCustomMs, new ReqSysCustomInfo
            {
                Type = SysCustomType.Comment,
                Skip = SysCustomCommentView.SysCommentFiler.DataSource.Count,
                Limt = 20,
            });
        }
    }
    private void LoadSysCommentFinish(IEvent eEvent)
    {

       var  list =(List<SysCustomCommentModel>) eEvent.data;
        if (_mList == null)
        {
            _mList = new List<SysCustomCommentModel>();
        }
        for (var i = 0; i < list.Count; i++)
        {
            _mList.Add(list[i]);
        }
        SysCustomCommentView.SysCommentFiler.DataSource = _mList;
        SysCustomCommentView.SysCommentFiler.Refresh();
        SysCustomCommentView.IsVisibleScrollView(_mList.Count>0);
    }

    private void BackClick()
    {
       iocViewManager.DestroyAndOpenNew(SysCustomCommentView.GetUiId(),(int)UiId.ChatSession);
    }

    public override void OnRemove()
    {
        SysCustomCommentView.BackBut.onClick.RemoveListener(BackClick);
        SysCustomCommentView.SysCommentFiler.OnCellClick -= OnCellClick;
        SysCustomCommentView.SysCommentFiler.ScrollView.onValueChanged.RemoveListener(ScrollViewListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSysCommentFinish, LoadSysCommentFinish);
        if (_mList != null)
        {
            _mList.Clear();
            _mList = null;
        }

        if (SysCustomCommentView.SysCommentFiler.DataSource != null)
        {
            SysCustomCommentView.SysCommentFiler.DataSource.Clear();
            SysCustomCommentView.SysCommentFiler.DataSource = null;
        }
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
