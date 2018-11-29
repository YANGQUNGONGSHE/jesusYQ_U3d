using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachView : iocView
{
    [HideInInspector]
    public HotPostFiler HotPostFiler;
    [HideInInspector] public Button PublishOption;
    [HideInInspector] public Text LoadMoreTips;
    [HideInInspector] public RectTransform PostListContenTransform;
    [HideInInspector] public RectTransform ActionTransform;
    [HideInInspector] public Button BlockPostBut;
    [HideInInspector] public Button ReportBut;
    [HideInInspector] public Button BlockUserBut;
    [HideInInspector] public Button ActionBut;
    [HideInInspector] public Text AuthorName;
    protected override void Awake()
    {
        base.Awake();
        HotPostFiler = transform.Find("HotPostFiler").GetComponent<HotPostFiler>();
        PublishOption = transform.Find("TopBar/EditorTypeBut").GetComponent<Button>();
        PostListContenTransform = transform.Find("PostListScrollRect/Content").GetComponent<RectTransform>();
        LoadMoreTips = transform.Find("PostListScrollRect/Content/LoadMoreTips").GetComponent<Text>();
        ActionTransform = transform.Find("Action").GetComponent<RectTransform>();
        BlockPostBut = transform.Find("Action/Bg/BlockPostBut").GetComponent<Button>();
        ReportBut = transform.Find("Action/Bg/ReportBut").GetComponent<Button>();
        BlockUserBut = transform.Find("Action/Bg/BlockUserBut").GetComponent<Button>();
        AuthorName = transform.Find("Action/Bg/BlockUserBut/Text").GetComponent<Text>();
        ActionBut = transform.Find("Action").GetComponent<Button>();
    }

    public void IsVisiblePostContent(bool flag)
    {
        PostListContenTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    public void IsVisibleAction(bool flag)
    {
       ActionTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    public void SetBlockAuthorName(string disPlayName)
    {
        AuthorName.text = string.Format("{0}{1}", "拉黑作者:", disPlayName);
    }
    public override int GetUiId()
    {
        return (int)UiId.Preach;
    }
    public override int GetLayer()
    {
        return (int) UiLayer.Default;
    }
    public void LoadStatus(bool flag)
    {
        LoadMoreTips.text = flag ? "已到底部" : "正在加载中...";
    }
}
