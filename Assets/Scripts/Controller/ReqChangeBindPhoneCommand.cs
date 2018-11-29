using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqChangeBindPhoneCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }


    public override void Execute()
    {
        Retain();
        var param = evt.data as ReqBindPhoneInfo;

        if(param==null)return;
        //校验验证码
        AccountService.CheckVerfiyCode(param.NewPhoneNumber,param.Purpose,param.Token, (b, s) =>
        {
            if (b)
            {   //解绑旧号码
                AccountService.ReqCancelBindPhone(param.OldPhoneNumber, (b1, s1) =>
                {
                    if (b1)
                    {   //绑定新号码
                        AccountService.ReqBindPhone(param.NewPhoneNumber,param.Token, (b2, s2) =>
                        {
                            if (b2)
                            {  //绑定新号码后，查询账户信息
                                AccountService.QueryAccountInfo((b3, info) =>
                                {
                                    Release();
                                    if (b3)
                                    {
                                        UserModel.User = info.Account;
                                        LocalDataManager.Instance.SaveJsonObj(LocalDataObjKey.USER, info.Account);
                                        dispatcher.Dispatch(CmdEvent.ViewEvent.ReqBindPhoneFinish);
                                    }
                                    else
                                    {
                                        UIUtil.Instance.ShowFailToast(info.ResponseStatus.Message);
                                    }
                                } );
                            }
                            else
                            {
                                UIUtil.Instance.ShowFailToast(s2);
                            }
                        });
                    }
                    else
                    {
                        UIUtil.Instance.ShowFailToast(s1);
                    }
                } );
            }
            else
            {
                UIUtil.Instance.ShowFailToast(s);
            }

        });
    }
}

public class ReqBindPhoneInfo
{
    /// <summary>
    /// 旧号码
    /// </summary>
    public string OldPhoneNumber;
    /// <summary>
    /// 新号码
    /// </summary>
    public string NewPhoneNumber;
    /// <summary>
    /// 用途
    /// </summary>
    public string Purpose;
    /// <summary>
    /// 验证码
    /// </summary>
    public string Token;
}
