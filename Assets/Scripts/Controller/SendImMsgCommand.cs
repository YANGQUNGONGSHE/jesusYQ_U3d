using System;
using NIM;
using strange.extensions.command.impl;

public class SendImMsgCommand : EventCommand
{
    [Inject]
    public IImService ImService { get; set; }

    [Inject]
    public UserModel UserModel {get; set;}
    
    public override void Execute()
    {
        var arg = (ArgSendChatMsg)evt.data;
        switch (arg.Type)
        {
            case NIMMessageType.kNIMMessageTypeText:
                SendTextMsg(arg.ReceiverId, arg.Text, arg.LocalExtension);
                break;

            case NIMMessageType.kNIMMessageTypeImage:
                SendImageMsg(arg.ReceiverId, arg.AttachmentPath, arg.AttachmentMd5, arg.Width, arg.Height, arg.LocalExtension);
                break;

            case NIMMessageType.kNIMMessageTypeAudio:
                SendAudioMsg(arg.ReceiverId, arg.AttachmentPath, arg.AttachmentMd5, arg.Duration, arg.LocalExtension);
                break;
        }
    }

    private void SendTextMsg(string receiverId, string text, string localExt)
    {
        NIMTextMessage msg = new NIMTextMessage()
        {
            LocalExtension =  localExt,
            SessionType = UserModel.UserSelectedChatModel,
            MessageType = NIMMessageType.kNIMMessageTypeText,
            ReceiverID = receiverId,
            TextContent = text,
            //TimeStamp = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now)
        };
        ImService.SendMessage(msg);
    }

    private void SendImageMsg(string receiverId, string imagePath, string md5, int width, int height, string localExt)
    {
        NIMImageMessage msg = new NIMImageMessage()
        {
            LocalExtension = localExt,
            SessionType = UserModel.UserSelectedChatModel,
            MessageType = NIMMessageType.kNIMMessageTypeImage,
            ReceiverID = receiverId,
            LocalFilePath = imagePath,
            ImageAttachment = new NIMImageAttachment() { Width = width, Height = height,MD5 = md5},
            //TimeStamp = NimUtility.DateTimeConvert.ToTimetag(DateTime.Now)
        };
        ImService.SendMessage(msg);
    }

    private void SendAudioMsg(string receiverId, string audioPath, string md5, int durtion, string localExt)
    {
        NIMAudioMessage msg = new NIMAudioMessage()
        {
            LocalExtension = localExt,
            SessionType = UserModel.UserSelectedChatModel,
            MessageType = NIMMessageType.kNIMMessageTypeAudio,
            ReceiverID = receiverId,
            LocalFilePath = audioPath,
            AudioAttachment = new NIMAudioAttachment() { Duration = durtion, MD5 = md5 }
        };
        ImService.SendMessage(msg);
    }
}

public struct ArgSendChatMsg
{
    /// <summary>
    /// 消息接受者
    /// 必填
    /// </summary>
    public string ReceiverId;
    /// <summary>
    /// 消息类型
    /// 必填
    /// </summary>
    public NIMMessageType Type;
    /// <summary>
    /// 文字内容
    /// 文本类型的消息必填
    /// </summary>
    public string Text;
    /// <summary>
    /// 附件绝对路径
    /// 发送带有附件的必填
    /// </summary>
    public string AttachmentPath;
    /// <summary>
    /// 附件名Md5
    /// 发送带有附件的消息必填
    /// </summary>
    public string AttachmentMd5;
    /// <summary>
    /// 图片宽度
    /// 图片类型选填
    /// </summary>
    public int Width;
    /// <summary>
    /// 图片高度
    /// 图片类型选填
    /// </summary>
    public int Height;
    /// <summary>
    /// 扩展字段
    /// </summary>
    public string LocalExtension;
    /// <summary>
    /// 语音文件必填
    /// </summary>
    public int Duration;
}