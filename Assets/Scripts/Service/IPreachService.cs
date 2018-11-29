using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPreachService
{
    /// <summary>
    /// 请求创建帖子
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="title"></param>
    /// <param name="paragraphs"></param>
    /// <param name="callBack"></param>
    void RequestCreatePost<T>(string title,T paragraphs,Action<bool,PostInfo> callBack);

    /// <summary>
    /// 请求单个帖子
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="callBack"></param>
    void RequestSinglePreach(string postId, Action<bool, string, BasePostInfo> callBack);

    /// <summary>
    /// 请求热门帖子
    /// </summary>
    /// <param name="titlefilter">过滤标题及概要</param>
    /// <param name="ispublished">是否已发布</param>
    /// <param name="orderby">排序的字段</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="descending">是否按降序排列</param>
    /// <param name="callBack"></param>
    void RequestHotPreach(string titlefilter, string ispublished, string orderby, string skip, string limit, string descending, Action<bool,string, List<BasePostInfo>> callBack);

    /// <summary>
    /// 请求被关注用户的论道帖子
    /// </summary>
    /// <param name="ispublished">是否已发布</param>
    /// <param name="descending">是否按降序排序</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="callBack"></param>
    /// <param name="orderby">排序的字段</param>
    void RequestPreachByFocused(string ispublished, string orderby,string descending, string skip, string limit, Action<bool, string, List<BasePostInfo>> callBack);

    /// <summary>
    /// 根据用户的ID请求论道帖子
    /// </summary>
    /// <param name="authorid"></param>
    /// <param name="titlefilter">过滤标题及概要</param>
    /// <param name="ispublished">是否已发布</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="descending">是否按降序排列</param>
    /// <param name="callBack"></param>
    void RequestPreachByAuthor(string authorid, string titlefilter, string ispublished, string skip, string limit, string descending, Action<bool, string, List<BasePostInfo>> callBack);
    /// <summary>
    /// 点赞请求
    /// </summary>
    /// <param name="parentType">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    /// <param name="callBack">回调</param>
    void RequestCreateLike(string parentType,string parentId, Action<bool,string, LikeInfo> callBack);
    /// <summary>
    /// 取消点赞请求
    /// </summary>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    /// <param name="callback">回调</param>
    void RequestDeleteLike(string parentId,Action<bool,string> callback);

    /// <summary>
    /// 评论请求
    /// </summary>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    /// <param name="content">正文内容</param>
    /// <param name="callBack">回调</param>
    /// <param name="parentType">上级类型（可选值：帖子, 章, 节）</param>
    void RequestCreateComment(string parentType, string parentId,string content, Action<bool,string, CommentInfo> callBack);
    /// <summary>
    /// 删除评论请求
    /// </summary>
    /// <param name="commentId">评论Id</param>
    /// <param name="callback">回调</param>
    void ReqDeleteComment(string commentId, Action<bool, string> callback);

    /// <summary>
    /// 转发请求
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="annotion"></param>
    /// <param name="callBack"></param>
    void RequestForwardPost(string postId, string annotion,  Action<bool, PostInfo> callBack);

    /// <summary>
    /// 发布帖子请求
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="callBack"></param>
    void RequestPublishPost(string postId, Action<bool> callBack);

    ///// <summary>
    ///// 请求指定帖子的所有点赞
    ///// </summary>
    ///// <param name="postId"></param>
    ///// <param name="callBack"></param>
    //void RequestAllLileByPost(long postId, Action<bool, List<LikeInfo>> callBack);

    ///// <summary>
    ///// 请求指定帖子的所有评论
    ///// </summary>
    ///// <param name="postId"></param>
    ///// <param name="loadReplies"></param>
    ///// <param name="callBack"></param>
    //void RequestAllCommentByPost(long postId, bool loadReplies, Action<bool, List<CommentInfo>> callBack);

    /// <summary>
    /// 请求回复评论
    /// </summary>
    /// <param name="parentType">上级类型（可选值：评论）</param>
    /// <param name="parentId">上级编号（如评论编号）</param>
    /// <param name="content">正文内容</param>
    /// <param name="callBack">回调</param>
    void RequestReplyComment(string parentType, string parentId,string content, Action<bool,string, ReplyInfo> callBack);
    /// <summary>
    /// 删除回复评论请求
    /// </summary>
    /// <param name="replyId">回复Id</param>
    /// <param name="callback">回调</param>
    void ReqDeleteReply(string replyId,Action<bool,string> callback);
    /// <summary>
    /// 举报帖子请求
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="reason"></param>
    /// <param name="content"></param>
    /// <param name="callBack"></param>
    void RequestReportPost(string postId, string reason, string content, Action<bool> callBack);
    /// <summary>
    /// 请求指定用户的已发布的帖子
    /// </summary>
    /// <param name="posterId"></param>
    /// <param name="limit"></param>
    /// <param name="callBack"></param>
    /// <param name="skip"></param>
    void RequestPublishedPostByPoster(string posterId,string skip,string limit, Action<bool,List<PostInfo>> callBack);
    /// <summary>
    /// 根据指定用户查询点赞信息
    /// </summary>
    /// <param name="userid">用户Id</param>
    /// <param name="skip">忽略行数</param>
    /// <param name="limit">获取行数</param>
    /// <param name="descending">是否按降序</param>
    /// <param name="callback">回调</param>
    void ReqLikesByUser(string userid, string skip, string limit, string descending,Action<bool,string,List<LikeInfo>> callback);
    /// <summary>
    /// 删除帖子
    /// </summary>
    /// <param name="postid">帖子Id</param>
    /// <param name="callback">回调</param>
    void ReqDeletePost(string postid,Action<bool,string> callback);
    /// <summary>
    /// 根据作者帖子列表查询一组点赞信息
    /// </summary>
    /// <param name="skip">忽略条数</param>
    /// <param name="limit">获取条数</param>
    /// <param name="following">是否已关注 true  false </param>
    /// <param name="callback">回调</param>
    void ReqQuerySysLikeInfo(string skip,string limit,string following,Action<bool,string,List<SysLikeInfo>> callback);
    /// <summary>
    /// 根据作者帖子列表查询一组评论信息
    /// </summary>
    /// <param name="skip">忽略行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="following">是否已关注 true false</param>
    /// <param name="callback">回调</param>
    void ReqQuerySysCommentInfo(string skip, string limit, string following,
        Action<bool, string, List<SysCommentInfo>> callback);
    /// <summary>
    /// 查询一组论道推荐信息
    /// </summary>
    /// <param name="contenttype">内容类型（可选值：帖子）</param>
    /// <param name="callback">回调</param>
    void ReqRecommendationInfo(string contenttype ,Action<bool,string ,List<RecommendationInfo>> callback);

    /// <summary>
    /// 请求屏蔽用户
    /// </summary>
    /// <param name="blockeeId">被屏蔽用户的Id</param>
    /// <param name="callback">回调</param>
    void ReqCreateBlack(string blockeeId,Action<bool,string> callback);
    /// <summary>
    /// 请求删除屏蔽用户
    /// </summary>
    /// <param name="blockeeId">被屏蔽用户Id</param>
    /// <param name="callback">回调</param>
    void ReqDeleteBlack(string blockeeId, Action<bool, string> callback);
    /// <summary>
    /// 请求查询被屏蔽用户的信息
    /// </summary>
    /// <param name="blockerid">屏蔽者编号</param>
    /// <param name="skip">忽略的条数</param>
    /// <param name="limit">获取的条数</param>
    /// <param name="callback">回调</param>
    void ReqQueryBlocksInfo(string blockerid, string skip,string limit,Action<bool,string,List<BlockerInfo>> callback);
    /// <summary>
    /// 请求屏蔽一个帖子
    /// </summary>
    /// <param name="postId">帖子Id</param>
    /// <param name="callback">回调</param>
    void ReqCreatePostBlack(string postId,Action<bool,string> callback);
    /// <summary>
    /// 请求取消一个帖子屏蔽
    /// </summary>
    /// <param name="postId">帖子Id</param>
    /// <param name="callback">回调</param>
    void ReqDeletePostBlack(string postId, Action<bool, string> callback);
    /// <summary>
    /// 请求查询一组被屏蔽的帖子信息
    /// </summary>
    /// <param name="blockerid">屏蔽者编号</param>
    /// <param name="skip">忽略的条数</param>
    /// <param name="limit">获取的条数</param>
    /// <param name="callback">回调</param>
    void ReqQueryPostsBlockInfo(string blockerid,string skip,string limit,Action<bool,string,List<PostBlockInfo>> callback);



}
