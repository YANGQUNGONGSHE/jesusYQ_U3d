using System.Collections;
using System.Collections.Generic;
using NIM.SysMessage;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqFocusOptionCommand : EventCommand {

	[Inject]
    public IFriendService FriendService { get; set; }
    [Inject]
    public IImService ImService { get; set; }

    private FocusOptionInfo _fInfo;

    public override void Execute()
    {
        Retain();

         _fInfo = (FocusOptionInfo)evt.data;

        switch (_fInfo.Options)
        {
            case FocusOptions.AddFcous:
                AddFocus(_fInfo.Id);
                break;
            case FocusOptions.DeleteFocus:
                DeleteFocus(_fInfo.Id);
                break;
        }
    }
    private void AddFocus(string id)
    {
        FriendService.RequestAddFollow(id, (b,error ,follow) =>
        {
            Release();
            if (b)
            {
                //UIUtil.Instance.ShowSuccToast("关注成功");
                if (_fInfo.IsSysMsg)
                {
                    ImService.SetSysMessagesStatus(_fInfo.MsgId,_fInfo.SysMsgStatus, code =>
                    {
                        Log.I("设置系统消息状态回调："+code);
                    } );
                }
                dispatcher.Dispatch(CmdEvent.Command.ReqFriends);
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        } );
    }

    private void DeleteFocus(string id)
    {
        FriendService.RequestDelectFollow(id, (b, s) =>
        {
            Release();
            if (b)
            {
                //UIUtil.Instance.ShowSuccToast("取消关注成功");
                dispatcher.Dispatch(CmdEvent.Command.ReqFriends);
            }
            else
            {
                UIUtil.Instance.ShowFailToast(s);
            }
            
        });
    }
}

public class FocusOptionInfo
{
    /// <summary>
    /// 类型
    /// </summary>
    public FocusOptions Options;
    /// <summary>
    /// 用户Id
    /// </summary>
    public string Id;
    /// <summary>
    /// 是否是处理系统消息
    /// </summary>
    public bool IsSysMsg;
    /// <summary>
    /// 消息Id(处理系统消息必填)
    /// </summary>
    public long MsgId;
    /// <summary>
    /// 设置系统消息状态(处理系统消息必填)
    /// </summary>
    public NIMSysMsgStatus SysMsgStatus;
}

public enum FocusOptions
{
    /// <summary>
    /// 添加关注
    /// </summary>
    AddFcous,
    /// <summary>
    /// 取消关注
    /// </summary>
    DeleteFocus
}
