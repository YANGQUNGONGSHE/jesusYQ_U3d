using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqRecommendationCommand : EventCommand {

    [Inject]public IPreachService PreachService { get; set; }

    public override void Execute()
    {
       Retain();
        var param = (string)evt.data;
        var list = new List<RecommendationModel>();
       PreachService.ReqRecommendationInfo(param, (b, error, info) =>
       {
           Release();
           if (b)
           {
               for (var i = 0; i < info.Count; i++)
               {
                   var model = new RecommendationModel()
                   {
                       Id = info[i].Id,
                       ContentType = info[i].ContentType,
                       ContentId = info[i].ContentId,
                       Title = info[i].Title,
                       Summary = info[i].Summary,
                       PictureUrl = info[i].PictureUrl,
                       Position = info[i].Position,
                       CreatedDate = info[i].CreatedDate
                   };
                   list.Add(model);
               }
               dispatcher.Dispatch(CmdEvent.ViewEvent.LoadRecommendationFinish,list);
           }
           else
           {
               Log.I(error);
           }
       } );
    }
}
