using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatFriendView : iocView
{
    [SerializeField]
    private ChatFriendFiller _mFiller;
    public ChatFriendFiller Filler
    {
        get { return _mFiller; }
    }

    [SerializeField]
    private Button _mBackButton;
    public Button BackButton
    {
        get { return _mBackButton; }
    }

    [SerializeField] private Button _mSearchButton;
    public Button SearchButton
    {
        get { return _mSearchButton; }
    }

    public override float AnimationTime()
    {
        return .25f;
    }

    public override void OnRender()
    {
        base.OnRender();
        RectTransform.DOAnchorPosX(0, AnimationTime());
    }

    public override void OnNoRender()
    {
        base.OnNoRender();
        RectTransform.DOAnchorPosX(Screen.width, AnimationTime());
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatFriend;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
