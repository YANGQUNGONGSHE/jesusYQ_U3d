using System.Collections;
using System.Collections.Generic;
using NIM;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqSetBlackCommand : EventCommand {

    //[Inject] public IImService ImService { get; set; }
    [Inject] public IPreachService PreachService { get; set; }

    public override void Execute()
    {
        Retain();
        var info = evt.data as ReqSetBlackInfo;

        if(info==null)return;

        //        ImService.SetBlacklist(info.AccountId,info.IsBlack, code =>
        //        {
        //            if (code == ResponseCode.kNIMResSuccess)
        //            {
        //                Log.I("黑名单设置完成!!!");
        //               // dispatcher.Dispatch(CmdEvent.ViewEvent.ReqSetBlackSucc,info);
        //            }
        //        } );
        if (info.IsBlack)
        {
            PreachService.ReqCreateBlack(info.AccountId, (b, s) =>
            {
                if (b)
                {
                    Log.I("加入黑名单设置完成!!!");
                    UIUtil.Instance.ShowTextToast("已屏蔽");
                }
            });
        }
        else
        {
            PreachService.ReqDeleteBlack(info.AccountId, (b, s) =>
            {
                if (b)
                {
                    Log.I("移除出黑名单设置完成!!!");
                    UIUtil.Instance.ShowTextToast("已取消屏蔽");
                }
            });
        }
         

    }
}

public class ReqSetBlackInfo
{
    /// <summary>
    /// 用户
    /// </summary>
    public string AccountId;

    /// <summary>
    /// true 设置拉入黑名单  false 设置移除出黑名单
    /// </summary>
    public bool IsBlack;
}
