using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAccountService
{

    /// <summary>
    /// 请求验证码
    /// </summary>
    /// <param name="phoneNumber">号码</param>
    /// <param name="purpose">验证码用途</param>
    /// <param name="callback">回调</param>
    void RequestVerfiyCode(string phoneNumber,string purpose,Action<bool, string> callback);

    /// <summary>
    /// 请求校验验证码
    /// </summary>
    /// <param name="phoneNumber">号码</param>
    /// <param name="purpose">验证码用途</param>
    /// <param name="token">验证码</param>
    /// <param name="callBack">回调</param>
    void CheckVerfiyCode(string phoneNumber, string purpose,string token,Action<bool, string> callBack);

    /// <summary>
    /// 手机登验证码录
    /// </summary>
    /// <param name="phoneNumber">号码</param>
    /// <param name="token">验证码</param>
    /// <param name="callback">回调</param>
    void RequestLogins(string phoneNumber, string token, Action<bool, Js_ResponseLogins> callback);
    
    /// <summary>
    /// 解除绑定手机号
    /// </summary>
    /// <param name="phoneNumber">号码</param>
    /// <param name="callback">回调</param>
    void ReqCancelBindPhone(string phoneNumber, Action<bool, string> callback);
    
    /// <summary>
    /// 绑定手机号码
    /// </summary>
    /// <param name="phoneNumber">号码</param>
    /// <param name="token">验证码</param>
    /// <param name="callback">回调</param>
    void ReqBindPhone(string phoneNumber, string token, Action<bool, string> callback);

    /// <summary>
    /// 更改用户显示名称
    /// </summary>
    /// <param name="displayName">名称</param>
    /// <param name="callback">回调</param>
    void RequestUpdateDisplayName(string displayName,Action<bool , string> callback);
    
    /// <summary>
    /// 查询用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="callback"></param>
     void QuerySingleUserInfo(string userId, Action<bool, Js_ResponeSingleUserInfo> callback);
    /// <summary>
    /// 根据用户Ids查询用户数据
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="skip"></param>
    /// <param name="limit"></param>
    /// <param name="callback"></param>
    void QueryUsersByIds(string userIds, string skip, string limit, Action<bool,string, List<User>> callback);
    /// <summary>
    /// 查询一组用户
    /// </summary>
    /// <param name="orderby">排序的字段</param>
    /// <param name="descending">是否按降序排序</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="callback">回调</param>
    void QueryUsers(string orderby,string descending,string skip,string limit,Action<bool,string,List<User>> callback);
    
    /// <summary>
    /// 登陆成功后查询账号信息
    /// </summary>
    /// <param name="callback">回调</param>
    void QueryAccountInfo(Action<bool, Js_ResponeAccountInfo> callback);
    
    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="callback">回调</param>
    void LoginOut(Action<bool, string> callback);
    
    /// <summary>
    /// 请求修改用户头像
    /// </summary>
    /// <param name="texture2D"></param>
    /// <param name="callback"></param>
    void ReqUpdateUserHead(Texture2D texture2D, Action<bool,string> callback);
    
    /// <summary>
    /// 请求修改个性签名
    /// </summary>
    /// <param name="signature"></param>
    /// <param name="callback"></param>
    void ReqUpdateSignature(string signature, Action<bool, string> callback);
    
    /// <summary>
    /// 请求修改所属地
    /// </summary>
    /// <param name="country"></param>
    /// <param name="state"></param>
    /// <param name="city"></param>
    /// <param name="callback"></param>
    void ReqUpdateLocal(string country, string state, string city, Action<bool, string> callback);
    
    /// <summary>
    /// 请求修改性别
    /// </summary>
    /// <param name="gender"></param>
    /// <param name="callback"></param>
    void ReqUpdateGender(string gender, Action<bool, string> callback);
    
    /// <summary>
    /// 请求修改出生日期
    /// </summary>
    /// <param name="birthDate"></param>
    /// <param name="callback"></param>
    void ReqUpdateBirthday(string birthDate, Action<bool, string> callback);
    
    /// <summary>
    /// 查询所有国家的数据
    /// </summary>
    /// <param name="callback"></param>
    void ReqQueryCountries(Action<bool, List<LocalInfo>> callback);

    /// <summary>
    /// 根据国家Id查询省份的数据
    /// </summary>
    /// <param name="countryid"></param>
    /// <param name="callback"></param>
    void ReqQueryStates(string countryid, Action<bool, List<LocalInfo>> callback);

    /// <summary>
    /// 根据省份Id查询城市的数据
    /// </summary>
    /// <param name="stateid"></param>
    /// <param name="callback"></param>
    void ReqQueryCities(string stateid, Action<bool, List<LocalInfo>> callback);

    /// <summary>
    /// 根据用户Id请求阅读记录数据
    /// </summary>
    /// <param name="userid">用户Id</param>
    /// <param name="parenttype">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="parentidprefix">上级编号的前缀（如帖子编号）</param>
    /// <param name="descending">是否按降序排序</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="callback">回调</param>
    void ReqQueryUserReadRecord(string userid, string parenttype, string parentidprefix, string descending, string skip, string limit, Action<bool, string, List<ReadRecordInfo>> callback);
    /// <summary>
    /// 根据用户Id请求阅读记录数量
    /// </summary>
    /// <param name="userid">用户Id</param>
    /// <param name="parenttype">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="parentidprefix">上级编号的前缀（如帖子编号）</param>
    /// <param name="callback">回调</param>
    void ReqQueryUserReadRecordCount(string userid, string parenttype, string parentidprefix,Action<bool,string, ReadRecordCount> callback);
    /// <summary>
    /// 根据用户Id列表请求阅读记录数量
    /// </summary>
    /// <param name="userids">用户Ids</param>
    /// <param name="parenttype">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="parentidprefix">上级编号的前缀（如帖子编号）</param>
    /// <param name="createdsince">创建日期在指定的时间之后</param>
    /// <param name="callback">回调</param>
    void ReqQueryReadRecordCountByUsers(string userids, string parenttype, string parentidprefix, string createdsince, Action<bool, string, Dictionary<string, ReadRecordCount>> callback);
    /// <summary>
    /// 根据用户Id请求收藏记录
    /// </summary>
    /// <param name="userid">用户ID</param>
    /// <param name="parenttype">上级类型（可选值：帖子, 章, 节）</param>
    /// <param name="descending">是否按降序排序</param>
    /// <param name="skip">忽略的行数</param>
    /// <param name="limit">获取的行数</param>
    /// <param name="callback">回调</param>
    void ReqCollectByUser(string userid,string parenttype,string descending,string skip,string limit,Action<bool,string,List<CollectInfo>> callback);
    /// <summary>
    /// 根据Id取消收藏
    /// </summary>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    /// <param name="callback">回调</param>
    void ReqDeleteCollectRecord(string parentId,Action<bool,string> callback);
    /// <summary>
    /// 创建举报
    /// </summary>
    /// <param name="parentType">上级类型（可选值：用户, 帖子, 评论, 回复）</param>
    /// <param name="parentId">上级编号（如帖子编号）</param>
    /// <param name="reason">理由</param>
    /// <param name="callback">回调</param>
    void ReqCreateReport(string parentType,string parentId,string reason,Action<bool,string, AbuseReport> callback);
    /// <summary>
    /// 创建反馈
    /// </summary>
    /// <param name="content">反馈内容</param>
    /// <param name="callback">回调</param>
    void ReqCreateFeedBack(string content, Action<bool, string> callback);
    /// <summary>
    /// 请求查询个人排名
    /// </summary>
    /// <param name="orderby">排序的字段 ParagraphViewsRank </param>
    /// <param name="skip">忽略的条数</param>
    /// <param name="limit">获取的条数</param>
    /// <param name="callback">回调</param>
    void ReqQueryPersonalRank(string orderby,string skip,string limit,Action<bool,string,List<PersonalRankInfo>> callback);
    /// <summary>
    /// 请求查询群组排名
    /// </summary>
    /// <param name="orderby">排序的字段 ParagraphViewsRank</param>
    /// <param name="skip">忽略的条数</param>
    /// <param name="limit">获取的条数</param>
    /// <param name="callback">回调</param>
    void ReqQueryGroupsRank(string orderby, string skip, string limit,Action<bool,string,List<GroupRankInfo>> callback);
    /// <summary>
    /// 请求查询单个用户排名
    /// </summary>
    /// <param name="userid">用户Id</param>
    /// <param name="callback">回调</param>
    void ReqQueryPersonalRankById(string userid, Action<bool, string, PersonalRankInfo> callback);
    /// <summary>
    /// 请求查询群组排名ByGroupIds
    /// </summary>
    /// <param name="groupids">群组id列表</param>
    /// <param name="orderby">排序的字段 ParagraphViewsRank</param>
    /// <param name="callback">回调</param>
    void ReqQueryGroupsRankByIds( string groupids,string orderby,Action<bool,string,List<GroupRankInfo>> callback);
    /// <summary>
    /// 请求查询最后一次阅读记录
    /// </summary>
    /// <param name="bookid">bookId</param>
    /// <param name="callback">回调</param>
    void ReqQueryLastReadRecordData(string bookid,Action<bool,string, ChapterRead> callback);
}
