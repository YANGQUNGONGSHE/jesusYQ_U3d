using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class FansAndFocusView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public Toggle FocusToggle;
    [HideInInspector] public Toggle FansToggle;
    [HideInInspector] public FansAndFocusFiler FansAndFocusFiler;
    [HideInInspector] public Text FocusLabel;
    [HideInInspector] public Text FansLabel;
    [HideInInspector] public Button SearchButton;
    [HideInInspector] public RectTransform ScRectTransform;


    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        FocusToggle = transform.Find("NavigationBar/TitleBar/FocusToggle").GetComponent<Toggle>();
        FansToggle = transform.Find("NavigationBar/TitleBar/FansToggle").GetComponent<Toggle>();
        FansAndFocusFiler = transform.Find("FansOrFocusFiler").GetComponent<FansAndFocusFiler>();
        FocusLabel = transform.Find("NavigationBar/TitleBar/FocusToggle/Text").GetComponent<Text>();
        FansLabel = transform.Find("NavigationBar/TitleBar/FansToggle/Text").GetComponent<Text>();
        SearchButton = transform.Find("SearchBar").GetComponent<Button>();
        ScRectTransform = transform.Find("FansAndFocusScrollRect").GetComponent<RectTransform>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">0 表示关注Toggle,1 表示FansToggle</param>
    public void ToggleLabelColor(int type)
    {
        switch (type)
        {
            case 0:
                FocusLabel.color = new Color(255/255f,110/255f,107/255f,255/255f);
                FansLabel.color = new Color(25/255f,25/255f,25/255f,255/255f);
                break;
            case 1:
                FansLabel.color = new Color(255/255f, 110/255f, 107/255f, 255/255f);
                FocusLabel.color = new Color(25/255f, 25/255f, 25/255f, 255/255f);
                break;
        }
    }

    public void IsVisibleScroll(bool flag)
    {
        if (flag)
        {
            ScRectTransform.DOAnchorPosX(0f, 0f);
        }
        else
        {
            ScRectTransform.DOAnchorPosX(-720f, 0f);
        }
    }
    public override int GetUiId()
    {
        return (int) UiId.FocusAndFans;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
