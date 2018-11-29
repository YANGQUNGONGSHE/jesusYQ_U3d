using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class ReportViewMediator : EventMediator {

    [Inject]
    public ReportView ReportView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private string _mReportMessage;
    private List<string> _mReportList;

    public override void OnRegister()
    {
        ReportView.BackBut.onClick.AddListener(BackClick);
        ReportView.CommitBut.onClick.AddListener(CommitClick);
        ReportView.ReportFiler.OnCellClick = OnCellClick;
        LoadReportData();
    }

    private void LoadReportData()
    {
        ReportView.SetUi(UserModel.ReportedUserModel);
        _mReportList = new List<string>()
        {
            "垃圾营销","不实信息","违法信息","有害信息","淫秽色情","欺诈骗局","冒充我","抄袭我的内容","人身攻击我","泄露我的隐私"
        };
        ReportView.ReportFiler.DataSource = _mReportList;
        ReportView.ReportFiler.Refresh();
    }

    private void OnCellClick(int index, string content)
    {
        _mReportMessage = content;
        ReportView.ReportFiler.IsSelected = index;
    }
    private void CommitClick()
    {
        if (!string.IsNullOrEmpty(_mReportMessage))
        {
            switch (UserModel.ReportedUserModel.ReportType)
            {
                case ReportType.User:
                    dispatcher.Dispatch(CmdEvent.Command.ReqCreateReport, new ReqReportInfo()
                    {
                        ParentType = "用户",
                        ParentId = UserModel.ReportedUserModel.ParentId,
                        Reason = _mReportMessage
                    });
                    break;
                case ReportType.Post:
                    dispatcher.Dispatch(CmdEvent.Command.ReqCreateReport, new ReqReportInfo()
                    {
                        ParentType = "帖子",
                        ParentId = UserModel.ReportedUserModel.ParentId,
                        Reason = _mReportMessage
                    });
                    break;
            }
            ReportView.SetCommitTips();
        }
        else
        {
            UIUtil.Instance.ShowTextToast("请选择举报理由");
        }
    }
    private void BackClick()
    {
        switch (UserModel.ReportedUserModel.FromReportViewType)
        {
            case FromReportViewType.ChatMain:
                iocViewManager.DestroyAndOpenNew(ReportView.GetUiId(),(int)UiId.ChatMain);
                break;
            case FromReportViewType.Personal:
                iocViewManager.DestroyAndOpenNew(ReportView.GetUiId(), (int)UiId.Personal);
                break;
            case FromReportViewType.PreachPost:
                iocViewManager.DestroyAndOpenNew(ReportView.GetUiId(), (int)UiId.PreachPost);
                break;
            case FromReportViewType.Preach:
                iocViewManager.DestroyAndOpenNew(ReportView.GetUiId(), (int)UiId.Preach);
                break;
        }
    }
    public override void OnRemove()
    {
        ReportView.BackBut.onClick.RemoveListener(BackClick);
        ReportView.CommitBut.onClick.RemoveListener(CommitClick);
        ReportView.ReportFiler.OnCellClick -= OnCellClick;
        _mReportList.Clear();
        _mReportList = null;
        _mReportMessage = null;
    }

    private void OnDestroy()
    {
        OnRemove();
    }
}
/// <summary>
/// 举报类型
/// </summary>
public enum ReportType
{
    /// <summary>
    /// 举报用户
    /// </summary>
    User,
    /// <summary>
    /// 举报帖子
    /// </summary>
    Post,
    /// <summary>
    /// 举报评论
    /// </summary>
    Comment,
    /// <summary>
    /// 举报回复评论
    /// </summary>
    Reply
}


