using System.Collections;
using System.Collections.Generic;
using NIM;
using NIM.Session;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqDeleteSessionCommand : EventCommand {

    [Inject]
    public IImService ImService { get; set; }

    public override void Execute()
    {
       Retain();
        var mDataDeletedInfo = (DeletedInfo)evt.data  ;

        switch (mDataDeletedInfo.DeleteType)
        {
            case DeleteType.DeleteSession:

                DeleteSession(mDataDeletedInfo.Id,mDataDeletedInfo.SessionType);
                break;
            case DeleteType.DeleteRecord:

                CleanRecordData(mDataDeletedInfo.Id, mDataDeletedInfo.SessionType);
                break;
        }
    }


    private void DeleteSession(string id, NIMSessionType type)
    {

        ImService.DeleteSession(type, id, (rescode, info, count) =>
        {
            Release();
            if (rescode == 200)
            {
                Log.I("删除会话成功:" + rescode + " " + info.SessionType);
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqDeleteSessionFinish, info);
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqDeleteSessionFail);
            }
        });
    }


    private void CleanRecordData(string id, NIMSessionType type)
    {
        
        ImService.CleanRecordData(type, id, (code, s, nimSessionType) =>
        {
            Release();
            if (code == ResponseCode.kNIMResSuccess)
            {
                UIUtil.Instance.ShowSuccToast("已清空");
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqCleanRecordFinish,new CleanRecordRsInfo(){Type = nimSessionType,Uid = s});
            }
            else
            {
                //dispatcher.Dispatch(CmdEvent.ViewEvent.ReqCleanRecordFail);
                UIUtil.Instance.ShowFailToast("删除会话失败");
            }
        } );
    }
}

public class DeletedInfo
{
    /// <summary>
    /// 类型
    /// </summary>
    public DeleteType DeleteType;
    /// <summary>
    /// 会话类型
    /// </summary>
    public NIMSessionType SessionType;
    /// <summary>
    /// 会话者Id
    /// </summary>
    public string Id;
}

public enum DeleteType
{
    /// <summary>
    /// 删除最近联系人
    /// </summary>
    DeleteSession,
    /// <summary>
    /// 清空聊天记录
    /// </summary>
    DeleteRecord,

}

public class CleanRecordRsInfo
{
    public NIMSessionType Type;

    public string Uid;
}
