using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupTransferMembersView : iocView
{


    [HideInInspector] public Button BackBut;
    [HideInInspector] public ChatGroupTransferfiler ChatGroupTransferfiler;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        ChatGroupTransferfiler = transform.Find("TransferGroupFiler").GetComponent<ChatGroupTransferfiler>();
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatGroupTransMembers;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
