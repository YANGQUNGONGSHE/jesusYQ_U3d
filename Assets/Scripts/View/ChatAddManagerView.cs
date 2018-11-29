using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatAddManagerView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button AddManagerBut;
    [HideInInspector] public ChatAddGroupManagerFiler ChatAddGroupManagerFiler;
   
    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        AddManagerBut = transform.Find("NavigationBar/AddBut").GetComponent<Button>();
        ChatAddGroupManagerFiler = transform.Find("ChatAddGroupManagerFiler").GetComponent<ChatAddGroupManagerFiler>();
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatAddManager;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
