using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class BibleMediator :EventMediator {

    [Inject]
    public BibleView BibleView { get; set; }

    private Biography _mBiography;

    private BibleRecord _mBibleRecord;

    public override void OnRegister()
    {
        BibleView.BigraphyToggle.onValueChanged.AddListener(BigraphyToggleListener);
        BibleView.ChapterToggle.onValueChanged.AddListener(ChapterToggleListener);
        BibleView.SemenToggle.onValueChanged.AddListener(SementToggleListener);
        BibleView.OldBibleToggle.onValueChanged.AddListener(OldBibleToggleListener);
        BibleView.NewBibleToggle.onValueChanged.AddListener(NewBibleToggleListener);
        BibleView.ReadRecordBut.onClick.AddListener(BibleRecordListener);

        BibleView.BiographyFiler.OnCellClick = BiographyOnCellClick;
        BibleView.ChapterFiler.OnCellClick = ChapterOnCellClick;
        BibleView.SementFiler.OnCellClick = SementOnCellClick;
        
        dispatcher.AddListener(CmdEvent.ViewEvent.NextChapter, NextChapterCallback);
        dispatcher.AddListener(CmdEvent.ViewEvent.LastChapter,LastChapterCallBack);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpDateBibleRecord,UpdateBibleRecordCallBack);

        UpdateRecordData();

    }

    private void UpdateBibleRecordCallBack()
    {
        UpdateRecordData();
    }

    private void UpdateRecordData()
    {
        _mBibleRecord = LocalDataManager.Instance.LoadFileObj(LocalDataObjKey.BibleRecord, typeof(BibleRecord)) as BibleRecord;

        if (_mBibleRecord != null)
        {
            BibleView.BiographyFiler.IsSelected = _mBibleRecord.biography.Id - 1;
            BibleView.BiographyFiler.Refresh();
            BibleView.ChapterFiler.IsSelected = _mBibleRecord.chapter.Number.ToInt() - 1;
            BibleView.ChapterFiler.Refresh(_mBibleRecord.chapter.BiographyId);
            SetBibleRecordText();
        }
        else
        {
            BibleView.BiographyFiler.IsSelected = 0;
            BibleView.BiographyFiler.Refresh();

            BibleView.ChapterFiler.IsSelected = 0;
            BibleView.ChapterFiler.Refresh(1);
        }
    }

    private void BibleRecordListener()
    {
        if (_mBibleRecord == null)
        {

            BibleView.ChapterFiler.InitDataSourceWith(1);
            BibleView.SementFiler.InitDataSourceWith(BibleView.ChapterFiler.DataSource[0].Id);

            iocViewManager.CloseCurrentOpenNew((int)UiId.BibleShow);
            StartCoroutine(SendInfo());
        }
        else
        {
            BibleView.ChapterFiler.InitDataSourceWith(_mBibleRecord.biography.Id);
            BibleView.SementFiler.InitDataSourceWith(_mBibleRecord.chapter.Id);

            iocViewManager.CloseCurrentOpenNew((int)UiId.BibleShow);
            StartCoroutine(SendInfo());
        }
    }

    private void SetBibleRecordText()
    {
        BibleView.RecordText.text = string.Format("{0} {1}章 {2}节", _mBibleRecord.biography.Name,
            _mBibleRecord.chapter.Number, _mBibleRecord.segment.Number);
    }

    /// <summary>
   /// 卷目录 条目点击事件
   /// </summary>
   /// <param name="index"></param>
   /// <param name="biography"></param>
    private void BiographyOnCellClick(int index, Biography biography)
    {
            Log.I("卷被点击    " + "主键："+biography.Id + biography.Name);

            BibleView.ChapterFiler.InitDataSourceWith(biography.Id);
            BibleView.ChapterFiler.IsSelected = 0;
            BibleView.ChapterFiler.Refresh(biography.Id);
            BibleView.ClickItemType(0);
    }
    /// <summary>
    /// 章目录 条目点击事件
    /// </summary>
    /// <param name="index"></param>
    /// <param name="chapter"></param>
    private void ChapterOnCellClick(int index, Chapter chapter)
    {
        Log.I("章被点击  " + "主键："+ chapter.Id + chapter.Name+"   序号："+chapter.Number);
        BibleView.SementFiler.InitDataSourceWith(chapter.Id);
        BibleView.ClickItemType(1);  
    }
    /// <summary>
    /// 节目录 条目点击事件
    /// </summary>
    /// <param name="index"></param>
    /// <param name="segment"></param>
    private void SementOnCellClick(int index, Segment segment)
    {
        Log.I("节被点击   " + segment.Sentence);
        iocViewManager.CloseCurrentOpenNew((int)UiId.BibleShow);
        StartCoroutine(SendInfo());
    }
    IEnumerator SendInfo()
    {
        yield return new WaitForSeconds(.1f);
        dispatcher.Dispatch(CmdEvent.ViewEvent.BibleShow, BibleView.SementFiler.DataSource);
    }
    /// <summary>
    /// 下一章点击返回 请求数据事件
    /// </summary>
    /// <param name="eEvent"></param>
    private void NextChapterCallback(IEvent eEvent)
    {
        var data = eEvent.data as ChapterShowModel;
        if (data == null) return;

        if (data.Chapter.Number.ToInt() == data.Biography.ChaptersCount)
        {
            if (data.Chapter.BiographyId == 66)
            {
                Log.I("已经到最后");
            }
            else
            {
                BibleView.ChapterFiler.InitDataSourceWith(data.Chapter.BiographyId+1);
                BibleView.SementFiler.InitDataSourceWith(BibleView.ChapterFiler.DataSource[0].Id);
                StartCoroutine(SendInfo());
            }
        }
        else
        {
            var number = data.Chapter.Number.ToInt();
            TurnChapter(number);
        } 
    }
    /// <summary>
    /// 上一章点击返回 请求数据事件
    /// </summary>
    /// <param name="eEvent"></param>
    private void LastChapterCallBack(IEvent eEvent)
    {
        var data = eEvent.data as ChapterShowModel;
        if (data == null) return;
        if (data.Chapter.Number.ToInt() == 1)
        {
            if (data.Chapter.BiographyId == 1)
            {
                Log.I("已经到开始位置");
            }
            else
            {
                 BibleView.ChapterFiler.InitDataSourceWith(data.Chapter.BiographyId-1);
                _mBiography = LocalDataManager.LoadSQLTable<Biography>(SQL.QUERY_BIOGRAPHY_BYID, BibleView.ChapterFiler.DataSource[0].BiographyId).First();
                BibleView.SementFiler.InitDataSourceWith(BibleView.ChapterFiler.DataSource[_mBiography.ChaptersCount-1].Id);
                StartCoroutine(SendInfo());
            }
        }
        else
        {
            var number = data.Chapter.Number.ToInt() - 2;
            TurnChapter(number);
        }

    }

    private void TurnChapter(int number )
    {
        BibleView.SementFiler.InitDataSourceWith(BibleView.ChapterFiler.DataSource[number].Id);
        StartCoroutine(SendInfo());
    }
    /// <summary>
    /// 圣经卷目录Toggle选中事件
    /// </summary>
    /// <param name="arg0"></param>
    private void BigraphyToggleListener(bool arg0)
    {
        BibleView.IsOnToggle(0,arg0);
    }
    /// <summary>
    /// 圣经章目录Toggle选中事件
    /// </summary>
    /// <param name="arg0"></param>
    private void ChapterToggleListener(bool arg0)
    {
        BibleView.IsOnToggle(1, arg0);

        if (BibleView.ChapterFiler.DataSource == null)
        {
            BibleView.ChapterFiler.InitDataSourceWith(1);
        }
    }
    /// <summary>
    /// 圣经节目录toggle选中事件
    /// </summary>
    /// <param name="arg0"></param>
    private void SementToggleListener(bool arg0)
    {
        BibleView.IsOnToggle(2, arg0);

        if (BibleView.SementFiler.DataSource != null) return;
        if (BibleView.ChapterFiler.DataSource != null)
        {
            BibleView.SementFiler.InitDataSourceWith(BibleView.ChapterFiler.DataSource[0].Id);
        }
        else
        {
            BibleView.ChapterFiler.InitDataSourceWith(1);
            BibleView.SementFiler.InitDataSourceWith(BibleView.ChapterFiler.DataSource[0].Id);
        }
    }
    /// <summary>
    /// 圣经目录界面新约Toggle选中事件
    /// </summary>
    /// <param name="isOn"></param>
    private void NewBibleToggleListener(bool isOn)
    {
      
            
        BibleView.NewBibleText.color = isOn ? new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f) : new Color(180 / 255f, 180 / 255f, 180 / 255f, 255 / 255f);
        if (!isOn) return;
        BibleView.BiographyFiler.ScrollView.GoToListItem(39);
        BibleView.OldNewBibleTitle.text = "新约";
    }
    /// <summary>
    /// 圣经目录界面旧约Toggle选中事件
    /// </summary>
    /// <param name="isOn"></param>
    private void OldBibleToggleListener(bool isOn)
    {
      
            
        BibleView.OldBibleText.color = isOn ? new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f) : new Color(180 / 255f, 180 / 255f, 180 / 255f, 255 / 255f);
        if (!isOn) return;
        BibleView.BiographyFiler.ScrollToTop(false);
        BibleView.OldNewBibleTitle.text = "旧约";
    }
    public override void OnRemove()
    {
        BibleView.BigraphyToggle.onValueChanged.RemoveListener(BigraphyToggleListener);
        BibleView.ChapterToggle.onValueChanged.RemoveListener(ChapterToggleListener);
        BibleView.SemenToggle.onValueChanged.RemoveListener(SementToggleListener);
        BibleView.OldBibleToggle.onValueChanged.RemoveListener(OldBibleToggleListener);
        BibleView.NewBibleToggle.onValueChanged.RemoveListener(NewBibleToggleListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.NextChapter, NextChapterCallback);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LastChapter, LastChapterCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.UpDateBibleRecord, UpdateBibleRecordCallBack);
        BibleView.ReadRecordBut.onClick.RemoveListener(BibleRecordListener);

        BibleView.BiographyFiler.OnCellClick -= BiographyOnCellClick;
        BibleView.ChapterFiler.OnCellClick -= ChapterOnCellClick;
        BibleView.SementFiler.OnCellClick -= SementOnCellClick;
        
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
