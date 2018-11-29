using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.SceneManagement;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;
using Object = UnityEngine.Object;

public class SetMediator : EventMediator
{

    [Inject]
    public SetView SetView { get; set; }

    public override void OnRegister()
    {
        SetView.BackBut.onClick.AddListener(BackClick);
        SetView.LoginOutBut.onClick.AddListener(LoginOutClick);
        SetView.AccountSafeBut.onClick.AddListener(AccountSafeClick);
        SetView.SureLoginOutBut.onClick.AddListener(SureLoginOutClick);
        SetView.CancelLoginOutBut.onClick.AddListener(CancelLoginOutClick);
        SetView.CancelBgBut.onClick.AddListener(CancelBgClick);
        SetView.FeedBackBut.onClick.AddListener(FeedBackClick);
        SetView.BlackBut.onClick.AddListener(BlackClick);

        dispatcher.AddListener(CmdEvent.ViewEvent.LoginOutSucc, LoginOutSuccListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoginOutFail,LoginFaiListener);
    }

    #region Click Event

    private void FeedBackClick()
    {
          iocViewManager.CloseCurrentOpenNew((int)UiId.FeedBack);
    }
    private void CancelBgClick()
    {
        SetView.IsVisibleLoginOutBg(false);
    }

    private void CancelLoginOutClick()
    {
        SetView.IsVisibleLoginOutBg(false);
    }

    private void SureLoginOutClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoginOut);
    }

    private void AccountSafeClick()
    {
        iocViewManager.DestroyAndOpenNew(SetView.GetUiId(),(int)UiId.AccountSafe);
    }

    private void LoginOutClick()
    {
        SetView.IsVisibleLoginOutBg(true);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(SetView.GetUiId(),(int)UiId.Me);
    }

    private void BlackClick()
    {
        iocViewManager.DestroyAndOpenNew(SetView.GetUiId(), (int)UiId.Black);
    }
    #endregion

    #region dispatcher Listener

    private void LoginFaiListener(IEvent eEvent)
    {
        UIUtil.Instance.ShowFailToast(eEvent.data as string);
    }
    private void LoginOutSuccListener()
    {
        iocViewManager.DestoryView((int)UiId.Preach);
        iocViewManager.DestoryView(SetView.GetUiId());
        Dispatcher.InvokeAsync(SceneManager.LoadScene,2);
    }
    #endregion

    public override void OnRemove()
    {
        SetView.BackBut.onClick.RemoveListener(BackClick);
        SetView.LoginOutBut.onClick.RemoveListener(LoginOutClick);
        SetView.AccountSafeBut.onClick.RemoveListener(AccountSafeClick);
        SetView.SureLoginOutBut.onClick.RemoveListener(SureLoginOutClick);
        SetView.CancelLoginOutBut.onClick.RemoveListener(CancelLoginOutClick);
        SetView.CancelBgBut.onClick.RemoveListener(CancelBgClick);
        SetView.FeedBackBut.onClick.RemoveListener(FeedBackClick);
        SetView.BlackBut.onClick.RemoveListener(BlackClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoginOutSucc, LoginOutSuccListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoginOutFail, LoginFaiListener);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
