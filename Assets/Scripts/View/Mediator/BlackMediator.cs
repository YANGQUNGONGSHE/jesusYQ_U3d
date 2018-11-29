using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class BlackMediator : EventMediator {

    [Inject]public BlackView BlackView { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private BlackModel _model;
    private PostModel _mBlockPostModel;
    private BlockPostOptionInfo _mPostOptionInfo;
    private List<PostModel> _postModels;
    private bool IsDeleteUser = false;

    public override void OnRegister()
    {
       BlackView.BackBut.onClick.AddListener(BackClick);
       BlackView.ActionBut.onClick.AddListener(ActionClick);
       BlackView.DeleteBlackBut.onClick.AddListener(DeleteBlackClick);
       BlackView.BlackFiler.OnCellClick+= OnCellClick;
       BlackView.BlockPostFiler.OnCellClick += BlockPostOnCellClick;
       dispatcher.AddListener(CmdEvent.ViewEvent.ReqBlackListSucc,ReqBlackListFinish);
       dispatcher.AddListener(CmdEvent.ViewEvent.LoadBlockPostsFinish,LoadBlockPostFinish);
       dispatcher.AddListener(CmdEvent.ViewEvent.DeleteBlockPostFinish,DeleteBlockPostFinish);
       BlackView.BlockUserToggle.onValueChanged.AddListener(BlockUserToggleListener);
       BlackView.BlockPosToggle.onValueChanged.AddListener(BlockPostToggleListener);
       BlackView.BlockPostFiler.ScrollView.onValueChanged.AddListener(BlockPostScrollViewListener);
       RefreshData();
       dispatcher.Dispatch(CmdEvent.Command.BlockPostOptions,new BlockPostOptionInfo()
       {
            Options = BlockPostOptions.LoadBlockPostData,
            Skip = 0,
            Limit = 20
       });
    }

    private void DeleteBlackClick()
    {
        if (IsDeleteUser)
        {
            dispatcher.Dispatch(CmdEvent.Command.ReqSetBlack, new ReqSetBlackInfo()
            {
                AccountId = _model.Id.ToString(),
                IsBlack = false
            });
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.BlockPostOptions,new BlockPostOptionInfo()
            {
                Options = BlockPostOptions.DeleteBlockPost,
                PostId = _mBlockPostModel.Id
            });
        }
        BlackView.IsVisibleAction(false);
    }

    private void ActionClick()
    {
        BlackView.IsVisibleAction(false);
    }

    private void OnCellClick(int i, BlackModel blackModel)
    {
        IsDeleteUser = true;
        _model = blackModel;
        BlackView.IsVisibleAction(true);
    }

    private void BlockPostOnCellClick(int index, PostModel postModel)
    {
        IsDeleteUser = false;
        _mBlockPostModel = postModel;
        BlackView.IsVisibleAction(true);
    }
    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(BlackView.GetUiId(),(int)UiId.Setting);
    }

    private void RefreshData()
    {
        BlackView.IsVisibleContent(UserModel.BlackModels.Count > 0);
        BlackView.BlackFiler.DataSource = UserModel.BlackModels;
        BlackView.BlackFiler.Refresh();
    }

    private void ReqBlackListFinish()
    {
        Log.I("*************ReqBlackListFinish*************");
        RefreshData();
    }

    private void LoadBlockPostFinish(IEvent eEvent)
    {
        _mPostOptionInfo = eEvent.data as BlockPostOptionInfo;
        if(_mPostOptionInfo==null) return;
        if (_postModels == null)
        {
            _postModels = new List<PostModel>();
        }
        if (_mPostOptionInfo.IsReefresh)
        {
            _postModels.Clear();
        }

        if (_mPostOptionInfo.Models != null)
        {
            for (var i = 0; i < _mPostOptionInfo.Models.Count; i++)
            {
                _postModels.Add(_mPostOptionInfo.Models[i]);
            }
        }
        BlackView.IsVisibleBlockPostContent(_postModels.Count>0);
        BlackView.BlockPostFiler.DataSource =_postModels;
        BlackView.BlockPostFiler.Refresh();
    }

    private void DeleteBlockPostFinish()
    {
        dispatcher.Dispatch(CmdEvent.Command.BlockPostOptions, new BlockPostOptionInfo()
        {
            Options = BlockPostOptions.LoadBlockPostData,
            Skip = 0,
            Limit = 20,
            IsReefresh = true
        });
    }

    private void BlockPostToggleListener(bool arg0)
    {
        if (arg0)
        {
            BlackView.IsVisibleBlockPostScrollRect(true);
        }
    }

    private void BlockUserToggleListener(bool arg0)
    {
        if (arg0)
        {
            BlackView.IsVisibleBlockPostScrollRect(false);
        }
    }

    private void BlockPostScrollViewListener(Vector2 arg0)
    {
        if (BlackView.BlockPostFiler.ScrollView.normalizedPosition.y >= 1.2f && Input.GetMouseButtonUp(0))
        {
            RefreshBlockPostData();
        }

        if (BlackView.BlockPostFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
        {
            if(BlackView.BlockPostFiler.DataSource.Count<2)return;
            dispatcher.Dispatch(CmdEvent.Command.BlockPostOptions, new BlockPostOptionInfo()
            {
                Options = BlockPostOptions.LoadBlockPostData,
                Skip = BlackView.BlockPostFiler.DataSource.Count,
                Limit = 20,
                IsReefresh = false
            });
        }
    }

    private void RefreshBlockPostData()
    {
        var limit = 20;
        if (BlackView.BlockPostFiler.DataSource.Count >= 20)
        {
            limit = BlackView.BlockPostFiler.DataSource.Count;
        }
        dispatcher.Dispatch(CmdEvent.Command.BlockPostOptions, new BlockPostOptionInfo()
        {
            Options = BlockPostOptions.LoadBlockPostData,
            Skip = 0,
            Limit = limit,
            IsReefresh = true
        });
    }

    public override void OnRemove()
    {
        BlackView.BackBut.onClick.RemoveListener(BackClick);
        BlackView.ActionBut.onClick.RemoveListener(ActionClick);
        BlackView.DeleteBlackBut.onClick.RemoveListener(DeleteBlackClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqBlackListSucc, ReqBlackListFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadBlockPostsFinish, LoadBlockPostFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.DeleteBlockPostFinish, DeleteBlockPostFinish);
        BlackView.BlockUserToggle.onValueChanged.RemoveListener(BlockUserToggleListener);
        BlackView.BlockPosToggle.onValueChanged.RemoveListener(BlockPostToggleListener);
        BlackView.BlockPostFiler.ScrollView.onValueChanged.RemoveListener(BlockPostScrollViewListener);
        BlackView.BlackFiler.OnCellClick -= OnCellClick;
        BlackView.BlockPostFiler.OnCellClick -= BlockPostOnCellClick;
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
