using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ClassicsMediator : EventMediator {

    [Inject]
    public ClassicsView ClassicsView { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    private Sprite[] _mBookCoverSprites;
    private string[] _mBookName;
    private string[] _mBookId;
    private List<RankPersonalModel> _mPersonalModels;
    private List<RankGroupModel> _rankGroupModels;

    public override void OnRegister()
    {
       GetBookData();
       ClassicsView.BookFiler.OnCellClick = OncellClick;
       dispatcher.AddListener(CmdEvent.ViewEvent.LoadAllOwnGroupsRankSucc,LoadAllOwnGroupsRankSuccCallBack);
       dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupsRankSucc, LoadGroupsRankSuccCallBack);
       dispatcher.AddListener(CmdEvent.ViewEvent.LoadSinglePersonalRankSucc, LoadSinglePersonalRankCallBack);
       dispatcher.AddListener(CmdEvent.ViewEvent.LoadPersonalRankSucc, LoadPersonalRankCallBack);
       ClassicsView.PersonalReadRankFiler.ScrollView.onValueChanged.AddListener(PersonalRankScrollRectListener);
       ClassicsView.AllGroupReadRankFiler.ScrollView.onValueChanged.AddListener(AllGroupsRankScrollRectListener);

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIUtil.Instance.ShowTextToast("当前网络不可用");
        }
        else
        {
            //请求所有群组的读经排行数据
            dispatcher.Dispatch(CmdEvent.Command.LoadGroupsRank, new AllGroupsRankInfo()
            {
                Skip = 0,
                Limit = 20,
                IsRefresh = false,
                Type = LoadRankType.AllGroupsRank
            });
            //请求所有用户的读经排行数据
            dispatcher.Dispatch(CmdEvent.Command.LoadPersonalRank, new AllPersonalRankInfo()
            {
                Skip = 0,
                Limit = 20,
                IsRefresh = false,
                Type = LoadRankType.AllPersonalRank
            });
        }
    }

    private void GetBookData()
    {
        var bookDatas = new List<BookModel>();
        _mBookCoverSprites = new[]
        {
            Resources.Load<Sprite>("icon_bible"),
            Resources.Load<Sprite>("icon_lunyun"),
            Resources.Load<Sprite>("icon_laozi"),
        };
        _mBookName = new[] { "圣经", "论语", "老子" };
        _mBookId = new[] { "bible", "lunyu", "laozi" };
        for (var i = 0; i < 3; i++)
        {
            var bookModel = new BookModel()
            {
                Id = _mBookId[i],
                BookCover = _mBookCoverSprites[i],
                BookName = _mBookName[i]
            };
            bookDatas.Add(bookModel);
        }
        ClassicsView.BookFiler.DataSource = bookDatas;
        ClassicsView.BookFiler.Refresh();

        if (UserModel.SingleRankModel != null && UserModel.AllJoinRankGroupModels != null)
        {
            ClassicsView.SetOwnRankUi(UserModel.SingleRankModel);
            if (UserModel.AllJoinRankGroupModels.Count > 0)
            {
                ClassicsView.AllMyGroupReadRankFiler.DataSource = UserModel.AllJoinRankGroupModels;
                ClassicsView.AllMyGroupReadRankFiler.Refresh();
            }
        }
        else
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)return;
            //请求自己的读经排行数据
            dispatcher.Dispatch(CmdEvent.Command.LoadPersonalRank, new AllPersonalRankInfo()
            {
                Type = LoadRankType.SinglePersonalRank
            });
        }
    }
    /// <summary>
    /// 加载我加入的群组排名回调
    /// </summary>
    /// <param name="eEvent"></param>
    private void LoadAllOwnGroupsRankSuccCallBack(IEvent eEvent)
    {
        var lists = (List<RankGroupModel>)eEvent.data;
        if(lists==null)return;
        
        ClassicsView.AllMyGroupReadRankFiler.DataSource = lists;
        ClassicsView.AllMyGroupReadRankFiler.Refresh();
    }
    /// <summary>
    /// 加载所有群组回调
    /// </summary>
    /// <param name="eEvent"></param>
    private void LoadGroupsRankSuccCallBack(IEvent eEvent)
    {
        var infos = (AllGroupsRankInfo)eEvent.data;
        if (infos == null) return;

        if (infos.IsRefresh)
        {
            ClassicsView.AllGroupReadRankFiler.DataSource.Clear();
            ClassicsView.AllGroupReadRankFiler.DataSource = null;

            if (_rankGroupModels != null)
            {
                _rankGroupModels.Clear();
                _rankGroupModels = null;
            }
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        if (_rankGroupModels == null)
        {
            _rankGroupModels = new List<RankGroupModel>();
        }

        for (var i = 0; i < infos.Models.Count; i++)
        {
            _rankGroupModels.Add(infos.Models[i]);
        }
        ClassicsView.AllGroupReadRankFiler.DataSource = _rankGroupModels;
        ClassicsView.AllGroupReadRankFiler.Refresh();
    }
    /// <summary>
    /// 加载用户本人读经排名回调
    /// </summary>
    /// <param name="eEvent"></param>
    private void LoadSinglePersonalRankCallBack(IEvent eEvent)
    {
        var model = (RankPersonalModel)eEvent.data;
        ClassicsView.SetOwnRankUi(model);
        //请求查询所有加入的群组阅读排名数据
        dispatcher.Dispatch(CmdEvent.Command.LoadGroupsRank, new AllGroupsRankInfo()
        {
            Type = LoadRankType.AllMyGroupsRank
        });
    }
    /// <summary>
    /// 加载所有用户读经排名回调
    /// </summary>
    /// <param name="eEvent"></param>
    private void LoadPersonalRankCallBack(IEvent eEvent)
    {
        var infos = (AllPersonalRankInfo)eEvent.data;
        if (infos == null) return;

        if (infos.IsRefresh)
        {
            ClassicsView.PersonalReadRankFiler.DataSource.Clear();
            ClassicsView.PersonalReadRankFiler.DataSource = null;
            if (_mPersonalModels != null)
            {
                _mPersonalModels.Clear();
                _mPersonalModels = null;
            }
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }
        if (_mPersonalModels == null)
        {
            _mPersonalModels = new List<RankPersonalModel>();
        }
        for (var i = 0; i < infos.Models.Count; i++)
        {
            _mPersonalModels.Add(infos.Models[i]);
        }
        ClassicsView.PersonalReadRankFiler.DataSource = _mPersonalModels;
        ClassicsView.PersonalReadRankFiler.Refresh();
    }

    private void OncellClick(int index, BookModel bookModel)
    {
        //if(bookModel.Id.Equals("more"))
        //{
        //   UIUtil.Instance.ShowTextToast("敬请期待！");
        //}
        //else
        //{
            NotificationCenter.DefaultCenter().PostNotification(NotifiyName.OpenClassiceBook, this);
            UserModel.BookModel = bookModel;
            iocViewManager.DestroyAndOpenNew(ClassicsView.GetUiId(), (int)UiId.BookDetail);
        //}
    }

    private void AllGroupsRankScrollRectListener(Vector2 arg0)
        {
            if (ClassicsView.AllGroupReadRankFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
            {
                dispatcher.Dispatch(CmdEvent.Command.LoadGroupsRank, new AllGroupsRankInfo()
                {
                    Skip = ClassicsView.AllGroupReadRankFiler.DataSource.Count,
                    Limit = 20,
                    IsRefresh = false,
                    Type = LoadRankType.AllGroupsRank
                });
            }
        }
    
    private void PersonalRankScrollRectListener(Vector2 arg0)
        {
            if (ClassicsView.PersonalReadRankFiler.ScrollView.normalizedPosition.y <= 0f && Input.GetMouseButtonUp(0))
            {
                dispatcher.Dispatch(CmdEvent.Command.LoadPersonalRank, new AllPersonalRankInfo()
                {
                    Skip = ClassicsView.PersonalReadRankFiler.DataSource.Count,
                    Limit = 20,
                    IsRefresh = false,
                    Type = LoadRankType.AllPersonalRank
                });
            }
        }

    public override void OnRemove()
    { 
        ClassicsView.BookFiler.OnCellClick -= OncellClick;
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadPersonalRankSucc, LoadPersonalRankCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupsRankSucc, LoadGroupsRankSuccCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSinglePersonalRankSucc, LoadSinglePersonalRankCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadAllOwnGroupsRankSucc, LoadAllOwnGroupsRankSuccCallBack);
        ClassicsView.PersonalReadRankFiler.ScrollView.onValueChanged.RemoveListener(PersonalRankScrollRectListener);
        ClassicsView.AllGroupReadRankFiler.ScrollView.onValueChanged.RemoveListener(AllGroupsRankScrollRectListener);
    }

    private void OnDestroy()
    {
        if (ClassicsView.BookFiler.DataSource != null)
        {
            ClassicsView.BookFiler.DataSource.Clear();
            ClassicsView.BookFiler.DataSource = null;
        }

        if (ClassicsView.AllGroupReadRankFiler.DataSource != null)
        {
            ClassicsView.AllGroupReadRankFiler.DataSource.Clear();
            ClassicsView.AllGroupReadRankFiler.DataSource = null;
        }

//        if (ClassicsView.AllMyGroupReadRankFiler.DataSource != null)
//        {
//            ClassicsView.AllMyGroupReadRankFiler.DataSource.Clear();
//            ClassicsView.AllMyGroupReadRankFiler.DataSource = null;
//        }

        if (ClassicsView.PersonalReadRankFiler.DataSource != null)
        {
            ClassicsView.PersonalReadRankFiler.DataSource.Clear();
            ClassicsView.PersonalReadRankFiler.DataSource = null;
        }
        _mBookCoverSprites.SetNull();
        _mBookName = null;
        _mBookId = null;
        OnRemove();
    }
}
