using System;
using System.Collections.Generic;
using LitJson;
using NIM;
using NIM.Session;
using NIM.User;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core;

public class LoadSessionCommand :EventCommand
{
    [Inject]
    public IImService ImService { get; set; }
    [Inject]
    public IAccountService AccountService { get; set; }
    
    [Inject]
    public UserModel UserModel { get; set; }

    [Inject]
    public IGroupService GroupService { get; set; }
    private string headIconUrl;
    private string displayName;
    private string signature;
    private string userName;
    public override void Execute()
    {
        Retain();
        Log.I("LoadSessionCommand");
        var cellModelList = new List<ChatSessionCellModel>();
        var like = new ChatSessionCellModel()
        {
            ChatSessionType = ChatSessionType.Like,
            DisplayName = "赞"
        };
        var comment = new ChatSessionCellModel()
        {
            ChatSessionType = ChatSessionType.Comment,
            DisplayName = "评论"
        };
        if (UserModel.LastSysCustomLikeMsg!=null)
        {
            like.IsSysCustomMsg = true;
        }
        if (UserModel.LastSysCustomCommentMsg!=null)
        {
            comment.IsSysCustomMsg = true;
        }
        cellModelList.Add(comment);
        cellModelList.Add(like);
   
        ImService.QuerySessionRecord((unReadCount, sessinfoList) =>
        {
            if (sessinfoList.SessionList != null && sessinfoList.SessionList.Count > 0 )
            {
                for (var i = 0; i < sessinfoList.SessionList.Count; i++)
                {
                    if(sessinfoList.SessionList[i].SessionType == NIMSessionType.kNIMSessionTypeP2P)
                    {
                         FollowInfo user;
                         headIconUrl = string.Empty;
                         displayName = string.Empty;
                         signature = string.Empty;
                         userName = string.Empty;
                        if (string.IsNullOrEmpty(sessinfoList.SessionList[i].Id))
                        {
                            continue;
                        }
                        try
                        {
                            UserModel.Follows.TryGetValue(sessinfoList.SessionList[i].Id, out user); //首先查找我关注的人
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.Message);
                            throw;
                        }

                        if (user == null)  //从关注我的人（粉丝）里查找信息
                        {
                            FollowFnInfo fnuser;
                            UserModel.Fans.TryGetValue(sessinfoList.SessionList[i].Id, out fnuser);
                            if (fnuser == null) //陌生人 或者 没有粉丝也没有关注 新用户！
                            {
                                Dispatcher.InvokeAsync(LoadUnkonwUser, sessinfoList.SessionList[i].Id);
                            }
                            else
                            {
                                headIconUrl = fnuser.Follower.AvatarUrl;
                                displayName = fnuser.Follower.DisplayName;
                                signature = fnuser.Follower.Signature;
                                userName = fnuser.Follower.UserName;
                            }
                        }
                        else
                        {
                            headIconUrl = user.Owner.AvatarUrl;
                            displayName = user.Owner.DisplayName;
                            signature = user.Owner.Signature;
                            userName = user.Owner.UserName;
                        }

                        var chatSessionCellModel = new ChatSessionCellModel()
                        {
                            SessionInfo = sessinfoList.SessionList[i],
                            HeadIconUrl = headIconUrl,
                            DisplayName = displayName,
                            Signature = signature,
                            UserName = userName,
                            ChatSessionType = ChatSessionType.Default
                        };
                        cellModelList.Add(chatSessionCellModel);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sessinfoList.SessionList[i].Id))
                        {
                            continue;
                        }
                        else
                        {
                            var teamInfo = GroupService.QueryCachedTeamInfo(sessinfoList.SessionList[i].Id);

//                            if (teamInfo.TeamValid&&teamInfo.TeamMemberValid)
//                            {
                                var chatSessionCellModel = new ChatSessionCellModel
                                {
                                    SessionInfo = sessinfoList.SessionList[i],
                                    HeadIconUrl = teamInfo.TeamIcon,
                                    DisplayName = teamInfo.Name,
                                    ChatSessionType = ChatSessionType.Default,
                                };
                                cellModelList.Add(chatSessionCellModel);
                            //}
                        }
                    }
                }
            }

            if (UserModel.LastSysTime!=null)
            {
                Log.I("系统消息查询！！！");
                ImService.QuerySystemMsUnCount((code, unCount) =>
                {
                    Release();
                    if (code == ResponseCode.kNIMResSuccess)
                    {
                        //if (unCount > 0)
                        //{
                            var sys = new ChatSessionCellModel()
                            {
                                DisplayName = "系统消息",
                                Content = "群通知",
                                ChatSessionType = ChatSessionType.SystemMsg,
                                SystemImUnReadCount = unCount,
                                SortTime = UserModel.LastSysTime.SystimeTag.ToLong()
                            };
                            cellModelList.Add(sys);
                        cellModelList.Sort((x, y) => y.SortTime.CompareTo(x.SortTime));
                        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSessionFinish, cellModelList);
                        dispatcher.Dispatch(CmdEvent.ViewEvent.UnReadCount, unReadCount);
                        //}
                    }
                });
            }
            else
            {
                cellModelList.Sort((x, y) => y.SortTime.CompareTo(x.SortTime));
                dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSessionFinish, cellModelList);
                dispatcher.Dispatch(CmdEvent.ViewEvent.UnReadCount, unReadCount);
            }
            //else
            //{
            //    //ToDo:如果初始化还没有添加过好友，如何更新运行数据。
            //}
            //cellModelList.Sort((x, y) => y.SortTime.CompareTo(x.SortTime));
            //dispatcher.Dispatch(CmdEvent.ViewEvent.LoadSessionFinish,cellModelList);
            //dispatcher.Dispatch(CmdEvent.ViewEvent.UnReadCount, unReadCount);
        });
    }
    private void LoadUnkonwUser(string id)
    {
        AccountService.QuerySingleUserInfo(id, (b, info) =>
        {
            if (b)
            {
                headIconUrl = info.User.AvatarUrl;
                displayName = info.User.DisplayName;
                signature = info.User.Signature;
                userName = info.User.UserName;
            }
        });
    }
}