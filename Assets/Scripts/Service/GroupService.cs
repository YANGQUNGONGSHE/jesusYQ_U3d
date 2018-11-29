using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using NIM.Team;
using UnityEngine;

public class GroupService : IGroupService
{
    public void AcceptTeamInvitation(string tid, string invitor, Action<NIMTeamEvent> callback)
    {
        TeamAPI.AcceptTeamInvitation(tid, invitor, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void AddTeamManagers(string tid, string[] managerIdArray, Action<NIMTeamEvent> callback)
    {
        TeamAPI.AddTeamManagers(tid,managerIdArray, data =>
        {
            if (callback != null)
            {
                callback(data.TeamEvent);
            }
        } );
    }

    public void AgreeJoinTeamApplication(string tid, string uid, Action<NIMTeamEvent> callback)
    {
        TeamAPI.AgreeJoinTeamApplication(tid, uid, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void ApplyForJoiningTeam(string tid, string reason, Action<NIMTeamEvent> callback)
    {
        TeamAPI.ApplyForJoiningTeam(tid, reason, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void CreateGroup(NIMTeamInfo teamInfo, string[] idList, string postscript, Action<NIMTeamEvent> callback)
    {
        TeamAPI.CreateTeam(teamInfo, idList, postscript, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void DismissTeam(string tid, Action<NIMTeamEvent> callback)
    {
        TeamAPI.DismissTeam(tid, (teamEventData) => 
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void Invite(string tid, string[] idList, string postscript, Action<NIMTeamEvent> callback)
    {
        TeamAPI.Invite(tid, idList, postscript, (teamEventData) =>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void KickMemberOutFromTeam(string tid, string[] idList, Action<NIMTeamEvent> callback)
    {
        TeamAPI.KickMemberOutFromTeam(tid, idList, (teamEventData)=>
        {
            if(callback != null)
            {
                 callback(teamEventData.TeamEvent);
            }
        });
    }

    public void LeaveTeam(string tid, Action<NIMTeamEvent> callback)
    {
        TeamAPI.LeaveTeam(tid, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void QueryAllMyTeams(Action<int, List<string>> callback)
    {
        TeamAPI.QueryAllMyTeams((count, accountIdList)=>
        {
            if(callback != null)
            {
                callback(count, count > 0 ? accountIdList.ToList() : null);
            }
        });
    }

    public void QueryAllMyTeamsInfo(Action<NIMTeamInfo[]> callback)
    {

    }

    public void QueryCachedTeamInfo(string tid, Action<string, NIMTeamInfo> callback)
    {
        TeamAPI.QueryCachedTeamInfo(tid, (id, teamInfo)=>
        {
            if(callback != null)
            {
                callback(id, teamInfo);
            }
        });
    }

    //public void QueryTeamInfoOnline(string tid, Action<NIMTeamInfo> callback)
    //{
    //    TeamAPI.QueryTeamInfoOnline(tid, data =>
    //    {
    //        if (callback != null)
    //        {
    //            callback(data.TeamEvent.TeamInfo);
    //        }
    //    } );
    //}

    public NIMTeamInfo QueryCachedTeamInfo(string tid)
    {
        return TeamAPI.QueryCachedTeamInfo(tid);
    }

    public void QueryMutedListOnlineAsync(string tid, Action<int, int, string, NIMTeamMemberInfo[]> callback)
    {
        throw new NotImplementedException();
    }

    public void UpLoadGroupHeadImage(Texture2D texture2D,string groupId, Action<bool, string> callback)
    {
        var param = new Dictionary<string,string>()
        {
            {"GroupId", groupId}
        };
        HttpManager.UploadHead(NEWURLPATH.UpdateGroupAvatar, texture2D, param,(request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeUpdateGroupAvatar>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, rs.IconUrl);
                Log.I("修改群组头像返回的地址：" + rs.IconUrl);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void QueryMyValidTeamsInfo(Action<List<NIMTeamInfo>> callback)
    {
        TeamAPI.QueryAllMyTeamsInfo((cb)=>{
            if(callback != null)
                callback(cb.ToList());
        });
    }

    public void QuerySingleMemberInfo(string tid, string uid, Action<NIMTeamMemberInfo> callback)
    {
        throw new NotImplementedException();
    }

    public NIMTeamMemberInfo QuerySingleMemberInfo(string tid, string uid)
    {
        throw new NotImplementedException();
    }

    public void QueryTeamInfoOnline(string tid, Action<NIMTeamEvent> callback)
    {
        TeamAPI.QueryTeamInfoOnline(tid, data =>
        {
            if (callback != null)
            {
                callback(data.TeamEvent);
            }
        });
    }

    public void QueryTeamMembersInfo(string tid, Action<string, int, bool, NIMTeamMemberInfo[]> callback)
    {
        TeamAPI.QueryTeamMembersInfo(tid, (Tid, count,includeUserInfo,members)=>
        {
            if(callback != null)
            {
                callback(Tid, count, includeUserInfo, members);
            }
        });
    }

    public void RejectJoinTeamApplication(string tid, string uid, string reason, Action<NIMTeamEvent> callback)
    {
        TeamAPI.RejectJoinTeamApplication(tid, uid, reason, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void RejectTeamInvitation(string tid, string invitor, string reason, Action<NIMTeamEvent> callback)
    {
        TeamAPI.RejectTeamInvitation(tid, invitor, reason, (teamEventData)=>
        {
            if(callback != null)
            {
                callback(teamEventData.TeamEvent);
            }
        });
    }

    public void RemoveTeamManagers(string tid, string[] managerIdArray, Action<NIMTeamEvent> callback)
    {
       TeamAPI.RemoveTeamManagers(tid,managerIdArray, data =>
       {
           if (callback != null)
           {
               callback(data.TeamEvent);
           }
       });
    }

    public void TransferTeamAdmin(string tid, string newOwnerId, bool leaveTeam, Action<NIMTeamEvent> callback)
    {
        TeamAPI.TransferTeamAdmin(tid, newOwnerId, leaveTeam, data =>
        {
        if (callback!= null)
        {
            callback(data.TeamEvent);
        }
        });
    }

    public void UpdateMemberNickName(NIMTeamMemberInfo info, Action<NIMTeamEvent> callback)
    {
    }

    public void UpdateMyTeamProperty(NIMTeamMemberInfo info, Action<NIMTeamEvent> callback)
    {
        throw new NotImplementedException();
    }

    public void UpdateTeamInfo(string tid, NIMTeamInfo info, Action<NIMTeamEvent> callback)
    {
        TeamAPI.UpdateTeamInfo(tid, info, (teamEvent)=>
        {
            if(callback != null)
            {
                callback(teamEvent.TeamEvent);
            }
        });
    }
}