using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupMemberView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button ManageBut;
    [HideInInspector] public Text MemberCout;
    [HideInInspector] public ChatGroupMemberFiler ChatGroupMemberFiler;
    [HideInInspector] public RectTransform BottomBarRectTransform;
    [HideInInspector] public Button TakeOutBut;
    [HideInInspector] public Button CancelBut;
    [HideInInspector] public Text ManageText;
    [HideInInspector] public Button SearchBut;


    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        ManageBut = transform.Find("NavigationBar/manageBut").GetComponent<Button>();
        MemberCout = transform.Find("NavigationBar/Title").GetComponent<Text>();
        ManageText = transform.Find("NavigationBar/manageBut").GetComponent<Text>();
        ChatGroupMemberFiler = transform.Find("ChatGroupMemberFiler").GetComponent<ChatGroupMemberFiler>();
        BottomBarRectTransform = transform.Find("BottomBar").GetComponent<RectTransform>();
        TakeOutBut = transform.Find("BottomBar/Image/TakeOutBut").GetComponent<Button>();
        CancelBut = transform.Find("BottomBar/Image/CancelBut").GetComponent<Button>();
        SearchBut = transform.Find("SearchBar").GetComponent<Button>();

    }

    public override int GetUiId()
    {
        return (int) UiId.ChatGroupMember;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
