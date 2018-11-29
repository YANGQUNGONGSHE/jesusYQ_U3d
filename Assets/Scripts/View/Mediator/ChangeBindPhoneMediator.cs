using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class ChangeBindPhoneMediator : EventMediator {


    [Inject]
    public ChangeBindPhoneView ChangeBindPhoneView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private bool _isPhoneNumberPassed;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqBindVerifyCodeFinish,ReqBindCodeFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqBindPhoneFinish,ReqBindPhoneFinish);
        ChangeBindPhoneView.BackBut.onClick.AddListener(BackClick);
        ChangeBindPhoneView.BindBut.onClick.AddListener(BindClick);
        ChangeBindPhoneView.SendCodeBut.onClick.AddListener(SendCodeClick);
        ChangeBindPhoneView.PhoneInputField.onValueChanged.AddListener(PhoneInputListener);
        ChangeBindPhoneView.VeryCodeInputField.onValueChanged.AddListener(VeryCodeInputListener);
    }

  

    #region Event Click

    private void SendCodeClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqVerifyCode, new ReqLoginVerifyInfo()
        {
            PhoneNumber = ChangeBindPhoneView.PhoneInputField.text,
            Purpose = "Bind",
            Type = VeryType.Bind
        });
    }

    private void BindClick()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqBindPhone,new ReqBindPhoneInfo()
        {
            OldPhoneNumber = UserModel.User.PhoneNumber,
            NewPhoneNumber = ChangeBindPhoneView.PhoneInputField.text,
            Purpose = "Bind",
            Token = ChangeBindPhoneView.VeryCodeInputField.text
        });
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(ChangeBindPhoneView.GetUiId(),(int)UiId.AccountSafe);
    }
    #endregion

    private void VeryCodeInputListener(string arg0)
    {
        CheckBindButton();
    }

    private void PhoneInputListener(string arg0)
    {
        if (CommUtil.Instance.DxRegex().IsMatch(ChangeBindPhoneView.PhoneInputField.text) ||
            CommUtil.Instance.YdRegex().IsMatch(ChangeBindPhoneView.PhoneInputField.text) ||
            CommUtil.Instance.LtRegex().IsMatch(ChangeBindPhoneView.PhoneInputField.text))
        {
            _isPhoneNumberPassed = true;
        }
        else
        {
            _isPhoneNumberPassed = false;
        }
        ChangeBindPhoneView.ShowCheckPhoneFlag(_isPhoneNumberPassed);
        CheckBindButton();
    }


    private bool CheckInputNull()
    {
        return !string.IsNullOrEmpty(ChangeBindPhoneView.PhoneInputField.text) &&
               !string.IsNullOrEmpty(ChangeBindPhoneView.VeryCodeInputField.text);
    }

    private void CheckBindButton()
    {
        if (CheckInputNull() && _isPhoneNumberPassed)
        {
            ChangeBindPhoneView.ShowBindBtnEnable(true);
        }
        else
        {
            ChangeBindPhoneView.ShowBindBtnEnable(false);
        }
    }

    #region dispatcher Event

    private void ReqBindCodeFinish(IEvent eEvent)
    {

        var arg = (ArgReqVerfiyCode)eEvent.data;
        Log.I("请求发送绑定验证码成功回馈" + arg.IsSucc);

        if (arg.IsSucc)
        {
            CoroutineController.Instance.StartCoroutine(ChangeBindPhoneView.BeginTiming());
        }
        else
        {
            UIUtil.Instance.ShowFailToast(arg.ResponseMsg);
        }
    }

    private void ReqBindPhoneFinish()
    {
        UIUtil.Instance.ShowSuccToast("改绑成功");
        iocViewManager.DestroyAndOpenNew(ChangeBindPhoneView.GetUiId(),(int)UiId.AccountSafe);
    }
    #endregion





    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqBindVerifyCodeFinish, ReqBindCodeFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqBindPhoneFinish, ReqBindPhoneFinish);
        ChangeBindPhoneView.BackBut.onClick.RemoveListener(BackClick);
        ChangeBindPhoneView.BindBut.onClick.RemoveListener(BindClick);
        ChangeBindPhoneView.SendCodeBut.onClick.RemoveListener(SendCodeClick);
        ChangeBindPhoneView.PhoneInputField.onValueChanged.RemoveListener(PhoneInputListener);
        ChangeBindPhoneView.VeryCodeInputField.onValueChanged.RemoveListener(VeryCodeInputListener);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
