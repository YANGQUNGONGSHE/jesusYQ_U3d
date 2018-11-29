using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatAudioCell : BaseCell<ChatAudioModel>
{
   
    [SerializeField] private Text _mTimeText;

    [SerializeField] private Transform _mTimeBody;

    [SerializeField] private Image _mBubbleImage;

    [SerializeField] private CircleRawImage _mHeadIcon;

    [SerializeField] private VerticalLayoutGroup _mHeadIconContainerLayout;

    [SerializeField] private HorizontalLayoutGroup _mAudioBodyContainerLayout;

    [SerializeField] private Transform _mAudioContent;

    [SerializeField] private Button _mPlayAudioButton;

    [SerializeField] private Transform _mReadIcon;

    [SerializeField] private Text _mDuartionText;

    [SerializeField] private RectTransform _mAudioIcon;
    [SerializeField] public Button _mHeadButton;

    public IAudioService AudioService = new AudioService();

    public Sprite[] BubbleSprites;

    private bool _mPlaying = false;

    private readonly Color _mOtherColour = Color.white;
    private readonly Color _mMyColour = new Color(255f / 255f, 110f / 255f, 107f / 255f, 255f / 255.0f);

    protected override void OnAwake()
    {
        base.OnAwake();
        _mHeadButton.onClick.AddListener(HeadClick);
    }
    public override void InitUi(int index, ChatAudioModel t, Action<int, ChatAudioModel> onCellClickCallback = null, Action<int, ChatAudioModel> onCellLongPressCallback = null,
        bool isShowTime = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isShowTime);

        _mTimeText.text = t.ShowTime;
        _mTimeBody.gameObject.SetActive(isShowTime);

        int sec = (int)Math.Round((double) t.AudioDuration / 1000);
        int width = 30 + (sec * ((300 - 30) / 59));
        _mAudioContent.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 36);

        _mPlayAudioButton.onClick.AddListener(OnPlayAudio);

        switch (t.ChatOwener)
        {
            case ChatOwener.Me:
                _mHeadIconContainerLayout.padding.left = 0;
                _mHeadIconContainerLayout.padding.right = 10;
                _mHeadIconContainerLayout.childAlignment = TextAnchor.UpperRight;
                _mHeadIcon.texture = DefaultImage.ImHeadTexture2D;
                
                _mBubbleImage.sprite = BubbleSprites[1];
                _mBubbleImage.color = _mMyColour;

                _mAudioBodyContainerLayout.childAlignment = TextAnchor.UpperRight;
                _mAudioBodyContainerLayout.padding.left = 0;
                _mAudioBodyContainerLayout.padding.right = 100;
                
                _mAudioIcon.localScale = new Vector3(-1, 1, 1);
                _mAudioIcon.SetAsLastSibling();

                _mDuartionText.text = sec + "\"  ";
                _mDuartionText.transform.SetAsFirstSibling();

                 _mReadIcon.gameObject.SetActive(false);
                break;

            case ChatOwener.Other:

                
                _mHeadIconContainerLayout.padding.left = 10;
                _mHeadIconContainerLayout.padding.right = 0;
                _mHeadIconContainerLayout.childAlignment = TextAnchor.UpperLeft;

                _mBubbleImage.sprite = BubbleSprites[0];
                _mBubbleImage.color = _mOtherColour;

                _mAudioBodyContainerLayout.childAlignment = TextAnchor.UpperLeft;
                _mAudioBodyContainerLayout.padding.left = 100;
                _mAudioBodyContainerLayout.padding.right = 0;

                _mAudioIcon.localScale = Vector3.one;
                _mAudioIcon.SetAsFirstSibling();

                _mDuartionText.text = "  " + sec + "\"";
                _mDuartionText.transform.SetAsLastSibling();
                
                _mReadIcon.gameObject.SetActive(!t.IsRead);
                _mReadIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(width + 20, 20, 0);

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

    private void OnPlayAudio()
    {
        _mPlaying = !_mPlaying;
        NotificationCenter.DefaultCenter().PostNotification(_mPlaying ? NotifiyName.StopPlayAudio : NotifiyName.PlayAuido, this, t);
    }

    private void HeadClick()
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.ChatMainHeadClick,this,t);
    }

    private void OnDestroy()
    {
        Log.I("OnDestroy  HeadClick");
        _mHeadButton.onClick.RemoveListener(HeadClick);
    }
}
