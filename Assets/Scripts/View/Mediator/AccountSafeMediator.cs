using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class AccountSafeMediator : EventMediator {


    [Inject] 
    public AccountSafeView AccountSafeView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    public override void OnRegister()
    {
        AccountSafeView.BackBut.onClick.AddListener(BackClick);
        AccountSafeView.ChangeBindPhNumBut.onClick.AddListener(ChangeBindPhoneNumClick);
        AccountSafeView.SetUi(UserModel.User.DisplayName,UserModel.User.UserName,UserModel.User.PhoneNumber);
    }
    #region Click Event

    private void ChangeBindPhoneNumClick()
    {
        iocViewManager.DestroyAndOpenNew(AccountSafeView.GetUiId(),(int)UiId.ChangeBindPhone);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(AccountSafeView.GetUiId(),(int)UiId.Setting);
    }

    #endregion

    public override void OnRemove()
    {
        AccountSafeView.BackBut.onClick.RemoveListener(BackClick);
        AccountSafeView.ChangeBindPhNumBut.onClick.RemoveListener(ChangeBindPhoneNumClick);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
