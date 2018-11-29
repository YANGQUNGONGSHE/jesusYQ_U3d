using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackModel{

    /// <summary>
    /// 用户Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    ///用户昵称
    /// </summary>
    public string DisPlayName { get; set; }
    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 个性签名
    /// </summary>
    public string Signature { get; set; }
    /// <summary>
    /// 头像地址
    /// </summary>
    public string HeadUrl { get; set; }
    /// <summary>
    /// 用户头像Texture2D
    /// </summary>
    public Texture2D HeadTexture2D { get; set; }
}
