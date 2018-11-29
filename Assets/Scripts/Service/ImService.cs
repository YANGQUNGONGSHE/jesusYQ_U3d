using System;
using System.Collections.Generic;
using NIM;
using NIM.Friend;
using NIM.Messagelog;
using NIM.Session;
using NIM.User;
using UnityEngine;
using System.IO;
using JPush;
using LitJson;
using NIM.DataSync;
using NIM.Nos;
using NIM.SysMessage;
using NIMAudio;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using WongJJ.Game.Core;

public class ImService : IImService
{
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }
    public const string Appkey = "e96df34549d7bb638e3403efc9c07754";
    private static string _appDataPath;
    public string AppDataPath
    {
        get
        {
            if (string.IsNullOrEmpty(_appDataPath))
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    _appDataPath = Application.persistentDataPath + "/JesusYQChat";
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    string androidPathName = "com.yangqungongshe.app";
                    if (Directory.Exists("/sdcard"))
                        _appDataPath = Path.Combine("/sdcard", androidPathName);
                    else
                        _appDataPath = Path.Combine(Application.persistentDataPath, androidPathName);
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor ||
                         Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    _appDataPath = "JesusYQChat";
                }
                Debug.Log("AppDataPath:" + _appDataPath);
            }
            return _appDataPath;
        }
    }

    public ImService()
    {
        ClientAPI.Init(AppDataPath);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        bool flag = AudioAPI.InitModule(@"D:/testLog");
        Log.I("语音模块是否初始化成功？" + flag);
#else
        bool flag = AudioAPI.InitModule(AppDataPath);
		Log.I("语音模块是否初始化成功？" + flag);
#endif
    }

    private void RegisterCallback()
    {
        //NIM SDK 在登录后会同步群信息，离线消息，漫游消息，系统通知等数据。
        //NIMDataSyncType 定义了同步数据的类型，数据同步完成时sdk会调用注册的委托通知应用程序。
        DataSyncAPI.RegCompleteCb((NIMDataSyncType syncType, NIMDataSyncStatus status, string jsonAttachmen) =>
        {
            Debug.Log(string.Format("同步类型：{0}，同步状态：{1}", syncType, status));
        });

        //调用 RegKickoutCb 注册委托后，在当前账号被其他客户端踢下线时会执行委托函数，参数中包含了对方的客户端类型和被踢掉的原因。
        ClientAPI.RegKickoutCb((NIMKickoutResult result) =>
        {
            Debug.Log(string.Format("您已被踢下线，原因：{0}", result.KickReason));
        });

        //掉线
        ClientAPI.RegDisconnectedCb(() =>
        {
            Debug.Log(string.Format("您掉线了~！"));
        });

        //自动重连
        ClientAPI.RegAutoReloginCb((NIMLoginResult r) =>
        {
            Log.I(string.Format("您掉线了，正在自动重连...！"));
        }, null);

        //注册该事件，能够获取消息的发送状态。该事件为全局静态变量，在登录成功后注册一次即可。
        TalkAPI.OnSendMessageCompleted += (object sender, MessageArcEventArgs e) =>
        {
            if (e.ArcInfo.Response == ResponseCode.kNIMResSuccess)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.SendImMsgSucc,e.ArcInfo);
            }
            else if (e.ArcInfo.Response == ResponseCode.kNIMResTimeoutError)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.SendImMsgFail, "发送超时,请检查你的网络");
            }
            else if (e.ArcInfo.Response == ResponseCode.kNIMResConnectionError)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.SendImMsgFail, "发送失败,请检查你的网络连接是否正常");
            }
            else
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.SendImMsgFail, "发送失败,内部错误,原因:" + e.ArcInfo.Response);
            }
            if (e.ArcInfo.Response != ResponseCode.kNIMResSuccess)
            {
                Log.I("消息Id：" + e.ArcInfo.MsgId + "会话Id:" + e.ArcInfo.TalkId + "错误原因:" + e.ArcInfo.Response);
            }
        };

        //会话列表变化事件
        SessionAPI.RecentSessionChangedHandler += delegate (object sender, SessionChangedEventArgs args)
        {
            //NotificationCenter.DefaultCenter().PostNotification(SystemNotify.RecentSessionChanged, this, args);
            Log.I("会话列表变化事件："+ args.Info.Content+"    "+args.ResCode);
        };

        //接受消息事件
        TalkAPI.OnReceiveMessageHandler = (object sender, NIMReceiveMessageEventArgs args) =>
        {
            if (args.Message != null && args.Message.MessageContent.MessageType == NIMMessageType.kNIMMessageTypeCustom)
            {
                
            }
            else if (args.Message != null)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.ReceiveImMsg, args.Message.MessageContent);
            }
        };
        //接受系统消息事件
        SysMsgAPI.ReceiveSysMsgHandler = (object sender,  NIMSysMsgEventArgs args) =>
        {
            if (args.Message != null)
            {
                Log.I("系统消息通知来啦！！！"+args.Message.Content.Attachment+"  " + args.Message.Content.Message+"   "+ args.Message.Content.MsgType);
                if (args.Message.Content.MsgType == NIMSysMsgType.kNIMSysMsgTypeFriendDel ||
                    args.Message.Content.MsgType == NIMSysMsgType.kNIMSysMsgTypeFriendAdd)
                {
                    dispatcher.Dispatch(CmdEvent.Command.ReqFriends);
                    //UserModel.LastSysTime = new LastSysTime() { SystimeTag = args.Message.Content.Timetag.ToString() };
                    //Dispatcher.InvokeAsync(LocalDataManager.Instance.SaveJsonObj, LocalDataObjKey.SysLastTimeTag, new LastSysTime() { SystimeTag = args.Message.Content.Timetag.ToString() });
                    //dispatcher.Dispatch(CmdEvent.ViewEvent.ReceiveSysImMsg);
                    Log.I("系统添加或删除好友关系：：："+ args.Message.Content.MsgType);
                }
                 else if (args.Message.Content.MsgType == NIMSysMsgType.kNIMSysMsgTypeCustomP2PMsg)
                 {
                    var im = args.Message.Content.Attachment.Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}");
                    var ms = JsonMapper.ToObject<SysCustomMsgModel>(im);
                    if (ms.Type.Equals("Comment"))
                    {
                        UserModel.LastSysCustomCommentMsg = new LastSysCustomCommentMsg(){ IsComment = ms.Type };
                        Dispatcher.InvokeAsync(LocalDataManager.Instance.SaveJsonObj, LocalDataObjKey.SysLastCommentMsg, new LastSysCustomCommentMsg() { IsComment = ms.Type });
                        dispatcher.Dispatch(CmdEvent.ViewEvent.ReceiveSysCustomCommentImMsg);
                    }
                    else if(ms.Type.Equals("Like"))
                    {
                        UserModel.LastSysCustomLikeMsg = new LastSysCustomLikeMsg(){ IsLike = ms.Type };
                        Dispatcher.InvokeAsync(LocalDataManager.Instance.SaveJsonObj, LocalDataObjKey.SysLastLikeMsg, new LastSysCustomLikeMsg() { IsLike = ms.Type });
                        dispatcher.Dispatch(CmdEvent.ViewEvent.ReceiveSysCustomLikeImMsg);
                    }
                    else if (ms.Type.Equals("FollowCreate") || ms.Type.Equals("FollowDelete"))
                    {
                        Log.I("FollowCreateFollowCreateFollowCreateFollowDeleteFollowDeleteFollowDeleteFollowDelete");
                        dispatcher.Dispatch(CmdEvent.Command.ReqFriends);
                    }
                 }
                  else if (args.Message.Content.MsgType == NIMSysMsgType.kNIMSysMsgTypeCustomTeamMsg ||
                           args.Message.Content.MsgType == NIMSysMsgType.kNIMSysMsgTypeUnknown)
                  {
                    
                  }
                 else
                 {
                    Log.I("群组相关的消息:::"+ args.Message.Content.MsgType);
                    UserModel.LastSysTime = new LastSysTime() { SystimeTag = args.Message.Content.Timetag.ToString() };
                     Dispatcher.InvokeAsync(LocalDataManager.Instance.SaveJsonObj, LocalDataObjKey.SysLastTimeTag, new LastSysTime() { SystimeTag = args.Message.Content.Timetag.ToString() });
                     dispatcher.Dispatch(CmdEvent.ViewEvent.ReceiveSysImMsg);
                 }
            }
        };

        //注册->自动下载回调
        NosAPI.RegDownloadCb(((rescode, path, id, resId) =>
        {
            if (rescode == 200)
            {
                dispatcher.Dispatch(CmdEvent.ViewEvent.AutoDownloadImResSucc, new ArgAutoDownloadSucc(){RetSessionId = id, RetMsgId = resId});
                Log.I("会话ID： " + id);
                Log.I("消息ID： " + resId);
                Log.I("下载的路径： " + path);
            }
            else
            {
                //TODO:
            }
        }));

        //注册监听黑名单变化情况
        UserAPI.UserRelationshipChangedHandler += (sender, args) =>
        {
            if (args.ChangedType == NIMUserRelationshipChangeType.AddRemoveBlacklist)
            {
                //账号被加入黑名单
                Log.I(args.AccountId+"******被添加到或是移除出黑名单*********"+ args.IsSetted);
                dispatcher.Dispatch(CmdEvent.Command.ReqBlackData);
            }
        };

    }

    public void LoginIm(string account, string token, Action<bool> callback)
    {
        ClientAPI.Login(Appkey, account, token, result =>
        {
            if (result.LoginStep == NIMLoginStep.kNIMLoginStepLogin && result.Code == ResponseCode.kNIMResSuccess)
            {
                Log.I("恭喜，网易云IM登录成功!");
                RegisterCallback();
                if (callback != null)
                    callback(true);
            }
            else if (result.Code != ResponseCode.kNIMResSuccess)
            {
                Log.I("抱歉，网易云IM登录失败：LoginStep = " + result.LoginStep + ", ResponseCode = " + result.Code);
                if (callback != null)
                    callback(false);
            }
        });
    }

    public void LogoutIm(NIMLogoutType type, Action<NIMLogoutResult> callback)
    {
        ClientAPI.Logout(type, (r) =>
        {
            if (callback != null)
                callback(r);
            Log.I("NIMLogoutResult：：："+r.Code.ToString());
        });
    }

    public void SendMessage(NIMIMMessage msg)
    {
        TalkAPI.SendMessage(msg);
    }

    public void QuerySessionRecord(Action<int, SesssionInfoList> callback)
    {
        SessionAPI.QueryAllRecentSession((int totalUnreadCount, SesssionInfoList result) =>
        {
            if (callback != null)
                callback(totalUnreadCount, result);
        });
    }

    public void QueryChatRecord(string accountId, NIMSessionType sessionType, int limit, long timeTag, Action<ResponseCode, string, NIMSessionType, MsglogQueryResult> callback)
    {
        MessagelogAPI.QueryMsglogLocally(accountId, sessionType, limit, timeTag,
            (ResponseCode code, string accountid, NIMSessionType sType, MsglogQueryResult result) =>
            {
               
                if (callback != null)
                    callback(code, accountId, sType, result);
            });
    }

    public void QuerySingleRecord(string msgId, Action<ResponseCode, string, NIMIMMessage> callback)
    {
        MessagelogAPI.QuerylogById(msgId, (code, id, msg) =>
        {
            if (callback != null)
                callback(code, id, msg);
        });
    }

    public void QueryUserNameCard(List<string> iDs, Action<UserNameCard[]> callback)
    {
        UserAPI.GetUserNameCard(iDs, (UserNameCard[] list) =>
        {
            if (callback != null)
                callback(list);
        });
    }

    public void QueryUserNameCard(string accountId, Action<UserNameCard> callback)
    {
        QueryUserNameCard(new List<string>() { accountId }, (UserNameCard[] list) =>
        {
            if (callback != null)
                callback(list[0]);
        });
    }

    public void QueryFirendList(Action<NIMFriends> callback)
    {
        FriendAPI.GetFriendsList((NIMFriends friends) =>
        {
            if (callback != null)
                callback(friends);
        });
    }

    public void QuerySystemMessage(int limit, long lastTimetag, Action<NIMSysMsgQueryResult> callback)
    {
        SysMsgAPI.QueryMessage(limit, lastTimetag, (NIMSysMsgQueryResult queryResult) =>
        {
            if (callback != null)
            {
                callback(queryResult);
            }
        });
    }

    public void QuerySystemMsUnCount(Action<ResponseCode, int> callback)
    {
        SysMsgAPI.QueryUnreadCount((response, count) =>
        {
            if (callback != null)
                callback(response, count);
        } );
    }

    public void MarkMessagesStatusRead(string id, NIMSessionType sType, Action<ResponseCode, string, NIMSessionType> callback)
    {
        MessagelogAPI.MarkMessagesStatusRead(id, sType, (code, uid, type) =>
        {
            if (callback != null)
                callback(code, uid, type);
        });
    }

    public void DeleteSession(NIMSessionType sessionType, string id, Action<int, SessionInfo, int> callback)
    {
        SessionAPI.DeleteRecentSession(sessionType, id, (rescode, result, counts) =>
        {
            if (callback != null)
            {
                callback(rescode, result, counts);
            }
        } );
    }

    public void CleanRecordData(NIMSessionType sessionType, string id, Action<ResponseCode, string, NIMSessionType> callback)
    {
        MessagelogAPI.BatchDeleteMeglog(id, sessionType, (code, uid, type) =>
        {
            if (callback != null)
            {
                callback(code, uid, type);
            }
        });
    }

    public void SetSysMessagesStatus(long msgId, NIMSysMsgStatus status, Action<int> callback)
    {
        SysMsgAPI.SetMsgStatus(msgId,status, (code, id, count, extension, data) =>
        {
            if (callback != null)
                callback(code);
        } );
    }

    public void SetMsgStatusByType(NIMSysMsgType type, NIMSysMsgStatus status, Action<int> callback)
    {
        SysMsgAPI.SetMsgStatusByType(type,status, (code, count, extension, data) =>
        {
            if (callback != null)
                callback(code);
        } );
    }

    public void DeleteAllSys(Action<int> callback)
    {
       SysMsgAPI.DeleteAll((code, count, extension, data) =>
       {
           if (callback != null)
               callback(code);
       } );
    }

    public void DeleteMsgByType(NIMSysMsgType type, Action<int> callback)
    {
       SysMsgAPI.DeleteMsgByType(type, (code, count, extension, data) =>
       {
           if (callback != null)
               callback(code);
       });
    }

    public void SetBlacklist(string accountId, bool inBlacklist, Action<ResponseCode> callback)
    {
        UserAPI.SetBlacklist(accountId,inBlacklist, (response, accid, opt, extension, data) =>
        {
            if (callback != null)
            {
                callback(response);
            }
        } );
    }

    public void GetRelationshipList(Action<ResponseCode, UserSpecialRelationshipItem[]> callback)
    {
        UserAPI.GetRelationshipList((code, list) =>
        {
            if (callback != null)
            {
                callback(code,list);
            }
        } );
    }

    public void InitSdk()
    {
        ClientAPI.Init(AppDataPath);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        bool flag = AudioAPI.InitModule(@"D:/testLog");
        Log.I("语音模块是否初始化成功？" + flag);
#else
        bool flag = AudioAPI.InitModule(AppDataPath);
		Log.I("语音模块是否初始化成功？" + flag);
#endif
        dispatcher.Dispatch(CmdEvent.ViewEvent.LoginOutSucc);
    }
}
