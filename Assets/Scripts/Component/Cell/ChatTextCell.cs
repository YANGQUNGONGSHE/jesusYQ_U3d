using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatTextCell : BaseCell<ChatTextModel>
{

    
    [SerializeField]
    private Text _mMessageText;

    [SerializeField]
    private Text _mTimeText;

    [SerializeField]
    private Transform _mTimeBody;

    [SerializeField]
    private Image _mBubbleImage;

    [SerializeField]
    private CircleRawImage _mHeadIcon;

    [SerializeField]
    private VerticalLayoutGroup _mHeadIconContainerLayout;

    [SerializeField]
    private HorizontalLayoutGroup _mMsgBodyContainerLayout;

    [SerializeField]
    private VerticalLayoutGroup _mMsgBodyLayout;

    [SerializeField] private Button _mHeadButton;

    public Sprite[] BubbleSprites;

    private readonly Color _mOtherColour = Color.white;
    private readonly Color _mMyColour = new Color(255f / 255f, 110f / 255f, 107f / 255f, 255f / 255.0f);

    protected override void OnAwake()
    {
        base.OnAwake();
        _mHeadButton.onClick.AddListener(HeadClick);
    }
    public override void InitUi(int index, ChatTextModel t, Action<int, ChatTextModel> onCellClickCallback = null, Action<int, ChatTextModel> onCellLongPressCallback = null,
        bool isShowTime = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isShowTime);

        _mMessageText.text = t.Context;
        _mTimeText.text = t.ShowTime;
        _mTimeBody.gameObject.SetActive(isShowTime);
        switch (t.ChatOwener)
        {
            case ChatOwener.Me:
                _mMessageText.color = Color.white;
                _mHeadIconContainerLayout.padding.left = 0;
                _mHeadIconContainerLayout.padding.right = 10;
                _mHeadIconContainerLayout.childAlignment = TextAnchor.UpperRight;
                _mHeadIcon.texture = DefaultImage.ImHeadTexture2D;

                _mBubbleImage.sprite = BubbleSprites[1];
                _mBubbleImage.color = _mMyColour;

                _mMsgBodyContainerLayout.childAlignment = TextAnchor.UpperRight;
                _mMsgBodyContainerLayout.padding.left = 110;
                _mMsgBodyContainerLayout.padding.right = 95;

                _mMsgBodyLayout.padding.left = 15;
                _mMsgBodyLayout.padding.right = 20;
                _mMsgBodyLayout.padding.top = 15;
                _mMsgBodyLayout.padding.bottom = 15;
                break;

            case ChatOwener.Other:
                
                _mMessageText.color = Color.black;
                _mHeadIconContainerLayout.padding.left = 10;
                _mHeadIconContainerLayout.padding.right = 0;
                _mHeadIconContainerLayout.childAlignment = TextAnchor.UpperLeft;

                _mBubbleImage.sprite = BubbleSprites[0];
                _mBubbleImage.color = _mOtherColour;

                _mMsgBodyContainerLayout.childAlignment = TextAnchor.UpperLeft;
                _mMsgBodyContainerLayout.padding.left = 95;
                _mMsgBodyContainerLayout.padding.right = 110;

                _mMsgBodyLayout.padding.left = 20;
                _mMsgBodyLayout.padding.right = 15;
                _mMsgBodyLayout.padding.top = 15;
                _mMsgBodyLayout.padding.bottom = 15;

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

    private void HeadClick()
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.ChatMainHeadClick,this, t);
    }
    private void OnDestroy()
    {
        _mHeadButton.onClick.RemoveListener(HeadClick);
    }
}
