using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class MyLikePostsView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public LikePostsFiler LikePostsFiler;
   // [HideInInspector] public Button ExpandBut;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackButton").GetComponent<Button>();
        LikePostsFiler = transform.Find("LikePostFiler").GetComponent<LikePostsFiler>();
       // ExpandBut = transform.Find("NavigationBar/OptionButton").GetComponent<Button>();
    }


    public void IsVisibleScrollRect(bool flag)
    {
        LikePostsFiler.ScrollView.GetComponent<RectTransform>().DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
    public override int GetUiId()
    {
        return (int) UiId.MyLikePosts;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
