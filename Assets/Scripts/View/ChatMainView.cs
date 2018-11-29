using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatMainView : iocView
{
    [SerializeField] private ChatMainFiller _mFiller;

    public ChatMainFiller Filler
    {
        get { return _mFiller; }
    }

    [SerializeField] private Button _mBackButton;

    public Button BackButton
    {
        get { return _mBackButton; }
    }

    [SerializeField] private Button _mOptionButton;

    public Button OptionButton
    {
        get { return _mOptionButton; }
    }

    [SerializeField] private Button _mVoiceButton;

    public Button VoiceButton
    {
        get { return _mVoiceButton; }
    }

    [SerializeField] private Button _mKeyboradButton;

    public Button KeyboardButton
    {
        get { return _mKeyboradButton; }
    }

    [SerializeField] private Button _mSendTextButton;

    public Button SendTextButton
    {
        get { return _mSendTextButton; }
    }

    [SerializeField] private Button _mExtendButton;

    public Button ExtendButton
    {
        get { return _mExtendButton; }
    }

    [SerializeField] private InputField _mTextInput;

    public InputField TextInput
    {
        get { return _mTextInput; }
    }

    [SerializeField] private Text _mTitleDisplayName;

    public Text TitleDisplayName
    {
        get { return _mTitleDisplayName; }
    }

    [SerializeField] private RectTransform _mExtendBar;
    [SerializeField] private RectTransform _mBottomBar;
    [SerializeField] private RectTransform _mList;

    [SerializeField] private Button _mTakePhotoButton;

    public Button TakePhotoButton
    {
        get { return _mTakePhotoButton; }
    }

    [SerializeField] private Button _mGetPhotoButton;

    public Button GetPhotoButton
    {
        get { return _mGetPhotoButton; }
    }

    [SerializeField] private Button _mHitHiddenBtn;

    public Button HitHiddenBtn
    {
        get { return _mHitHiddenBtn; }
    }

    [SerializeField] private PressAudio _mPressAudioBtn;

    public PressAudio PressAudioBtn
    {
        get { return _mPressAudioBtn; }
    }

    [SerializeField] private Transform _mCaptureAudioBox;

    public Transform CaptureAudioBox
    {
        get { return _mCaptureAudioBox; }
    }

    [HideInInspector] public Button CleanRecordBut;
    [HideInInspector] public Button ReportBut;
    [HideInInspector] public Button CancelBut;
    [HideInInspector] public Button BgCancelBut;
    [HideInInspector] public Button BlockBut;
    [HideInInspector] public RectTransform P2PExpandRectTransform;
    [HideInInspector] public RectTransform ReadInfoRectTransform;
    [HideInInspector] public Button ReadInfoShowBut;
    [HideInInspector] public Button ReadInfoCloseBut;
    [HideInInspector] public Text AnnounText;
    [HideInInspector] public GroupReadRecordFiler GroupReadRecordFiler;
    [HideInInspector] public Text BlockStatusText;


    protected override void Awake()
    {
        base.Awake();
        CleanRecordBut = transform.Find("P2pExpandBar/Bg/CleanDataBut").GetComponent<Button>();
        ReportBut = transform.Find("P2pExpandBar/Bg/ReportBut").GetComponent<Button>();
        CancelBut = transform.Find("P2pExpandBar/Bg/CancelBut").GetComponent<Button>();
        BlockBut = transform.Find("P2pExpandBar/Bg/BlockBut").GetComponent<Button>();
        P2PExpandRectTransform = transform.Find("P2pExpandBar").GetComponent<RectTransform>();
        BgCancelBut = transform.Find("P2pExpandBar").GetComponent<Button>();
        ReadInfoRectTransform = transform.Find("ReadClassicsAction").GetComponent<RectTransform>();
        ReadInfoCloseBut = transform.Find("ReadClassicsAction/backTipBg/BackTip").GetComponent<Button>();
        ReadInfoShowBut = transform.Find("ReadRecordBut").GetComponent<Button>();
        AnnounText = transform.Find("ReadClassicsAction/Image/AnnouncementPart/Content").GetComponent<Text>();
        GroupReadRecordFiler = transform.Find("GroupReadRecordFiler").GetComponent<GroupReadRecordFiler>();
        BlockStatusText = transform.Find("P2pExpandBar/Bg/BlockBut/Text").GetComponent<Text>();
    }


    public void IsVisibleP2PExpandBg(bool flag)
    {
        if (flag)
        {
            P2PExpandRectTransform.DOAnchorPosY(0f, 0f);
        }
        else
        {
            P2PExpandRectTransform.DOAnchorPosY(-1280f, 0f);
        }
    }

    public void BlockTextStatus(bool flag)
    {
        BlockStatusText.text = flag ? "移除出黑名单" : "拉入黑名单";
    }

    public override int GetUiId()
    {
        return (int) UiId.ChatMain;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }

    public override float AnimationTime()
    {
        return .25f;
    }

    public override void OnRender()
    {
        base.OnRender();
        RectTransform.DOAnchorPosX(0, AnimationTime());
    }

    public override void OnNoRender()
    {
        base.OnNoRender();
        RectTransform.DOAnchorPosX(Screen.width, AnimationTime());
    }

    public void SetTitleDisplayName(string disPlayName,string userName)
    {
        _mTitleDisplayName.text = !string.IsNullOrEmpty(disPlayName) ? disPlayName : userName;
    }

    public void ShowExtendBar(bool flag)
    {
        if (flag)
        {
            //_mList.offsetMax = new Vector2(0, 0);
            _mList.offsetMin = new Vector2(0, 280);
            //_mFiller.GoToBottom();

            _mBottomBar.DOAnchorPosY(200, 0.25f);
            _mExtendBar.DOAnchorPosY(0, 0.25f);
        }
        else
        {
            _mList.offsetMin = new Vector2(0, 80);
            _mBottomBar.DOAnchorPosY(0, 0.25f);
            _mExtendBar.DOAnchorPosY(-200, 0.25f);
        }

        HitHiddenBtn.gameObject.SetActive(flag);
    }

    public void ChangeAudioAndTextInput(bool flag)
    {
        if (flag)
        {
            _mKeyboradButton.gameObject.SetActive(true);
            _mVoiceButton.gameObject.SetActive(false);
            _mPressAudioBtn.gameObject.SetActive(true);
            _mTextInput.gameObject.SetActive(false);
        }
        else
        {
            _mKeyboradButton.gameObject.SetActive(false);
            _mVoiceButton.gameObject.SetActive(true);
            _mPressAudioBtn.gameObject.SetActive(false);
            _mTextInput.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 读经记录的Action是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsVisibleReadRecordAction(bool flag)
    {
        if (flag)
        {
            ReadInfoRectTransform.DOAnchorPosY(-127f, AnimationTime());
        }
        else
        {
            ReadInfoRectTransform.DOAnchorPosY(463f, AnimationTime());
        }
    }

    /// <summary>
    /// 展示读经记录的按钮是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsVisibleReadRecordShowBut(bool flag)
    {
        if (flag)
        {
            ReadInfoShowBut.GetComponent<RectTransform>().DOAnchorPosY(-134f, 0f);
        }
        else
        {
            ReadInfoShowBut.GetComponent<RectTransform>().DOAnchorPosY(80f, 0f);
        }
    }

    /// <summary>
    /// 设置群公告内容
    /// </summary>
    /// <param name="content"></param>
    public void SetAnnountUi(string content)
    {
        AnnounText.text = content;
    }
}