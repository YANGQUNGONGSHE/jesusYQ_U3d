using System.Collections.Generic;
using System.Diagnostics;
using strange.extensions.command.impl;

/// <summary>
/// 请求好友列表
/// </summary>
public class ReqFriendListCommand : EventCommand
{
    [Inject]
    public IFriendService FriendService { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    public override void Execute()
    {
		Log.I("开始请求洛！！！！ReqFriendListCommand");
        Retain();
        FriendService.RequestFollows(UserModel.User.Id.ToString(), (b, list) =>
        {
				Log.I("请求返回了！！！！，RequestFollows"+b);
            if (b)
            {
					Log.I("请求好友成功");
                var dict = new Dictionary<string, FollowInfo>();
                for (var i = 0; i < list.Count; i++)
                {
                    dict.Add(list[i].Owner.Id.ToString(), list[i]);
                }
                UserModel.Follows = dict;

                FriendService.RequestFans(UserModel.User.Id.ToString(), (b1, list1) =>
                {
                    if (b1)
                    {
						Log.I("请求粉丝成功");
                        Release();
                        var dict1 = new Dictionary<string, FollowFnInfo>();
                        for (var i = 0; i < list1.Count; i++)
                        {
                            dict1.Add(list1[i].Follower.Id.ToString(), list1[i]);
                        }
                        UserModel.Fans = dict1;

                        dispatcher.Dispatch(CmdEvent.Command.LoadFansAndFocus);
                    }
                    else
                    {
								Log.I("请求粉丝失败");
                        dispatcher.Dispatch(CmdEvent.ViewEvent.ReqFriendsFail);
                    }
                });
            }
            else
            {
					Log.I("请求好友失败");
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqFriendsFail);
            }
        });
    }
}