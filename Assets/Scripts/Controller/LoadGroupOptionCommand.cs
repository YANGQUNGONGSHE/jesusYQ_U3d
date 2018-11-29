
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
            Log.I("��ѯ����Ⱥ����Ϣ�ɹ�:" + info.TeamId);
            Dispatcher.InvokeAsync(dispatcher.Dispatch, (object)CmdEvent.ViewEvent.LoadSingleGroupFinish, info.TeamInfo);
        });
    }
}

public class LoadGroupedInfo
{
    /// <summary>
    /// ��������
    /// </summary>
    public LoadGroupType Type;
    /// <summary>
    /// Ⱥ��ID,(��ѯ����Ⱥ�����)
    /// </summary>
    public string Id;
}

public enum LoadGroupType
{
    LoadAllGroups,
    LoadSingleGroup,
}