using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqFamousUserCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }

    public override void Execute()
    {
        Retain();
        var list = new List<FamousPersonModel>();
        AccountService.QueryUsers("Reputation", "true", string.Empty,"15", (b, error, info) =>
        {
            if (b)
            {
                for (var i = 0; i < info.Count; i++)
                {
                    var model = new FamousPersonModel()
                    {
                        Uid = info[i].Id,
                        Name = info[i].DisplayName,
                        Signature = info[i].Signature,
                        HeadImageUrl =  info[i].AvatarUrl
                    };
                    list.Add(model);
                }
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadFamousUsersSucc,list);
            }
            else
            {
                Log.I("ReqFamousUserCommand：：："+error);
            }
        } );
    }
}
