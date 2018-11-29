using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqLikesPostsByUserCommand : EventCommand {

    [Inject]
    public IPreachService PreachService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    private ReqLikepostsInfo _mInfo;
    public override void Execute()
    {
        Retain();
        _mInfo = evt.data as ReqLikepostsInfo;

        if(_mInfo == null) return;

        PreachService.ReqLikesByUser(UserModel.User.Id.ToString(), _mInfo.Skip.ToString(), _mInfo.Limit.ToString(),"true",
            (b, err, info) =>
            {
                if (b)
                {
                     ReqPostsData(info);
                }
                else
                {
                    Log.I("请求点赞返回：：："+err);
                    UIUtil.Instance.ShowTextToast(err);
                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                }
            });
    }


    private void ReqPostsData(List<LikeInfo> likeInfos)
    {

        if (likeInfos.Count < 1)
        {
            Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadLikesPostsByUserFinish, _mInfo);
            return;
        }
        var postModels = new List<PostModel>();

        for (var i=0;i< likeInfos.Count;i++)
        {
            if(likeInfos[i].ParentType.Equals("帖子"))
            {
                PreachService.RequestSinglePreach(likeInfos[i].ParentId, (b, s, postInfo) =>
              {
                if (b)
                {
                   var model = new PostModel()
                   {
                       Id = postInfo.Id,
                       Title = postInfo.Title,
                       Summary = postInfo.Summary,
                       PictureUrl = postInfo.PictureUrl,
                       ContentType = postInfo.ContentType,
                       IsFeatured = postInfo.IsFeatured,
                       Author = postInfo.Author,
                       CommentsCount = postInfo.CommentsCount,
                       LikesCount = postInfo.LikesCount,
                       SharesCount = postInfo.SharesCount
                   };

                    postModels.Add(model);

                    if (postModels.Count != likeInfos.Count) return;
                    _mInfo.PostModels = postModels;
                    dispatcher.Dispatch(CmdEvent.ViewEvent.LoadLikesPostsByUserFinish, _mInfo);
                }
                else
                {
                    Log.I(s);
                    Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
                }
              });
            }
        }
        

    }

}

public class ReqLikepostsInfo
{
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
}
