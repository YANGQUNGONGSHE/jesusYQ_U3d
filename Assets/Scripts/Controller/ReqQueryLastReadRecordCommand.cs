using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqQueryLastReadRecordCommand : EventCommand {

    [Inject]public IAccountService AccountService { get; set; }
    public override void Execute()
    {
        Retain();

        var bookId = (string)evt.data;
        AccountService.ReqQueryLastReadRecordData(bookId, (b, error, info) =>
        {
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqQueryLastReadRecordSucc,info);
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqQueryLastReadRecordFail, error);
            }
        } );
    }
}
