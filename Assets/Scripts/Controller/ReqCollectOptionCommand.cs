using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqCollectOptionCommand : EventCommand {

    [Inject]public IAccountService AccountService { get; set; }
    private LoadCollectInfo _mLoadCollectInfo;

    public override void Execute()
    {
       Retain();
        _mLoadCollectInfo = evt.data as LoadCollectInfo;
        if(_mLoadCollectInfo == null)return;
        switch (_mLoadCollectInfo.Option)
        {
            case CollectOption.ReqCollectInfo:
                ReqCollectInfo(_mLoadCollectInfo.UserId, _mLoadCollectInfo.ParentType,"true", _mLoadCollectInfo.Skip, _mLoadCollectInfo.Limit);
                break;
            case CollectOption.DeleteCollect:
                ReqCancelCollect(_mLoadCollectInfo.ParentId);
                break;
        }
    }

    private void ReqCollectInfo(string userId,string parentType,string descending,string skip,string limit)
    {

        AccountService.ReqCollectByUser(userId,parentType,descending,skip,limit, (b, error, info) =>
        {
            Release();
            if (b)
            { 
               var models = new List<CollectModel>();
                for (var i = 0; i < info.Count; i++)
                {
                   var model = new CollectModel()
                   {
                      ParentId = info[i].ParentId,
                      ParentType = info[i].ParentType,
                      ParentTitle = info[i].ParentTitle
                   };
                    models.Add(model);
                }
                _mLoadCollectInfo.CollectModels = models;
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadCollectsFinish, _mLoadCollectInfo);
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        } );
    }

    private void ReqCancelCollect(string parentId)
    {
        AccountService.ReqDeleteCollectRecord(parentId, (b, error) =>
        {
            Release();
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.DeleteCollectFinish);
            }
            else
            {
                Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                UIUtil.Instance.ShowFailToast(error);
            }
        });
    }
}

public class LoadCollectInfo
{
    /// <summary>
    /// 类型
    /// </summary>
    public CollectOption Option;
    /// <summary>
    /// 用户ID
    /// </summary>
    public string UserId;
    /// <summary>
    /// 上级类型（可选值：帖子, 章, 节）
    /// </summary>
    public string ParentType;
    /// <summary>
    /// 忽略的行数
    /// </summary>
    public string Skip;
    /// <summary>
    /// 获取的行数
    /// </summary>
    public string Limit;
    /// <summary>
    /// 上级编号（如帖子编号）(取消点赞必填)
    /// </summary>
    public string ParentId;
    /// <summary>
    /// 是否是刷新数据
    /// </summary>
    public bool IsRefresh;
    /// <summary>
    /// 收藏数据组
    /// </summary>
    public List<CollectModel> CollectModels;
}
public enum CollectOption
{
    /// <summary>
    /// 请求收藏的数据
    /// </summary>
    ReqCollectInfo,
    /// <summary>
    /// 取消收藏
    /// </summary>
    DeleteCollect,
}
