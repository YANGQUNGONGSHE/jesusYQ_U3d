using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class LoginsMediator : EventMediator {

    [Inject]
    public LoginsView LoginsView { get; set; }
    [Inject]public UserModel UserModel { get; set; }
    public override void OnRegister()
    {
        LoginsView.LoginBut.onClick.AddListener(LoginClick);
        LoginsView.SendVerifyCodeBut.onClick.AddListener(SendVerifyCodeClick);
        LoginsView.ReLoginBut.onClick.AddListener(ReLoginClick);
        LoginsView.TermServiceBut.onClick.AddListener(TermServiceClick);
        LoginsView.TermPricacyBut.onClick.AddListener(TermPrivacyClick);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqLoginVerifyCodeFinish,ReqLoginVerifyCodeFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqLoginFail,ReqLoginFailListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqCheckVerfiyCodeFail,ReqCheckVerfiyCodeFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqLoginFinish,ReqLoginFinish);
    }

    private void ReqLoginFinish()
    {
       //iocViewManager.DestroyAndOpenNew(LoginsView.GetUiId(),(int)UiId.SetDyName);
       iocViewManager.DestoryView(LoginsView.GetUiId());
       SceneUtil.Instance.LoadScene(3);
    }
    private void ReqCheckVerfiyCodeFail(IEvent eEvent)
    {
        LoginsView.SetErrorText("登录失败", eEvent.data as string);
        LoginsView.IsVisibleErrorAction(true);
    }

    private void ReqLoginFailListener(IEvent eEvent)
    {
        LoginsView.SetErrorText("登录失败", eEvent.data as string);
        LoginsView.IsVisibleErrorAction(true);
    }

    private void ReqLoginVerifyCodeFinish(IEvent eEvent)
    {
        var arg = (ArgReqVerfiyCode)eEvent.data;
        UIUtil.Instance.CloseWaiting();
        if (arg.IsSucc)
        {
            CoroutineController.Instance.StartCoroutine(LoginsView.BeginTiming());
        }
        else
        {
            LoginsView.SetErrorText("获取验证码失败", arg.ResponseMsg);
            LoginsView.IsVisibleErrorAction(true);
        }
    }
    #region Event Listener

    private void ReLoginClick()
    {
        LoginsView.IsVisibleErrorAction(false);
    }

    private void SendVerifyCodeClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqVerifyCode, new ReqLoginVerifyInfo()
        {
           PhoneNumber = LoginsView.PhoneInputField.text,
           Purpose = "Login",
           Type = VeryType.Login
        });
          UIUtil.Instance.ShowWaiting(); 
    }

    private void LoginClick()
    {
       dispatcher.Dispatch(CmdEvent.Command.ReqLogin, new ReqLoginInfo()
       {
         PhoneNumber = LoginsView.PhoneInputField.text,
         Token = LoginsView.VerifyInputField.text,
         Purpose = "Login"
       });
    }

    private void TermPrivacyClick()
    {
        //隐私条款点击事件
        UserModel.TermPath = "www/termPrivacy.html";
        iocViewManager.CloseCurrentOpenNew((int)UiId.Terms);
    }

    private void TermServiceClick()
    {
        //服务条款点击事件
        UserModel.TermPath = "www/termservice.html";
        iocViewManager.CloseCurrentOpenNew((int)UiId.Terms);
    }
    #endregion


    public override void OnRemove()
    {
        LoginsView.LoginBut.onClick.RemoveListener(LoginClick);
        LoginsView.SendVerifyCodeBut.onClick.RemoveListener(SendVerifyCodeClick);
        LoginsView.ReLoginBut.onClick.RemoveListener(ReLoginClick);
        LoginsView.TermServiceBut.onClick.RemoveListener(TermServiceClick);
        LoginsView.TermPricacyBut.onClick.RemoveListener(TermPrivacyClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqLoginVerifyCodeFinish, ReqLoginVerifyCodeFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqLoginFail, ReqLoginFailListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqCheckVerfiyCodeFail, ReqCheckVerfiyCodeFail);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqLoginFinish, ReqLoginFinish);
        CoroutineController.Instance.StopCoroutine(LoginsView.BeginTiming());
    }

    private void OnDestroy()
    {
        OnRemove();

    }


}
