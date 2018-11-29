using NIM;
using NIM.Session;
using System;
using UnityEngine;

public class ChatSessionCellModel
{
    /// <summary>
    /// 发送者昵称
    /// </summary>
    public string DisplayName{get; set;}
    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 正文内容
    /// </summary>
    public string Content{get; set;}

    /// <summary>
    /// 头像地址
    /// </summary>
    public string HeadIconUrl{get; set;}
    /// <summary>
    /// 头像Texture2D
    /// </summary>
    public Texture2D HeadIconTexture2D{get; set;}
    /// <summary>
    /// 个性签名(P2P)选填
    /// </summary>
    public string Signature { get; set; }

    /// <summary>
    /// 发送时间格式化后的
    /// </summary>
    public string FormatTime{get; set;}

    /// <summary>
    /// 发送时间，原数据用来排序
    /// </summary>
    public long SortTime{get; set;}

    /// <summary>
    /// 布局类型
    /// </summary>
    public ChatSessionType ChatSessionType{get; set;}
    /// <summary>
    /// 系统消息未读数
    /// </summary>
    public int SystemImUnReadCount { get; set; }
    /// <summary>
    /// 系统消息最新一条时间戳
    /// </summary>
    public long SysLastTimeTag { get; set; }
    /// <summary>
    /// 是否有自定义新消息(点赞，评论)
    /// </summary>
    public bool IsSysCustomMsg { get; set; }

    /// <summary>
    /// 原数据
    /// </summary>
    private SessionInfo _mSessionInfo;
    public SessionInfo SessionInfo
    {
        get { return _mSessionInfo; }
        set
        {
            _mSessionInfo = value;
            switch (value.MsgType)
            {
                case NIMMessageType.kNIMMessageTypeText:
                    Content = value.Content;
                    break;

                case NIMMessageType.kNIMMessageTypeImage:
                    Content = "[图片]";
                    break;

                case NIMMessageType.kNIMMessageTypeAudio:
                    Content = "[语音]";
                    break;
                case NIMMessageType.kNIMMessageTypeNotification:
                    Content = "[群通知]";
                    break;
            }
            SortTime = value.Timetag;
            var dt = NimUtility.DateTimeConvert.FromTimetag(SortTime);
             FormatTime = CommUtil.Instance.FormatTime2CostomData(dt);
            if (value.SessionType == NIMSessionType.kNIMSessionTypeP2P)
            {
//                FollowInfo user;
//                UserModel.Follows.TryGetValue(value.Id, out user);
//                if (user != null) //首先查找我关注的人
//                {
//                    HeadIconUrl = user.UserAvatarThumbnailUrl;
//                    DisplayName = user.UserDisplayName;
//                    ChatSessionIconType = ChatSessionIconType.Person;
//                }
//                else
//                {
//                    //从关注我的人（粉丝）里查找信息
//                    UserModel.Fans.TryGetValue(value.Id, out user)
//;                   if (user != null)
//                    {
//                        HeadIconUrl = user.FollowerAvatarThumbnailUrl;
//                        DisplayName = user.FollowerDisplayName;
//                        ChatSessionIconType = ChatSessionIconType.Person;
//                    }
//                    else //陌生人
//                    {
//                        HeadIconUrl = string.Empty;
//                        DisplayName = "陌生人";
//                        ChatSessionIconType = ChatSessionIconType.Stranger;
//                    }
//                }
            }
            else
            {
                //TODO:群组
                //ChatSessionIconType = ChatSessionIconType.Group;
            }
        }
    }

}


/// <summary>
/// 会话类型
/// </summary>
public enum ChatSessionType
{
    Default = 0,
    Comment = 1,
    Like = 2,
    ReadBible = 3,
    SystemMsg = 4,
}