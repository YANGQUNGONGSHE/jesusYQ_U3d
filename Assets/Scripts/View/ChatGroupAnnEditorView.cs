using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupAnnEditorView : iocView
{


    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button PushBut;
    [HideInInspector] public InputField InputField;
    [HideInInspector] public Text PubshText;
    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        PushBut = transform.Find("NavigationBar/PushBut").GetComponent<Button>();
        InputField = transform.Find("InputField").GetComponent<InputField>();
        PubshText = transform.Find("NavigationBar/PushBut").GetComponent<Text>();
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatGrouAnnEditor;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
