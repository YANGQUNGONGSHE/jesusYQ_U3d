using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankGroupModel {

    /// <summary>
    /// 群组ID
    /// </summary>
	public string GroupId { get; set; }
    /// <summary>
    /// 群组名称
    /// </summary>
    public string GroupName { get; set; }
    /// <summary>
    /// 群组头像地址
    /// </summary>
    public string GroupHeadUrl { get; set; }
    /// <summary>
    /// 群组排名名次
    /// </summary>
    public int RankNumber { get; set; }
    /// <summary>
    /// 上次排名名次
    /// </summary>
    public int LastRankNumber { get; set; }
    /// <summary>
    /// 群组头像Texture2D
    /// </summary>
    public Texture2D GroupTexture2D { get; set; }
}
