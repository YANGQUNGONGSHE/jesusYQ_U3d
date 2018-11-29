using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ClassicsView : iocView
{
    public BookFiler BookFiler;

    [SerializeField]
    private PersonalReadRankFiler _personalReadRankFiler;

    public PersonalReadRankFiler PersonalReadRankFiler
    {
        get { return _personalReadRankFiler; }
    }

    [SerializeField]
    private AllGroupReadRankFiler _allGroupReadRankFiler;

    public AllGroupReadRankFiler AllGroupReadRankFiler
    {
        get { return _allGroupReadRankFiler; }
    }

    [SerializeField]
    private AllMyGroupReadRankFiler _allMyGroupReadRankFiler;

    public AllMyGroupReadRankFiler AllMyGroupReadRankFiler
    {
        get { return _allMyGroupReadRankFiler; }
    }

    [SerializeField] private CircleRawImage _rankHeadRawImage;

    public CircleRawImage RankHeadRawImage
    {
        get { return _rankHeadRawImage; }
    }

    [SerializeField] private Text _rankOwnDisplyName;

    public Text RankOwnDisplyName
    {
        get { return _rankOwnDisplyName; }
    }

    [SerializeField] private Text _ownRankNumber;

    public Text OwnRankNumber
    {
        get { return _ownRankNumber; }
    }

    [SerializeField] private Text _changeRankNumber;

    public Text ChangeRankNumber
    {
        get { return _changeRankNumber; }
    }

    public override int GetUiId()
    {
        return (int) UiId.Classics;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Default;
    }

    public void SetOwnRankUi(RankPersonalModel model)
    {
        if (!string.IsNullOrEmpty(model.AvatarUrl))
        {
            HttpManager.RequestImage(model.AvatarUrl + LoadPicStyle.ThumbnailHead, d =>
            {
                RankHeadRawImage.texture = d ? d : DefaultImage.Head;
            });
        }
        else
        {
            RankHeadRawImage.texture = DefaultImage.Head;
        }
        RankOwnDisplyName.text = !string.IsNullOrEmpty(model.DisplayName) ? model.DisplayName : model.UserName;
        OwnRankNumber.text = model.RankNumber.ToString();
        var count = model.LastRankNumber - model.RankNumber;
        ChangeRankNumber.text = count >= 0 ? string.Format("{0}{1}", "+", count) : count.ToString();
    }
}
