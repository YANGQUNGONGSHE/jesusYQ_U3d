using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqQueryUserCommand : EventCommand {

    [Inject]
    public IFriendService FriendService { get; set; }


    public override void Execute()
    {
        Retain();
        var str = evt.data as string;
        if(string.IsNullOrEmpty(str))return;
        if (CommUtil.Instance.DxRegex().IsMatch(str) || CommUtil.Instance.YdRegex().IsMatch(str) ||
            CommUtil.Instance.LtRegex().IsMatch(str))
        {
            Log.I("查询用户手机号码验证通过："+str);
            FriendService.RequestUserInfoByUnameOrEmail("Mobile" + str, (b, user) =>
            {
                Release();
                if (b)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqQueryUserInfoFinish,user);
                }
                else
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqQueryUserInfoFail,"查询用户失败");
                }
            });
        }
        else
        {
            FriendService.RequestUserInfoByDisplayName(str, (b, user) =>
            {
                if (b)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqQueryUserInfoFinish, user);
                }
                else
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqQueryUserInfoFail, "查询用户失败");
                }
            });
        }
    }
}
