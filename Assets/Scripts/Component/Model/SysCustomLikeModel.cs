using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SysCustomLikeModel{

    /// <summary>
    /// 用户Id
    /// </summary>
    public string UserId { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    public string UserDisplayName { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 头像地址
    /// </summary>
    public string UserAvatarUrl { get; set; }
    /// <summary>
    /// 用户个性签名
    /// </summary>
    public string Signature { get; set; }
    /// <summary>
    /// 头像Texture2D
    /// </summary>
    public Texture2D AvatarTexture2D { get; set; }
    /// <summary>
    /// 帖子Id
    /// </summary>
    public string PostId { get; set; }
    /// <summary>
    /// 帖子标题
    /// </summary>
    public string PostTitle { get; set; }
    /// <summary>
    /// 帖子封面
    /// </summary>
    public string PostPictureUrl { get; set; }
    /// <summary>
    /// 帖子封面Texture2D
    /// </summary>
    public Texture2D PostCoverTexture2D { get; set; }
    /// <summary>
    /// 帖子类型  图文 视频 音频
    /// </summary>
    public string PostContentType { get; set; }
    /// <summary>
    /// 点赞创建时间
    /// </summary>
    public int LikeCreatedDate { get; set; }
}
