using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.Extensions;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class EditorBirthdayMediator : EventMediator {

    [Inject]public EditorBirthdayView EditorBirthdayView { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private DateTime _mDateTime;

    public override void OnRegister()
    {
        _mDateTime =DateTime.Now;
        EditorBirthdayView.BackBut.onClick.AddListener(BackListener);
        SetUi();
    }

    private void SetUi()
    {
        if (!string.IsNullOrEmpty(UserModel.User.BirthDate))
        {
            string[] dateStrings = UserModel.User.BirthDate.Split('-');
            EditorBirthdayView.YEarInputField.text = dateStrings[0];
            EditorBirthdayView.MonthInputField.text = dateStrings[1];
            EditorBirthdayView.DayInputField.text = dateStrings[2];
        }
        else
        {
            EditorBirthdayView.YEarInputField.text = _mDateTime.Year.ToString();
            EditorBirthdayView.MonthInputField.text = _mDateTime.Month.ToString();
            EditorBirthdayView.DayInputField.text = _mDateTime.Day.ToString();
        }
    }


    #region Event Listener
    private void BackListener()
    {
        string dt =string.Empty;
        if (!string.IsNullOrEmpty(EditorBirthdayView.YEarInputField.text)&&
            !string.IsNullOrEmpty(EditorBirthdayView.MonthInputField.text)&&
            !string.IsNullOrEmpty(EditorBirthdayView.DayInputField.text))
        {
            dt = string.Format("{0}/{1}/{2}", EditorBirthdayView.YEarInputField.text,
                EditorBirthdayView.MonthInputField.text, EditorBirthdayView.DayInputField.text);
            dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
            {
                Option = EditorOption.EditorBirthday,
                Content = dt
            });
        }
        iocViewManager.DestroyAndOpenNew(EditorBirthdayView.GetUiId(),(int)UiId.EditorUserData);
    }
    #endregion

    public override void OnRemove()
    {
        EditorBirthdayView.BackBut.onClick.RemoveListener(BackListener);
    }

    private void OnDestroy()
    {
        OnRemove();
    }

}
