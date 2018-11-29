using System.Collections;
using System.Collections.Generic;
using NIM.Team;
using UnityEngine;

public class ChatNotificationModel : ChatBaseModel
{
    ///// <summary>
    ///// 消息发送方Id
    ///// </summary>
    //public string SenderId;
    /// <summary>
    /// 消息发送方昵称
    /// </summary>
    public string SendName;
    /// <summary>
    /// 消息接受方Id
    /// </summary>
    public string ReceiverId;
    /// <summary>
    /// 消息接收方昵称
    /// </summary>
    public string ReceiverName;
    /// <summary>
    /// 通知类型
    /// </summary>
    public NIMNotificationType NotificationType;


}

 

