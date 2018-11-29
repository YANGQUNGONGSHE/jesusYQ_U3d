
using System.Collections.Generic;
using NIM.Team;
using strange.extensions.command.impl;
using WongJJ.Game.Core;

public class LoadGroupOptionCommand : EventCommand
{   

	[Inject]
	public IGroupService GroupService {get; set;}

    public override void Execute()
    {
        Retain();

        var param = (LoadGroupedInfo) evt.data;

        switch (param.Type)
        {
            case LoadGroupType.LoadAllGroups:
                LoadAllGroups();
                break;
            case LoadGroupType.LoadSingleGroup:
                LoadSingleGroup(param.Id);
                break;
        }
    }


    private void LoadAllGroups()
    {
        GroupService.QueryMyValidTeamsInfo(teams =>
        {
            Release();
            dispatcher.Dispatch(CmdEvent.ViewEvent.LoadAllGroupsFinish, teams);
        });
    }

    private void LoadSingleGroup(string tid)
    {
        GroupService.QueryTeamInfoOnline(tid, info =>
        {
            Release();
            Log.I("查询单个群组信息成功:" + info.TeamId);
            Dispatcher.InvokeAsync(dispatcher.Dispatch, (object)CmdEvent.ViewEvent.LoadSingleGroupFinish, info.TeamInfo);
        });
    }
}

public class LoadGroupedInfo
{
    /// <summary>
    /// 加载类型
    /// </summary>
    public LoadGroupType Type;
    /// <summary>
    /// 群组ID,(查询单个群组必填)
    /// </summary>
    public string Id;
}

public enum LoadGroupType
{
    LoadAllGroups,
    LoadSingleGroup,
}