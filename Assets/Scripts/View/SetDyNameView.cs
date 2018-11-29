using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class SetDyNameView : iocView
{

    [SerializeField]
    private Sprite[] _mFlagImages;
    [HideInInspector] public Button SummbitBut;
    [HideInInspector] public InputField DyNameInputField;
    [HideInInspector] public Image ErrorIcon;
    [HideInInspector] public Transform TipTransform;
    [HideInInspector] public RectTransform PhotoOptionTransform;
    [HideInInspector] public Button CancelTakeBut;
    [HideInInspector] public Button TakePhotoBut;
    [HideInInspector] public Button PickPhotoBut;
    [HideInInspector] public Button DefalutPhotoBut;
    [HideInInspector] public CircleRawImage HeadImage;
    [HideInInspector] public Button HeadChioseBut;

    protected override void Awake()
    {
        base.Awake();
        SummbitBut = transform.Find("NavigationBar/OptionButton").GetComponent<Button>();
        DyNameInputField = transform.Find("SetNickNameInput").GetComponent<InputField>();
        ErrorIcon = transform.Find("SetNickNameInput/ErrorIcon").GetComponent<Image>();
        TipTransform = transform.Find("SetNickNameInput/RegistedTip").GetComponent<Transform>();
        PhotoOptionTransform = transform.Find("PhotoOption").GetComponent<RectTransform>();
        CancelTakeBut = transform.Find("PhotoOption/CancelTake").GetComponent<Button>();
        TakePhotoBut = transform.Find("PhotoOption/TakePhoto").GetComponent<Button>();
        PickPhotoBut = transform.Find("PhotoOption/PickPhoto").GetComponent<Button>();
        DefalutPhotoBut = transform.Find("PhotoOption/DefaultPhoto").GetComponent<Button>();
        HeadChioseBut = transform.Find("SetHeadImage").GetComponent<Button>();
        HeadImage = transform.Find("SetHeadImage").GetComponent<CircleRawImage>();
    }


    public void ShowSummbitButEnable(bool flag)
    {
        SummbitBut.interactable = flag;
    }

    public void ShowCheckNameFlag(bool passed)
    {
        ErrorIcon.sprite = _mFlagImages[passed ? 1 : 0];
        TipTransform.gameObject.SetActive(!passed);
    }

    public void IsVisiblePhotoBg(bool flag)
    {
        if (flag)
        {
            PhotoOptionTransform.DOAnchorPosY(0f, AnimationTime());
        }
        else
        {
            PhotoOptionTransform.DOAnchorPosY(-1280f, AnimationTime());
        }
    }

    public override int GetUiId()
    {
        return (int) UiId.SetDyName;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
