using strange.extensions.command.impl;
using WongJJ.Game.Core;

public class ReqLoginsCommand : EventCommand
{
    private ReqLoginInfo _mReqLoginInfo;

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
        _mReqLoginInfo = (ReqLoginInfo) evt.data;

        AccountService.CheckVerfiyCode(_mReqLoginInfo.PhoneNumber, _mReqLoginInfo.Purpose, _mReqLoginInfo.Token,
            (b, s) =>
            {
                if (b)
                {
                    Log.I("验证码校验成功");
                    AccountService.RequestLogins(_mReqLoginInfo.PhoneNumber, _mReqLoginInfo.Token, (b1, logins) =>
                    {
                        if (b1)
                        {
                            Log.I("手机登录成功"+logins.SessionId+"   "+logins.UserId+" "+ logins.NewlyCreated);
                            AccountService.QuerySingleUserInfo(logins.UserId.ToString(),(b2, userInfo) =>
                            {
                                if (b2)
                                {
                                    Log.I("查询用户信息成功" + userInfo.User.Id + "  " + userInfo.User.DisplayName + "  " + userInfo.User.AvatarUrl);
                                    ImService.LoginIm(logins.UserId.ToString(), Util.Md5(logins.UserId.ToString()),
                                        b3 =>
                                        {
                                            Release();
                                            if (b3)
                                            {
                                                Log.I("IM登录成功" + "是否首次登录" + logins.NewlyCreated);
                                                Dispatcher.InvokeAsync(Save, userInfo.User, logins.NewlyCreated, logins.SessionId);
                                            }
                                            else
                                            {
                                                Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                                                Dispatcher.InvokeAsync(dispatcher.Dispatch,
                                                    (object)CmdEvent.ViewEvent.ReqLoginFail, "Im服务器连接失败");
                                            }
                                        });
                                }
                                else
                                {
                                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                                    Dispatcher.InvokeAsync(dispatcher.Dispatch,
                                        (object)CmdEvent.ViewEvent.ReqLoginFail, userInfo.ResponseStatus.Message);
                                }
                            });
                        }
                        else
                        {
                            Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                            Dispatcher.InvokeAsync(dispatcher.Dispatch, (object) CmdEvent.ViewEvent.ReqLoginFail,
                                logins.ResponseStatus.Message);
                        }
                    });
                }
                else
                {
                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                    Dispatcher.InvokeAsync(dispatcher.Dispatch, (object) CmdEvent.ViewEvent.ReqCheckVerfiyCodeFail, s);
                }
            });
    }

    private void Save(User userInfo, bool isNewCreate,string sessionId)
    {
        //dispatcher.Dispatch(CmdEvent.Command.LoadAccountHeadT2D,userInfo.AvatarUrl);
        LocalDataManager.Instance.SaveJsonObj(LocalDataObjKey.USER, userInfo);
        LocalDataManager.Instance.SaveJsonObj(LocalDataObjKey.Ssid, new SessionId(){ Ssid = sessionId });
        UserModel.User = userInfo;
        UserModel.SessionId = new SessionId(){ Ssid = sessionId };

        dispatcher.Dispatch(CmdEvent.ViewEvent.ReqLoginFinish);

//        if (isNewCreate)
//        {
//			Log.I("准备跳转到首次登录设置页面");
//            dispatcher.Dispatch(CmdEvent.ViewEvent.ReqLoginFinish);
//        }
//        else
//        {
//			Log.I("准备跳转到首页");
//            Dispatcher.InvokeAsync(SceneUtil.Instance.LoadScene, 3);
//        }   
    }
}

public struct ReqLoginInfo
{
    /// <summary>
    ///     手机号码
    /// </summary>
    public string PhoneNumber;

    /// <summary>
    ///     验证码
    /// </summary>
    public string Token;

    /// <summary>
    ///     验证码用途
    /// </summary>
    public string Purpose;
}