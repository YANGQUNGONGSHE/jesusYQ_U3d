using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqCreateFeedBackCommand : EventCommand {

    [Inject] public IAccountService AccountService { get; set; }
    public override void Execute()
    {
        Retain();
        var content = (string)evt.data;
        AccountService.ReqCreateFeedBack(content, (b, s) =>
        {
            Release();
            UIUtil.Instance.ShowTextToast(b ? "感谢您的反馈" : s);
        });
    }
}
