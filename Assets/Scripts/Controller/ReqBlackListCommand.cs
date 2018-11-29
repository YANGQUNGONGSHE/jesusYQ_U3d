using System.Collections;
using System.Collections.Generic;
using System.Text;
using NIM;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class ReqBlackListCommand : EventCommand {

   // [Inject] public IImService ImService { get; set; }
    [Inject] public UserModel UserModel { get; set; }
   // [Inject] public IAccountService AccountService { get; set; }
    [Inject] public IPreachService PreachService { get; set; }
    public override void Execute()
    {
        Log.I("******请求黑名单列表*******ReqBlackListCommand");
        Retain();
//        ImService.GetRelationshipList((code, items) =>
//        {
//            if (code == ResponseCode.kNIMResSuccess)
//            {
//                Log.I("获取用户关系列表返回结果:::"+ code);
//                if (items == null)
//                {
//                    if (UserModel.BlackModels != null)
//                    {
//                        UserModel.BlackModels.Clear();
//                    }
//                    else
//                    {
//                        UserModel.BlackModels = new List<BlackModel>();
//                    }
//                    Dispatcher.InvokeAsync(dispatcher.Dispatch,(object)CmdEvent.ViewEvent.ReqBlackListSucc);
//                    Dispatcher.InvokeAsync(dispatcher.Dispatch, (object)CmdEvent.ViewEvent.ReqSetBlackSucc);
//                    return;
//                }
//                var idList = new StringBuilder();
//                for (var i = 0; i < items.Length; i++)
//                {
//                    if (items[i].IsBlacklist)
//                    {
//                        idList.Append(items[i].AccountId);
//                        idList.Append(",");
//                    }
//                }
//                LoadBlackList(idList.ToString());
//            }
//        });

         PreachService.ReqQueryBlocksInfo(UserModel.User.Id.ToString(),string.Empty,string.Empty, (b, error, infos) =>
         {
             if (b)
             {
//                 if (infos == null || infos.Count < 1)
//                 {
//                     if (UserModel.BlackModels != null)
//                     {
//                         UserModel.BlackModels.Clear();
//                     }
//                     else
//                     {
//                         UserModel.BlackModels = new List<BlackModel>();
//                     }
//                     Dispatcher.InvokeAsync(dispatcher.Dispatch, (object)CmdEvent.ViewEvent.ReqBlackListSucc);
//                     Dispatcher.InvokeAsync(dispatcher.Dispatch, (object)CmdEvent.ViewEvent.ReqSetBlackSucc);
//                     return;
//                 }

                 var list = new List<BlackModel>();
                 for (var i = 0; i < infos.Count; i++)
                 {
                     var model = new BlackModel()
                     {
                         Id = infos[i].Blockee.Id,
                         DisPlayName = infos[i].Blockee.DisplayName,
                         UserName = infos[i].Blockee.UserName,
                         Signature = infos[i].Blockee.Signature,
                         HeadUrl = infos[i].Blockee.AvatarUrl
                     };
                     list.Add(model);
                 }
                 UserModel.BlackModels = list;
                 dispatcher.Dispatch(CmdEvent.ViewEvent.ReqBlackListSucc);
                 dispatcher.Dispatch(CmdEvent.ViewEvent.ReqSetBlackSucc);
             }
             else
             {
                 Log.I(error);
             }
         });


    }

//    private void LoadBlackList(string ids)
//    {
//        Log.I("黑名单所有用户IDS:::"+ids);
//        AccountService.QueryUsersByIds(ids,string.Empty,string.Empty, (b, error, info) =>
//        {
//            Release();
//            if (b)
//            {
//                var list = new List<BlackModel>();
//
//                for (var i = 0; i < info.Count; i++)
//                {
//                    var model = new BlackModel()
//                    {
//                        Id = info[i].Id,
//                        UserName = info[i].UserName,
//                        DisPlayName = info[i].DisplayName,
//                        HeadUrl = info[i].AvatarUrl,
//                        Signature = info[i].Signature
//                    };
//                    list.Add(model);
//                }
//                UserModel.BlackModels = list;
//
//                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqBlackListSucc);
//                dispatcher.Dispatch(CmdEvent.ViewEvent.ReqSetBlackSucc);
//            }
//            else
//            {
//                Log.I("请求黑名单用户信息出错:::"+error);
//            }
//        } );
//        
//    }
}
