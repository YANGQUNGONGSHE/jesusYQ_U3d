using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupManageView : iocView
{



    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button SetManagerBut;
    [HideInInspector] public Button EditorDataBut;
    [HideInInspector] public Text GroupMembersText;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        SetManagerBut = transform.Find("AddManager").GetComponent<Button>();
        EditorDataBut = transform.Find("EditorData").GetComponent<Button>();
        GroupMembersText = transform.Find("AddManager/mamagersCount").GetComponent<Text>();
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatGroupManage;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
