using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class MeView : iocView
{

    [HideInInspector] public Text NickName;
    [HideInInspector] public CircleRawImage HeadImage;
    [HideInInspector] public Button SetBut;
    [HideInInspector] public Button EditorDataBut;
    [HideInInspector] public Button FansAndFocusBut;
    [HideInInspector] public Button LikeBut;
    [HideInInspector] public Button PersonalBut;
    [HideInInspector] public Button CollectBut;
    [HideInInspector] public AccountBookRecordFiler AccountBookRecordFiler;
    public Button ChangeBookBut;
    public Text ReadDaysText;
    public Text ReadParentCount;
    public RectTransform BookLisRectTransform;
    public Button BlBut;
    public Text ChioseBookText;

    protected override void Awake()
    {
        base.Awake();
        NickName = transform.Find("MainPart/Content/PersonalData/BG/NickName").GetComponent<Text>();
        HeadImage = transform.Find("MainPart/Content/PersonalData/BG/HeadImage").GetComponent<CircleRawImage>();
        SetBut = transform.Find("MainPart/Content/Set/BG").GetComponent<Button>();
        EditorDataBut = transform.Find("MainPart/Content/PersonalData/BG/EditorData").GetComponent<Button>();
        FansAndFocusBut = transform.Find("MainPart/Content/FansAndFollow/BG").GetComponent<Button>();
        LikeBut = transform.Find("MainPart/Content/LikeAndCollect/LikeBg").GetComponent<Button>();
        PersonalBut = transform.Find("MainPart/Content/PersonalData/BG").GetComponent<Button>();
        CollectBut = transform.Find("MainPart/Content/LikeAndCollect/CollectBg").GetComponent<Button>();
        AccountBookRecordFiler = transform.Find("AccountBookRecordFiler").GetComponent<AccountBookRecordFiler>();
    }


    public void SetReadRecordCountUi(ReadRecordCount dataCount)
    {
        ReadDaysText.text = dataCount.DaysCount.ToString();
        ReadParentCount.text = dataCount.ViewsCount.ToString();
    }

    public void SetChioseedBookName(string bookName)
    {
        ChioseBookText.text = string.Concat(bookName,"阅读记录");
    }

    public void IsVisibleBookListPart(bool flag)
    {
        if (flag)
        {
            BookLisRectTransform.DOAnchorPosX(0f, 0f);
        }
        else
        {
            BookLisRectTransform.DOAnchorPosX(720f, 0f);
        }
    }
    public override int GetUiId()
    {
        return (int)UiId.Me;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Default;
    }
}
