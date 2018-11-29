using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class EditorBirthdayView :iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public InputField YEarInputField;
    [HideInInspector] public InputField MonthInputField;
    [HideInInspector] public InputField DayInputField;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        YEarInputField = transform.Find("YearInputField").GetComponent<InputField>();
        MonthInputField = transform.Find("MonthInputField").GetComponent<InputField>();
        DayInputField = transform.Find("DayInputField").GetComponent<InputField>();
    }

    public override int GetUiId()
    {
        return (int) UiId.EditorBirthday;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
