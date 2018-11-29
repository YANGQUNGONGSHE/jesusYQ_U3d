using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class BibleView : iocView
{


    [HideInInspector] public BiographyFiler BiographyFiler;
    [HideInInspector] public ChapterFiler ChapterFiler;
    [HideInInspector] public SementFiler SementFiler;

    [HideInInspector] public Toggle BigraphyToggle;
    [HideInInspector] public Toggle ChapterToggle;
    [HideInInspector] public Toggle SemenToggle;
    [HideInInspector] public Toggle OldBibleToggle;
    [HideInInspector] public Toggle NewBibleToggle;

    [HideInInspector] public Button ReadPlanBut;
    [HideInInspector] public Button ReadRecordBut;

    [HideInInspector] public Text RecordText;
    [HideInInspector] public Text OldNewBibleTitle;

    private RectTransform _mChapteRectTransform;
    private RectTransform _mSementRectTransform;
    private RectTransform _mBiographyTransform;

    private Text _mBiographyText;
    private Text _mChapterText;
    private Text _mSegmentText;

    [HideInInspector] public Text OldBibleText;
    [HideInInspector] public Text NewBibleText;


    



    protected override void Awake()
    {
        base.Awake();
        BiographyFiler = transform.Find("BibleList/BiographyFiler").GetComponent<BiographyFiler>();
        ChapterFiler = transform.Find("BibleList/ChapterScrollRect").GetComponent<ChapterFiler>();
        SementFiler = transform.Find("BibleList/SementScrollRect").GetComponent<SementFiler>();

        BigraphyToggle = transform.Find("ToggleGroup/BiographyToggle").GetComponent<Toggle>();
        ChapterToggle = transform.Find("ToggleGroup/ChapterToggle").GetComponent<Toggle>();
        SemenToggle = transform.Find("ToggleGroup/SegmentToggle").GetComponent<Toggle>();
        OldBibleToggle = transform.Find("BibleList/BiographyScrollRect/ToggleGroup/OldToggle").GetComponent<Toggle>();
        NewBibleToggle = transform.Find("BibleList/BiographyScrollRect/ToggleGroup/NewToggle").GetComponent<Toggle>();

        _mBiographyText = transform.Find("ToggleGroup/BiographyToggle/Label").GetComponent<Text>();
        _mChapterText = transform.Find("ToggleGroup/ChapterToggle/Label").GetComponent<Text>();
        _mSegmentText = transform.Find("ToggleGroup/SegmentToggle/Label").GetComponent<Text>();
        OldNewBibleTitle = transform.Find("BibleList/BibleName").GetComponent<Text>();
        OldBibleText = transform.Find("BibleList/BiographyScrollRect/ToggleGroup/OldToggle/Text").GetComponent<Text>();
        NewBibleText = transform.Find("BibleList/BiographyScrollRect/ToggleGroup/NewToggle/Text").GetComponent<Text>();

        ReadPlanBut = transform.Find("ReadPlan").GetComponent<Button>();
        ReadRecordBut = transform.Find("BibleRecord").GetComponent<Button>();

        RecordText = transform.Find("BibleRecord/RecordText").GetComponent<Text>();

        _mChapteRectTransform = transform.Find("BibleList/ChapterScrollRect").GetComponent<RectTransform>();
        _mSementRectTransform = transform.Find("BibleList/SementScrollRect").GetComponent<RectTransform>();
        _mBiographyTransform = transform.Find("BibleList/BiographyScrollRect").GetComponent<RectTransform>();

        _mBiographyText.color = Color.red;
    }


    public void ClickItemType(int type)
    {
        
        switch (type)
        {
            case 0:
                ChapterToggle.isOn = true;
                _mChapteRectTransform.DOAnchorPosX(0, .2f);
                _mBiographyTransform.DOAnchorPosX(720, .2f);
                _mBiographyText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                _mChapterText.color = new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f);
                break;
            case 1:
                SemenToggle.isOn = true;
                _mSementRectTransform.DOAnchorPosX(0, .2f);
                _mChapteRectTransform.DOAnchorPosX(720, .2f);
                _mChapterText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                _mSegmentText.color = new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f);
                break;
            case 2:
                break;
        }
    }

    public void IsOnToggle(int type,bool isOn)
    {
        if (isOn)
        {
            switch (type)
            {
                case 0:
                    _mBiographyTransform.DOAnchorPosX(0, .2f);
                    _mSementRectTransform.DOAnchorPosX(720, .2f);
                    _mChapteRectTransform.DOAnchorPosX(720, .2f);

                    _mBiographyText.color = new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f);
                    _mChapterText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                    _mSegmentText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                    break;
                case 1:
                    _mChapteRectTransform.DOAnchorPosX(0, .2f);
                    _mBiographyTransform.DOAnchorPosX(720, .2f);
                    _mSementRectTransform.DOAnchorPosX(720, .2f);

                    _mChapterText.color = new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f);
                    _mBiographyText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                    _mSegmentText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                    break;
                case 2:
                    _mSementRectTransform.DOAnchorPosX(0, .2f);
                    _mChapteRectTransform.DOAnchorPosX(720, .2f);
                    _mBiographyTransform.DOAnchorPosX(720, .2f);

                    _mSegmentText.color = new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f);
                    _mChapterText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                    _mBiographyText.color = new Color(25 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
                    break;
            }
        }
        
    }
    public override int GetUiId()
    {
        return (int)UiId.Bible;
    }

    public override int GetLayer()
    {
        return (int)UiLayer.Default;
    }
}
