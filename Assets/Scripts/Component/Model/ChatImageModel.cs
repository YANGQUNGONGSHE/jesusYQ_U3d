using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatImageModel : ChatBaseModel
{
    /// <summary>
    /// 图片宽度
    /// </summary>
    public int Width{get; set;}

    /// <summary>
    /// 图片高度
    /// </summary>
    public int Height{get; set;}

    /// <summary>
    ///图片的下载地址 
    /// </summary>
    public string ResDownloadUrl{get; set;}

    /// <summary>
    /// 图片的二进制
    /// </summary>
    public byte[] ImageBytes{get; set;}

    /// <summary>
    /// 暂留
    /// </summary>
    public Texture2D ImageTexture2D{get; set;}
}
 