using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ReqQuerySingleUserCommand : EventCommand {
    [Inject]
    public IAccountService AccountService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    public override void Execute()
    {
        Retain();
        var param = (string)evt.data;
        AccountService.QuerySingleUserInfo(param, (b, info) =>
        {
            Release();
            if (b)
            {
                UserModel.PostModel = new PostModel()
                {
                    FromType = FromViewType.FromReadRecordView,
                    Author = new User()
                    {
                        Id = info.User.Id,
                        UserName = info.User.UserName,
                        DisplayName = info.User.DisplayName,
                        Signature = info.User.Signature,
                        AvatarUrl = info.User.AvatarUrl
                    }
                };
                Dispatcher.InvokeAsync(iocViewManager.DestroyAndOpenNew, (int)UiId.ChatMain, (int)UiId.Personal);
            }
            else
            {
                UIUtil.Instance.ShowFailToast("进入个人页面失败");
            }
        } );
    }
}
