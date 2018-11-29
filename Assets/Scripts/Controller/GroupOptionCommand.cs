using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NIM;
using NIM.SysMessage;
using NIM.Team;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class GroupOptionCommand : EventCommand
{
	[Inject]
    public IGroupService GroupService { get; set; }
	[Inject]
	public UserModel UserModel{get; set;}
    [Inject]
    public IImService ImService { get; set; }

	public override void Execute()
	{
		Retain();
		var param = (ArgGroupOptionParam)evt.data;
		switch(param.Option)
		{
			case EGroupOption.Create:
				CreateGroup(param.GroupName, param.GroupIntroduce);
				break;

			case EGroupOption.Dismiss:
				DismissGroup(param.GroupId);
				break;

			case EGroupOption.Level:
                LeaveGroup(param.GroupId);
				break;
			
			case EGroupOption.AddMember:
				if(param.AddMembersId.Count > 0)
				{
					AddMemberToGroup(param.GroupId, param.AddMembersId.ToArray());
				}
				break;

			case EGroupOption.Kick:
				if(param.KickMembersId.Count > 0)
				{
					KickMember(param.GroupId, param.KickMembersId.ToArray());
				}
				break;
            case EGroupOption.QuerySingleTeam:
                QuerySingleTeam(param.GroupId);
                break;

            case EGroupOption.UpdateAnnounce:
                UpdateAnnounceInfo(param.GroupId,param.TeamInfo);
                break;
            case EGroupOption.AddTeamManager:
                if (param.AddMembersId.Count > 0)
                {
                    AddGroupManager(param.GroupId,param.AddMembersId.ToArray());
                }
                break;
            case EGroupOption.RemoveTeamManager:

                if (param.KickMembersId.Count > 0)
                {
                    RemoveGroupManager(param.GroupId,param.KickMembersId.ToArray());
                }
                break;
            case EGroupOption.UpdateTeamData:

                UpdateGroupData(param.GroupId,param.TeamInfo);
                break;
            case EGroupOption.TransferTeam:

                TransferGroup(param.GroupId,param.NewOwnId,param.IsLeave);
                break;
            case EGroupOption.ApplyJoinTeam:

                ApplyJoinGroup(param.GroupId,param.ApplyReson);
                break;
            case EGroupOption.AgreeJoinTeam:

                AgreeJoinTeam(param.GroupId,param.Uid,param.MsgId,param.SysMsgStatus);
                break;
            case EGroupOption.RejectJoinTeam:

                RejectJoinTeam(param.GroupId,param.Uid,param.ApplyReson,param.MsgId,param.SysMsgStatus);
                break;
            case EGroupOption.UploadGroupHeadImage:
                UploadGroupHeadImage(param.GroupId, param.GroupTexture2D);
                break;
		}
	}

	private void CreateGroup(string groupName, string introduce)
	{
		NIMTeamInfo info = new NIMTeamInfo()
		{
			Name = groupName,
			TeamType = NIMTeamType.kNIMTeamTypeAdvanced,
			OwnerId = UserModel.User.Id.ToString(),
			Introduce = introduce,
			Announcement = "发布新公告，邀请好友！",
			//被邀请人同意方式
			BeInvitedMode = NIMTeamBeInviteMode.kNIMTeamBeInviteModeNotNeedAgree,
			//群主或管理员才有权限邀请他人入群
			InvitedMode = NIMTeamInviteMode.kNIMTeamInviteModeEveryone
		};
		GroupService.CreateGroup(info, null, "我拉你进群一起玩", cb=>
		{
			Release();
			if(cb.ResponseCode == ResponseCode.kNIMResSuccess)
			{
				Log.I("创建群组成功");
                dispatcher.Dispatch(CmdEvent.ViewEvent.CreateGroupFinish,cb.TeamId);
                UIUtil.Instance.ShowSuccToast("创建群组成功");
			}
			else
			{

				UIUtil.Instance.ShowFailToast(cb.ResponseCode+"");
			}
		});
	}

	private void AddMemberToGroup(string tid, string[] idList)
	{
		GroupService.Invite(tid, idList, "", (teamEvent)=>
		{
			Release();
			if (teamEvent.ResponseCode == ResponseCode.kNIMResSuccess)
			{
				dispatcher.Dispatch(CmdEvent.ViewEvent.AddMemberSucc);
			}
			else
			{
				dispatcher.Dispatch(CmdEvent.ViewEvent.AddMemberFail);
			}
		});
	}

	private void KickMember(string tid, string[] idList)
	{
		GroupService.KickMemberOutFromTeam(tid, idList,(teamEvent)=>
		{
			Release();
			if (teamEvent.ResponseCode == ResponseCode.kNIMResSuccess)
			{
				dispatcher.Dispatch(CmdEvent.ViewEvent.KickMemberSucc);
			}
			else
			{
				dispatcher.Dispatch(CmdEvent.ViewEvent.KickMemberFail);
			}
		});
	}

	private void DismissGroup(string tid)
	{
		GroupService.DismissTeam(tid, (teamEvent)=>
		{
			Release();
			Log.I("解散群组结果 "+teamEvent.ResponseCode);
            dispatcher.Dispatch(CmdEvent.ViewEvent.DismissGroupFinish);
		});
	}

    private void QuerySingleTeam(string tid)
    {
        GroupService.QueryTeamInfoOnline(tid, teamEvent =>
        {
            Release();

            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSingleTeamFinish, teamEvent.TeamInfo);
            Log.I("查询群组信息  "+teamEvent.TeamInfo.Name);

        } );
    }

    private void UpdateAnnounceInfo(string tid, NIMTeamInfo info)
    {
       GroupService.UpdateTeamInfo(tid , info, teamEvent =>
       {
           Release();
           if (teamEvent.TeamInfo != null)
           {
               dispatcher.Dispatch(CmdEvent.ViewEvent.UpdateAnnouncementFinish, teamEvent.TeamInfo.Announcement);
           }
          
       } );
    }

    private void AddGroupManager(string tid,string[] idList)
    {
        
        GroupService.AddTeamManagers(tid,idList, teamEvent =>
        {
            Release();
            Log.I("添加群组管理员返回！！！！");
            dispatcher.Dispatch(CmdEvent.ViewEvent.AddGroupManagerFinish,teamEvent);
            
        });
    }

    private void RemoveGroupManager(string tid, string[] idList)
    {
        GroupService.RemoveTeamManagers(tid,idList, teamEvent =>
        {
            Release();
            Log.I("移除群组管理员返回！！！");
            dispatcher.Dispatch(CmdEvent.ViewEvent.RemoveManagerFinish,teamEvent);
        } );
    }

    private void UpdateGroupData(string tid, NIMTeamInfo info)
    {
        GroupService.UpdateTeamInfo(tid, info, teamEvent =>
        {
            Release();
            if (teamEvent.TeamInfo != null)
            {
              Log.I("群组资料修改，返回");
              dispatcher.Dispatch(CmdEvent.ViewEvent.UpdateTeamInfoFinish, teamEvent.TeamInfo);
            }
        });

    }

    private void LeaveGroup(string tid)
    {
        GroupService.LeaveTeam(tid, teamEvent =>
        {
            Release();
        if (teamEvent.TeamInfo != null)
        {
            Log.I("退出群组成功");
                dispatcher.Dispatch(CmdEvent.ViewEvent.LeaveGroupFinish);
            }
        } );
    }

    private void TransferGroup(string tid,string newOwnerId,bool leaveTeam)
    {
        
        GroupService.TransferTeamAdmin(tid,newOwnerId,leaveTeam, teamEvent =>
        {
            if (teamEvent.TeamInfo != null)
            {
                Log.I("转让群主成功 "+ teamEvent.TeamInfo.Name);
                dispatcher.Dispatch(CmdEvent.ViewEvent.TransferGroupFinish);
            }
        });
    }

    private void ApplyJoinGroup(string tid,string reson)
    {
        GroupService.ApplyForJoiningTeam(tid,reson, teamEvent =>
        {
            Log.I("申请加入群组回调");
        } );
    }

    private void AgreeJoinTeam(string tid, string uid, long msgId, NIMSysMsgStatus status)
    {
        GroupService.AgreeJoinTeamApplication(tid,uid, teamEvent =>
        {
            Log.I("同意入群申请回调");
            ImService.SetSysMessagesStatus(msgId,status, code =>
            {
                Log.I("设置系统消息状态回调："+code);
            } );
        } );
    }

    private void RejectJoinTeam(string tid, string uid, string reason, long msgId, NIMSysMsgStatus status)
    {
        GroupService.RejectJoinTeamApplication(tid,uid,reason, teamEvent =>
        {
            Log.I("拒绝入群申请回调");
            ImService.SetSysMessagesStatus(msgId,status, code =>
            {
                Log.I("设置系统消息状态回调：" + code);
            } );
        } );
    }

    private void UploadGroupHeadImage(string tid,Texture2D heTexture2D)
    {
        GroupService.UpLoadGroupHeadImage(heTexture2D,tid, (b, s) =>
        {
            if (b)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.UpLoadGroupHeadImageFinish,s);
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.UpLoadGroupHeadImageFail, s);
            }
        });
    }
}

public struct ArgGroupOptionParam
{	
	/// <summary>
	/// 操作类型
	/// </summary>
	public EGroupOption Option;
	public string GroupId;
	/// <summary>
	/// 组名
	/// 创建群组必填项
	/// </summary>
	public string GroupName;
	/// <summary>
	/// 组介绍
	/// 创建群组必填项
	/// </summary>
	public string GroupIntroduce;
	/// <summary>
	/// 组头像
	/// 创建群组必填项
	/// </summary>
	public string GroupHeadIconUrl;
	/// <summary>
	/// 要添加的所有成员
	/// 添加成员必填项
	/// </summary>
	public HashSet<string> AddMembersId;
	/// <summary>
	/// 要踢出的所有成员
	/// 踢出成员必填项
	/// </summary>
	public HashSet<string> KickMembersId;
    /// <summary>
    /// 群组信息
    /// 更新群组公告必填项
    /// </summary>
    public NIMTeamInfo TeamInfo;
    /// <summary>
    /// 被转让的用户Id
    /// 转让群组必填项
    /// </summary>
    public string NewOwnId;
    /// <summary>
    /// 转让是否同时退出
    /// </summary>
    public bool IsLeave;
    /// <summary>
    /// 申请入群理由
    /// </summary>
    public string ApplyReson;
    /// <summary>
    /// 入群申请用户的Id
    /// </summary>
    public string Uid;
    /// <summary>
    /// 消息Id
    /// </summary>
    public long MsgId;
    /// <summary>
    /// 系统消息状态
    /// </summary>
    public NIMSysMsgStatus SysMsgStatus;
    /// <summary>
    /// 群组Texture2D(上传群组头像必填)
    /// </summary>
    public Texture2D GroupTexture2D;
}

public enum EGroupOption
{
	Create,
	Dismiss,
	Level,
	AddMember,
	Kick,
    QuerySingleTeam,
    UpdateAnnounce,
    AddTeamManager,
    RemoveTeamManager,
    UpdateTeamData,
    TransferTeam,
    ApplyJoinTeam,
    AgreeJoinTeam,
    RejectJoinTeam,
    UploadGroupHeadImage,


}
