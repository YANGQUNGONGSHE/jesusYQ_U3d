using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class AccountSafeView : iocView
{

    [HideInInspector] public Text DisplayName;
    [HideInInspector] public Text PhoneNumber;
    [HideInInspector] public Button ChangeBindPhNumBut;
    [HideInInspector] public Button BackBut;

    protected override void Awake()
    {
        base.Awake();
        DisplayName = transform.Find("DisplayNameBg/DisplayName").GetComponent<Text>();
        PhoneNumber = transform.Find("PhoneNumberBg/PhoneNumber").GetComponent<Text>();
        ChangeBindPhNumBut = transform.Find("ChangeBindNumber").GetComponent<Button>();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
    }


    public void SetUi(string disPlayName,string userName,string phoneNumber)
    {
        DisplayName.text = !string.IsNullOrEmpty(disPlayName) ? disPlayName : userName;
        PhoneNumber.text = phoneNumber;
    }

    public override int GetUiId()
    {
        return (int) UiId.AccountSafe;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
