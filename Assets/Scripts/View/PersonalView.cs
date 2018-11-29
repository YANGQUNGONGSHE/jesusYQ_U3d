using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class PersonalView : iocView
{
    
    #region 属性

    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button ExpandBut;
    [HideInInspector] public Button PersonalMsBut;
    [HideInInspector] public Button AddFocusBut;
    [HideInInspector] public Button SerachBut;
    [HideInInspector] public Button CloseExpand;
    [HideInInspector] public Button CancelExpand;
    [HideInInspector] public Button CancelFocusBut;
    [HideInInspector] public Button ReportBut;
    [HideInInspector] public Button SetBlackBut;
    [HideInInspector] public Text BibleDay;
    [HideInInspector] public Text PosterName;
    [HideInInspector] public Text PersonalSignature;
    [HideInInspector] public Text FansCount;
    [HideInInspector] public Text FollowCount;
    [HideInInspector] public Text PostCount;
    [HideInInspector] public Text ReadDaysCount;
    [HideInInspector] public Text BlackText;
    [HideInInspector] public CircleRawImage HeadImage;

    [HideInInspector] public PersonalPostFiler PersonalPostFiler;
    [HideInInspector] public ScrollRect MainRect;
    [HideInInspector] public RectTransform ExpandAction;
    [HideInInspector] public RectTransform SocialRectTransform;
    [HideInInspector] public RectTransform EditorDataRectTransform;
    [HideInInspector] public RectTransform ScrollContenTransform;
    [HideInInspector] public GameObject CancelFocusRectTransform;
    [HideInInspector] public Button EditorDataBut;
    [HideInInspector] public Texture2D HeadTexture2D;
    public Image FocusTips;
    public Sprite[] FocuSprites;
     #endregion
    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackButton").GetComponent<Button>();
        ExpandBut = transform.Find("NavigationBar/ExpandButton").GetComponent<Button>();
        PersonalMsBut = transform.Find("MainPart/Content/PersonalInfoPart/Container/SocialPart/PersonalMs").GetComponent<Button>();
        AddFocusBut = transform.Find("MainPart/Content/PersonalInfoPart/Container/SocialPart/AddFocus").GetComponent<Button>();
        CancelFocusBut = transform.Find("ExpandAction/CancelFocus").GetComponent<Button>();
        SerachBut = transform.Find("NavigationBar/Search").GetComponent<Button>();
        ReportBut = transform.Find("ExpandAction/Report").GetComponent<Button>();
        SetBlackBut = transform.Find("ExpandAction/SetBlackBut").GetComponent<Button>();
        BlackText = transform.Find("ExpandAction/SetBlackBut/Text").GetComponent<Text>();
        HeadImage = transform.Find("MainPart/Content/PersonalInfoPart/Container/HeadImage").GetComponent<CircleRawImage>();
        BibleDay = transform.Find("NavigationBar/Ttile").GetComponent<Text>();
        PosterName = transform.Find("MainPart/Content/PersonalInfoPart/Container/DisplayName").GetComponent<Text>();
        PersonalSignature = transform.Find("MainPart/Content/PersonalInfoPart/Container/Signature").GetComponent<Text>();
        FansCount = transform.Find("MainPart/Content/PersonalInfoPart/Container/FocusCount/Count").GetComponent<Text>();
        FollowCount = transform.Find("MainPart/Content/PersonalInfoPart/Container/FansCount/Count").GetComponent<Text>();
        PostCount  = transform.Find("MainPart/Content/PostColumn/Image/PostCount").GetComponent<Text>();
        SocialRectTransform = transform.Find("MainPart/Content/PersonalInfoPart/Container/SocialPart").GetComponent<RectTransform>();
        EditorDataRectTransform = transform.Find("MainPart/Content/PersonalInfoPart/Container/EditorBut").GetComponent<RectTransform>();
        ScrollContenTransform = transform.Find("MainPart/Content/PostListPart/PersonalListScrollRect/Content").GetComponent<RectTransform>();
        CancelFocusRectTransform = transform.Find("ExpandAction/CancelFocus").gameObject;

        PersonalPostFiler = transform.Find("PersonalPostFiler").GetComponent<PersonalPostFiler>();
        MainRect = transform.Find("MainPart").GetComponent<ScrollRect>();
        ExpandAction = transform.Find("ExpandAction").GetComponent<RectTransform>();
        CloseExpand = transform.Find("ExpandAction").GetComponent<Button>();
        CancelExpand = transform.Find("ExpandAction/Cancel").GetComponent<Button>();
        EditorDataBut = transform.Find("MainPart/Content/PersonalInfoPart/Container/EditorBut").GetComponent<Button>();
        ReadDaysCount = transform.Find("NavigationBar/Ttile").GetComponent<Text>();
    }


    public void SetDefaultUi(PostModel model)
    {
        PosterName.text = !string.IsNullOrEmpty(model.Author.DisplayName) ? model.Author.DisplayName : model.Author.UserName;
        PersonalSignature.text = model.Author.Signature;
        if (model.HeadTexture2D==null)
        {
            if (!string.IsNullOrEmpty(model.Author.AvatarUrl))
            {
                HttpManager.RequestImage(model.Author.AvatarUrl + LoadPicStyle.ThumbnailHead, text2D =>
                {
                    if (text2D != null)
                        HeadImage.texture = text2D;
                        HeadTexture2D = text2D;
                });
            }
            else
            {
                HeadImage.texture = DefaultImage.Head;
                HeadTexture2D = DefaultImage.Head;
            }
        }
        else
        {
            HeadImage.texture = model.HeadTexture2D;
            HeadTexture2D = model.HeadTexture2D;
        }
    }
    public void SetFansAndFollowerUi(string fansCount,string followerCount)
    {
        FansCount.text = followerCount;
        FollowCount.text = fansCount;
    }

    public void SetReadDaysUi(string days)
    {
        ReadDaysCount.text = string.Format("{0}{1}{2}", "已坚持读经",days,"天");
    }

    public void IsVisibleExpandAction(bool flag)
    {
        ExpandAction.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    public void IsFocus(bool flag)
    {
        if (flag)
        {
            FocusTips.sprite = FocuSprites[1];
            AddFocusBut.interactable = false;
        }
        else
        {
            FocusTips.sprite = FocuSprites[0];
            AddFocusBut.interactable = true;
        }
    }

    public void IsVisibleSocialPart(bool flag)
    {
        if (flag)
        {
            SocialRectTransform.DOAnchorPosX(-30f, 0f);
        }
        else
        {
            SocialRectTransform.DOAnchorPosX(232f, 0f);
        }
    }

    public void IsVisibleEditorRect(bool flag)
    {
        if (flag)
        {
            EditorDataRectTransform.DOAnchorPosX(-30f, 0f);
        }
        else
        {
            EditorDataRectTransform.DOAnchorPosX(180f, 0f);
        }
    }

    public void IsVisibleScrollContent(bool flag)
    {
        ScrollContenTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }

    public void IsVisibleCancelFocusRecTrans(bool flag)
    {
        //CancelFocusRectTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
        CancelFocusRectTransform.SetActive(flag);
    }

    public void DisVisibleTopAction()
    {
        ExpandBut.GetComponent<RectTransform>().DOAnchorPosX(100f, 0f);
    }

    public void BlackTextStaus(bool flag)
    {
        Log.I("判断是否在黑名单：：："+ flag);
        BlackText.text = flag ? "解除黑名单" : "拉入黑名单";
    }

    public override int GetUiId()
    {
        return (int) UiId.Personal;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
