using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqLocalDataCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }

    private ReqLocalInfo _mReqLocalInfo;

    private List<LocalModel> mLocalModels;


    public override void Execute()
    {
        Retain();
        _mReqLocalInfo = (ReqLocalInfo)evt.data;
      
        switch (_mReqLocalInfo.Type)
        {
            case LocalType.Country:
                ReqCountries();
                break;
            case LocalType.State:
                ReqStates(_mReqLocalInfo.Id);
                break;
            case LocalType.City:
                ReqCities(_mReqLocalInfo.Id);
                break;
        }
    }

    /// <summary>
    /// 加载国家数据
    /// </summary>
    private void ReqCountries()
    {

        AccountService.ReqQueryCountries((b, list) =>
        {
            Release();
            if (b)
            {
                LoadSucc(list);
            }
            else
            {
                Log.I("加载国家数据失败");
            }
        } );
    }
    /// <summary>
    /// 加载省份数据
    /// </summary>
    /// <param name="id"></param>
    private void ReqStates(string id)
    {
        AccountService.ReqQueryStates(id, (b, list) =>
        {
            Release();
            if (b)
            {
                LoadSucc(list);
            }
            else
            {
                Log.I("加载省份数据失败");
            }
        } );
    }
    /// <summary>
    /// 加载城市数据
    /// </summary>
    /// <param name="id"></param>
    private void ReqCities(string id)
    {
        AccountService.ReqQueryCities(id, (b, list) =>
        {
            Release();
            if (b)
            {
                LoadSucc(list);
            }
            else
            {
                Log.I("加载城市数据失败");
            }
        });
    }

    private void LoadSucc(List<LocalInfo> list)
    {
        if (mLocalModels == null)
        {
            mLocalModels = new List<LocalModel>();
        }
         mLocalModels.Clear();

        foreach (var info in list)
        {
            var model = new LocalModel()
            {
                Id = info.Id,
                Name = info.Name,
                Type = _mReqLocalInfo.Type
            };

            mLocalModels.Add(model);
        }

        _mReqLocalInfo.LocalInfos = mLocalModels;
        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadLocalDataFinish, _mReqLocalInfo);
    }
}

public class ReqLocalInfo
{

    public LocalType Type;

    public string Id;

    public List<LocalModel> LocalInfos;
}

public enum LocalType
{
    /// <summary>
    /// 国家
    /// </summary>
    Country,
    /// <summary>
    /// 省份
    /// </summary>
    State,
    /// <summary>
    /// 城市
    /// </summary>
    City,
}
