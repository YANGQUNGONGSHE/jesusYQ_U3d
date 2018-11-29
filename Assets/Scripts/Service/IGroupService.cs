using System;
using NIM.Team;
using UnityEngine;

public interface IGroupService
{
    /// <summary>
    /// 创建群
    /// </summary>
    /// <param name="teamInfo">群组信息</param>
    /// <param name="idList">成员id列表(不包括自己)</param>
    /// <param name="postscript">附言</param>
    /// <param name="action"></param>
    void CreateGroup(NIMTeamInfo teamInfo, string[] idList, string postscript, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 邀请好友入群
    /// </summary>
    /// <param name="tid">群id</param>
    /// <param name="idList">被邀请人员id列表</param>
    /// <param name="postscript">邀请附言</param>
    /// <param name="action">操作结果回调</param>
    void Invite(string tid, string[] idList, string postscript, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 申请入群
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="reason"></param>
    /// <param name="action"></param>
    void ApplyForJoiningTeam(string tid, string reason, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 同意入群申请
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="uid"></param>
    /// <param name="action"></param>
    void AgreeJoinTeamApplication(string tid, string uid, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 拒绝入群申请
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="uid"></param>
    /// <param name="reason"></param>
    /// <param name="action"></param>
    void RejectJoinTeamApplication(string tid, string uid, string reason, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 接受入群邀请
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="invitor"></param>
    /// <param name="action"></param>
    void AcceptTeamInvitation(string tid, string invitor,  Action<NIMTeamEvent> callback);

    /// <summary>
    /// 拒绝入群邀请
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="invitor"></param>
    /// <param name="reason"></param>
    /// <param name="action"></param>
    void RejectTeamInvitation(string tid, string invitor, string reason, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 离开群
    /// </summary>
    /// <param name="tid">群id</param>
    /// <param name="action">操作结果回调函数</param>
    void LeaveTeam(string tid, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 解散群组
    /// </summary>
    /// <param name="tid">群id</param>
    /// <param name="action">操作结果回调函数</param>
    void DismissTeam(string tid, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 更新群信息
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="info"></param>
    /// <param name="action"></param>
    void UpdateTeamInfo(string tid, NIMTeamInfo info, Action<NIMTeamEvent> callback);
    
    /// <summary>
    /// 添加群管理员
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="managerIdArray"></param>
    /// <param name="action"></param>
    void AddTeamManagers(string tid, string[] managerIdArray,  Action<NIMTeamEvent> callback);
    
    /// <summary>
    /// 删除群管理员
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="managerIdArray"></param>
    /// <param name="action"></param>
    void RemoveTeamManagers(string tid, string[] managerIdArray, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 移交群主
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="newOwnerId"></param>
    /// <param name="leaveTeam">是否在移交后退出群</param>
    /// <param name="action"></param>
    void TransferTeamAdmin(string tid, string newOwnerId, bool leaveTeam, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 更新自己的群属性
    /// </summary>
    /// <param name="info"></param>
    /// <param name="action"></param>
    void UpdateMyTeamProperty(NIMTeamMemberInfo info, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 修改其他成员的群昵称
    /// </summary>
    /// <param name="info"></param>
    /// <param name="action"></param>
    void UpdateMemberNickName(NIMTeamMemberInfo info, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 查询自己的群
    /// </summary>
    /// <param name="action">int:count, string:ids</param>
    void QueryAllMyTeams(Action<int, System.Collections.Generic.List<string>> callback);

    /// <summary>
    /// 查询所有有效群信息
    /// </summary>
    /// <param name="action">NIMTeamInfo[]</param>
    void QueryMyValidTeamsInfo(Action<System.Collections.Generic.List<NIMTeamInfo>> callback);

    /// <summary>
    /// 查询所有群信息，包含无效的群
    /// </summary>
    /// <param name="includeInvalid"></param>
    /// <param name="action"></param>
    void QueryAllMyTeamsInfo(Action<NIMTeamInfo[]> callback);

    /// <summary>
    /// 查询本地缓存的群信息
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="action">string:tid</param>
    void QueryCachedTeamInfo(string tid, Action<string, NIMTeamInfo> callback);
    ///// <summary>
    ///// 在线查询群组信息
    ///// </summary>
    ///// <param name="tid">群组ID</param>
    ///// <param name="callback">回调</param>
    //void QueryTeamInfoOnline(string tid, Action<NIMTeamInfo> callback);

    /// <summary>
    /// 本地查询群信息(同步版本，堵塞NIM内部线程，谨慎使用)
    /// </summary>
    /// <param name="tid"></param>
    /// <returns></returns>
    NIMTeamInfo QueryCachedTeamInfo(string tid);

    /// <summary>
    /// 在线查询群信息
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="action"></param>
    void QueryTeamInfoOnline(string tid, Action<NIMTeamEvent> callback);

    /// <summary>
    /// 查询群成员信息
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="action">string:tid int:memberCount bool:includeUserInfo</param>
    void QueryTeamMembersInfo(string tid, Action<string, int, bool ,NIMTeamMemberInfo[]> callback);

    /// <summary>
    /// 查询(单个)群成员信息
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="uid"></param>
    /// <param name="action"></param>
    void QuerySingleMemberInfo(string tid, string uid, Action<NIMTeamMemberInfo> callback);

    /// <summary>
    /// 查询(单个)群成员信息(同步版本，堵塞NIM内部线程，谨慎使用)
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="uid"></param>
    NIMTeamMemberInfo QuerySingleMemberInfo(string tid, string uid);

    /// <summary>
    /// 踢出成员
    /// </summary>
    /// <param name="tid"></param>
    /// <param name="idList"></param>
    /// <param name="callback"></param>
    void KickMemberOutFromTeam(string tid, string[] idList, Action<NIMTeamEvent> callback);
    
    /// <summary>
    /// 获取群禁言成员列表
    /// </summary>
    /// <param name="tid">群组id</param>
    /// <param name="cb">int:resCode, int:count; string:tid</param>
    void QueryMutedListOnlineAsync(string tid, Action<int, int, string, NIMTeamMemberInfo[]> callback);

    /// <summary>
    /// 更新群组头像
    /// </summary>
    /// <param name="texture2D">头像texture2D</param>
    /// <param name="groupId">群组Id</param>
    /// <param name="callback">回调</param>
    void UpLoadGroupHeadImage(Texture2D texture2D, string groupId, Action<bool,string> callback);
} 
