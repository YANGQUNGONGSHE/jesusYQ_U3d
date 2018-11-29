using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class EditorNameOrSignatureView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button FinishBut;
    [HideInInspector] public Text TypeText;
    [HideInInspector] public InputField InputField;
    [HideInInspector] public Text TextCount;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar_3/BackButton").GetComponent<Button>();
        FinishBut = transform.Find("NavigationBar_3/OptionButton").GetComponent<Button>();
        TypeText = transform.Find("NavigationBar_3/Ttile").GetComponent<Text>();
        InputField = transform.Find("InputField").GetComponent<InputField>();
        TextCount = transform.Find("InputField/TextCount").GetComponent<Text>();

    }

    public override int GetUiId()
    {
        return (int) UiId.EditorNameOrSignature;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
