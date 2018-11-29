using System.Collections;
using System.Collections.Generic;
using JPush;
using NIM;
using NIMAudio;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqLoginOutCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }

    [Inject]
    public IImService ImService { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }


    public override void Execute()
    {
        Retain();
        UIUtil.Instance.ShowWaiting();
        AccountService.LoginOut((b, s) =>
        {
            if (b)
            {
                ImService.LogoutIm(NIMLogoutType.kNIMLogoutChangeAccout, result =>
                {
                    Release();
                    Dispatcher.InvokeAsync(CleanUserData);
                    AudioAPI.UninitModule();
                    ClientAPI.Cleanup();
                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                    Dispatcher.InvokeAsync(IsInitSdk);
                });
            }
            else
            {
                Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoginOutFail,s);
            }
        });
    }

    private void CleanUserData()
    {
        UserModel.User = null;
        UserModel.SessionId = null;
        UserModel.LastSysCustomLikeMsg = null;
        UserModel.LastSysCustomCommentMsg = null;
        if (UserModel.SingleRankModel != null)
        {
            UserModel.SingleRankModel = null;
        }
        if (UserModel.AllJoinRankGroupModels != null)
        {
            UserModel.AllJoinRankGroupModels.Clear();
            UserModel.AllJoinRankGroupModels = null;
        }
        string path = Application.persistentDataPath + "/" + LocalDataObjKey.USER;
        string recordPath = Application.persistentDataPath + "/" + LocalDataObjKey.LastReadRecord;
        string ssidPath = Application.persistentDataPath + "/" + LocalDataObjKey.Ssid;
        string sysTimePath = Application.persistentDataPath + "/" + LocalDataObjKey.SysLastTimeTag;
        string sysLikePath = Application.persistentDataPath + "/" + LocalDataObjKey.SysLastLikeMsg;
        string sysCommentPath = Application.persistentDataPath + "/" + LocalDataObjKey.SysLastCommentMsg;
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        if (System.IO.File.Exists(recordPath))
        {
            System.IO.File.Delete(recordPath);
        }
        if (System.IO.File.Exists(ssidPath))
        {
            System.IO.File.Delete(ssidPath);
        }
        if (System.IO.File.Exists(sysTimePath))
        {
            System.IO.File.Delete(sysTimePath);
        }
        if (System.IO.File.Exists(sysLikePath))
        {
            System.IO.File.Delete(sysLikePath);
        }
        if (System.IO.File.Exists(sysCommentPath))
        {
            System.IO.File.Delete(sysCommentPath);
        }

#if UNITY_ANDROID
        if (!JPushBinding.IsPushStopped())
        {
            JPushBinding.StopPush();
        }
#endif
        LocalDataManager.Instance.DeleteByKey(LocalDataObjKey.BibleRecord);
    }

    private void IsInitSdk()
    {
        ImService.InitSdk();
    }
}
