using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class LoadAllGroupReadRecordCommand : EventCommand {

    [Inject] public IGroupService GroupService { get; set; }
    [Inject] public IAccountService AccountService { get; set; }

    public override void Execute()
    {
        Retain();
        GroupService.QueryAllMyTeams((count, list) =>
        {
            var idList = new List<string>() ;
            for(var i = 0; i < list.Count; i++)
            {
                var selectIndex = i;
                GroupService.QueryTeamMembersInfo(list[selectIndex], (teamId, counts, isinculd, infos) =>
                {
                    Release();
                    for(var i1 = 0; i1 < infos.Length; i1++)
                    {
                        idList.Add(infos[i1].AccountId);
                    }
                    if (selectIndex == count-1)
                    {
                        var idLists = idList.Distinct().ToList();
                        var ids = new string[idLists.Count];
                        for (int i2 = 0; i2 < idLists.Count; i2++)
                        {
                            ids[i2] = idLists[i2];
                        }
                        var sb = string.Join(",", ids);

                        Dispatcher.InvokeAsync(LoadUsers,sb);
                    }
                });
            }
        });
    }

    private void LoadUsers(string ids)
    {
        var nowTime = DateTime.Now.Date;
        var list = new List<GroupMemberReadRecordModel>();

        AccountService.ReqQueryReadRecordCountByUsers(ids, "节", string.Empty, nowTime.ToUnixTime().ToString(),
            (b, error, info) =>
            {
                if (b)
                {
                    Log.I("已经读经人数：：：："+info.Count);
                    var uids = new StringBuilder();
                    foreach (var id in info.Keys)
                    {
                        uids.Append(id);
                        uids.Append(",");
                    }
                    if (uids.Length > 0)
                    {
                        AccountService.QueryUsersByIds(uids.ToString(), string.Empty, string.Empty,
                            (b1, errors, infos) =>
                            {
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
                                    dispatcher.Dispatch(CmdEvent.ViewEvent.LoadAllGroupReadRecordSucc,list);
                                }
                                else
                                {
                                    Log.E(errors);
                                }
                            });
                    }
                }
                else
                {
                    Log.E(error);
                }
            } );
    }
}
