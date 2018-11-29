using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ReportView : iocView
{

    [HideInInspector] public Button BackBut;
    [HideInInspector] public ReportFiler ReportFiler;
    [HideInInspector] public Button CommitBut;
    private RectTransform _mDisAction;
    private CircleRawImage _mHeadImage;
    private Text _mName;
    private Text _mSignatureText;
    private Text _mCommitTips;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        ReportFiler = transform.Find("ReportInfoBg").GetComponent<ReportFiler>();
        CommitBut = transform.Find("ReportInfoBg/CommitBut").GetComponent<Button>();
        _mHeadImage = transform.Find("HeadImage").GetComponent<CircleRawImage>();
        _mName = transform.Find("NickName").GetComponent<Text>();
        _mSignatureText = transform.Find("Signature").GetComponent<Text>();
        _mCommitTips = transform.Find("ReportInfoBg/CommitBut/Text").GetComponent<Text>();
        _mDisAction = transform.Find("DisAction").GetComponent<RectTransform>();
    }


    public void SetUi(ReportedUserModel model)
    {

        _mName.text = !string.IsNullOrEmpty(model.DisplyName) ? model.DisplyName : model.UserName;
        _mSignatureText.text = model.Signature;
        if (model.HeadTexture2D == null)
        {
            if (!string.IsNullOrEmpty(model.HeadUrl))
            {
                HttpManager.RequestImage(model.HeadUrl, texture2D =>
                {
                    if (texture2D)
                        _mHeadImage.texture = texture2D;
                } );
            }
        }
        else
        {
            _mHeadImage.texture = model.HeadTexture2D;
        }
    }

    public void SetCommitTips()
    {
        _mDisAction.DOAnchorPosX(0f, 0f);
        _mCommitTips.text = "已举报，受理中";
        CommitBut.interactable = false;
    }
    public override int GetUiId()
    {
        return (int) UiId.Report;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
