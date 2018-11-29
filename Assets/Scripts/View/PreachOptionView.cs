using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachOptionView : iocView
{
    [HideInInspector] public Button ArticleButton;
    [HideInInspector] public Button VideoButton;
    [HideInInspector] public Button CancelButton;

    protected override void Awake()
    {
        base.Awake();
        ArticleButton = transform.Find("ArticleOption").GetComponent<Button>();
        VideoButton = transform.Find("VideoOption").GetComponent<Button>();
        CancelButton = transform.Find("Cancel").GetComponent<Button>();
    }

    public override int GetUiId()
    {
        return (int) UiId.PreachEditorOption;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
