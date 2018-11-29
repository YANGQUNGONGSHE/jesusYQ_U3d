using UnityEngine;

public class ChatBaseModel
{

    /// <summary>
    /// 发送者Id
    /// </summary>
    public string SenderId { get; set; }
    /// <summary>
    /// 是否是P2P
    /// </summary>
    public bool IsP2P { get; set; }
    /// <summary>
    /// 消息Id
    /// </summary>
    public string MsgId{get; set;}

    /// <summary>
    /// 扩展字段
    /// </summary>
    public string LocalExtension{get; set;}

    /// <summary>
    /// 头像图片
    /// </summary>
    public Texture2D HeadIconTexture2D{get; set;}

    /// <summary>
    /// 头像url地址
    /// </summary>
    public string HeadIconUrl{get; set;}

    /// <summary>
    /// 消息类型
    /// </summary>
    public ChatMsgType ChatMsgType{get; set;}

    /// <summary>
    /// 消息拥有者
    /// </summary>
    public ChatOwener ChatOwener{get; set;}

    private string _mShowTime{get; set;}
    /// <summary>
    /// 显示时间
    /// </summary>
    public string ShowTime
    {
        get
        {
            return _mShowTime;
        }
        private set
        {
            _mShowTime = value;
        }
    }

    private long _mOriginalTime;
    /// <summary>
    /// 原始时间
    /// </summary>
    public long OriginalTime
    {
        get { return _mOriginalTime; }
        set
        {
            _mOriginalTime = value;
            _mShowTime = CommUtil.Instance.FormatTime2CostomData(NimUtility.DateTimeConvert.FromTimetag(value));
        }
    }

    /// <summary>
    /// 消息发送状态
    /// </summary>
    public MsgSendState MsgSendState{get; set;}

}

public enum ChatOwener
{
    Me,
    Other,
}

public enum ChatMsgType
{
    Text = 0,
    Image = 1,
    Audio = 2,
    Notification =3,
}

public enum MsgSendState
{
    None,
    Sending,
    SendSucc,
    SendFail
}
