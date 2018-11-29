using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class LocalMediator : EventMediator {

    [Inject] public  LocalView LocalView { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private ReqLocalInfo _mLocalInfo;
    private LocalType _type;
    private LocalModel _localModel;
    private string[] _localStrings;
    public override void OnRegister()
    {
        LoadData();
        LocalView.BackBut.onClick.AddListener(BackClick);
        _localStrings = new string[3];
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadLocalDataFinish,LoadLocalDataFinish);
        LocalView.CountriesFiler.OnCellClick = OnCellClick;

    }

    private void LoadData()
    {
        dispatcher.Dispatch(CmdEvent.Command.LoadLocalData, new ReqLocalInfo()
        {
            Type = LocalType.Country
        });
    }
    private void OnCellClick(int index, LocalModel localModel)
    {
        _localModel = localModel;
        switch (_localModel.Type)
        {
            case LocalType.Country:
                _localStrings[0] = _localModel.Name;
                _localStrings[1] = string.Empty;
                _localStrings[2] = string.Empty;
                dispatcher.Dispatch(CmdEvent.Command.LoadLocalData,new ReqLocalInfo()
                {
                    Type  = LocalType.State,
                    Id = _localModel.Id
                });
                break;
            case LocalType.State:
                _localStrings[1] = _localModel.Name;
                _localStrings[2] = string.Empty;
                dispatcher.Dispatch(CmdEvent.Command.LoadLocalData, new ReqLocalInfo()
                {
                    Type = LocalType.City,
                    Id = _localModel.Id
                });
                break;
            case LocalType.City:
                _localStrings[2] = _localModel.Name;
                dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption,new EditorUserData()
                {
                    Option = EditorOption.EditorLocal,
                    LocalStrings =_localStrings
                });

                iocViewManager.DestroyAndOpenNew(LocalView.GetUiId(),(int)UiId.EditorUserData);
                break;
        }
    }

    private void LoadLocalDataFinish(IEvent eEvent)
    {
        _mLocalInfo = (ReqLocalInfo)eEvent.data;

        if(_mLocalInfo==null)return;
        _type = _mLocalInfo.Type;

        if (_type == LocalType.Country)
        {
            LocalView.SetUi(UserModel.User);
        }

        if (_mLocalInfo.LocalInfos.Count >0)
        {
            LocalView.CountriesFiler.DataSource = _mLocalInfo.LocalInfos;
            LocalView.CountriesFiler.Refresh();
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
            {
                Option = EditorOption.EditorLocal,
                LocalStrings = _localStrings
            });

            //iocViewManager.CloseCurrentOpenNew((int)UiId.EditorUserData);
            iocViewManager.DestroyAndOpenNew(LocalView.GetUiId(), (int)UiId.EditorUserData);
        }
    }


    private void BackClick()
    {

        switch (_type)
        {
            case LocalType.Country:

                iocViewManager.DestroyAndOpenNew(LocalView.GetUiId(),(int)UiId.EditorUserData);
                break;
            case LocalType.State:
                dispatcher.Dispatch(CmdEvent.Command.LoadLocalData, new ReqLocalInfo()
                {
                    Type = LocalType.Country
                });
                break;
            case LocalType.City:

                dispatcher.Dispatch(CmdEvent.Command.LoadLocalData, new ReqLocalInfo()
                {
                    Type = LocalType.State,
                    Id = _localModel.Id.Remove(3)
                });
                break;
        }
    }

    public override void OnRemove()
    {
        LocalView.BackBut.onClick.AddListener(BackClick);
        LocalView.CountriesFiler.OnCellClick -= OnCellClick;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadLocalDataFinish, LoadLocalDataFinish);
    }

    private void OnDestroy()
    {
        if (LocalView.CountriesFiler.DataSource != null)
        {
            LocalView.CountriesFiler.DataSource.Clear();
            LocalView.CountriesFiler.DataSource = null;
        }
        OnRemove();
    }


}
