using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

public class ReqHotPreachCommand : EventCommand {

    [Inject]
    public IPreachService PreachService { get; set; }
    //private int _mSkip = 0;
    private PreachPostInfo _mPostInfo;
    public override void Execute()
    {
		Log.I("请求帖子ReqHotPreachCommand");
        Retain(); 

        _mPostInfo = (PreachPostInfo)evt.data;

        switch (_mPostInfo.Type)
        {
            case PostType.Hot:
                ReqPreachPosts(string.Empty,"true", "ContentQuality", _mPostInfo.Skip.ToString(), _mPostInfo.Limit.ToString(), "true");
                break;
            case PostType.New:
                ReqPreachPosts(string.Empty,"true", "CreatedDate", _mPostInfo.Skip.ToString(), _mPostInfo.Limit.ToString(), "true");
                break;
            case PostType.Focus:
                ReqPreachPostsByFocused("true",_mPostInfo.Skip.ToString(),_mPostInfo.Limit.ToString());
                break;
            case PostType.Search:
                ReqPreachPosts(_mPostInfo.SearchContent, "true", "ContentQuality", _mPostInfo.Skip.ToString(), _mPostInfo.Limit.ToString(), "true");
                break;
            case PostType.DeletePost:
                DeletePost(_mPostInfo.PostId);
                break;
        }
    }

    /// <summary>
    /// 请求帖子数据
    /// </summary>
    /// <param name="titlefilter">过滤标题及概要</param>
    /// <param name="isPublished">是否已发布</param>
    /// <param name="oerderBy">排序的字段</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="descending">是否按降序排列</param>
    private void ReqPreachPosts(string titlefilter,string isPublished,string oerderBy,string skip ,string limit,string descending)
    {
        PreachService.RequestHotPreach(titlefilter,isPublished, oerderBy, skip, limit, descending,(b, err, list) =>
        {
				Log.I("RequestHotPreach");
            Release();
            if (b)
            {
                //if (list == null || list.Count <= 0) return;
                Log.I("返回的帖子数据：" + list.Count);
                GetPostModels(list);
            }
            else
            {
                Log.I("获取帖子失败！！");
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqPreachFail, err);
            }
        });
    }
   
    private void ReqPreachPostsByFocused(string ispublished, string skip, string limit)
    {
        PreachService.RequestPreachByFocused(ispublished, "ContentQuality","true", skip,limit, (b, error, list) =>
        {
            Release();
            if (b)
            {
                //if (list == null || list.Count <= 0) return;
                Log.I("返回的帖子数据：" + list.Count);
                GetPostModels(list);
            }
            else
            {
                Log.I("获取帖子失败！！");
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqPreachFail, error);
            }
        });
    }

    private void GetPostModels(List<BasePostInfo> list)
    {

        var postModels = new List<PostModel>();

        for (var i = 0; i < list.Count; i++)
        {
            var postModel = new PostModel()
            {
                Id = list[i].Id,
                Title = list[i].Title,
                Summary = list[i].Summary,
                PictureUrl = list[i].PictureUrl,
                ContentType = list[i].ContentType,
                PublishedDate = list[i].PublishedDate,
                IsFeatured = list[i].IsFeatured,
                Author = list[i].Author,
                CommentsCount = list[i].CommentsCount,
                LikesCount = list[i].LikesCount,
                SharesCount = list[i].SharesCount
            };
            postModels.Add(postModel);
        }
        Log.I("加载帖子成功后，数量：：：" + postModels.Count);
        _mPostInfo.PostModels = postModels;
        if (_mPostInfo.Type == PostType.Search)
        {
            dispatcher.Dispatch(CmdEvent.ViewEvent.ReqPreachSearchSucc, _mPostInfo);
        }
        else
        {
            dispatcher.Dispatch(CmdEvent.ViewEvent.ReqPreachSucc, _mPostInfo);
        }
    }
    /// <summary>
    /// 删除帖子
    /// </summary>
    /// <param name="postId">帖子Id</param>
    private void DeletePost(string postId)
    {
        PreachService.ReqDeletePost(postId, (b, error) =>
        {
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.DeletePostFinish);
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        } );
    }
}

public class PreachPostInfo
{
    /// <summary>
    /// 请求的类型
    /// </summary>
    public PostType Type;
    /// <summary>
    /// 帖子数据
    /// </summary>
    public List<PostModel> PostModels;
    /// <summary>
    /// 忽略的行数
    /// </summary>
    public int Skip;
    /// <summary>
    /// 获取的行数
    /// </summary>
    public int Limit;
    /// <summary>
    /// 是否是刷新数据
    /// </summary>
    public bool IsRefresh;
    /// <summary>
    /// 搜索的关键字(搜索必填)
    /// </summary>
    public string SearchContent;
    /// <summary>
    /// 帖子Id(删除帖子必填)
    /// </summary>
    public string PostId;
}

public enum PostType
{
    /// <summary>
    /// 热门
    /// </summary>
    Hot,
    /// <summary>
    /// 最新
    /// </summary>
    New,
    /// <summary>
    /// 关注者的帖子
    /// </summary>
    Focus,
    /// <summary>
    /// 关键字搜索(标题摘要)
    /// </summary>
    Search,
    /// <summary>
    /// 删除帖子
    /// </summary>
    DeletePost,
}
