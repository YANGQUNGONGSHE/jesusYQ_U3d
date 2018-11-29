using System.Collections;
using System.Collections.Generic;
using NIM.Team;
using strange.extensions.command.impl;
using UnityEngine;

public class LoadGroupMembersCommand : EventCommand {



    private ArgLoadGroupInfo _mGroupInfo;

    private GroupMembersInfo _mGroupAndManagers;
    private List<GroupMeberInfoModel> _mGroupmembers;
    private List<GroupMeberInfoModel> _mNormalMembers;


    public override void Execute()
    {

        Retain();
        _mGroupInfo = evt.data as ArgLoadGroupInfo;
        var type =  (CmdEvent.Command)evt.type;
        if (_mGroupInfo == null) return;

        _mGroupmembers = new List<GroupMeberInfoModel>();
        _mNormalMembers = new List<GroupMeberInfoModel>();

        _mGroupAndManagers = new GroupMembersInfo { ArgLoadGroupInfo = _mGroupInfo };

        for (var i=0; i< _mGroupInfo.GroupMeberInfoModels.Count;i++ )
        {

            if (_mGroupInfo.GroupMeberInfoModels[i].UserType == NIMTeamUserType.kNIMTeamUserTypeManager || _mGroupInfo.GroupMeberInfoModels[i].UserType == NIMTeamUserType.kNIMTeamUserTypeCreator)
            {
                if (_mGroupInfo.GroupMeberInfoModels[i].UserType == NIMTeamUserType.kNIMTeamUserTypeManager)
                {
                    _mGroupmembers.Add(_mGroupInfo.GroupMeberInfoModels[i]);
                }
                else
                {
                    _mGroupAndManagers.CreaterInfo = _mGroupInfo.GroupMeberInfoModels[i];
                }
            }
            else
            {
                _mNormalMembers.Add(_mGroupInfo.GroupMeberInfoModels[i]);
            }
        }
        _mGroupAndManagers.ManagersList = _mGroupmembers;
        _mGroupAndManagers.NormalMemberList = _mNormalMembers;

        switch (type)
        {
            case CmdEvent.Command.LoadGroupMembers:
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupMembersFinish, _mGroupAndManagers);
                break;
            case CmdEvent.Command.LoadGroupTransferMemers:
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupTransferMemersFinish, _mGroupAndManagers);
                break;
        }  
    }
   
}

public class GroupMembersInfo
{

    /// <summary>
    /// 群主信息
    /// </summary>
    public GroupMeberInfoModel CreaterInfo;

    /// <summary>
    /// 群组管理员列表
    /// </summary>
    public List<GroupMeberInfoModel> ManagersList;
    /// <summary>
    /// 群组信息
    /// </summary>
    public ArgLoadGroupInfo ArgLoadGroupInfo;
    /// <summary>
    /// 普通成员
    /// </summary>
    public List<GroupMeberInfoModel> NormalMemberList;
}
