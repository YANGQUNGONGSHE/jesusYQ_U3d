using NIM;
using strange.extensions.command.impl;

/// <summary>
/// 根据消息Id查询聊天记录
/// </summary>
public class LoadChatRecordByMsgIdCommand : EventCommand
{
    [Inject]
    public IImService ImService { get; set; }

    private ArgLoadSingleChatRecord _mArg;
    public override void Execute()
    {
        Retain();
        _mArg = (ArgLoadSingleChatRecord) evt.data;
        ImService.QuerySingleRecord(_mArg.MsgId, (code, id, msg) =>
        {
            _mArg.RetCode = code;
            _mArg.RetMsg = msg;
            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSingleChatRecordFinish,_mArg);
        });
    }
}

public struct ArgLoadSingleChatRecord
{
    /// <summary>
    /// 消息ID
    /// 必填
    /// </summary>
    public string MsgId;

    /// <summary>
    /// 返回状态码
    /// </summary>
    public ResponseCode RetCode;
    /// <summary>
    /// 返回信息
    /// </summary>
    public NIMIMMessage RetMsg;
}