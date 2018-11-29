using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class UpdateFinishCommand : EventCommand
{
    [Inject]
    public UserModel UserModel { get; set; }

    [Inject]
    public IImService ImService { get; set; }

    [Inject]
    public IFriendService FriendService { get; set; }

    public override void Execute()
    {
        Retain();
        if (UserModel.User == null)
        {
            Log.I("用户信息为空");
            Dispatcher.InvokeAsync(SceneUtil.Instance.LoadScene, 2);
        }
        else
        {
            var uId = UserModel.User.Id;
            //dispatcher.Dispatch(CmdEvent.Command.LoadAccountHeadT2D, UserModel.User.AvatarUrl);
#if UNITY_EDITOR_OSX && !REALMACHINE
                        Dispatcher.InvokeAsync(SceneUtil.Instance.LoadScene, 3);
#else
            ImService.LoginIm(uId.ToString(), Util.Md5(uId.ToString()),
                succ =>
                {
                    if (succ)
                    {
                        Dispatcher.InvokeAsync(LoadUserFriends);
                        //Dispatcher.InvokeAsync(SceneUtil.Instance.LoadScene, 3);
                    }
                    else
                    {

                        //UIUtil.Instance.ShowFailToast("IM启动出错");
                        //Application.Quit();
                        Dispatcher.InvokeAsync(dispatcher.Dispatch,
                            (object)CmdEvent.ViewEvent.ReqLoginImFail);
                    }
                });
#endif
        }
    }

    private void LoadUserFriends()
    {
        Dispatcher.InvokeAsync(SceneUtil.Instance.LoadScene, 3);
        //dispatcher.Dispatch(CmdEvent.Command.ReqFriends);
    }
}
