using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class LoadGroupsRankCommand : EventCommand {
    
    [Inject]public IAccountService AccountService { get; set; }
    [Inject] public IGroupService GroupService { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private AllGroupsRankInfo _mGroupsRankInfo;

    public override void Execute()
    {
		Log.I("LoadGroupsRankCommand加载群组排名请求!!!!!!!!!!!!!!!!!!!!!");
        Retain();
        _mGroupsRankInfo = (AllGroupsRankInfo) evt.data;

        if(_mGroupsRankInfo==null) return;

        switch (_mGroupsRankInfo.Type)
        {
            case LoadRankType.AllGroupsRank:

			Log.I("请求全部群组排名!!!!!!!!!!!!!!!!!!!!!");

                AccountService.ReqQueryGroupsRank("ParagraphViewsRank", _mGroupsRankInfo.Skip.ToString(), _mGroupsRankInfo.Limit.ToString(), (b, error, info) =>
                {
                    Release();
                    if (b)
                    {
                        var list = new List<RankGroupModel>();

                        for (var i = 0; i < info.Count; i++)
                        {
                            var model = new RankGroupModel()
                            {
                                GroupId = info[i].Group.Id,
                                GroupName = info[i].Group.DisplayName,
                                GroupHeadUrl = info[i].Group.IconUrl,
                                RankNumber = info[i].ParagraphViewsRank,
                                LastRankNumber = info[i].LastParagraphViewsRank
                            };
                            list.Add(model);
                        }
                        _mGroupsRankInfo.Models = list;
                        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupsRankSucc, _mGroupsRankInfo);
                    }
                    else
                    {
                        Log.E(error);
                    }
                });

                break;
            case LoadRankType.AllMyGroupsRank:
			Log.I("请求加入的所有群组排名!!!!!!!!!!!!!!!!!!!!!");

                GroupService.QueryAllMyTeams((count, list) =>
                {
                    Log.I("加入的群组数量::"+count);
                    if (count < 1)
                    {
                        var modelList = new List<RankGroupModel>();
                        UserModel.AllJoinRankGroupModels = modelList;
                        return;
                    }
                    var groupIds = new string[list.Count];
                    for (var i = 0; i < list.Count; i++)
                    {
                        groupIds[i] = list[i];
                    }
                    var sb = string.Join(",", groupIds);
                    AccountService.ReqQueryGroupsRankByIds(sb, "ParagraphViewsRank", (b, error, info) =>
                    {
                        Release();
                        if (b)
                        {
                            var models = new List<RankGroupModel>();
                            for (var i = 0; i < info.Count; i++)
                            {
                                var model = new RankGroupModel()
                                {
                                    GroupId = info[i].Group.Id,
                                    GroupName = info[i].Group.DisplayName,
                                    GroupHeadUrl = info[i].Group.IconUrl,
                                    RankNumber = info[i].ParagraphViewsRank,
                                    LastRankNumber = info[i].LastParagraphViewsRank
                                };
                                models.Add(model);
                            }
                            UserModel.AllJoinRankGroupModels = models;
                            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadAllOwnGroupsRankSucc,models);
                        }
                        else
                        {
                            Log.E(error);
                        }
                    });
                } );
                break;
        }
    }
}

public class AllGroupsRankInfo
{
    /// <summary>
    /// 一组群组排名数据
    /// </summary>
    public List<RankGroupModel> Models;
    /// <summary>
    /// 忽略的条数
    /// </summary>
    public int Skip;
    /// <summary>
    /// 获取的条数
    /// </summary>
    public int Limit;
    /// <summary>
    /// 是否刷新
    /// </summary>
    public bool IsRefresh;
    /// <summary>
    /// 加载排名类型
    /// </summary>
    public LoadRankType Type;
}
