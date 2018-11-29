using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class BibleShowView : iocView
{

    [HideInInspector] public Text Title;
    [HideInInspector] public Button ReadPlanBut;
    [HideInInspector] public Button CollectBut;
    [HideInInspector] public Button DirectoryBut;
    [HideInInspector] public Slider FontSizeSlider;
    [HideInInspector] public BibleShowFiler BibleShowFiler;

    [HideInInspector] public Button CopyBut;
    [HideInInspector] public Button LikeBut;
    [HideInInspector] public Button NoteBut;
    [HideInInspector] public Button ShareBut;
    [HideInInspector] public ScrollRect MainScrollRect;

    private RectTransform _mBottomBar;
    private RectTransform _mToolBar;



    protected override void Awake()
    {
        base.Awake();
        Title = transform.Find("TopBar/Title").GetComponent<Text>();
        ReadPlanBut = transform.Find("TopBar/ReadPlan").GetComponent<Button>();
        CollectBut = transform.Find("TopBar/CollectBut").GetComponent<Button>();
        DirectoryBut = transform.Find("BottomBar/DirectoryBut").GetComponent<Button>();
        CopyBut = transform.Find("ToolBar/Tool/CopyButton").GetComponent<Button>();
        LikeBut = transform.Find("ToolBar/Tool/CollectButton").GetComponent<Button>();
        NoteBut = transform.Find("ToolBar/Tool/NoteButton").GetComponent<Button>();
        ShareBut = transform.Find("ToolBar/Tool/ShareButton").GetComponent<Button>();

        BibleShowFiler = transform.Find("BibleShowFiler").GetComponent<BibleShowFiler>();

        FontSizeSlider = transform.Find("BottomBar/Slider/Slider").GetComponent<Slider>();

        _mBottomBar = transform.Find("BottomBar").GetComponent<RectTransform>();
        _mToolBar = transform.Find("ToolBar").GetComponent<RectTransform>();
        MainScrollRect = transform.Find("MainPart").GetComponent<ScrollRect>();

    }


    public void VisibleBottomBar(bool isVisible)
    {
        if (isVisible)
        {
            _mBottomBar.DOAnchorPosY(0f, .3f);
        }
        else
        {
            _mBottomBar.DOAnchorPosY(-90f, .3f);
        }
    }

    public void VisibleToolBar(bool isVisible)
    {
        if (isVisible)
        {
            _mToolBar.DOAnchorPosY(0f, .2f);
        }
        else
        {
            _mToolBar.DOAnchorPosY(-160f, .2f);
        }
    }
    public override int GetUiId()
    {
        return (int) UiId.BibleShow;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
