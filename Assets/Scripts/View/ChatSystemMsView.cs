using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSystemMsView : iocView
{


    [HideInInspector] public Button BackBut;
    [HideInInspector] public ChatSystemFiler ChatSystemFiler;
    [HideInInspector] public RectTransform ApplyDealWindowTransform;
    [HideInInspector] public Button RejectApplyBut;
    [HideInInspector] public Button AgreeApplyBut;
    [HideInInspector] public CircleRawImage ApplyHeadImage;
    [HideInInspector] public Text ApplyerName;
    [HideInInspector] public Text SingureText;
    [HideInInspector] public Button OptionBut;


    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        ChatSystemFiler = transform.Find("ChatSysMsFiler").GetComponent<ChatSystemFiler>();
        ApplyDealWindowTransform = transform.Find("applyDealWindow").GetComponent<RectTransform>();
        RejectApplyBut = transform.Find("applyDealWindow/bg/RejectBut").GetComponent<Button>();
        AgreeApplyBut = transform.Find("applyDealWindow/bg/AgreeApply").GetComponent<Button>();
        ApplyHeadImage = transform.Find("applyDealWindow/bg/headImage").GetComponent<CircleRawImage>();
        ApplyerName = transform.Find("applyDealWindow/bg/displayName").GetComponent<Text>();
        SingureText = transform.Find("applyDealWindow/bg/Singure").GetComponent<Text>();
        OptionBut = transform.Find("NavigationBar/OptionButton").GetComponent<Button>();

    }
    /// <summary>
    /// 申请处理弹窗是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsvibileApplyDealWindow(bool flag)
    {
        if (flag)
        {
            ApplyDealWindowTransform.DOAnchorPosY(0f, AnimationTime());
        }
        else
        {
            ApplyDealWindowTransform.DOAnchorPosY(-1280f, AnimationTime());
        }
    }
    /// <summary>
    /// 设置申请加群处理Ui
    /// </summary>
    /// <param name="head"></param>
    /// <param name="disPlayName"></param>
    /// <param name="signature"></param>
    public void SetApplyDealUi( Texture2D head,string disPlayName,string signature)
    {
        ApplyHeadImage.texture = head;
        ApplyerName.text = disPlayName;
        SingureText.text = signature;
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatSysTemMs;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
