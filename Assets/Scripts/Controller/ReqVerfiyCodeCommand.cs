using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqVerfiyCodeCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }

    private ReqLoginVerifyInfo _info;

    public override void Execute()
    {
        Retain();
        _info = (ReqLoginVerifyInfo) evt.data ;

        AccountService.RequestVerfiyCode(_info.PhoneNumber,_info.Purpose, (b, s) =>
        {
            Release();
            var arg = new ArgReqVerfiyCode() { IsSucc = b,ResponseMsg = s};

              Dispatcher.InvokeAsync(ResponeBack,arg);
        } );
       
    }


    private void ResponeBack(ArgReqVerfiyCode arg)
    {

        switch (_info.Type)
        {
            case  VeryType.Login:
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqLoginVerifyCodeFinish, arg);
                break;

            case VeryType.Bind:
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqBindVerifyCodeFinish, arg);
                break;
        }
    }
}

public struct ArgReqVerfiyCode
{


    public bool IsSucc;
    public string ResponseMsg;
}

public struct ReqLoginVerifyInfo
{
    /// <summary>
    /// 号码
    /// </summary>
    public string PhoneNumber;
    /// <summary>
    /// 用途
    /// </summary>
    public string Purpose;
    /// <summary>
    /// 用途类型
    /// </summary>
    public VeryType Type;
}

public enum VeryType
{
    /// <summary>
    /// 登陆
    /// </summary>
    Login,
    /// <summary>
    /// 绑定
    /// </summary>
    Bind
}
