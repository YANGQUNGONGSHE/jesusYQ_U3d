using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class FeedBackView :iocView
{
    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button CommitBut;
    [HideInInspector] public InputField InputField;
    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        CommitBut = transform.Find("NavigationBar/OptionBut").GetComponent<Button>();
        InputField = transform.Find("InputField").GetComponent<InputField>();
    }

    public override int GetUiId()
    {
        return (int) UiId.FeedBack;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
