using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class FriendService : IFriendService
{
  
    public void RequestFollows(string followerid, Action<bool, List<FollowInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.MyFollowList + "?followerid=" + followerid, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseMyFollow>(response.DataAsText);

            Log.I("我关注的用户数据!!!!!：" + rs.Follows.Count);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, rs.Follows);
                }
            }
            else
            {
                callback(false, null);
            }
        });
    }

    public void RequestFans(string ownerid, Action<bool, List<FollowFnInfo>> callback)
    {

        HttpManager.RequestGet(NEWURLPATH.MyFansList + "?ownerid=" + ownerid, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseMyFans>(response.DataAsText);
            Log.I("我的粉丝用户数据!!!!!：" + rs.Follows.Count);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, rs.Follows);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false, null);
                }
            }
        });
    }

    public void RequestAddFollow(string ownerId, Action<bool,string, Follow> callback)
    {
       
        var param = new Dictionary<string,string>()
        {
            {"OwnerId", ownerId}
        };

        HttpManager.RequestPost(NEWURLPATH.AddFollow,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseAddFollow>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true,string.Empty, rs.Follow);
                }
            }
            else
            {
                callback(false, rs.ResponseStatus.Message,null);
            }
        });
    }

    public void RequestDelectFollow(string ownerId, Action<bool, string> callback)
    {

        HttpManager.RequestDelete(NEWURLPATH.DeleteFollow+ "?OwnerId="+ownerId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseDeleteFollow>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, "取消关注成功");
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

    public void RequestUserInfoById(string userId, Action<int, User> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryUserById + "?UserId=" + userId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeSingleUserInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(rs.User.Id, rs.User);
                }
            }
            else
            {
                Log.I("!!!!!" + rs.ResponseStatus.Message);
            }
        });
    }
    public void RequestUserInfoById(string userId, object obj, Action<int, User, object> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryUserById + "?UserId=" + userId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeSingleUserInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(rs.User.Id, rs.User, obj);
                }
            }
            else
            {
                Log.I("!!!!!" + rs.ResponseStatus.Message);
            }
        });
    }
    public void RequestUserInfoById(string userId, object obj1, object obj2, Action<int, User, object, object> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryUserById + "?UserId=" + userId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeSingleUserInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(rs.User.Id, rs.User, obj1, obj2);
                }
            }
            else
            {
                Log.I("!!!!!" + rs.ResponseStatus.Message);
            }
        });
    }

    public void RequestUserInfoByUnameOrEmail(string userNameOrEmail, Action<bool, User> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryUserByUNameOrEmail+ "?UserNameOrEmail="+userNameOrEmail, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeSingleUserInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, rs.User);
                }
            }
            else
            {
                Log.I("!!!!!" + rs.ResponseStatus.Message);
            }
        } );
    }

    public void RequestUserInfoByDisplayName(string dN, Action<bool, User> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryUserByDisplayName + "?DisplayName=" + dN, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeSingleUserInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, rs.User);
                }
            }
            else
            {
                Log.I("!!!!!" + rs.ResponseStatus.Message);
            }
        });
    }
}
