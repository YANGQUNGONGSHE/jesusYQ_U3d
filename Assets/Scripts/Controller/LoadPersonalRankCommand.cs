using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class LoadPersonalRankCommand : EventCommand {

    [Inject]public IAccountService AccountService { get; set; }
    [Inject] public UserModel UserModel { get; set; }

    private AllPersonalRankInfo _mPersonalRankInfo;

    public override void Execute()
    {
		Log.I("LoadPersonalRankCommand加载个人用户数据请求!!!!!!!!!!!");
       Retain();

        _mPersonalRankInfo = (AllPersonalRankInfo)evt.data;

        if(_mPersonalRankInfo==null)return;

        switch (_mPersonalRankInfo.Type)
        {
            case LoadRankType.AllPersonalRank:
			Log.I("请求全部用户排名!!!!!!!!!!!");
            
                AccountService.ReqQueryPersonalRank("ParagraphViewsRank", _mPersonalRankInfo.Skip.ToString(), _mPersonalRankInfo.Limit.ToString(),
                    (b, error, info) =>
                    {
                        Release();
                        if (b)
                        {
                            var list = new List<RankPersonalModel>();

                            for (var i = 0; i < info.Count; i++)
                            {
                                var model = new RankPersonalModel
                                {
                                    Id = info[i].User.Id,
                                    UserName = info[i].User.UserName,
                                    DisplayName = info[i].User.DisplayName,
                                    AvatarUrl = info[i].User.AvatarUrl,
                                    Signature = info[i].User.Signature,
                                    RankNumber = info[i].ParagraphViewsRank,
                                    LastRankNumber = info[i].LastParagraphViewsRank
                                };
                                list.Add(model);
                            }
                            _mPersonalRankInfo.Models = list;
                            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadPersonalRankSucc, _mPersonalRankInfo);
                        }
                        else
                        {
                            Log.E(error);
                        }
                    });
                break;

            case LoadRankType.SinglePersonalRank:
			Log.I("请求单个用户排名!!!!!!!!!!!");
                AccountService.ReqQueryPersonalRankById(UserModel.User.Id.ToString(), (b, error, info) =>
                {
                    Release();
                    if (b)
                    {
                        var model = new RankPersonalModel()
                        {
                            Id = info.User.Id,
                            UserName = info.User.UserName,
                            DisplayName = info.User.DisplayName,
                            AvatarUrl = info.User.AvatarUrl,
                            Signature = info.User.Signature,
                            RankNumber = info.ParagraphViewsRank,
                            LastRankNumber = info.LastParagraphViewsRank
                        };
                        UserModel.SingleRankModel = model;
                        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSinglePersonalRankSucc,model);
                    }
                    else
                    {
                        Log.E(error);
                    }
                } );
                break;
        }
    }
}

public class AllPersonalRankInfo
{
    /// <summary>
    /// 个人排名数据
    /// </summary>
    public List<RankPersonalModel> Models;
    /// <summary>
    /// 忽略的行数
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
    /// 加载类型
    /// </summary>
    public LoadRankType Type;
}
/// <summary>
/// 加载排名的类型
/// </summary>
public enum LoadRankType
{
    /// <summary>
    /// 请求全部用户排名
    /// </summary>
    AllPersonalRank,
    /// <summary>
    /// 请求全部群组排名
    /// </summary>
    AllGroupsRank,
    /// <summary>
    /// 请求单个用户排名
    /// </summary>
    SinglePersonalRank,
    /// <summary>
    /// 请求加入的所有群组排名
    /// </summary>
    AllMyGroupsRank
}
