using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class LoadGroupMemberReadRecordCommand : EventCommand {

    [Inject]
    public IGroupService GroupService { get; set; }
    [Inject]
    public IAccountService AccountService { get; set; }

    private ReqGroupMemberReadInfo _info;

    public override void Execute()
    {
        Retain();
        var groupId = (string)evt.data;
        _info = new ReqGroupMemberReadInfo();
        if (string.IsNullOrEmpty(groupId))return;
        GroupService.QueryCachedTeamInfo(groupId, (s, info) =>
        {
            _info.Annount = info.Announcement;
            GroupService.QueryTeamMembersInfo(groupId, (teamId, count, isinculd, infos) =>
            {
                var uids = new string[infos.Length];
                for (var i = 0; i < infos.Length; i++)
                {
                    uids[i] = (infos[i].AccountId);
                }
                var sb = string.Join(",", uids);
                Dispatcher.InvokeAsync(LoadUsers, sb);
            });
        });
    }

    private void LoadUsers(string uidStrings)
    {
        var nowTime = DateTime.Now.Date;
        var list = new List<GroupMemberReadRecordModel>();
        Log.I("时间：：："+ nowTime.ToUnixTime());
        AccountService.ReqQueryReadRecordCountByUsers(uidStrings,"节",string.Empty, nowTime.ToUnixTime().ToString(), (b, error, info) =>
        {
            if (b)
            {
                var uids = new StringBuilder();
                foreach (var id in info.Keys)
                {
                    uids.Append(id);
                    uids.Append(",");
                }
                if (uids.Length < 1)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupMemberReadRecordFinish, _info);
                }
                else
                {
                    AccountService.QueryUsersByIds(uids.ToString(), string.Empty, string.Empty, (b1, errors, infos) =>
                    {
                        Release();
                        if (b1)
                        {
                            for (var i = 0; i < infos.Count; i++)
                            {
                                var model = new GroupMemberReadRecordModel()
                                {
                                    Uid = infos[i].Id,
                                    DisplayName = infos[i].DisplayName,
                                    AvatarUrl = infos[i].AvatarUrl,
                                    Signature = infos[i].Signature
                                };
                                list.Add(model);
                            }
                            _info.MemberReadRecordModels = list;
                            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupMemberReadRecordFinish, _info);
                        }
                        else
                        {
                            Log.I("QueryUsersByIds" + error);
                        }
                    });
                } 
            }
            else
            {
                Log.I("ReqQueryReadRecordCountByUsers!!!" + error);
            }
        } ); 
    }
}

public class ReqGroupMemberReadInfo
{
    /// <summary>
    /// 群公告
    /// </summary>
    public string Annount;
    /// <summary>
    /// 已读经成员
    /// </summary>
    public List<GroupMemberReadRecordModel> MemberReadRecordModels;

}
