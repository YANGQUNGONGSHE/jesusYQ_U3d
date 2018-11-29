using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class FansAndFocusMediator : EventMediator {

    [Inject]
    public FansAndFocusView FansAndFocusView { get; set; }
    [Inject]
    public FriendsShipModel FriendsShipModel { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    private ArgSelectedMember _argSelectedMember;

    public override void OnRegister()
    {
       FansAndFocusView.BackBut.onClick.AddListener(BackClick);
       FansAndFocusView.SearchButton.onClick.AddListener(SearchClick);
       FansAndFocusView.FocusToggle.onValueChanged.AddListener(FocusToggleListener);
       FansAndFocusView.FansToggle.onValueChanged.AddListener(FansToggleListener);
       NotificationCenter.DefaultCenter().AddObserver(NotifiyName.FansAddFocus, AddFocusCallBack);
       FansAndFocusView.FansAndFocusFiler.OnCellClick = OnCellClick;
       LoadData();
    }

    private void LoadData()
    {
        FansAndFocusView.FansAndFocusFiler.DataSource = FriendsShipModel.FocusModels;
        FansAndFocusView.FansAndFocusFiler.Refresh();
        FansAndFocusView.IsVisibleScroll(FriendsShipModel.FocusModels.Count > 0);
    }
    private void OnCellClick(int i, FansAndFocusModel fansAndFocusModel)
    {
        UserModel.PostModel = new PostModel()
        {
            HeadTexture2D = fansAndFocusModel.HeadTexture2D,
            FromType = FromViewType.FromFansAndFollowView,
            Author = new User()
            {
                Id = fansAndFocusModel.Id,
                UserName = fansAndFocusModel.UserName,
                DisplayName = fansAndFocusModel.DisPlayName,
                AvatarUrl = fansAndFocusModel.HeadUrl,
                Signature = fansAndFocusModel.Signature
            }
        };
        iocViewManager.DestroyAndOpenNew(FansAndFocusView.GetUiId(),(int)UiId.Personal);
    }
    private void AddFocusCallBack(Notification notification)
    {
        _argSelectedMember = (ArgSelectedMember)notification.Content;

        if (IsBlack())
        {
            UIUtil.Instance.ShowTextToast("该用户已被拉黑！");
            FansAndFocusView.FansAndFocusFiler.Refresh();
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.FocusOptions, new FocusOptionInfo()
            {
                Options = FocusOptions.AddFcous,
                Id = _argSelectedMember.MemberUid
            });
        } 
    }
    private void FansToggleListener(bool arg0)
    {
        if (arg0)
        {
            FansAndFocusView.ToggleLabelColor(1);
            FansAndFocusView.FansAndFocusFiler.DataSource = FriendsShipModel.FansModels;
            FansAndFocusView.FansAndFocusFiler.Refresh();
            FansAndFocusView.IsVisibleScroll(FriendsShipModel.FansModels.Count > 0);
        }
    }
    private void FocusToggleListener(bool arg0)
    {
        if (arg0)
        {
            FansAndFocusView.ToggleLabelColor(0);
            FansAndFocusView.FansAndFocusFiler.DataSource = FriendsShipModel.FocusModels;
            FansAndFocusView.FansAndFocusFiler.Refresh();
            FansAndFocusView.IsVisibleScroll(FriendsShipModel.FocusModels.Count > 0);
        }
    }
    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(FansAndFocusView.GetUiId(),(int)UiId.Me);
    }
    private void SearchClick()
    {
        UserModel.SearchUserType = SearchUserType.FansAndFollow;
        iocViewManager.DestroyAndOpenNew(FansAndFocusView.GetUiId(),(int)UiId.ChatSerach);
    }

    private bool IsBlack()
    {
        if (UserModel.BlackModels == null)
        {
            return false;
        }
        for (var i = 0; i < UserModel.BlackModels.Count; i++)
        {
            if (UserModel.BlackModels[i].Id == _argSelectedMember.MemberUid.ToInt())
            {
                return true;
            }
        }
        return false;
    }
    public override void OnRemove()
    {
        FansAndFocusView.BackBut.onClick.RemoveListener(BackClick);
        FansAndFocusView.SearchButton.onClick.RemoveListener(SearchClick);
        FansAndFocusView.FocusToggle.onValueChanged.RemoveListener(FocusToggleListener);
        FansAndFocusView.FansToggle.onValueChanged.RemoveListener(FansToggleListener);
        FansAndFocusView.FansAndFocusFiler.OnCellClick -= OnCellClick;
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.FansAddFocus, AddFocusCallBack);
    }
    private void OnDestroy()
    {
        OnRemove();
    }


}
