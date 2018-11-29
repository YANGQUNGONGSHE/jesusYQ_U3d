using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class SysCustomCommentView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public SysCommentFiler SysCommentFiler;
    [HideInInspector] public RectTransform ScrollRectTransform;
    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        SysCommentFiler = transform.Find("SysCommentFiler").GetComponent<SysCommentFiler>();
        ScrollRectTransform = transform.Find("SysCommentRect").GetComponent<RectTransform>();
    }

    public void IsVisibleScrollView(bool flag)
    {
        ScrollRectTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
    public override int GetUiId()
    {
        return (int) UiId.SysCustomComment;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
