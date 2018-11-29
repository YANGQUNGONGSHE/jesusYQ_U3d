using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqPostInteractionCommand : EventCommand {

    [Inject]
    public IPreachService PreachService { get; set; }

    private InteractionInfo _mInfo;

    public override void Execute()
    {
        Retain();
        _mInfo = (InteractionInfo)evt.data;
        switch (_mInfo.Type)
        {
            case InteractionType.Like:
                CreateLike(_mInfo.ParentType,_mInfo.ParentId);
                break;
            case InteractionType.DisLike:
                DeleteLike(_mInfo.ParentId);
                break;
            case InteractionType.Comment:
                CreateComment(_mInfo.ParentType,_mInfo.ParentId,_mInfo.Content);
                break;
            case InteractionType.DeleteComment:
                DeleteComment(_mInfo.CommentId);
                break;
            case InteractionType.ReplyComment:
                ReplyComment(_mInfo.ParentType, _mInfo.ParentId, _mInfo.Content);
                break;
            case InteractionType.DeleteReplyComment:
                DeleteReplyComment(_mInfo.ReplyId);
                break;
        }
    }

    /// <summary>
    /// 请求点赞
    /// </summary>
    /// <param name="parentType">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    private void CreateLike(string parentType,string parentId)
    {
        PreachService.RequestCreateLike(parentType,parentId, (b, error, info) =>
        {
            Release();
            if (b)
            {
               Log.I("点赞成功");
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        } );
    }
    /// <summary>
    /// 取消点赞
    /// </summary>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    private void DeleteLike(string parentId)
    {
        PreachService.RequestDeleteLike(parentId, (b, error) =>
        {
            Release();
            if (b)
            {
                Log.I("取消点赞");
            }
            else
            {
                UIUtil.Instance.ShowFailToast(error);
            }
        } );
    }
    /// <summary>
    /// 创建评论
    /// </summary>
    /// <param name="parentType">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    /// <param name="content">正文内容</param>
    private void CreateComment(string parentType, string parentId, string content)
    {
        PreachService.RequestCreateComment(parentType,parentId,content, (b, error, info) =>
        {
            Release();
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqCommentPreachSucc);
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqCommentPreachFail);
            }
        });
    }
    /// <summary>
    /// 删除评论
    /// </summary>
    /// <param name="commentId">评论编号</param>
    private void DeleteComment(string commentId)
    {
        PreachService.ReqDeleteComment(commentId, (b, error) =>
        {
            Release();
            if (b)
            {
                

            }
            else
            {
                
            }
        } );
    }
    /// <summary>
    /// 请求回复评论
    /// </summary>
    /// <param name="parentType">上级类型（可选值：评论）</param>
    /// <param name="parentId">上级编号（如评论编号）</param>
    /// <param name="content">正文内容</param>
    private void ReplyComment(string parentType, string parentId, string content)
    {
        PreachService.RequestReplyComment(parentType,parentId,content, (b, error, info) =>
        {
            Release();
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqReplyCommentSucc);
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqReplyCommentFail);
            }
        } );
    }
    /// <summary>
    /// 请求删除回复评论的内容
    /// </summary>
    /// <param name="replyId">回复编号</param>
    private void DeleteReplyComment(string replyId)
    {
        PreachService.ReqDeleteReply(replyId, (b, error) =>
        {
            Release();
            if (b)
            {
                
            }
            else
            {
                
            }
        } );
    }
}

public class InteractionInfo
{
    /// <summary>
    /// 交互类型
    /// </summary>
    public InteractionType Type;
    /// <summary>
    /// 上级类型（可选值：帖子,评论， 章, 节）
    /// </summary>
    public string ParentType;
    /// <summary>
    /// 上级编号（如帖子编号，评论编号）
    /// </summary>
    public string ParentId;
    /// <summary>
    /// 正文内容
    /// </summary>
    public string Content;
    /// <summary>
    /// 评论编号(删除评论必填)
    /// </summary>
    public string CommentId;
    /// <summary>
    /// 回复编号(删除评论回复必填)
    /// </summary>
    public string ReplyId;


}

public enum InteractionType
{
    /// <summary>
    /// 点赞
    /// </summary>
    Like,
    /// <summary>
    /// 取消点赞
    /// </summary>
    DisLike,
    /// <summary>
    /// 评论
    /// </summary>
    Comment,
    /// <summary>
    /// 删除评论
    /// </summary>
    DeleteComment,
    /// <summary>
    /// 回复评论
    /// </summary>
    ReplyComment,
    /// <summary>
    /// 删除评论回复
    /// </summary>
    DeleteReplyComment,
    /// <summary>
    /// 转发帖子
    /// </summary>
    ForwardPost,
}
