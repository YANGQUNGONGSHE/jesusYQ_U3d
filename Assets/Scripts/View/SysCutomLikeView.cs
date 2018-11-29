using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class SysCutomLikeView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public SysLikeFiler SysLikeFiler;
    [HideInInspector] public RectTransform ScrollRectTransform;
    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        SysLikeFiler = transform.Find("SysLikeFiler").GetComponent<SysLikeFiler>();
        ScrollRectTransform = transform.Find("SysLikeRect").GetComponent<RectTransform>();
    }


    public void IsVisible(bool flag)
    {
        ScrollRectTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
    public override int GetUiId()
    {
        return (int) UiId.SysCustomLike;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
