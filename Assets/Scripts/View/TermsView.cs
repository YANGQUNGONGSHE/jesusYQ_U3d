using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class TermsView : iocView
{
    [HideInInspector] public Button TermBackBut;
    [HideInInspector] public Text TermsNameText;
    [HideInInspector] public RectTransform WebRect;

    protected override void Awake()
    {
        base.Awake();
        TermBackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        TermsNameText = transform.Find("NavigationBar/Title").GetComponent<Text>();
        WebRect = transform.Find("WebRect").GetComponent<RectTransform>();
    }

    public override int GetUiId()
    {
        return (int) UiId.Terms;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }

    public void SetTermName(string termName)
    {
        TermsNameText.text = termName;
    }
}
