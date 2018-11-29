using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqLoginImCommand : EventCommand {

    [Inject]
    public IImService ImService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    public override void Execute()
    {
       Retain();
        ImService.LoginIm(UserModel.User.Id.ToString(), Util.Md5(UserModel.User.Id.ToString()),
            succ =>
            {
                Release();
                if (succ)
                {
                    Log.I("IM登录成功");
                    dispatcher.Dispatch(CmdEvent.Command.LoadSession);
                    dispatcher.Dispatch(CmdEvent.ViewEvent.LoginImSucc);
                }
                else
                {
                    UIUtil.Instance.ShowFailToast("IM登录失败");
                   // dispatcher.Dispatch(CmdEvent.ViewEvent.LoginImFail);
                }
            });
    }
}
