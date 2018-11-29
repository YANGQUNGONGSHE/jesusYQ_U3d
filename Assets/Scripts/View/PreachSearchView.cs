using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachSearchView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public InputField SearchInputField;
    [HideInInspector] public PreachSearchFiler PreachSearchFiler;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        SearchInputField = transform.Find("SearchBar").GetComponent<InputField>();
        PreachSearchFiler = transform.Find("PreachSearchFiler").GetComponent<PreachSearchFiler>();
    }


    public void IsVisibleScrollRect(bool flag)
    {
        PreachSearchFiler.ScrollRect.GetComponent<RectTransform>().DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
    public override int GetUiId()
    {
        return (int) UiId.PreachSearch;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
