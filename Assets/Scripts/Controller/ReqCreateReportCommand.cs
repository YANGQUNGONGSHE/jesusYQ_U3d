using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqCreateReportCommand : EventCommand {

    [Inject]
    public IAccountService AccountService { get; set; }

    public override void Execute()
    {
        Retain();
        var param = evt.data as ReqReportInfo;
        if(param==null)return;
        AccountService.ReqCreateReport(param.ParentType, param.ParentId, param.Reason, (b, error, info) =>
        {
            if (b)
            {
               Log.I("举报成功");
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        });
    }
}
/// <summary>
/// 举报信息
/// </summary>
public class ReqReportInfo
{
    /// <summary>
    /// 上级类型（可选值：用户, 帖子, 评论, 回复）
    /// </summary>
    public string ParentType;
    /// <summary>
    /// 上级编号（如帖子编号）
    /// </summary>
    public string ParentId;
    /// <summary>
    /// 举报理由
    /// </summary>
    public string Reason;
}

