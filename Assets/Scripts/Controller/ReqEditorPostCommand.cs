using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class ReqEditorPostCommand : EventCommand {

    [Inject]
    public IPreachService PreachService { get; set; }
    public override void Execute()
    {
        //Retain();
        //var str = "如何评价吴亦凡的新歌《Deserve》？";
        //var par = new List<ParagraphInput>();
        //var input = new ParagraphInput()
        //{
        //    Number = 1,Type = 0,Content = "最后一次见面的时候，是她唯一一次仔细的涂了脂粉，印了浓重的唇。静静的坐在他对面，垂着眉眼，缓缓的说出这些没结果的话",
        //};
        //var input1 = new ParagraphInput()
        //{
        //    Number = 2,Type = 1,ImageThumbnailUrl = "https://pic2.zhimg.com/50/v2-0e583b4e92ad0d67d4379b3bb2aba465_hd.jpg", ImageUrl = "https://pic2.zhimg.com/50/v2-0e583b4e92ad0d67d4379b3bb2aba465_hd.jpg"
        //};
        //par.Add(input);
        //par.Add(input1);

        //PreachService.RequestCreatePost(str,par, (b, info) =>
        //{
        //    if (b)
        //    {
        //        Release();
        //        Log.I("发布帖子成功："+info.PosterAvatarUrl);
        //    }
        //    else
        //    {
        //        Log.I("发布帖子失败！！");
        //    }
        //});
    }
}
