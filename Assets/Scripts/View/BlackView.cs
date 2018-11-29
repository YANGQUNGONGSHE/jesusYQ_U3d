using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class BlackView : iocView
{
    [HideInInspector] public Button BackBut;
    [HideInInspector] public BlackFiler BlackFiler;
    [HideInInspector] public BlockPostFiler BlockPostFiler;
    [HideInInspector] public RectTransform ContenTransform;
    [HideInInspector] public Button ActionBut;
    [HideInInspector] public Button DeleteBlackBut;
    [HideInInspector] public RectTransform ActionTransform;
    [HideInInspector] public RectTransform BlockPosTransform;
    [HideInInspector] public RectTransform BlockPostsContenTransform;
    [HideInInspector] public Toggle BlockUserToggle;
    [HideInInspector] public Toggle BlockPosToggle;


    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        BlackFiler = transform.Find("BlackFiler").GetComponent<BlackFiler>();
        BlockPostFiler = transform.Find("BlockPostFiler").GetComponent<BlockPostFiler>();
        ContenTransform = transform.Find("BlackScrollRect/Content").GetComponent<RectTransform>();
        ActionBut = transform.Find("Action").GetComponent<Button>();
        DeleteBlackBut = transform.Find("Action/Button").GetComponent<Button>();
        ActionTransform = transform.Find("Action").GetComponent<RectTransform>();
        BlockPosTransform = transform.Find("BlockPostScrollRect").GetComponent<RectTransform>();
        BlockPostsContenTransform = transform.Find("BlockPostScrollRect/Content").GetComponent<RectTransform>();
        BlockUserToggle = transform.Find("TogglePart/ToggleBar/UserToggle").GetComponent<Toggle>();
        BlockPosToggle = transform.Find("TogglePart/ToggleBar/PostToggle").GetComponent<Toggle>();
    }

    public override int GetUiId()
    {
        return (int) UiId.Black;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }

    public void IsVisibleContent(bool flag)
    {
        ContenTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    public void IsVisibleAction(bool flag)
    {
        ActionTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    /// <summary>
    /// BlockPostScrollRect是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsVisibleBlockPostScrollRect(bool flag)
    {
        BlockPosTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
    /// <summary>
    /// 被屏蔽帖子的内容Content是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsVisibleBlockPostContent(bool flag)
    {
        BlockPostsContenTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
}
