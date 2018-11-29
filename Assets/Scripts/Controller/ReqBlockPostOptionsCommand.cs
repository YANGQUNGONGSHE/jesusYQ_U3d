using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqBlockPostOptionsCommand : EventCommand {


    [Inject] public IPreachService PreachService { get; set; }
    [Inject] public UserModel UserModel { get; set; }
    private BlockPostOptionInfo _mInfo;
    public override void Execute()
    {
        Retain();
        _mInfo = evt.data as BlockPostOptionInfo;
        if(_mInfo==null)return;
        switch (_mInfo.Options)
        {
            case BlockPostOptions.SetBlockPost:
                SetBlock(_mInfo.PostId,true);
                break;
            case BlockPostOptions.DeleteBlockPost:
                SetBlock(_mInfo.PostId,false);
                break;
            case BlockPostOptions.LoadBlockPostData:
                LoadBlockPostsData(_mInfo.Skip.ToString(),_mInfo.Limit.ToString());
                break;
        }
    }

    private void SetBlock(string id,bool isBlock)
    {
        if (isBlock)
        {
            PreachService.ReqCreatePostBlack(id, (b, error) =>
            {
                Release();
                if (b)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.BlockPostFinish);
                }
                else
                {
                    Log.I(error);
                }
            });
        }
        else
        {
            PreachService.ReqDeletePostBlack(id, (b, error) =>
            {
                Release();
                if (b)
                {
                    dispatcher.Dispatch(CmdEvent.ViewEvent.DeleteBlockPostFinish);
                    UIUtil.Instance.ShowTextToast("已取消屏蔽");
                }
                else
                {
                    Log.I(error);
                }
            });
        }
    }

    private void LoadBlockPostsData(string skip,string limit)
    {
        PreachService.ReqQueryPostsBlockInfo(UserModel.User.Id.ToString(),skip,limit, (b, error, infos) =>
        {
            Release();
            if (b)
            {
                var list = new List<PostModel>();
                for (var i = 0; i < infos.Count; i++)
                {
                    var model = new PostModel()
                    {
                        Id = infos[i].Post.Id,
                        Title = infos[i].Post.Title,
                        Summary = infos[i].Post.Summary,
                        PictureUrl = infos[i].Post.PictureUrl,
                        ContentType = infos[i].Post.ContentType,
                        PublishedDate = infos[i].Post.PublishedDate,
                        IsFeatured = infos[i].Post.IsFeatured,
                        Author = infos[i].Post.Author,
                        CommentsCount = infos[i].Post.CommentsCount,
                        LikesCount = infos[i].Post.LikesCount,
                        SharesCount = infos[i].Post.SharesCount
                    };
                    list.Add(model);
                }
                _mInfo.Models = list;
                Log.I("加载被屏蔽帖子数据Count:::"+ _mInfo.Models.Count);
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadBlockPostsFinish,_mInfo);
            }
            else
            {
                Log.I(error);
            }
        });
    }
}

public class BlockPostOptionInfo
{
    /// <summary>
    /// 
    /// </summary>
    public BlockPostOptions Options;

    public string PostId;
    /// <summary>
    /// 是否是刷新
    /// </summary>
    public bool IsReefresh;
    /// <summary>
    /// 被屏蔽帖子的数据
    /// </summary>
    public List<PostModel> Models;

    public int Skip;

    public int Limit;
}

/// <summary>
/// 屏蔽帖子相关操作
/// </summary>
public enum BlockPostOptions
{
    /// <summary>
    /// 屏蔽帖子
    /// </summary>
    SetBlockPost,
    /// <summary>
    /// 取消屏蔽帖子
    /// </summary>
    DeleteBlockPost,
    /// <summary>
    /// 获取已屏蔽的帖子数据
    /// </summary>
    LoadBlockPostData,
} 

