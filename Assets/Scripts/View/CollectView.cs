using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class CollectView : iocView
{

    [HideInInspector] public CollectFiler CollectFiler;
    [HideInInspector] public Button BackBut;
    [HideInInspector] public RectTransform LisRectTransform;


    protected override void Awake()
    {
        base.Awake();
        CollectFiler = transform.Find("CollectFiler").GetComponent<CollectFiler>();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        LisRectTransform = transform.Find("CollectScrollRect").GetComponent<RectTransform>();
    }

    public void IsVisibleScrollrect(bool flag)
    {
        LisRectTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    public override int GetUiId()
    {
        return (int) UiId.Collect;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
