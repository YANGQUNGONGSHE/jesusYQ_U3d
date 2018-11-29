using System.Collections;
using System.Collections.Generic;
using NIM;
using NIM.Session;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class MarkMessagesStatusCommand : EventCommand {

    [Inject]
    public IImService ImService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    public override void Execute()
    {
        Retain();
        var param = (MessagesTypeInfo)evt.data;

        switch (param.MarkType)
        {
            case MarkType.MarkMessagesStaus:

                MarkMessagsStatus(param.Id,param.Type);
                break;
            case MarkType.DeleteAllSystemMessages:

                DeleteAllSysMsg();
                break;
        }
       
    }

    private void MarkMessagsStatus(string id, NIMSessionType sessionType)
    {
        ImService.MarkMessagesStatusRead(id, sessionType, (code, s, type) =>
        {
            Release();
            if (code == ResponseCode.kNIMResSuccess)
            {
                Dispatcher.InvokeAsync(dispatcher.Dispatch, (object)CmdEvent.Command.LoadSession); //更新
            }
        });
    }

    private void DeleteAllSysMsg()
    {
        ImService.DeleteAllSys(code =>
        {
            Log.I("删除全部系统消息回调:"+code);
            Dispatcher.InvokeAsync(DeleteSysTime);
            Dispatcher.InvokeAsync(dispatcher.Dispatch,(object)CmdEvent.Command.LoadSession);
        } );
    }

    private void DeleteSysTime()
    {
        UserModel.LastSysTime = null;
        string sysTimePath = Application.persistentDataPath + "/" + LocalDataObjKey.SysLastTimeTag;
        if (System.IO.File.Exists(sysTimePath))
        {
            System.IO.File.Delete(sysTimePath);
        }
    }
}

public class MessagesTypeInfo
{

    public string Id;

    public NIMSessionType Type;

    public MarkType MarkType;
}

public enum MarkType
{
    /// <summary>
    /// 批量设置消息状态(非系统消息)
    /// </summary>
    MarkMessagesStaus,
    /// <summary>
    /// 删除全部系统消息
    /// </summary>
    DeleteAllSystemMessages,
}
