using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class CollectMediator : EventMediator {

    [Inject]
    public CollectView CollectView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    private List<CollectModel> _collectModels;

    public override void OnRegister()
    {
        LoadCollectData();
        CollectView.BackBut.onClick.AddListener(BackClick);
        CollectView.CollectFiler.ScrollView.onValueChanged.AddListener(ScrollRectListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadCollectsFinish,LoadCollectFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.DeleteCollectFinish,DeleteCollectFinish);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.DeleteCollect,DeleteCollectEvent);
    }
    private void DeleteCollectEvent(Notification notification)
    {
        UIUtil.Instance.ShowWaiting();
        var param = (ArgSelectedMember) notification.Content;
        dispatcher.Dispatch(CmdEvent.Command.ReqCollectOption, new LoadCollectInfo()
        {
            Option = CollectOption.DeleteCollect,
            ParentId = param.MemberUid
        });
    }

    private void ScrollRectListener(Vector2 arg0)
    {
        if (CollectView.CollectFiler.ScrollView.normalizedPosition.y >= 1.2f && Input.GetMouseButtonUp(0))
        {
            dispatcher.Dispatch(CmdEvent.Command.ReqCollectOption, new LoadCollectInfo()
            {
                Option = CollectOption.ReqCollectInfo,
                UserId = UserModel.User.Id.ToString(),
                ParentType = "节",
                Skip = "0",
                Limit = "20",
                IsRefresh = true
            });
        }

        if (CollectView.CollectFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
        {
            if (CollectView.CollectFiler.DataSource.Count<2)return;
            dispatcher.Dispatch(CmdEvent.Command.ReqCollectOption, new LoadCollectInfo()
            {
                Option = CollectOption.ReqCollectInfo,
                UserId = UserModel.User.Id.ToString(),
                ParentType = "节",
                Skip = CollectView.CollectFiler.DataSource.Count.ToString(),
                Limit = "20",
                IsRefresh = false
            });
        }
    }

    private void LoadCollectData()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqCollectOption, new LoadCollectInfo()
        {
            Option = CollectOption.ReqCollectInfo,
            UserId = UserModel.User.Id.ToString(),
            ParentType = "节",
            Skip = "0",
            Limit = "50"
        });
    }

    private void DeleteCollectFinish()
    {
       dispatcher.Dispatch(CmdEvent.Command.ReqCollectOption,new LoadCollectInfo()
       {
           Option = CollectOption.ReqCollectInfo,
           UserId = UserModel.User.Id.ToString(),
           ParentType = "节",
           Skip = "0",
           Limit = CollectView.CollectFiler.DataSource.Count.ToString(),
           IsRefresh = true
       });
       UIUtil.Instance.CloseWaiting();
    }

    private void LoadCollectFinish(IEvent eEvent)
    {
        var param = eEvent.data as LoadCollectInfo;
        if(param==null)return;
        if (_collectModels == null)
        {
            _collectModels = new List<CollectModel>();
        }
        if (param.IsRefresh)
        {
            _collectModels.Clear();
        }
        for (var i = 0; i < param.CollectModels.Count; i++)
        {
            _collectModels.Add(param.CollectModels[i]);
        }
        CollectView.IsVisibleScrollrect(_collectModels.Count >= 1);
        CollectView.CollectFiler.DataSource = _collectModels;
        CollectView.CollectFiler.Refresh();
    }

    private void BackClick()
    {
        if (_collectModels != null)
        {
            _collectModels.Clear();
        }
        if (CollectView.CollectFiler.DataSource!=null)
        {
            CollectView.CollectFiler.DataSource.Clear();
            CollectView.CollectFiler.DataSource = null;
        }
        iocViewManager.DestroyAndOpenNew(CollectView.GetUiId(),(int)UiId.Me);
    }

    public override void OnRemove()
    {
        CollectView.BackBut.onClick.RemoveListener(BackClick);
        CollectView.CollectFiler.ScrollView.onValueChanged.RemoveListener(ScrollRectListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadCollectsFinish, LoadCollectFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.DeleteCollectFinish, DeleteCollectFinish);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.DeleteCollect, DeleteCollectEvent);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
