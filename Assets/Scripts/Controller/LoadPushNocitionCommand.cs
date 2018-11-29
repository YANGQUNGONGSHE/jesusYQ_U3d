using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class LoadPushNocitionCommand : EventCommand {

    [Inject]public UserModel UserModel { get; set; }
    [Inject]public IPreachService PreachService { get; set; }

    public override void Execute()
    {
        var postId = (string)evt.data;
        if(string.IsNullOrEmpty(postId))return;

        PreachService.RequestSinglePreach(postId, (b, error, info) =>
        {
            if (b)
            {
                UserModel.PostDetailModel = new PostModel()
                {
                    Id = info.Id,
                    Title = info.Title,
                    Summary = info.Summary,
                    SharesCount = info.SharesCount,
                    PictureUrl = info.PictureUrl,
                    ContentType = info.ContentType,
                    PublishedDate = info.PublishedDate,
                    IsFeatured = info.IsFeatured,
                    Author = info.Author,
                    CommentsCount = info.CommentsCount,
                    LikesCount = info.LikesCount,
                    FromType = FromViewType.FromNotification
                };
                Log.I("推送帖子数据："+ UserModel.PostDetailModel.Author.DisplayName);
               // iocViewManager.CloseAndOpenView((int)UiId.Preach,(int)UiId.PreachPost);
                UserModel.NowUid = iocViewManager.CurrentView.GetUiId();
                iocViewManager.CloseCurrentOpenNew((int)UiId.PreachPost);
            }
        } );
    }
}
