using System.Collections;
using System.Collections.Generic;
using LitJson;
using NIM.SysMessage;
using strange.extensions.command.impl;
using UnityEngine;

public class LoadSysCustomRecordCommand : EventCommand {

    [Inject]
    public IImService ImService { get; set; }

    [Inject] public IPreachService PreachService { get; set; }
     
    private List<SysCustomLikeModel> _mLikeList;
    private List<SysCustomCommentModel> _mCommentList;
    public override void Execute()
    {
       Retain();
        var param = (ReqSysCustomInfo) evt.data;
        _mLikeList = new List<SysCustomLikeModel>();
        _mCommentList = new List<SysCustomCommentModel>();
        switch (param.Type)
        {
            case SysCustomType.Like:
                LoadLikeMessage(param.Limt,param.Skip);
                break;
            case SysCustomType.Comment:
                LoadComment(param.Limt,param.Skip);
                break;
        }
    }

    private void LoadLikeMessage( int limit,int skip)
    {
       PreachService.ReqQuerySysLikeInfo(skip.ToString(),limit.ToString(),"false", (b, error, info) =>
       {
           if (b)
           {
               for (var i = 0; i < info.Count; i++)
               {
                   var model = new SysCustomLikeModel()
                   {
                       UserId = info[i].User.Id.ToString(),
                       UserDisplayName = info[i].User.DisplayName,
                       UserName = info[i].User.UserName,
                       UserAvatarUrl = info[i].User.AvatarUrl,
                       Signature = info[i].User.Signature,
                       PostContentType = info[i].ParentType,
                       PostTitle = info[i].ParentTitle,
                       PostId = info[i].ParentId,
                       PostPictureUrl = info[i].ParentPictureUrl,
                       LikeCreatedDate = info[i].CreatedDate
                   };
                   _mLikeList.Add(model);
               }
               dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSysLikeFinish, _mLikeList);
           }
           else
           {
               UIUtil.Instance.ShowFailToast(error);
           }
       } );
    }

    private void LoadComment(int limit, long skip)
    {
       PreachService.ReqQuerySysCommentInfo(skip.ToString(),limit.ToString(),"false", (b, error, info) =>
       {
           if (b)
           {
               for (var i = 0; i < info.Count; i++)
               {
                   var model = new SysCustomCommentModel()
                   {
                       UserId = info[i].User.Id.ToString(),
                       UserDisplayName = info[i].User.DisplayName,
                       UserName = info[i].User.UserName,
                       UserAvatarUrl = info[i].User.AvatarUrl,
                       Signature = info[i].User.Signature,
                       PostId = info[i].ParentId,
                       PostTitle = info[i].ParentTitle,
                       PostPictureUrl = info[i].ParentPictureUrl,
                       PostContentType = info[i].ParentContentType,
                       CommentCreatedDate = info[i].CreatedDate,
                       CommentContent = info[i].Content,
                       CommentId = info[i].Id
                   };
                   _mCommentList.Add(model);
               }
               dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSysCommentFinish, _mCommentList);
           }
           else
           {
               UIUtil.Instance.ShowFailToast(error);
           }
       } );
    }
}

public class ReqSysCustomInfo
{  
    /// <summary>
    /// 请求类型
    /// </summary>
    public SysCustomType Type;
    /// <summary>
    /// 忽略的条数
    /// </summary>
    public int Skip;
    /// <summary>
    /// 获取条数
    /// </summary>
    public int Limt;
}
public enum SysCustomType
{
    /// <summary>
    /// 点赞消息
    /// </summary>
    Like,
    /// <summary>
    /// 评论消息
    /// </summary>
    Comment
}
