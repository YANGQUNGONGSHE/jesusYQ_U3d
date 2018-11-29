using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NIM.SysMessage;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class LoadSystemRecordCommand : EventCommand {


    [Inject]
    public IImService ImService { get; set; }
    [Inject]
    public IFriendService FriendService { get; set; }

    private List<SysTemModel> _sysTemModels;
    private List<SysTemModel> _sysMsModels;
    private NIMSysMsgQueryResult _msgQueryResult;
    private long _mSortTime;

    public override void Execute()
    {
        Retain();
        _mSortTime = (long) evt.data;
        _sysMsModels = new List<SysTemModel>();
        _sysTemModels = new List<SysTemModel>();
        Log.I("********************************开始查询系统消息**************************   时间戳："+ _mSortTime);
        ImService.QuerySystemMessage(20, _mSortTime + 1000000, sysMsResult =>
        {
            Release();
            _msgQueryResult = sysMsResult;
            if (_msgQueryResult != null)
            {

                foreach (var ms in _msgQueryResult.MsgCollection)
                {

                    if(ms.Status == NIMSysMsgStatus.kNIMSysMsgStatusDeleted||
                    ms.Status == NIMSysMsgStatus.kNIMSysMsgStatusInvalid)return;
                    if(ms.MsgType == NIMSysMsgType.kNIMSysMsgTypeCustomP2PMsg ) return;

                        var msg = new SysTemModel()
                        {
                            Time = ms.Timetag,
                            SenderId = ms.SenderId,
                            ReceiverId = ms.ReceiverId,
                            Message = ms.Message,
                            MsgType = ms.MsgType,
                            Status = ms.Status,
                            Id = ms.Id
                        };
                        if (ms.MsgType == NIMSysMsgType.kNIMSysMsgTypeFriendAdd|| ms.MsgType == NIMSysMsgType.kNIMSysMsgTypeFriendDel)
                        {
                            msg.Type = SystemType.FocusOption;
                        }
                        else if (ms.MsgType == NIMSysMsgType.kNIMSysMsgTypeTeamApply ||
                                 ms.MsgType == NIMSysMsgType.kNIMSysMsgTypeTeamReject)
                        {
                            msg.Type = SystemType.ApplyGpOption;
                        }
                        _sysTemModels.Add(msg);
                        Log.I("*********系统消息查询结果条数：" + _sysTemModels.Count);
                }
                Dispatcher.InvokeAsync(LoadSendNameAndHead, _sysTemModels);
            }
        } );
    }

    private void LoadSendNameAndHead(List<SysTemModel> models)
    {
        LinkedList<SysTemModel> modelLinks = new LinkedList<SysTemModel>(models);
        if (modelLinks.First != null)
        {
            HandlerModel(modelLinks.First);
        }
    }

    public void HandlerModel(LinkedListNode<SysTemModel> currentNode)
    {
        FriendService.RequestUserInfoById(currentNode.Value.SenderId, currentNode, (i, user, n) =>
        {
            var node = (LinkedListNode<SysTemModel>)n;
            node.Value.SenderName = user.DisplayName;
            node.Value.SenderSigure = user.Signature;
            node.Value.SendAvatarUrl = user.AvatarUrl;
            node.Value.SenderUserName = user.UserName;
            _sysMsModels.Add(node.Value);
            if (node.Next != null)
            {
                HandlerModel(node.Next); 
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSystemMsFinish, _sysMsModels);
            }
        });
    }
}

public class SysTemModel
{

    /// <summary>
    /// 消息Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 系统消息大类型
    /// </summary>
    public SystemType Type { get; set; }

    private long _mTime;
    /// <summary>
    /// 消息日期
    /// </summary>
    public long Time
    {
        get { return _mTime; }
        set
        {
            _mTime = value;
            _mShowTime = CommUtil.Instance.FormatTime2CostomData(NimUtility.DateTimeConvert.FromTimetag(value));
        }
    }

    private string _mShowTime { get; set; }
    /// <summary>
    /// 显示时间
    /// </summary>
    public string ShowTime
    {
        get
        {
            return _mShowTime;
        }
        private set
        {
            _mShowTime = value;
        }
    }
    /// <summary>
    /// 发送者Id
    /// </summary>
    public string SenderId { get; set; }
    /// <summary>
    /// 发送者昵称
    /// </summary>
    public string SenderName { get; set; }

    public string SenderUserName { get; set; }
    /// <summary>
    /// 接受者Id
    /// </summary>
    public string ReceiverId { get; set; }
    /// <summary>
    /// 接受昵称
    /// </summary>
    public string ReceiverName { get; set; }
    /// <summary>
    /// 附言信息
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 消息内容类型
    /// </summary>
    public NIMSysMsgType MsgType { get; set; }
    /// <summary>
    /// 系统消息状态
    /// </summary>
    public NIMSysMsgStatus Status { get; set; }
    /// <summary>
    /// 发送者头像
    /// </summary>
    public Texture2D HeadTexture2D { get; set; }
    /// <summary>
    /// 发送者个性签名
    /// </summary>
    public string SenderSigure { get; set; }
    /// <summary>
    /// 发送者头像地址
    /// </summary>
    public string SendAvatarUrl { get; set; }

}

public enum SystemType
{
    ApplyGpOption = 0,
    FocusOption = 1

   
}
