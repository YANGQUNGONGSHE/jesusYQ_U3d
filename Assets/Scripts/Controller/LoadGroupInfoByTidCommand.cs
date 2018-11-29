using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NIM.Team;
using NIM;
using strange.extensions.command.impl;
using UnityEngine;
using NIM.User;

public class LoadGroupInfoByTidCommand : EventCommand
{   
    [Inject]
    public IGroupService GroupService {get; set;}

    [Inject]
    public IFriendService FriendService {get; set;}
    [Inject]
    public IAccountService AccountService { get; set; }

    private ArgLoadGroupInfo m_ArgLoadGroupInfo;

    private List<GroupMeberInfoModel> m_GroupMebers;

    private int m_CurrentTraversalCount = 0;
    private LoadType _mLoadType;

    public override void Execute()
    {
        Retain();
        _mLoadType = (LoadType)evt.data ;
        
        m_GroupMebers = new List<GroupMeberInfoModel>();
        GroupService.QueryCachedTeamInfo(_mLoadType.SessionId, (tid, info)=>
        {
            var id = tid;
            var teamInfo = info;
            GroupService.QueryTeamMembersInfo(id, (teamId, count, isinculd, mebs)=>
            {
                m_ArgLoadGroupInfo = new ArgLoadGroupInfo()
                {
                    Tid = id,
                    Announcement = teamInfo.Announcement,
                    Name = teamInfo.Name
                };
                List<NIMTeamMemberInfo> meberinofs = mebs.ToList();
                WongJJ.Game.Core.Dispatcher.InvokeAsync(iteratorInMainThread,meberinofs);
            });
        });
    }

    private void iteratorInMainThread(List<NIMTeamMemberInfo> meberinofs)
    {
        if(meberinofs != null && meberinofs.Count > 0)
        {
            m_CurrentTraversalCount = meberinofs.Count;
            for(int i =0; i< meberinofs.Count; i++)
            {
                var i1 = i;
                FriendService.RequestUserInfoById(meberinofs[i].AccountId, (uid, userInfo) =>
                {
                    var meberinfo = new GroupMeberInfoModel()
                    {
                        Uid = uid,
                        HeadIconUrl = userInfo.AvatarUrl,
                        UserName = userInfo.UserName,
                        Displayname = userInfo.DisplayName,
                        UserType = meberinofs[i1].Type,
                        Signature = userInfo.Signature,
                    };
                    m_GroupMebers.Add(meberinfo);
                    if (m_GroupMebers.Count == m_CurrentTraversalCount)
                        LoadMemberHeadicons();
                });
            }
        }
    }

    private void LoadMemberHeadicons()
    {
        if(m_GroupMebers != null && m_GroupMebers.Count > 0)
        {
            int[] count = {0};
            for (var i = 0; i < m_GroupMebers.Count; i++)
            {
                if (!string.IsNullOrEmpty(m_GroupMebers[i].HeadIconUrl))
                {
                    var i1 = i;
                    HttpManager.RequestImage(m_GroupMebers[i].HeadIconUrl + LoadPicStyle.ThumbnailHead, (url, texture2D)=> 
                    {

                        m_GroupMebers[i1].HeadIconTexture2D = texture2D != null ? texture2D : DefaultImage.Head;
                        count[0] += 1;
                     
                        if (count[0] == m_GroupMebers.Count)
                        {
                            m_ArgLoadGroupInfo.GroupMeberInfoModels = m_GroupMebers;
                            m_ArgLoadGroupInfo.IsUpdateGroup = _mLoadType.IsUpdateTeam == true;
                            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupInfoByIdFinish, m_ArgLoadGroupInfo);
                        }
                    });
                }
                else
                {
                    m_GroupMebers[i].HeadIconTexture2D = DefaultImage.Head;
                    count[0] += 1;

                    if (count[0] == m_GroupMebers.Count)
                    {
                        m_ArgLoadGroupInfo.GroupMeberInfoModels = m_GroupMebers;
                        m_ArgLoadGroupInfo.IsUpdateGroup = _mLoadType.IsUpdateTeam == true;
                        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupInfoByIdFinish, m_ArgLoadGroupInfo);
                    }
                }
            }
        }
    }
}

public class GroupMeberInfoModel
{
    public int Uid;

    public NIMTeamUserType UserType;

    public string Displayname;
    public string UserName;

    public string HeadIconUrl;

    public string Signature;

    public Texture2D HeadIconTexture2D;
    
}

public class ArgLoadGroupInfo
{
    public bool IsUpdateGroup;
    /// <summary>
    /// 群组Id
    /// </summary>
    public string Tid;
    /// <summary>
    /// 群组名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 群组公告
    /// </summary>
    public string Announcement;
    /// <summary>
    /// 群内成员信息
    /// </summary>
    public List<GroupMeberInfoModel> GroupMeberInfoModels;
}

public struct LoadType
{
    /// <summary>
    /// 会话Id
    /// </summary>
    public string SessionId;
    /// <summary>
    /// 是否是更新群组
    /// </summary>
    public bool IsUpdateTeam;
}