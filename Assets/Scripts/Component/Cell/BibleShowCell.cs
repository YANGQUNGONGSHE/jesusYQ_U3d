using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class BibleShowCell : BaseCell<Segment>
{

    [HideInInspector]
    public Text SegmentContent;

    private Button _mBeginReadBut;
    private Button _mEndReadBut;
    private RectTransform _mStartReadLineIcon;
    private RectTransform _mEndReadLineIcon;
    private RectTransform _mStartReadIcon;
    private RectTransform _mEndReadIcon;

    public Action<int> OnBeginReaderClick;
    public Action<int> OnEndReaderClick;

    protected override void OnAwake()
    {
        SegmentContent = transform.Find("ContextContainer/Context/Text").GetComponent<Text>();
        _mBeginReadBut = transform.Find("BtnBeginReader").GetComponent<Button>();
        _mEndReadBut = transform.Find("BtnEndReader").GetComponent<Button>();

        _mStartReadLineIcon = transform.Find("BtnBeginReader/LineIcon").GetComponent<RectTransform>();
        _mEndReadLineIcon = transform.Find("BtnEndReader/LineIcon").GetComponent<RectTransform>();

        _mStartReadIcon = transform.Find("BtnBeginReader/PlayIcon/Image").GetComponent<RectTransform>();
        _mEndReadIcon = transform.Find("BtnEndReader/EndIcon/Image").GetComponent<RectTransform>();

        AddListener();

    }

    private void AddListener()
    {
        _mBeginReadBut.onClick.AddListener(() =>
        {
            if (OnBeginReaderClick != null)
            {
                OnBeginReaderClick(MIndex);
            }
        });

        _mEndReadBut.onClick.AddListener(() =>
        {
            if (OnEndReaderClick != null)
            {
                OnEndReaderClick(MIndex);
            }
        });

    }

    private void ShowBeginReadUi()
    {
        _mStartReadLineIcon.sizeDelta = new Vector2(60, _mStartReadLineIcon.sizeDelta.y);
        _mStartReadIcon.sizeDelta = new Vector2(22, 28);
        _mEndReadLineIcon.sizeDelta = new Vector2(0, _mEndReadLineIcon.sizeDelta.y);
        _mEndReadIcon.sizeDelta = new Vector2(0, 0);

    }

    private void ShowEndReadUi()
    {
        _mStartReadLineIcon.sizeDelta = new Vector2(0, _mStartReadLineIcon.sizeDelta.y);
        _mStartReadIcon.sizeDelta = new Vector2(0, 0);
        _mEndReadLineIcon.sizeDelta = new Vector2(60, _mEndReadLineIcon.sizeDelta.y);
        _mEndReadIcon.sizeDelta = new Vector2(22, 28);
    }

    public  void InitUi(int index, Segment t, Action<int, Segment> onCellClickCallback = null, Action<int, Segment> onCellLongPressCallback = null,
       bool isBeginRead = false, bool isSelected = false)
    {
        MIndex = index;
        this.t = t;
        MOnCellClick = onCellClickCallback;
        MOnCellLongPress = onCellLongPressCallback;

        if (isBeginRead)
        {
            ShowEndReadUi();
        }
        else
        {
            ShowBeginReadUi();
        }

        SegmentContent.color =  isSelected? Color.red: Color.black;
        SetDefaultUi();
    }

    private void SetDefaultUi()
    {
        if (t == null) return;
        var replace = t.Sentence.Replace("&nbsp;", "");
        SegmentContent.text = t.Number + replace.Trim();
    }
}
