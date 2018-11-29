using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFriendService
{
    /// <summary>
    /// 请求关注列表
    /// </summary>
    /// <param name="followerid">谁的关注列表</param>
    /// <param name="callback">回调</param>
    void RequestFollows(string followerid, Action<bool, List<FollowInfo>> callback);

    /// <summary>
    /// 请求粉丝列表
    /// </summary>
    /// <param name="ownerid">谁的粉丝列表</param>
    /// <param name="callback">回调</param>
    void RequestFans(string ownerid, Action<bool, List<FollowFnInfo>> callback);
    /// <summary>
    /// 请求新建关注
    /// </summary>
    /// <param name="ownerId">被关注者Id</param>
    /// <param name="callback">回调</param>
    void RequestAddFollow(string ownerId,Action<bool,string, Follow> callback);
    /// <summary>
    /// 请求取消关注
    /// </summary>
    /// <param name="ownerId">被关注者Id</param>
    /// <param name="callback">回调</param>
    void RequestDelectFollow(string ownerId, Action<bool,string> callback);
    /// <summary>
    /// 请求单个用户信息ById
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="callback">回调</param>
    void RequestUserInfoById(string userId, Action<int, User> callback);

    void RequestUserInfoById(string userId, object obj, Action<int, User, object> callback);

    void RequestUserInfoById(string userId, object obj1, object obj2, Action<int, User, object, object> callback);

    /// <summary>
    /// 请求用户信息By UserNameOrEmail
    /// </summary>
    /// <param name="userNameOrEmail">手机号码或邮箱地址</param>
    /// <param name="callback">回调</param>
    void RequestUserInfoByUnameOrEmail(string userNameOrEmail, Action<bool, User> callback);
    /// <summary>
    /// 请求用户信息By DisplayName
    /// </summary>
    /// <param name="dN">显用户示名称</param>
    /// <param name="callback">回调</param>
    void RequestUserInfoByDisplayName(string dN, Action<bool, User> callback);

}
