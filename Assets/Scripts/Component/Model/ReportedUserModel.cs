using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportedUserModel{

    /// <summary>
    /// 被举报用户Id
    /// </summary>
	public string Uid { get; set; }
    /// <summary>
    /// 被举报用户昵称
    /// </summary>
    public string DisplyName { get; set; }
    /// <summary>
    /// 被举报用户名称
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 被举报用户个性签名
    /// </summary>
    public string Signature { get; set; }
    /// <summary>
    /// 被举报用户头像地址
    /// </summary>
    public string HeadUrl { get; set; }
    /// <summary>
    /// 被举报用户Texture2D
    /// </summary>
    public Texture2D HeadTexture2D { get; set; }
    /// <summary>
    /// 被举报所需的上级编号
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 举报类型
    /// </summary>
    public ReportType ReportType { get; set; }
    /// <summary>
    /// 举报界面(来自哪个View)
    /// </summary>
    public FromReportViewType FromReportViewType { get; set; }

}
