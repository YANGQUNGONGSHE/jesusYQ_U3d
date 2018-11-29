using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqPersonalCommand : EventCommand {

    [Inject] public IFriendService FriendService { get; set; }

    [Inject] public IPreachService PreachService { get; set; }

    private PersonalInfo _personalModel;
    private List<PostModel> _postModels;
    public override void Execute()
    {
        Retain();
        _personalModel = (PersonalInfo)evt.data;
         _postModels = new List<PostModel>();
        if (_personalModel.IsRequestAll)
        {
            UIUtil.Instance.ShowWaiting();
            FriendService.RequestFollows(_personalModel.UserId, (b, list) =>
            {
                if (b)
                {
                    _personalModel.FollowerCount = list.Count.ToString();
                    FriendService.RequestFans(_personalModel.UserId, (b1, infos) =>
                    {
                        if (b1)
                        {
                            _personalModel.FansCount = infos.Count.ToString();
                            LoadPreachPostData();
                        }
                        else
                        {
                            Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                            UIUtil.Instance.ShowFailToast("请求错误");
                        }
                    } );
                }
                else
                {
                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                    UIUtil.Instance.ShowFailToast("请求错误");
                }
            });
        }
        else
        {
            LoadPreachPostData();
        }
    }

    private void LoadPreachPostData()
    {
        PreachService.RequestPreachByAuthor(_personalModel.UserId, string.Empty, "true", _personalModel.Skip.ToString(),
            _personalModel.Limit.ToString(), "true",
            (b2, error, info) =>
            {
                Release();
                if (b2)
                {
                    for (var i = 0; i < info.Count; i++)
                    {
                        var postModel = new PostModel()
                        {
                            Id = info[i].Id,
                            Title = info[i].Title,
                            Summary = info[i].Summary,
                            PictureUrl = info[i].PictureUrl,
                            ContentType = info[i].ContentType,
                            PublishedDate = info[i].PublishedDate,
                            IsFeatured = info[i].IsFeatured,
                            Author = info[i].Author,
                            CommentsCount = info[i].CommentsCount,
                            LikesCount = info[i].LikesCount,
                            SharesCount = info[i].SharesCount
                        };

                        _postModels.Add(postModel);
                    }
                    Log.I("个人论道帖子Count:"+ _postModels.Count);
                    _personalModel.PostModels = _postModels;
                    dispatcher.Dispatch(CmdEvent.ViewEvent.ReqDataByPosterSucc,_personalModel);
                }
                else
                {
                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                    UIUtil.Instance.ShowFailToast(error);
                }
            });
    }
}

public struct PersonalInfo
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public string UserId;
    /// <summary>
    /// 关注的人数
    /// </summary>
    public string FollowerCount;
    /// <summary>
    /// 粉丝人数
    /// </summary>
    public string FansCount;
    /// <summary>
    /// 帖子集合
    /// </summary>
    public List<PostModel> PostModels;
    /// <summary>
    /// 是否请求个人全部数据
    /// </summary>
    public bool IsRequestAll;
    /// <summary>
    /// 忽略的行数
    /// </summary>
    public int Skip;
    /// <summary>
    /// 获取的行数
    /// </summary>
    public int Limit;
    /// <summary>
    /// 是否是刷新
    /// </summary>
    public bool IsRefresh;
}
