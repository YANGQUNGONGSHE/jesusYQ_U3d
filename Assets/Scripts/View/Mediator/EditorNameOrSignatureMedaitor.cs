using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class EditorNameOrSignatureMedaitor : EventMediator {


    [Inject]
    public EditorNameOrSignatureView EditorNameOrSignatureView { get; set; }
    [Inject] 
    public UserModel UserModel { get; set; }

    private EditorOption _mOption;
    private int _mTextCount;

    public override void OnRegister()
    {
        EditorNameOrSignatureView.BackBut.onClick.AddListener(BackClick);
        EditorNameOrSignatureView.FinishBut.onClick.AddListener(FinishClick);
        EditorNameOrSignatureView.InputField.onValueChanged.AddListener(InputFiledListener);
        LoadData();
    }

    private void LoadData()
    {
        EditorNameOrSignatureView.FinishBut.interactable = false;
        _mOption = UserModel.EditorOptionType;
        switch (UserModel.EditorOptionType)
        {
            case EditorOption.EditorDisplayName:
                EditorNameOrSignatureView.InputField.text = !string.IsNullOrEmpty(UserModel.User.DisplayName) ? UserModel.User.DisplayName : UserModel.User.UserName;
                EditorNameOrSignatureView.TypeText.text = "昵称";
                EditorNameOrSignatureView.TextCount.text = 8 + "";
                _mTextCount = 8;
                EditorNameOrSignatureView.TextCount.text =
                    _mTextCount - EditorNameOrSignatureView.InputField.text.Length + "";
                break;
            case EditorOption.EditorSignature:
                EditorNameOrSignatureView.InputField.text = UserModel.User.Signature;
                EditorNameOrSignatureView.TypeText.text = "个性签名";
                EditorNameOrSignatureView.TextCount.text = 30 + "";
                _mTextCount = 30;
                EditorNameOrSignatureView.TextCount.text =
                    _mTextCount - EditorNameOrSignatureView.InputField.text.Length + "";
                break;
        }
    }
    #region Event Click
    private void InputFiledListener(string arg0)
    {
        EditorNameOrSignatureView.TextCount.text = _mTextCount - EditorNameOrSignatureView.InputField.text.Length + "";

        if (EditorNameOrSignatureView.InputField.text.Length <= _mTextCount)
        {
            if (_mOption == EditorOption.EditorDisplayName)
            {
                EditorNameOrSignatureView.FinishBut.interactable = !string.IsNullOrEmpty(EditorNameOrSignatureView.InputField.text);
            }
            else
            {
                EditorNameOrSignatureView.FinishBut.interactable = true;
            } 
        }
        else
        {
            EditorNameOrSignatureView.FinishBut.interactable = false;
        }
    }

    private void FinishClick()
    {
        switch (_mOption)
        {
            case EditorOption.EditorDisplayName:
                dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption,new EditorUserData()
                {
                    Option  = EditorOption.EditorDisplayName,
                    Content = EditorNameOrSignatureView.InputField.text
                });

                break;
            case EditorOption.EditorSignature:
                dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
                {
                    Option = EditorOption.EditorSignature,
                    Content = EditorNameOrSignatureView.InputField.text
                });
                break;
        }
        iocViewManager.DestroyAndOpenNew(EditorNameOrSignatureView.GetUiId(),(int)UiId.EditorUserData);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(EditorNameOrSignatureView.GetUiId(), (int)UiId.EditorUserData);
    }

    #endregion
    public override void OnRemove()
    {
        EditorNameOrSignatureView.BackBut.onClick.RemoveListener(BackClick);
        EditorNameOrSignatureView.FinishBut.onClick.RemoveListener(FinishClick);
        EditorNameOrSignatureView.InputField.onValueChanged.RemoveListener(InputFiledListener);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
