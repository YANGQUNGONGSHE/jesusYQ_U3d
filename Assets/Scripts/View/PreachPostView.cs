using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachPostView : iocView
{  
    [HideInInspector] public Button BackButton;
    [HideInInspector] public Button TopExpandButton;
    [HideInInspector] public Button ShareButton;
    [HideInInspector] public Button CommentButton;
    [HideInInspector] public Button LikeButton;
    [HideInInspector] public Button CancelShareButton;
    [HideInInspector] public RectTransform WebRect;
    [HideInInspector] public Text NavTtile;
    [HideInInspector] public Button FocusBut;
    [HideInInspector] public Button DeOrRpBut;
    [HideInInspector] public Button CancelActionBut;
    [HideInInspector] public Image FocusBg;
    [HideInInspector] public RectTransform ActionRectTransform;
    private Image _mLikeBg;
    /// <summary>
    /// 0:
    /// </summary>
    public Sprite[] FocusStatuSprites;
    private GameObject _mShareBar;

    protected override void Awake()
    {
        base.Awake();
        BackButton = transform.Find("NavigationBar/BackButton").GetComponent<Button>();
        TopExpandButton = transform.Find("NavigationBar/OptionButton").GetComponent<Button>();
        ShareButton = transform.Find("BottomBar/ShareBut").GetComponent<Button>();
        CommentButton = transform.Find("BottomBar/CommentBut").GetComponent<Button>();
        LikeButton = transform.Find("BottomBar/LikeBut").GetComponent<Button>();
        CancelShareButton = transform.Find("ShareBar/Container/disShareBut").GetComponent<Button>();
        _mShareBar = transform.Find("ShareBar").gameObject;
        WebRect = transform.Find("WebRect").GetComponent<RectTransform>();
        NavTtile = transform.Find("NavigationBar/Ttile").GetComponent<Text>();
        FocusBut = transform.Find("BottomBar/FocusBut").GetComponent<Button>();
        DeOrRpBut = transform.Find("TopAction/DeOrRpBut").GetComponent<Button>();
        CancelActionBut = transform.Find("TopAction/CancelBut").GetComponent<Button>();
        FocusBg = transform.Find("BottomBar/FocusBut").GetComponent<Image>();
        ActionRectTransform = transform.Find("TopAction").GetComponent<RectTransform>();
        _mLikeBg = transform.Find("BottomBar/LikeBut/Image").GetComponent<Image>();
    }

    public override void OnRender()
    {
        //WebController.Instance.Show();
    }

    public override void OnNoRender()
    {
        //WebController.Instance.Hide();
    }

    public override int GetUiId()
    {
        return (int) UiId.PreachPost;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }

    public void VisibleShareBar(bool visible)
    {
        switch (visible)
        {
            case true:
                _mShareBar.SetActive(true);
                break;
            case false:
                _mShareBar.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 家俊哥加的
    /// </summary>
    /// <param name="title"></param>
    public void SetNavTitle(string title)
    {
        NavTtile.text = title;
    }

    public void IsSelf(bool flag)
    {
        if (flag)
        {
            TopExpandButton.GetComponent<RectTransform>().DOAnchorPosX(102f, 0f);
            FocusBg.sprite = FocusStatuSprites[2];
        }
    }

    public void IsFocused(bool flag)
    {
        FocusBg.sprite = flag ? FocusStatuSprites[1] : FocusStatuSprites[0];
    }

    public void IsVisibleActionRec(bool flag)
    {
        if (flag)
        {
            ActionRectTransform.DOAnchorPosY(0f, 0f);
        }
        else
        {
            ActionRectTransform.DOAnchorPosY(-80f, 0f);
        }
    }

    public void IsLike(bool flag)
    {
        if (flag)
        {
            _mLikeBg.sprite = FocusStatuSprites[3];
            _mLikeBg.color = Color.white;
        }
        else
        {
            _mLikeBg.sprite = FocusStatuSprites[4];
            _mLikeBg.color = new Color(40f / 255f, 60f / 255f, 90f / 255f, 255f / 255f);
        }
    }
}
