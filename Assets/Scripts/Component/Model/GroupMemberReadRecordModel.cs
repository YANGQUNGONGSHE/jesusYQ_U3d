using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMemberReadRecordModel {

    /// <summary>
    /// 用户Id
    /// </summary>
	public int Uid { get; set; }
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 头像地址
    /// </summary>
    public string AvatarUrl { get; set; }
    /// <summary>
    /// 个性签名
    /// </summary>
    public string Signature { get; set; }
    /// <summary>
    /// 头像HeadTexture2D
    /// </summary>
    public Texture2D HeadTexture2D { get; set; }
}
