using System;
using System.Linq;
using NIM;
using strange.extensions.command.impl;
using System.Collections.Generic;
using System.IO;
using NIM.Messagelog;
using NIM.Session;
using UnityEngine;
using WongJJ.Game.Core;

/// <summary>
/// 根据会话id查询聊天记录
/// </summary>
public class LoadChatRecordBySessionCommand :EventCommand
{
    [Inject]
    public IImService ImService { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    [Inject]
    public IFriendService FriendService { get; set; }

    /// <summary>
    /// 聊天记录
    /// </summary>
    private List<ChatBaseModel> _mChatRecords;

    private List<ChatBaseModel> _mChatRecordsModels  = new List<ChatBaseModel>();

    /// <summary>
    /// 当前传入的会话参数
    /// </summary>
    private ArgLoadChatRecord _mArg;

    /// <summary>
    /// 丢失资源的消息集
    /// </summary>
    private readonly List<NIMIMMessage> _mLostResMsgs = new List<NIMIMMessage>();

    private bool _isP2P;

    private Texture2D _headTexture2D;

    private string headUrl;

    public override void Execute()
    {
        Retain();
        _mArg = evt.data as ArgLoadChatRecord;
        _isP2P = false;


        if (_mArg != null)
        {
            Log.I("-----****************------SessionId:" + _mArg.SessionId + ",NIMSessionType:" + _mArg.SessionType);
            if (_mArg.SessionType == NIMSessionType.kNIMSessionTypeP2P)
            {
                _isP2P = true;
            } 
            ImService.QueryChatRecord(_mArg.SessionId, _mArg.SessionType, _mArg.Count+20, _mArg.TimeTag + 1000000,
                (code, accountId, sType, result) =>
                {
                    Release();
                    List<NIMIMMessage> msgLogList = result.MsglogCollection.OrderBy(x => x.TimeStamp).ToList();
                    for (var i = 0; i < msgLogList.Count; i++)
                    {
                        switch (msgLogList[i].MessageType)
                        {
                            case NIMMessageType.kNIMMessageTypeText:
                              
                                CreateTextModel(msgLogList[i]);
                                break;

                            case NIMMessageType.kNIMMessageTypeAudio:
                             
                                CreateAudioModel(msgLogList[i]);
                                break;

                            case NIMMessageType.kNIMMessageTypeImage:
                               
                              //  CreateImageModel(msgLogList[i]);
                                break;
                            case NIMMessageType.kNIMMessageTypeNotification:
                               
                                CreateNotifyModel(msgLogList[i]);
                                break;
                        }
                    }
                    //没有数据的情况下
                    if (_mChatRecords == null)
                    {
                        _mChatRecords = new List<ChatBaseModel>();
                    }
                    if (_mChatRecords.Count < 1)
                    {
                        ChatTextModel textModel = new ChatTextModel()
                        {
                            IsP2P = _isP2P, 
                            ChatMsgType = ChatMsgType.Text,
                            ChatOwener = ChatOwener.Other,
                            OriginalTime = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now),
                            HeadIconTexture2D = _mArg.HeadIconTexture2D,
                        };
                        if (_mArg.SessionType == NIMSessionType.kNIMSessionTypeP2P)
                            
                            textModel.Context = "快和我打个招呼吧~";
                        else
                            textModel.Context = "快和群里的成员打个招呼吧~";
                        _mChatRecords.Add(textModel);
                    }

                  Dispatcher.InvokeAsync(LoadSenderHead, _mChatRecords);
                });
        }
    }

    /// <summary>
    /// 构建文本类型消息cell模型
    /// </summary>
    /// <param name="message">IM消息体</param>
    private void CreateTextModel(NIMIMMessage message)
    {
        var msg = message as NIMTextMessage;
        if (msg != null)
        {
            var textModel = new ChatTextModel
            {
                SenderId = msg.SenderID,
                IsP2P = _isP2P,
                MsgId = message.ClientMsgID,
                ChatMsgType = ChatMsgType.Text,
                ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
                OriginalTime = msg.TimeStamp,
                Context = msg.TextContent,
                
            };
            if (textModel.IsP2P)
            {
                textModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
            }

            if (_mChatRecords == null)
            {
                _mChatRecords = new List<ChatBaseModel>();
            }
            _mChatRecords.Add(textModel);
        }
    }
    /// <summary>
    /// 构建图片类型消息cell模型
    /// </summary>
    /// <param name="message">IM消息体</param>
    private void CreateImageModel(NIMIMMessage message)
    {
        var msg = message as NIMImageMessage;
        Log.I("消息下载地址:" + msg.ImageAttachment.RemoteUrl);
        if (msg.MsgLogStatus == NIMMsgLogStatus.kNIMMsgLogStatusSendFailed)
            return;

        if (msg != null && !string.IsNullOrEmpty(msg.ImageAttachment.MD5))
        {
            string uidMd5 = Util.Md5(UserModel.User.Id.ToString());
          
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

            var imagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                               + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\image\" + msg.ImageAttachment.MD5;
            var resPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                          + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\res\" + Util.Md5(msg.ImageAttachment.RemoteUrl);
#elif UNITY_ANDROID
            var imagePath = msg.LocalFilePath;
            var resPath = ImService.AppDataPath + "/NIM/" + uidMd5 + "/res/" +
                        Util.Md5(msg.ImageAttachment.RemoteUrl);
#elif UNITY_IPHONE
            var imagePath = msg.LocalFilePath;
            var resPath = ImService.AppDataPath + "/NIM_Debug/" + uidMd5 + "/res/" +
                        Util.Md5(msg.ImageAttachment.RemoteUrl);
#endif
            var imageModel = new ChatImageModel
            {
                SenderId = msg.SenderID,
                IsP2P = _isP2P,
                MsgId = message.ClientMsgID,
                ChatMsgType = ChatMsgType.Image,
                ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
                OriginalTime = msg.TimeStamp,
                Width = msg.ImageAttachment.Width,
                Height = msg.ImageAttachment.Height,
                ResDownloadUrl = msg.ImageAttachment.RemoteUrl,
                ImageBytes = LoadImageBytes(message, imagePath, resPath),
            };
            if (imageModel.IsP2P)
            {
                imageModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
            }
            if (_mChatRecords == null)
            {
                _mChatRecords = new List<ChatBaseModel>();
            }
            _mChatRecords.Add(imageModel);
        }
    }
    /// <summary>
    /// 构建语音类型消息cell模型
    /// </summary>
    /// <param name="message">IM消息体</param>
    private void CreateAudioModel(NIMIMMessage message)
    {
        var msg = message as NIMAudioMessage;
        if (msg.MsgLogStatus == NIMMsgLogStatus.kNIMMsgLogStatusSendFailed)
            return;

        Log.I("消息id:" + msg.ClientMsgID + ",md5:" + Util.Md5(msg.AudioAttachment.RemoteUrl));

        if (msg != null && !string.IsNullOrEmpty(msg.AudioAttachment.MD5))
        {
            string uidMd5 = Util.Md5(UserModel.User.Id.ToString());
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            var audioPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                            + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\audio\" + msg.AudioAttachment.MD5;
            var resPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                          + @"\" + ImService.AppDataPath + @"\NIM\" + uidMd5 + @"\res\" + Util.Md5(msg.AudioAttachment.RemoteUrl);
#elif UNITY_ANDROID
            var audioPath = msg.LocalFilePath;
            var resPath = ImService.AppDataPath + "/NIM/" + uidMd5 + "/res/" +
				Util.Md5(msg.AudioAttachment.RemoteUrl);
#elif UNITY_IPHONE
            var audioPath = msg.LocalFilePath;
            var resPath = ImService.AppDataPath + "/NIM_Debug/" + uidMd5 + "/res/" +
				Util.Md5(msg.AudioAttachment.RemoteUrl);
#endif
            var audioModel = new ChatAudioModel
            {
                SenderId = msg.SenderID,
                IsP2P = _isP2P,
                MsgId = message.ClientMsgID,
                ChatMsgType = ChatMsgType.Audio,
                ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
                OriginalTime = msg.TimeStamp,
//                ResDownloadUrl = msg.AudioAttachment.RemoteUrl,
                IsRead = msg.MsgLogStatus == NIMMsgLogStatus.kNIMMsgLogStatusRead,
                AuidoPath = FindAudioPath(message, audioPath, resPath),
                AudioDuration = msg.AudioAttachment.Duration,                
            };
            if (audioModel.IsP2P)
            {
                audioModel.HeadIconTexture2D = _mArg.HeadIconTexture2D;
            }
            if (_mChatRecords == null)
            {
                _mChatRecords = new List<ChatBaseModel>();
            }
            _mChatRecords.Add(audioModel);
        }
    }
    /// <summary>
    /// 构建群通知类型消息cell模型
    /// </summary>
    /// <param name="message">IM消息体</param>
    private void CreateNotifyModel(NIMIMMessage message)
    {
        var msg = message as NIMTeamNotificationMessage;

        if(msg==null) return;
        var notifyModel = new ChatNotificationModel()
        {
            IsP2P = _isP2P,
            MsgId = msg.ClientMsgID,
            ReceiverId  = msg.ReceiverID,
            SendName = msg.SenderNickname,
            SenderId = msg.SenderID,
            ChatMsgType = ChatMsgType.Notification,
            NotificationType = msg.NotifyMsgData.NotificationId,
            ChatOwener = msg.SenderID == UserModel.User.Id.ToString() ? ChatOwener.Me : ChatOwener.Other,
            OriginalTime = msg.TimeStamp
        };

        if (_mChatRecords == null)
        {
            _mChatRecords = new List<ChatBaseModel>();
        }
        _mChatRecords.Add(notifyModel);

    }
    /// <summary>
    /// 读取图片二进制
    /// </summary>
    /// <param name="msg">IM消息体</param>
    /// <param name="imagePath">图片路径</param>
    /// <param name="resPath">下载完的路径</param>
    /// <returns></returns>
    private byte[] LoadImageBytes(NIMIMMessage msg, string imagePath, string resPath)
    {
        if (System.IO.File.Exists(imagePath))
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            return bytes;
        }
        else if (System.IO.File.Exists(resPath))
        {
            FileStream fileStream = new FileStream(resPath, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            return bytes;
        }
        else
        {
            _mLostResMsgs.Add(msg);
            return null;
        }
    }

    private string FindAudioPath(NIMIMMessage msg, string audioPath, string resPath)
    {
        if (System.IO.File.Exists(audioPath))
        {
            return audioPath;
        }
        else if (System.IO.File.Exists(resPath))
        {
            return resPath;
        }
        else
        {
            _mLostResMsgs.Add(msg);
            return string.Empty;
        }
    }

    private void  LoadSenderHead(List<ChatBaseModel> models)
    {
        for (var i=0; i< models.Count;i++)
        {
            var model = models[i];
            if (model.ChatMsgType != ChatMsgType.Notification && !model.IsP2P)
            {
                if(!string.IsNullOrEmpty(model.SenderId))
                FriendService.RequestUserInfoById(models[i].SenderId, (id, user) =>
                {
                    if (!string.IsNullOrEmpty(user.AvatarUrl))
                    {
                        model.HeadIconUrl = user.AvatarUrl;
                    }   
                });
            }
            _mChatRecordsModels.Add(model);
        }
        _mArg.RetChatRecordCellModels = _mChatRecordsModels;
        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadChatRecordFinish, _mArg);
        dispatcher.Dispatch(CmdEvent.Command.DownloadLostResChat, new ArgDownloadLostResChat() { Msgs = _mLostResMsgs, IsForceUpdateCell = true });
        _mLostResMsgs.Clear();
    }
}

public class ArgLoadChatRecord
{
    /// <summary>
    /// 查询的会话Id
    /// 必填
    /// </summary>
    public string SessionId;
    /// <summary>
    /// 查询的会话类型
    /// </summary>
    public NIMSessionType SessionType;
    /// <summary>
    /// 查询的结束时间
    /// </summary>
    public long TimeTag;
    /// <summary>
    /// 昵称
    /// 选填
    /// </summary>
    public string DisplayName;
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName;
    /// <summary>
    /// 个性签名(选填)
    /// </summary>
    public string Signature;
    /// <summary>
    /// 头像地址(选填)
    /// </summary>
    public string HeadUrl;
    /// <summary>
    /// 头像
    /// 选填
    /// </summary>
    public Texture2D HeadIconTexture2D;
    /// <summary>
    /// 需要查询的记录条数
    /// </summary>
    public int Count;
    /// <summary>
    /// 是否是加载更多
    /// </summary>
    public bool IsLoadMore;
    /// <summary>
    /// 返回聊天记录Cell实体
    /// </summary>
    public List<ChatBaseModel> RetChatRecordCellModels;
}