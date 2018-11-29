using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqReadRecordCommand : EventCommand {

    [Inject] public IAccountService AccountService { get; set; }

    public override void Execute()
    {
        Retain();
        var param = evt.data as ReadRecordData;
        if(param==null)return;
        switch (param.Type)
        {
            case QueryReadRecordType.QueryReadCount:
              
                QueryReadCount(param.UserId, param.ParentType, param.ParentidPrefix);
                break;
            case QueryReadRecordType.QueryReadDetail:
                QueryReadDetail(param.UserId,param.ParentType, param.ParentidPrefix, param.Skip, param.Limit);
                break;
        }
    }

    private void QueryReadCount(string id,string parentpype, string parentidprefix)
    {
        AccountService.ReqQueryUserReadRecordCount(id,parentpype,parentidprefix, (b, error, info) =>
        {
            Release();
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.QueryUserReadRecordCoutFinish, info);
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        });
    }

    private void QueryReadDetail(string id,string parenttype,string parentidprefix,string skip,string limit)
    {
        AccountService.ReqQueryUserReadRecord(id,parenttype,parentidprefix,"true",skip,limit, (b, error, list) =>
        {
            Release();
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.QueryUserReadRecordDetailFinish,list);
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        });
    }
}

public class ReadRecordData
{
    /// <summary>
    /// 查询类型
    /// </summary>
    public QueryReadRecordType Type;
    /// <summary>
    /// 用户Id
    /// </summary>
    public string UserId;
    /// <summary>
    /// 上级类型（可选值：帖子, 章, 节）
    /// </summary>
    public string ParentType;
    /// <summary>
    /// 上级编号的前缀（如帖子编号）
    /// </summary>
    public string ParentidPrefix;
    /// <summary>
    /// 忽略的行数(请求记录详情必填)
    /// </summary>
    public string Skip;
    /// <summary>
    /// 获取的行数(请求记录详情必填)
    /// </summary>
    public string Limit;

}
public enum QueryReadRecordType
{
    QueryReadCount,
    QueryReadDetail,
}
