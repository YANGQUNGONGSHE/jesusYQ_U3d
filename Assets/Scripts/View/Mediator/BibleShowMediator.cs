using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class BibleShowMediator : EventMediator {


    [Inject]
    public BibleShowView BibleShowView{ get; set; }

    private List<Segment> _mSegments;
    private Biography _mBiography;
    private Chapter _mChapter;
    private ChapterShowModel _showModel;
    private BibleRecord _mBibleRecord;
    private int _mEndIndex = -1;
    public override void OnRegister()
    {
        BibleShowView.ReadPlanBut.onClick.AddListener(ReadPlanClick);
        BibleShowView.CollectBut.onClick.AddListener(CollectClick);
        BibleShowView.DirectoryBut.onClick.AddListener(DirectoryClick);
        BibleShowView.FontSizeSlider.onValueChanged.AddListener(FontSizeChangeListenr);
        dispatcher.AddListener(CmdEvent.ViewEvent.BibleShow, SegmentsCallBack);
        BibleShowView.BibleShowFiler.OnCellClick = OnCellClick;
        BibleShowView.BibleShowFiler.OnBeginReader = OnBeginReader;
        BibleShowView.BibleShowFiler.OnEndReader = OnEndReader;
        BibleShowView.CopyBut.onClick.AddListener(CopySegmentClick);
        BibleShowView.LikeBut.onClick.AddListener(LikeClick);
        BibleShowView.NoteBut.onClick.AddListener(NoteClick);
        BibleShowView.ShareBut.onClick.AddListener(ShareClick);
        BibleShowView.BibleShowFiler.ScrollView.movementType = ScrollRect.MovementType.Clamped;
        BibleShowView.BibleShowFiler.ScrollView.onValueChanged.AddListener(ScrollChangeListener);
        BibleShowView.MainScrollRect.onValueChanged.AddListener(MainScrollChangeListenr);
        _showModel = new ChapterShowModel();

    }


    private void CopySegmentClick()
    {

        var sb = new StringBuilder();
        foreach (KeyValuePair<int, bool> key in BibleShowView.BibleShowFiler.SelectedIndexs)
        {
            sb.Append(BibleShowView.BibleShowFiler.DataSource[key.Key].Sentence);
        }
        UniPasteBoard.SetClipBoardString(sb.ToString());

        BibleShowView.BibleShowFiler.SelectedIndexs.Clear();
        BibleShowView.BibleShowFiler.Refresh();
        BibleShowView.VisibleToolBar(false);
    }
    private void ShareClick()
    {
       Log.I("分享");
    }

    private void NoteClick()
    {
        Log.I("笔记");
    }

    private void LikeClick()
    {
        Log.I("收藏");
    }

    private void MainScrollChangeListenr(Vector2 arg0)
    {
       Log.I("MainScrollChangeListenr数据   "+BibleShowView.MainScrollRect.normalizedPosition);
        if (arg0.x >= 1 && Input.GetMouseButtonUp(0))
        {
           Log.I("下一章！！！！");
        }
        if (arg0.x <= 0 && Input.GetMouseButtonUp(0))
        {
            Log.I("上一章！！！！");
        }
    }

    private void ScrollChangeListener(Vector2 arg0)
    {
      //  Log.I("读经界面XXXXXXXX  " + BibleShowView.BibleShowFiler.ScrollView.normalizedPosition);
    } 

    private void OnBeginReader(int index)
    {
        BibleShowView.BibleShowFiler.IsBeginReading = true;
        BibleShowView.BibleShowFiler.Refresh();
    }
    private void OnEndReader(int index)
    {
        _mEndIndex = index;
        BibleShowView.BibleShowFiler.IsBeginReading = false;
        SaveRecordData(index);
        BibleShowView.BibleShowFiler.Refresh();

    }

    private void SaveRecordData(int index)
    {
        _mBibleRecord = new BibleRecord() {biography = _showModel.Biography,chapter = _showModel.Chapter,
            segment =BibleShowView.BibleShowFiler.DataSource[index] };
        LocalDataManager.Instance.SaveFileObj(LocalDataObjKey.BibleRecord,_mBibleRecord);
 
       dispatcher.Dispatch(CmdEvent.ViewEvent.UpDateBibleRecord);
    }

    private void OnCellClick(int index, Segment segment)
    {
       BibleShowView.VisibleToolBar(BibleShowView.BibleShowFiler.SelectedIndexs.Count > 0); 
    }
    private void SegmentsCallBack(IEvent eEvent)
    {
        _mSegments = eEvent.data as List<Segment>;

        if(_mSegments==null)return;

        BibleShowView.BibleShowFiler.DataSource = _mSegments;
        BibleShowView.BibleShowFiler.Refresh();
        BibleShowView.BibleShowFiler.ScrollToTop(false);

        _mChapter = LocalDataManager.LoadSQLTable<Chapter>(SQL.QUERY_CHAPTER_BYCHAPTERID, _mSegments[0].ChapterId).First();
        if (_mChapter == null) return;
        _mBiography = LocalDataManager.LoadSQLTable<Biography>(SQL.QUERY_BIOGRAPHY_BYID, _mChapter.BiographyId).First();
        if (_mBiography != null)
        {
             BibleShowView.Title.text = _mBiography.Name + _mChapter.Number;
            _showModel.Chapter = _mChapter;
            _showModel.Biography = _mBiography;

            SaveRecordData(0);
        }

    }

    private void DirectoryClick()
    {
        if (_mEndIndex == -1)
        {
            SaveRecordData(0);
        }
        _mEndIndex = -1;
       iocViewManager.CloseCurrentOpenNew((int)UiId.Bible);
    }

    private void CollectClick()
    {
        //下一章
        dispatcher.Dispatch(CmdEvent.ViewEvent.NextChapter, _showModel);
    }

    private void ReadPlanClick()
    {
        //上一章
        dispatcher.Dispatch(CmdEvent.ViewEvent.LastChapter, _showModel);
    }

    private void FontSizeChangeListenr(float size)
    {
        Log.I("字体变化数据：  "+ size);
        BibleShowView.BibleShowFiler.FontSize = CommDefine.FontSize + (int)size;
        BibleShowView.BibleShowFiler.Refresh();
    }

    public override void OnRemove()
    {
        BibleShowView.ReadPlanBut.onClick.RemoveListener(ReadPlanClick);
        BibleShowView.CollectBut.onClick.RemoveListener(CollectClick);
        BibleShowView.DirectoryBut.onClick.RemoveListener(DirectoryClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.BibleShow, SegmentsCallBack);
        BibleShowView.FontSizeSlider.onValueChanged.RemoveListener(FontSizeChangeListenr);
        BibleShowView.CopyBut.onClick.RemoveListener(CopySegmentClick);
        BibleShowView.BibleShowFiler.ScrollView.onValueChanged.RemoveListener(ScrollChangeListener);
        BibleShowView.MainScrollRect.onValueChanged.RemoveListener(MainScrollChangeListenr);

        BibleShowView.LikeBut.onClick.RemoveListener(LikeClick);
        BibleShowView.NoteBut.onClick.RemoveListener(NoteClick);
        BibleShowView.ShareBut.onClick.RemoveListener(ShareClick);

        BibleShowView.BibleShowFiler.OnBeginReader -= OnBeginReader;
        BibleShowView.BibleShowFiler.OnEndReader -= OnEndReader;
    }

    private void OnDestroy()
    {
        OnRemove();
    }



}
