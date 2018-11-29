using System;
using System.Collections.Generic;
using System.IO;
using NIM;
using NIM.Nos;
using strange.extensions.command.impl;
using WongJJ.Game.Core;

/// <summary>
/// 下载缺失的资源
/// </summary>
public class DownloadLostResChatCommand : EventCommand
{
    private ArgDownloadLostResChat _mArg;

    public override void Execute()
    {
        Retain();
        _mArg = (ArgDownloadLostResChat) evt.data;
        List<NIMIMMessage> lostResList = _mArg.Msgs;
        if (lostResList != null && lostResList.Count > 0)
        {
            for (int i = 0; i < lostResList.Count; i++)
            {
                string downloadUrl = String.Empty;
                switch (lostResList[i].MessageType)
                {
                    case NIMMessageType.kNIMMessageTypeImage:
                        downloadUrl = (lostResList[i] as NIMImageMessage).ImageAttachment.RemoteUrl;
                        break;

                    case NIMMessageType.kNIMMessageTypeAudio:
                        downloadUrl = (lostResList[i] as NIMAudioMessage).AudioAttachment.RemoteUrl;
                        break;
                }
                NosAPI.Download(downloadUrl, (rescode, path, sessionId, msgId) =>
                {
                    Release();
                    if (rescode == 200)
                    {
                        Log.I("下载下载下载-------路径：" + path + " ,消息Id:" + msgId);
                        _mArg.RetUrl = msgId;
                        _mArg.ResPath = path;
                        _mArg.ResBytes = LoadDownloadPathToImageByte(path);
                        if (_mArg.IsForceUpdateCell)
                        {
                            Dispatcher.InvokeAsync(SendNotification);
                        }
                    }
                }, null);
            }
        }
    }

    private void SendNotification()
    {
        NotificationCenter.DefaultCenter().PostNotification("DownResCompleteSetCell", this, _mArg);
    }

    private byte[] LoadDownloadPathToImageByte(string path)
    {
        if (System.IO.File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            return bytes;
        }
        return null;
    }
}

public struct ArgDownloadLostResChat
{
    /// <summary>
    /// 需要下载的消息体集合
    /// 必填
    /// </summary>
    public List<NIMIMMessage> Msgs;
    /// <summary>
    /// 是否强制更新下载好的图片
    /// 选填 默认false
    /// </summary>
    public bool IsForceUpdateCell;

    /// <summary>
    /// 返回资源的下载Url
    /// </summary>
    public string RetUrl;
    /// <summary>
    /// 返回资源本地路径
    /// </summary>
    public string ResPath;
    /// <summary>
    /// 返回资源的二进制
    /// </summary>
    public byte[] ResBytes;
}