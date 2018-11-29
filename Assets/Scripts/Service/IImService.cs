using System;
using System.Collections;
using System.Collections.Generic;
using NIM;
using NIM.Friend;
using NIM.Messagelog;
using NIM.Session;
using NIM.SysMessage;
using NIM.User;
using UnityEngine;

public interface IImService
{
    /// <summary>
    /// AppDataDir目录必须具有读写权限，SDK数据都存储在这个目录中
    /// </summary>
    string AppDataPath { get; }

    /// <summary>
    /// 登录IM
    /// </summary>
    /// <param name="account"></param>
    /// <param name="token"></param>
    /// <param name="callback"></param>
    void LoginIm(string account, string token, Action<bool> callback);

    /// <summary>
    /// 登出IM
    /// </summary>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    void LogoutIm(NIMLogoutType type, Action<NIMLogoutResult> callback);

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    void SendMessage(NIMIMMessage msg);

    /// <summary>
    /// 查询会话记录
    /// </summary>
    /// <param name="callback"></param>
    void QuerySessionRecord(Action<int, SesssionInfoList> callback);

    /// <summary>
    /// 查询聊天记录
    /// </summary>
    /// <param name="accountId">会话Id</param>
    /// <param name="sessionType">会话类型</param>
    /// <param name="limit">获取的条数</param>
    /// <param name="timeTag"></param>
    /// <param name="callback"></param>
    void QueryChatRecord(string accountId, NIMSessionType sessionType, int limit, long timeTag,
        Action<ResponseCode, string, NIMSessionType, MsglogQueryResult> callback);

    /// <summary>
    /// 根据消息ID查询单条记录
    /// </summary>
    /// <param name="msgId"></param>
    /// <param name="callback"></param>
    void QuerySingleRecord(string msgId, Action<ResponseCode, string, NIMIMMessage> callback);

    /// <summary>
    /// 查询本地用户资料
    /// </summary>
    /// <param name="iDs"></param>
    /// <param name="callback"></param>
    void QueryUserNameCard(List<string> iDs, Action<UserNameCard[]> callback);

    /// <summary>
    /// 查询本地好友列表
    /// </summary>
    /// <param name="callback"></param>
    void QueryFirendList(Action<NIMFriends> callback);
    /// <summary>
    /// 查询本地系统消息
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="lastTimetag"></param>
    /// <param name="callback"></param>
    void QuerySystemMessage(int limit, long lastTimetag, Action<NIMSysMsgQueryResult> callback);
    /// <summary>
    /// 查询系统未读消息
    /// </summary>
    /// <param name="callback"></param>
    void QuerySystemMsUnCount(Action<ResponseCode, int> callback);

    /// <summary>
    /// 批量设置未读状态为已读消息状态(非系统消息)
    /// </summary>
    /// <param name="sType"></param>
    /// <param name="callback"></param>
    /// <param name="id"></param>
    void MarkMessagesStatusRead(string id, NIMSessionType sType, Action<ResponseCode ,string ,NIMSessionType> callback);
    /// <summary>
    /// 删除会话联系人
    /// </summary>
    /// <param name="sessionType">会话类型</param>
    /// <param name="id">对方的account id或者群组tid</param>
    /// <param name="callback">回调</param>
    void DeleteSession(NIMSessionType sessionType, string id,Action<int,SessionInfo,int> callback);
    /// <summary>
    /// 清空聊天记录
    /// </summary>
    /// <param name="sessionType">会话类型</param>
    /// <param name="id">对方的account id或者群组tid</param>
    /// <param name="callback">回调</param>
    void CleanRecordData(NIMSessionType sessionType, string id,Action<ResponseCode ,string ,NIMSessionType> callback);
    /// <summary>
    /// 设置系统消息的状态
    /// </summary>
    /// <param name="msgId"></param>
    /// <param name="status"></param>
    /// <param name="callback"></param>
    void SetSysMessagesStatus(long msgId, NIMSysMsgStatus status,Action<int> callback);
    /// <summary>
    /// 按消息类型批量设置消息状态
    /// </summary>
    /// <param name="type">消息类型</param>
    /// <param name="status">消息状态</param>
    /// <param name="callback">回调</param>
    void SetMsgStatusByType(NIMSysMsgType type, NIMSysMsgStatus status,Action<int> callback);
    /// <summary>
    /// 删除全部系统消息
    /// </summary>
    /// <param name="callback"></param>
    void DeleteAllSys(Action<int> callback);
    /// <summary>
    /// 按消息类型批量删除消息
    /// </summary>
    /// <param name="type">系统消息类型</param>
    /// <param name="callback">回调</param>
    void DeleteMsgByType(NIMSysMsgType type,Action<int> callback);

    /// <summary>
    /// 设置添加或删除黑名单
    /// </summary>
    /// <param name="accountId">用户Id</param>
    /// <param name="inBlacklist">if set to <c>true</c> [set_black].</param>
    /// <param name="callback">操作结果回调</param>
    void SetBlacklist(string accountId,bool inBlacklist,Action<ResponseCode> callback);

    /// <summary>
    /// 获取用户关系列表(黑名单和静音列表)
    /// </summary>
    /// <param name="callback">操作回调</param>
    void GetRelationshipList(Action<ResponseCode, UserSpecialRelationshipItem[]>callback);

    void InitSdk();



}
