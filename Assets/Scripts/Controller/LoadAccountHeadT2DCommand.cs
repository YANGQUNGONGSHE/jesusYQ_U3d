using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;

public class LoadAccountHeadT2DCommand : EventCommand {



    public override void Execute()
    {
		Log.I("请求头像LoadAccountHeadT2DCommand");
        Retain();
        var headUrl = evt.data as string;

        if (!string.IsNullOrEmpty(headUrl))
        {
            HttpManager.RequestImage(headUrl + LoadPicStyle.ThumbnailHead, text2dD =>
            {
                Log.I("请求Account头像完成！！！！！");
                Release();
                DefaultImage.ImHeadTexture2D = text2dD != null ? text2dD : DefaultImage.Head;
            } );
        }
        else
        {
            DefaultImage.ImHeadTexture2D = DefaultImage.Head;
        }
    }
}
