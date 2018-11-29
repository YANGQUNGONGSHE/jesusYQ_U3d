using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSearchModel
{
	/// <summary>
	/// 搜索类型：0-用户 1-群组
	/// </summary>
	public int Type;

    public string Id;
	/// <summary>
	/// 显示昵称
	/// </summary>
	public string DisplayName;

    public string UserName;

	public string HeadIconUrl;

	public Texture2D HeadIconTexture2D;

	/// <summary>
	/// 显示简介或签名
	/// </summary>
	public string Brief;

	/// <summary>
	/// 扩展内容:粉丝数或群主
	/// </summary>
	public string Ext;

	/// <summary>
	/// 是否拥有
	/// </summary>
	public bool IsOwn;
}
