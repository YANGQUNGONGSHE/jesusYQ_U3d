using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamousPersonModel
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public int Uid { get; set; }
   /// <summary>
   /// 用户昵称
   /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 个性签名
    /// </summary>
    public string Signature { get; set; }
    /// <summary>
    /// 头像地址
    /// </summary>
    public string HeadImageUrl { get; set; }
    /// <summary>
    /// 头像Texture2D
    /// </summary>
    public Texture2D HTexture2D { get; set; }
}
