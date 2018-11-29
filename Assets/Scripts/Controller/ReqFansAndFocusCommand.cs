using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqFansAndFocusCommand : EventCommand {

    [Inject]
    public UserModel UserModel { get; set; }
    [Inject] public  FriendsShipModel FriendsShipModel { get; set; }

    public override void Execute()
    {
        Retain();

        var fans = new List<FansAndFocusModel>();

        var focus = new List<FansAndFocusModel>();

        foreach (var fan in UserModel.Fans)
        {

            var param = new FansAndFocusModel()
            {
                Id = fan.Value.Follower.Id,
                DisPlayName = fan.Value.Follower.DisplayName,
                UserName = fan.Value.Follower.UserName,
                Signature = fan.Value.Follower.Signature,
                HeadUrl = fan.Value.Follower.AvatarUrl,
                IsBidirectional = fan.Value.IsBidirectional,
                IsFansOrFocus = true
            };
            fans.Add(param);
        }

        foreach (var info in UserModel.Follows)
        {

            var param = new FansAndFocusModel()
            {

                Id = info.Value.Owner.Id,
                DisPlayName = info.Value.Owner.DisplayName,
                UserName = info.Value.Owner.UserName,
                Signature = info.Value.Owner.Signature,
                HeadUrl = info.Value.Owner.AvatarUrl,
                IsBidirectional = info.Value.IsBidirectional,
                IsFansOrFocus = false
            };

            focus.Add(param);
        }

        FriendsShipModel.FansModels = fans;
        FriendsShipModel.FocusModels = focus;
        Log.I("加载粉丝和关注完毕！！！");
    }
}
