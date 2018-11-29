using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPersonalModel {


    /// <summary>
    /// 用户Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 用户显示名称
    /// </summary>
	public string DisplayName { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 排名名次
    /// </summary>
    public int RankNumber { get; set; }
    /// <summary>
    /// 上次排名名次
    /// </summary>
    public int LastRankNumber { get; set; }
    /// <summary>
    /// 用户头像地址
    /// </summary>
    public string AvatarUrl { get; set; }
    /// <summary>
    /// 用户头像Texture2D
    /// </summary>
    public Texture2D HeadTexture2D { get; set; }
    /// <summary>
    /// 用户个性签名
    /// </summary>
    public string Signature { get; set; }
}
