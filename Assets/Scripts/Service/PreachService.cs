using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using UnityEngine;

public class PreachService : IPreachService {


    public void RequestCreatePost<T>(string title, T paragraphs, Action<bool, PostInfo> callBack)
    {
        throw new NotImplementedException();
    }

    public void RequestSinglePreach(string postId, Action<bool, string,BasePostInfo> callBack)
    {
        HttpManager.RequestGet(NEWURLPATH.QuerySinglePost+ "?PostId="+postId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeSinglePostData>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true,string.Empty, rs.Post);
            }
            else
            {
                if (callBack != null)
                    callBack(false,rs.ResponseStatus.Message, null);
            }
        } );
    }

    public void RequestHotPreach(string titlefilter,string ispublished, string orderby,string skip, string limit,string descending, Action<bool, string, List<BasePostInfo>> callBack)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryBasePosts+ "?titlefilter="+ titlefilter+ "&ispublished="+ ispublished+ "&orderby=" + orderby+ "&skip=" + skip+ "&limit="+ limit+"&descending=" + descending, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponePostsData>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true,string.Empty, rs.Posts);
            }
            else
            {
                if (callBack != null)
                    callBack(false, rs.ResponseStatus.Message, null);
            }
        });
    }

    public void RequestPreachByFocused(string ispublished,string orderBy,string descending, string skip, string limit, Action<bool, string, List<BasePostInfo>> callBack)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryBasePostsbyfollowingowners+ "?ispublished="+ ispublished+ "&orderby=" + orderBy+ "&descending=" + descending +"&skip="+skip+"&limit="+limit, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponePostsData>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true, string.Empty, rs.Posts);
            }
            else
            {
                if (callBack != null)
                    callBack(false, rs.ResponseStatus.Message, null);
            }
        });
    }

    public void RequestPreachByAuthor(string authorid, string titlefilter, string ispublished, string skip, string limit, string descending,
        Action<bool, string, List<BasePostInfo>> callBack)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryBasePostsbyauthor + "?authorid="+ authorid + "&titlefilter=" + titlefilter + "&ispublished" + ispublished + "&skip=" + skip + "&limit=" + limit + "&descending=" + descending, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponePostsData>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true, string.Empty, rs.Posts);
            }
            else
            {
                if (callBack != null)
                    callBack(false, rs.ResponseStatus.Message, null);
            }
        }, false);
    }

    public void RequestCreateLike(string parentType, string parentId, Action<bool, string, LikeInfo> callBack)
    {
       var param = new Dictionary<string,string>()
       {
           {"ParentType", parentType},
           {"ParentId", parentId},
       };
       HttpManager.RequestPost(NEWURLPATH.ReqCreateLike,param, (request, response) =>
       {
           var rs = JsonMapper.ToObject<Js_ResponeCreateLike>(response.DataAsText);
           if (rs.ResponseStatus == null)
           {
               if (callBack != null)
                   callBack(true, string.Empty, rs.Like);
           }
           else
           {
               if (callBack != null)
                   callBack(false, rs.ResponseStatus.Message, null);
           }
       } );
    }

    public void RequestDeleteLike(string parentId, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.ReqCancelLike+ "?ParentId="+ parentId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void RequestCreateComment(string parentType, string parentId, string content, Action<bool, string, CommentInfo> callBack)
    {
        var param =new Dictionary<string,string>()
        {
            {"ParentType",parentType },
            {"ParentId",parentId },
            {"Content",content },
        };

        HttpManager.RequestPost(NEWURLPATH.CreateComment,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeCreateComment>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true,string.Empty, rs.Comment);
            }
            else
            {
                if (callBack != null)
                    callBack(false,rs.ResponseStatus.Message, null);
            }
        } );
    }

    public void ReqDeleteComment(string commentId, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.DeleteComment+ "?CommentId="+ commentId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }

        } );
    }

    public void RequestForwardPost(string postId, string annotion, Action<bool, PostInfo> callBack)
    {
        throw new NotImplementedException();
    }

    public void RequestPublishPost(string postId, Action<bool> callBack)
    {
        throw new NotImplementedException();
    }

    public void RequestReplyComment(string parentType, string parentId, string content, Action<bool, string, ReplyInfo> callBack)
    {
        var param = new Dictionary<string,string>()
        {
            {"ParentType",parentType},
            {"ParentId",parentId},
            {"Content",content},
        };
        HttpManager.RequestPost(NEWURLPATH.CreateReply,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeReplyComment>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true, string.Empty, rs.Reply);
            }
            else
            {
                if (callBack != null)
                    callBack(false, rs.ResponseStatus.Message, null);
            }
        } );
    }

    public void ReqDeleteReply(string replyId, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.DeleteReply+ "?ReplyId="+replyId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        } );
    }

    public void RequestReportPost(string postId, string reason, string content, Action<bool> callBack)
    {
        throw new NotImplementedException();
    }

    public void RequestPublishedPostByPoster(string posterId, string skip, string limit, Action<bool, List<PostInfo>> callBack)
    {
        throw new NotImplementedException();
    }

    public void ReqLikesByUser(string userid, string skip, string limit, string descending, Action<bool, string, List<LikeInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QuerylLikesByUser + "?userid=" +userid+ "&skip=" + skip + "&limit=" + limit + "&descending=" + descending,
            (request, response) =>
            {
                var rs = JsonMapper.ToObject<Js_ResponeLikesData>(response.DataAsText);
                if (rs.ResponseStatus == null)
                {
                    if (callback != null)
                        callback(true, string.Empty, rs.Likes);
                }
                else
                {
                    if (callback != null)
                        callback(false, rs.ResponseStatus.Message, null);
                }
            });
    }

    public void ReqDeletePost(string postid, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.DeletePost+ "?PostId="+ postid, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        } );
    }

    public void ReqQuerySysLikeInfo(string skip, string limit, string following, Action<bool, string, List<SysLikeInfo>> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.QueryLikeInfoForSelf+ "?following="+following+ "&skip="+skip+ "&limit="+limit,
           (request, response) =>
           {
               var rs = JsonMapper.ToObject<Js_ResponeSysLikeInfo>(response.DataAsText);
               if (rs.ResponseStatus == null)
               {
                   if (callback != null)
                       callback(true, string.Empty, rs.Likes);
               }
               else
               {
                   if (callback != null)
                       callback(false, rs.ResponseStatus.Message, null);
               }
           });
    }

    public void ReqQuerySysCommentInfo(string skip, string limit, string following, Action<bool, string, List<SysCommentInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryCommentInfoForSelf + "?following=" + following + "&skip=" + skip + "&limit=" + limit,
            (request, response) =>
            {
                var rs = JsonMapper.ToObject<Js_ResoneSysCommentInfo>(response.DataAsText);
                if (rs.ResponseStatus == null)
                {
                    if (callback != null)
                        callback(true, string.Empty, rs.Comments);
                }
                else
                {
                    if (callback != null)
                        callback(false, rs.ResponseStatus.Message, null);
                }
            });
    }

    public void ReqRecommendationInfo(string contenttype, Action<bool, string, List<RecommendationInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryRecommendation+ "?contenttype="+contenttype, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeRecommendationInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty, rs.Recommendations);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message, null);
            }
        } );
    }



    public void ReqCreateBlack(string blockeeId, Action<bool, string> callback)
    {
        var param = new Dictionary<string,string>()
        {
            {"BlockeeId",blockeeId }
        };
        HttpManager.RequestPost(NEWURLPATH.ReqCreateBlack,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeCreateBlockInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, string.Empty);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false, rs.ResponseStatus.Message);
                }
            }
        } );
    }

    public void ReqDeleteBlack(string blockeeId, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.ReqDeleteBlack+ "?BlockeeId="+blockeeId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, string.Empty);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false, rs.ResponseStatus.Message);
                }
            }
        } );
    }

    public void ReqQueryBlocksInfo(string blockerid, string skip, string limit, Action<bool, string, List<BlockerInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.ReqBlackedInfo+ "?blockerid="+ blockerid + "&skip=" + skip + "&limit=" + limit,
            (request, response) =>
            {
                var rs = JsonMapper.ToObject<Js_ResponeBlocksInfo>(response.DataAsText);
                if (rs.ResponseStatus == null)
                {
                    if (callback != null)
                    {
                        callback(true, string.Empty, rs.Blocks);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(false, rs.ResponseStatus.Message, null);
                    }
                }
            });
    }

    public void ReqCreatePostBlack(string postId, Action<bool, string> callback)
    {
        var param = new Dictionary<string, string>()
        {
            {"PostId",postId }
        };
        HttpManager.RequestPost(NEWURLPATH.ReqCreatePostBlack, param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeCreatePostBlockInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, string.Empty);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false, rs.ResponseStatus.Message);
                }
            }
        });
    }

    public void ReqDeletePostBlack(string postId, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.ReqDeletePostBlack + "?PostId=" + postId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, string.Empty);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false, rs.ResponseStatus.Message);
                }
            }
        });
    }

    public void ReqQueryPostsBlockInfo(string blockerid, string skip, string limit, Action<bool, string, List<PostBlockInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.ReqBlackedPostsInfo + "?blockerid=" + blockerid + "&skip=" + skip + "&limit=" + limit,
            (request, response) =>
            {
                var rs = JsonMapper.ToObject<Js_ResponePostsBlockInfo>(response.DataAsText);
                if (rs.ResponseStatus == null)
                {
                    if (callback != null)
                    {
                        callback(true, string.Empty, rs.PostBlocks);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(false, rs.ResponseStatus.Message, null);
                    }
                }
            });
    }
}
