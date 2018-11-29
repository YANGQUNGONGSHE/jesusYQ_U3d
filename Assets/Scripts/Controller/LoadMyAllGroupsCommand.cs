using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using NIM.Team;

public class LoadMyAllGroupsCommand : EventCommand 
{
	[Inject]
	public IGroupService GroupService {get; set;}

	public override void Execute()
	{
		Retain();
		GroupService.QueryAllMyTeams((count, tids)=>
		{
			Release();
			if(tids != null && tids.Count > 0)
			{
				var infos = new List<NIMTeamInfo>();
				foreach (string t in tids)
				{
				    infos.Add(GroupService.QueryCachedTeamInfo(t));
				}
				dispatcher.Dispatch(CmdEvent.ViewEvent.LoadMyAllGroupsFinish, infos);
			}
		});
	}
}
