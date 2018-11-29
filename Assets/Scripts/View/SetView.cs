using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class SetView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button LoginOutBut;
    [HideInInspector] public Button AccountSafeBut;
    [HideInInspector] public RectTransform LoginOuTransform;
    [HideInInspector] public Button SureLoginOutBut;
    [HideInInspector] public Button CancelLoginOutBut;
    [HideInInspector] public Button CancelBgBut;
    [HideInInspector] public Button FeedBackBut;
    [HideInInspector] public Button BlackBut;


    protected override void Awake()
    {
        base.Awake();

        BackBut = transform.Find("TopBar/BackBut").GetComponent<Button>();
        LoginOutBut = transform.Find("QuitAccount").GetComponent<Button>();
        AccountSafeBut = transform.Find("AccountSafe").GetComponent<Button>();
        LoginOuTransform = transform.Find("LoginOutAction").GetComponent<RectTransform>();
        SureLoginOutBut = transform.Find("LoginOutAction/LoginOutBut").GetComponent<Button>();
        CancelLoginOutBut = transform.Find("LoginOutAction/CancelBut").GetComponent<Button>();
        CancelBgBut = transform.Find("LoginOutAction").GetComponent<Button>();
        FeedBackBut = transform.Find("FeedBackCenter").GetComponent<Button>();
        BlackBut = transform.Find("BlackSet").GetComponent<Button>();
    }

    public void IsVisibleLoginOutBg(bool flag)
    {
        if (flag)
        {
            LoginOuTransform.DOAnchorPosY(0f, AnimationTime());
        }
        else
        {
            LoginOuTransform.DOAnchorPosY(-1280f, AnimationTime());
        }
    }

    public override int GetUiId()
    {
        return (int) UiId.Setting;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
