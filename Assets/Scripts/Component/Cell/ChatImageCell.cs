using System;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatImageCell : BaseCell<ChatImageModel>
{
   

    [SerializeField] private Text _mTimeText;

    [SerializeField] private Transform _mTimeBody;

    [SerializeField] private RawImage _mImage;

    [SerializeField] private Image _mBubbleImage;

    [SerializeField] private CircleRawImage _mHeadIcon;

    [SerializeField] private VerticalLayoutGroup _mHeadIconContainerLayout;

    [SerializeField] private HorizontalLayoutGroup _mImageBodyContainerLayout;

    [SerializeField] private VerticalLayoutGroup _mImageBodyLayout;

    public Sprite[] BubbleSprites;

    /// <summary>
    /// 资源缓存数据
    /// </summary>
    private Texture2D _mResTexture2D;

    protected override void OnAwake()
    {
        base.OnAwake();
        NotificationCenter.DefaultCenter().AddObserver("DownResCompleteSetCell", SetCellResource);
    }

    private void OnDestroy()
    {
        NotificationCenter.DefaultCenter().RemoveObserver("DownResCompleteSetCell", SetCellResource);
    }

    public override void InitUi(int index, ChatImageModel t, Action<int, ChatImageModel> onCellClickCallback = null, Action<int, ChatImageModel> onCellLongPressCallback = null,
        bool isShowTime = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isShowTime);

        _mImage.GetComponent<RectTransform>().sizeDelta = new Vector2(t.Width, t.Height);
        _mImage.texture = null;
       
        if (t.ImageTexture2D == null)
        {
            if (_mResTexture2D == null)
            {
                _mResTexture2D = new Texture2D(t.Width, t.Height);
            }
            _mResTexture2D.LoadImage(t.ImageBytes);
            t.ImageTexture2D = _mResTexture2D;
            _mImage.texture = _mResTexture2D;
        }
        else
        {
            _mImage.texture = t.ImageTexture2D;
        }

        _mTimeText.text = t.ShowTime;
        _mTimeBody.gameObject.SetActive(isShowTime);

        switch (t.ChatOwener)
        {
            case ChatOwener.Me:
                _mHeadIconContainerLayout.padding.left = 0;
                _mHeadIconContainerLayout.padding.right = 10;
                _mHeadIconContainerLayout.childAlignment = TextAnchor.UpperRight;
                _mHeadIcon.texture = DefaultImage.ImHeadTexture2D;

                _mBubbleImage.sprite = BubbleSprites[1];
                //_mBubbleImage.color = _mMyColour;

                _mImageBodyContainerLayout.childAlignment = TextAnchor.UpperRight;
                _mImageBodyContainerLayout.padding.left = 0;
                _mImageBodyContainerLayout.padding.right = 110;

                _mImageBodyLayout.padding.left = 15;
                _mImageBodyLayout.padding.right = 20;
                _mImageBodyLayout.padding.top = 15;
                _mImageBodyLayout.padding.bottom = 15;
                break;

            case ChatOwener.Other:

               
                _mHeadIconContainerLayout.padding.left = 10;
                _mHeadIconContainerLayout.padding.right = 0;
                _mHeadIconContainerLayout.childAlignment = TextAnchor.UpperLeft;

                _mBubbleImage.sprite = BubbleSprites[0];
                //_mBubbleImage.color = _mOtherColour;

                _mImageBodyContainerLayout.childAlignment = TextAnchor.UpperLeft;
                _mImageBodyContainerLayout.padding.left = 110;
                _mImageBodyContainerLayout.padding.right = 0;

                _mImageBodyLayout.padding.left = 20;
                _mImageBodyLayout.padding.right = 15;
                _mImageBodyLayout.padding.top = 15;
                _mImageBodyLayout.padding.bottom = 15;

                if (t.IsP2P)
                {
                    _mHeadIcon.texture = t.HeadIconTexture2D;
                }
                else
                {
                    if (t.HeadIconTexture2D == null)
                    {
                        if (!string.IsNullOrEmpty(t.HeadIconUrl))
                        {
                            HttpManager.RequestImage(t.HeadIconUrl + LoadPicStyle.ThumbnailHead, d =>
                            {
                                if (d)
                                {
                                    _mHeadIcon.texture = d;
                                    t.HeadIconTexture2D = d;
                                }
                                else
                                {
                                    t.HeadIconTexture2D = DefaultImage.Head;
                                }
                            });
                        }
                        else
                        {
                            _mHeadIcon.texture = DefaultImage.Head;
                        }
                    }
                    else
                    {
                        _mHeadIcon.texture = t.HeadIconTexture2D;
                    }
                }
                break;
        }
    }

    private void SetCellResource(Notification notification)
    {
        var arg = (ArgDownloadLostResChat)notification.Content;
        if (arg.RetUrl.Equals(t.ResDownloadUrl))
        {
            t.ImageBytes = arg.ResBytes;
            if (_mResTexture2D == null)
                _mResTexture2D = new Texture2D(t.Width, t.Height);
            _mResTexture2D.LoadImage(t.ImageBytes);
            _mImage.texture = _mResTexture2D;
        }
    }
}

