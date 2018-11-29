using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class BookDetailView : iocView
{
    [HideInInspector] public Button BackBut;
    [HideInInspector] public Text TitleText;
    [HideInInspector] public RectTransform WebRect;
    [HideInInspector] public Button OptionBut;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        TitleText = transform.Find("NavigationBar/Title").GetComponent<Text>();
        WebRect = transform.Find("WebRect").GetComponent<RectTransform>();
        OptionBut = transform.Find("NavigationBar/Option").GetComponent<Button>();
    }

    /// <summary>
    /// 设置导航栏标题
    /// </summary>
    /// <param name="content"></param>
    public void SetTitleUi(string content)
    {
        TitleText.text = content;
    }
    
    public override int GetUiId()
    {
        return (int) UiId.BookDetail;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
