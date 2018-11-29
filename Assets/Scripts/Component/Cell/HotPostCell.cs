using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;
using WongJJ.Game.Core.StrangeExtensions;

public  class HotPostCell : BaseCell<PostModel>
{
    public Action<ClickType,int, PostModel> TypeCallBack;
    #region 容器属性

    /// <summary>
    /// 发帖者昵称
    /// </summary>
    private Text _mPosterName;
//    /// <summary>
//    /// 发帖者个性签名
//    /// </summary>
//    private Text _mPublishTime;

    /// <summary>
    /// 标题
    /// </summary>
    private Text _mTitle;

//    /// <summary>
//    /// 摘要
//    /// </summary>
//    private Text _mSummary;

    ///// <summary>
    ///// 分享数量
    ///// </summary>
    //private Text _mShareCount;

    /// <summary>
    /// 评论数量
    /// </summary>
    private Text _mCommentCount;

    /// <summary>
    /// 点赞数量
    /// </summary>
    private Text _mLikeCount;

    ///// <summary>
    ///// 分享按钮
    ///// </summary>
    //private Button _mShareBut;
    /// <summary>
    /// 点赞按钮
    /// </summary>
    private Button _mLikeBut;
//    /// <summary>
//    /// 评论按钮
//    /// </summary>
//    private Button _mCommentBut;
//    /// <summary>
//    /// 用户头像
//    /// </summary>
//    private CircleRawImage _mHeadImage;
    /// <summary>
    /// 头像点击按钮
    /// </summary>
    private Button _mHeadBut;
    /// <summary>
    /// 封面图片
    /// </summary>
    private RawImage _mCover;
    /// <summary>
    /// 点赞背景
    /// </summary>
    private Image _mLikeBg;

    public Sprite[] LikeSprites;
    private RectTransform _mVideoTips;

    private Button _mBlockBut;
    #endregion

    protected override void OnAwake()
    {
       // _mHeadImage = transform.Find("PosterInfoContainer/HeadImage/CircleRawImage").GetComponent<CircleRawImage>();
        _mPosterName = transform.Find("PostContent/InteractContainer/DisplayName").GetComponent<Text>();
       // _mPublishTime = transform.Find("PostContent/InteractContainer/CommenContainer/Time").GetComponent<Text>();
        _mTitle = transform.Find("PostContent/Title").GetComponent<Text>();
        //_mSummary = transform.Find("Content/PostContent/Summary").GetComponent<Text>();
        //_mShareCount = transform.Find("InteractContainer/ForwardBut/Text").GetComponent<Text>();
        _mCommentCount = transform.Find("PostContent/InteractContainer/CommenContainer/Button/Text").GetComponent<Text>();
        _mLikeCount = transform.Find("PostContent/InteractContainer/CommenContainer/likeBut/Text").GetComponent<Text>();
       // _mShareBut = transform.Find("InteractContainer/ForwardBut").GetComponent<Button>();
        //_mCommentBut = transform.Find("InteractContainer/CommentBut").GetComponent<Button>();
        _mHeadBut = transform.Find("PostContent/InteractContainer/DisplayName").GetComponent<Button>();
        _mCover = transform.Find("Cover/RawImage").GetComponent<RawImage>();
        _mLikeBut = transform.Find("PostContent/InteractContainer/CommenContainer/likeBut").GetComponent<Button>();
        _mLikeBg = transform.Find("PostContent/InteractContainer/CommenContainer/likeBut/Image").GetComponent<Image>();
        _mVideoTips = transform.Find("Cover/RawImage/videoTips").GetComponent<RectTransform>();
        _mBlockBut = transform.Find("PostContent/InteractContainer/CommenContainer/BlockBut").GetComponent<Button>();
        AddListenr();
    }

    private void AddListenr()
    {
        //_mCommentBut.onClick.AddListener(() => { TypeCallBack(ClickType.Comment, t); });
        //_mShareBut.onClick.AddListener(() => { TypeCallBack(ClickType.Share, t); });
        _mHeadBut.onClick.AddListener(() => { TypeCallBack(ClickType.OpenPersonal,MIndex,t); });
                _mLikeBut.onClick.AddListener(() =>
                {
                    if (t.IsLike)
                    {
                        TypeCallBack(ClickType.DisLike, MIndex, t);
                        _mLikeCount.text = t.LikesCount.ToString();
                        _mLikeBg.sprite = LikeSprites[0];
                        _mLikeCount.color = Color.black;
                        t.IsLike = false;
                    }
                    else
                    {
                        TypeCallBack(ClickType.Like, MIndex, t);
                        _mLikeCount.text = (t.LikesCount + 1).ToString();
                        _mLikeBg.sprite = LikeSprites[1];
                        _mLikeCount.color = Color.red;
                        t.IsLike = true;
                    }
                });
        _mBlockBut.onClick.AddListener(() =>{TypeCallBack(ClickType.Block, MIndex,t);});
    }

    public override void InitUi(int index, PostModel t, Action<int, PostModel> onCellClickCallback = null,
        Action<int, PostModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        SetDefaultUi();
    }

    private void SetDefaultUi()
    {
        _mCover.gameObject.SetActive(!string.IsNullOrEmpty(t.PictureUrl));
        _mPosterName.text = !string.IsNullOrEmpty(t.Author.DisplayName) ? t.Author.DisplayName : t.Author.UserName;
        //_mSingure.text =t.Author.Signature;
         _mTitle.text = t.Title.Trim();
        // _mShareCount.text = t.SharesCount.ToString();
         _mCommentCount.text = t.CommentsCount.ToString();
        // _mSummary.text = t.Summary.Trim();
        
//        if (t.PublishTime != null)
//        {
//            _mPublishTime.text = t.PublishTime;
//        }  
        if (t.CoverPicture == null)
        {
            if (!string.IsNullOrEmpty(t.PictureUrl))
            {
                HttpManager.RequestImage(t.PictureUrl + LoadPicStyle.Cell, d =>
                {
                    if (d)
                    {
                        _mCover.texture = d;
                        t.CoverPicture = d;
                    }
                    else
                    {
                        _mCover.texture = DefaultImage.Cover;
                    }
                });
            }
            //else
            //{
            //    _mCover.texture = DefaultImage.Cover;
            //}
        }
        else
        {
            _mCover.texture = t.CoverPicture;
        }

//        if (t.HeadTexture2D == null)
//        {
//            if (!string.IsNullOrEmpty(t.Author.AvatarUrl))
//            {
//                HttpManager.RequestImage(t.Author.AvatarUrl + LoadPicStyle.ThumbnailHead, d =>
//                {
//                    if (d)
//                    {
//                        _mHeadImage.texture = d;
//                        t.HeadTexture2D = d;
//                    }
//                    else
//                    {
//                        t.HeadTexture2D = DefaultImage.Head;
//                    }
//                });
//            }
//            else
//            {
//                _mHeadImage.texture = DefaultImage.Head;
//            }
//        }
//        else
//        {
//            _mHeadImage.texture = t.HeadTexture2D;
//        }
// 
        if (t.IsLike)
        {
            _mLikeCount.text = (t.LikesCount + 1).ToString();
            _mLikeBg.sprite = LikeSprites[1];
            _mLikeCount.color = Color.red;
        }
        else
        {
            _mLikeCount.text = t.LikesCount.ToString();
            _mLikeBg.sprite = LikeSprites[0];
            _mLikeCount.color = Color.black;
        }
        _mVideoTips.DOAnchorPosX(t.ContentType.Equals("视频") ? 0f : 720f, 0f);
    }

    private void OnDestroy()
    {
       // _mCommentBut.onClick.RemoveListener(() => { TypeCallBack(ClickType.Comment, t); });
       // _mShareBut.onClick.RemoveListener(() => { TypeCallBack(ClickType.Share, t); });
        _mHeadBut.onClick.RemoveListener(() => { TypeCallBack(ClickType.OpenPersonal, MIndex, t); });
        _mBlockBut.onClick.RemoveListener(() => { TypeCallBack(ClickType.Block, MIndex, t); });
        //        _mLikeBut.onClick.RemoveListener(() =>
        //        {
        //            if (t.IsLike)
        //            {
        //                TypeCallBack(ClickType.DisLike, t);
        //                _mLikeCount.text = t.LikesCount.ToString();
        //                _mLikeBg.sprite = LikeSprites[0];
        //                t.IsLike = false;
        //            }
        //            else
        //            {
        //                TypeCallBack(ClickType.Like, t);
        //                _mLikeCount.text = (t.LikesCount + 1).ToString();
        //                _mLikeBg.sprite = LikeSprites[1];
        //                t.IsLike = true;
        //            }
        //        });
    }
}
public enum ClickType
{
    /// <summary>
    /// 评论事件
    /// </summary>
    Comment = 0,
    /// <summary>
    /// 点赞事件
    /// </summary>
    Like = 1,
    /// <summary>
    /// 取消点赞事件
    /// </summary>
    DisLike = 2,
    /// <summary>
    /// 分享事件
    /// </summary>
    Share = 3,
    /// <summary>
    /// 个人详情
    /// </summary>
    OpenPersonal = 4,
    /// <summary>
    /// 举报，拉黑...
    /// </summary>
    Block,
}
