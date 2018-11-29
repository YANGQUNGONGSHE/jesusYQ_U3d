using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class MeMediator : EventMediator
{
    [Inject]
    public UserModel UserModel { get; set; }
    [Inject]
    public MeView MeView { get; set; }
    private string[] _mBookName;
    private string[] _mBookId;
    private BookModel _mRecord;

    public override void OnRegister()
    {
          MeView.SetBut.onClick.AddListener(SetClick);
          MeView.EditorDataBut.onClick.AddListener(EditorDataClick);
          MeView.FansAndFocusBut.onClick.AddListener(FansAndFocusClick);
          MeView.LikeBut.onClick.AddListener(LikeClick);
          MeView.PersonalBut.onClick.AddListener(PersonalButClick);
          MeView.ChangeBookBut.onClick.AddListener(ChangeBookButClick);
          MeView.BlBut.onClick.AddListener(BlButClick);
          MeView.CollectBut.onClick.AddListener(CollectClick);
          MeView.AccountBookRecordFiler.OnCellClick = OnCellClick;
          dispatcher.AddListener(CmdEvent.ViewEvent.EditorAccountDataOptionFinish,UpdateAccountFinish);
          dispatcher.AddListener(CmdEvent.ViewEvent.QueryUserReadRecordCoutFinish,QueryUserReadRecordCoutFinish);
          SetUiInfo();
          GetBookList();
          Dispatcher.InvokeAsync(GetBookRecordInfo, LocalDataObjKey.LastReadRecord);
    }

    #region Click Event

    private void BlButClick()
    {
        MeView.IsVisibleBookListPart(false);
    }
    private void ChangeBookButClick()
    {
       MeView.IsVisibleBookListPart(true);
    }
    private void PersonalButClick()
    {
        UserModel.PostModel = new PostModel()
        {
            HeadTexture2D = DefaultImage.ImHeadTexture2D,
            Author = UserModel.User,
            FromType = FromViewType.FromAccountView
        };
        iocViewManager.DestroyAndOpenNew(MeView.GetUiId(),(int)UiId.Personal);
    }
    private void EditorDataClick()
    {
        UserModel.EditorUserDataType = EditorUserDataType.AccountCenter;
        iocViewManager.DestroyAndOpenNew(MeView.GetUiId(),(int)UiId.EditorUserData);
    }
    private void LikeClick()
    {
        UIUtil.Instance.ShowWaiting();
        iocViewManager.DestroyAndOpenNew(MeView.GetUiId(),(int)UiId.MyLikePosts);
    }
    private void FansAndFocusClick()
    {
        iocViewManager.DestroyAndOpenNew(MeView.GetUiId(),(int)UiId.FocusAndFans);
    }
    private void SetClick()
    {
        iocViewManager.DestroyAndOpenNew(MeView.GetUiId(),(int)UiId.Setting);
    }
    private void CollectClick()
    {
        iocViewManager.DestroyAndOpenNew(MeView.GetUiId(),(int)UiId.Collect);
    }
    #endregion

    #region dispatcher Event

    private void UpdateAccountFinish()
    {
        SetUiInfo();
    }

    private void QueryUserReadRecordCoutFinish(IEvent eEvent)
    {
        var recordCount = eEvent.data as ReadRecordCount;
        if(recordCount==null)return;
        MeView.SetReadRecordCountUi(recordCount);
    }

    #endregion

    private void OnCellClick(int index, BookModel bookModel1)
    {
        dispatcher.Dispatch(CmdEvent.Command.QueryUserReadRecord, new ReadRecordData()
        {
            Type = QueryReadRecordType.QueryReadCount,
            ParentType = "节",
            ParentidPrefix = bookModel1.Id,
            UserId = UserModel.User.Id.ToString()
        });
        MeView.IsVisibleBookListPart(false);
        MeView.SetChioseedBookName(bookModel1.BookName);
        LocalDataManager.Instance.SaveJsonObj(LocalDataObjKey.LastReadRecord, bookModel1);
    }

    private void GetBookList()
    {
        var bookDatas = new List<BookModel>();

        //_mBookName = new[] { "论语", "孟子", "中庸", "大学", "圣经", "老子", "尚书", "春秋", "诗经" };
        //_mBookId = new[] { "lunyu", "mengzi", "zhongyong", "daxue", "bible", "laozi", "shangshu", "chunqiu", "shijing" };
        _mBookName = new[] {"圣经", "论语", "老子"};
        _mBookId = new[] {"bible", "lunyu", "laozi"};

        for (var i = 0; i < 3; i++)
        {
            var bookModel = new BookModel()
            {
                Id = _mBookId[i],
                BookName = _mBookName[i]
            };
            bookDatas.Add(bookModel);
        }
        MeView.AccountBookRecordFiler.DataSource = bookDatas;
        MeView.AccountBookRecordFiler.Refresh();
    }

    private void GetBookRecordInfo(string key)
    {
        var json = (string)LocalDataManager.Instance.LoadJsonObj(key);
        if (!string.IsNullOrEmpty(json))
        {
            _mRecord = JsonMapper.ToObject<BookModel>(json);
        } 
        else
        {
            _mRecord = new BookModel()
            {
                BookName = "圣经",
                Id = "bible"
            };
        }
        dispatcher.Dispatch(CmdEvent.Command.QueryUserReadRecord, new ReadRecordData()
        {
            Type = QueryReadRecordType.QueryReadCount,
            ParentType = "节",
            ParentidPrefix = _mRecord.Id,
            UserId = UserModel.User.Id.ToString()
        });
        MeView.SetChioseedBookName(_mRecord.BookName);
    }

    private void SetUiInfo()
    {
        if(UserModel.User==null)return;
        MeView.NickName.text = !string.IsNullOrEmpty(UserModel.User.DisplayName) ? UserModel.User.DisplayName : UserModel.User.UserName;
        MeView.HeadImage.texture = DefaultImage.ImHeadTexture2D;
    }

    public override void OnRemove()
    {
        MeView.SetBut.onClick.RemoveListener(SetClick);
        MeView.LikeBut.onClick.RemoveListener(LikeClick);
        MeView.EditorDataBut.onClick.RemoveListener(EditorDataClick);
        MeView.FansAndFocusBut.onClick.RemoveListener(FansAndFocusClick);
        MeView.PersonalBut.onClick.RemoveListener(PersonalButClick);
        MeView.ChangeBookBut.onClick.RemoveListener(ChangeBookButClick);
        MeView.BlBut.onClick.RemoveListener(BlButClick);
        MeView.CollectBut.onClick.RemoveListener(CollectClick);
        MeView.AccountBookRecordFiler.OnCellClick -= OnCellClick;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.EditorAccountDataOptionFinish, UpdateAccountFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.QueryUserReadRecordCoutFinish, QueryUserReadRecordCoutFinish);
    }
    private void OnDestroy()
    {
        OnRemove();
        _mBookId = null;
        _mBookName = null;
        _mRecord = null;
    }

}
