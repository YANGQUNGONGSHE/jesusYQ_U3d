using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSetManagerView : iocView
{


    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button TakeOutBut;
    [HideInInspector] public Button AddManagerBut;
    [HideInInspector] public Text ManagerCount;
    [HideInInspector] public GameObject Tip;
    [HideInInspector] public GameObject ManagerContent;
    [HideInInspector] public ChatSetGroupManagerFiler ChatSetGroupManagerFiler;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        TakeOutBut = transform.Find("NavigationBar/TakeOutBut").GetComponent<Button>();
        AddManagerBut = transform.Find("MainPart/AddManagerBut").GetComponent<Button>();
        ManagerCount = transform.Find("MainPart/AddManagerBut/Image/ManagerCount").GetComponent<Text>();
        ChatSetGroupManagerFiler = transform.Find("ChatSetGroupManagerFiler").GetComponent<ChatSetGroupManagerFiler>();
        Tip = transform.Find("MainPart/Tip").gameObject;
        ManagerContent = transform.Find("MainPart/ManagerListScrollRect/Content").gameObject;

    }


    public void SetManagerCount(int count)
    {
        ManagerCount.text = count + "/5";
    }
    public override int GetUiId()
    {
        return (int) UiId.ChatGroupSetManager;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
