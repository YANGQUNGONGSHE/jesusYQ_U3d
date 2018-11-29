using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostModel {

    /// <summary>
    ///     编号。
    /// </summary>

    public string Id { get; set; }

    /// <summary>
    ///     标题。
    /// </summary>

    public string Title { get; set; }

    /// <summary>
    ///     概要。
    /// </summary>

    public string Summary { get; set; }

    /// <summary>
    ///     图片的地址。
    /// </summary>

    public string PictureUrl { get; set; }
    /// <summary>
    /// 封面Texture2D
    /// </summary>
    public Texture2D CoverPicture { get; set; }

    /// <summary>
    ///     内容的类型。（可选值：图文, 音频, 视频）
    /// </summary>

    public string ContentType { get; set; }

    private int? _mPublishedDate;

    /// <summary>
    ///     发布日期。
    /// </summary>

    public int? PublishedDate
    {
        get { return _mPublishedDate; }
        set
        {
            _mPublishedDate = value;
            if (value != null) _mPublishTime = value.Value.FromUnixTime().ToShortDateString();
        }
    }

    private string _mPublishTime { get; set; }
    /// <summary>
    /// 显示时间
    /// </summary>
    public string PublishTime
    {
        get { return _mPublishTime; }
        set { _mPublishTime = value; }
    }

    /// <summary>
    ///     是否为精选。
    /// </summary>

    public bool IsFeatured { get; set; }

    /// <summary>
    ///     作者的用户。
    /// </summary>

    public User Author { get; set; }
    /// <summary>
    /// 头像Texture2D
    /// </summary>
    public Texture2D HeadTexture2D { get; set; }

    /// <summary>
    ///     评论的次数。
    /// </summary>

    public int CommentsCount { get; set; }

    /// <summary>
    ///     点赞的次数。
    /// </summary>

    public int LikesCount { get; set; }
    /// <summary>
    /// 是否已赞
    /// </summary>
    public bool IsLike { get; set; }
    /// <summary>
    ///     分享的次数。
    /// </summary>

    public int SharesCount { get; set; }
    
    public FromViewType FromType { get; set; }
}

public enum FromViewType
{
    /// <summary>
    /// 论道浏览界面
    /// </summary>
    FromPreachView,
    /// <summary>
    /// 个人论道界面
    /// </summary>
    FromPersonalView,
    /// <summary>
    /// 论道搜索界面
    /// </summary>
    FromPreachSearchView,
    /// <summary>
    /// 个人点赞的帖子界面
    /// </summary>
    FromLikePostView,
    /// <summary>
    /// 个人中心界面
    /// </summary>
    FromAccountView,
    /// <summary>
    /// 关注和粉丝界面
    /// </summary>
    FromFansAndFollowView,
    /// <summary>
    /// 群组成员列表界面
    /// </summary>
    FromGroupMemberView,
    /// <summary>
    /// 读经记录成员列表界面(ChatMainView)
    /// </summary>
    FromReadRecordView,
    /// <summary>
    /// 系统点赞消息界面
    /// </summary>
    FromSysCustomLikeView,
    /// <summary>
    /// 系统评论消息界面
    /// </summary>
    FromSysCutomCustomView,
    /// <summary>
    /// 
    /// </summary>
    FromNotification
}
