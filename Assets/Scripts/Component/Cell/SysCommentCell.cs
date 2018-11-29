using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class SysCommentCell : BaseCell<SysCustomCommentModel>
{

    private CircleRawImage _mHeadImage;
    private Text _mName;
    private Text _mTime;
    private Text _mTips;
    private Text _mCommenText;
    private RawImage _mPostCover;
    private RectTransform _mVideoTipIcon;

    protected override void OnAwake()
    {
        base.OnAwake();
        _mHeadImage = transform.Find("HeadPart/Content/HeadImage").GetComponent<CircleRawImage>();
        _mName = transform.Find("HeadPart/Content/NamePart/Name").GetComponent<Text>();
        _mTime = transform.Find("HeadPart/Content/NamePart/Time").GetComponent<Text>();
        _mTips = transform.Find("HeadPart/Content/Tips").GetComponent<Text>();
        _mCommenText = transform.Find("Detail").GetComponent<Text>();
        _mPostCover = transform.Find("HeadPart/Content/Cover").GetComponent<RawImage>();
        _mVideoTipIcon = transform.Find("HeadPart/Content/Cover/Image").GetComponent<RectTransform>();
    }

    public override void InitUi(int index, SysCustomCommentModel t, Action<int, SysCustomCommentModel> onCellClickCallback = null, Action<int, SysCustomCommentModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        _mName.text = !string.IsNullOrEmpty(t.UserDisplayName) ? t.UserDisplayName : t.UserName;
        _mTips.text = "评论了你的文章";
        _mCommenText.text = t.CommentContent;
        _mTime.text = t.CommentCreatedDate.FromUnixTime().ToShortDateString();
        _mVideoTipIcon.DOAnchorPosX(t.PostContentType.Equals("视频") ? 0f : 120f, 0f);

        if (t.AvatarTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.UserAvatarUrl))
            {
                HttpManager.RequestImage(t.UserAvatarUrl + LoadPicStyle.ThumbnailHead, headTexture2D =>
                {
                    if (headTexture2D)
                    {
                        _mHeadImage.texture = headTexture2D;
                        t.AvatarTexture2D = headTexture2D;
                    }
                    else
                    {
                        _mHeadImage.texture = DefaultImage.Head;
                    }
                });
            }
            else
            {
                _mHeadImage.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mHeadImage.texture = t.AvatarTexture2D;
        }

        if (t.PostCoverTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.PostPictureUrl))
            {
                HttpManager.RequestImage(t.PostPictureUrl + LoadPicStyle.ThumbnailHead, coverTexture2D =>
                {
                    if (coverTexture2D)
                    {
                        _mPostCover.texture = coverTexture2D;
                        t.PostCoverTexture2D = coverTexture2D;
                    }
                    else
                    {
                        _mPostCover.texture = DefaultImage.Head;
                    }
                });
            }
            else
            {
                _mPostCover.texture = DefaultImage.Head;
            }
        }
        else
        {
            _mPostCover.texture = t.PostCoverTexture2D;
        }
    }
}
