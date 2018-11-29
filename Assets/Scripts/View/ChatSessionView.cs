using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSessionView : iocView
{

    [SerializeField]
    private ChatSessionFiller _mFiller;
    public ChatSessionFiller Filler {
        get { return _mFiller; }
    }

    [SerializeField]
    private RectTransform _mActionT;
    public RectTransform ActionT {
        get { return _mActionT; }
    }

    [SerializeField]
    private Button _mCreateP2PChat;
    public Button CreateP2PChat {
        get { return _mCreateP2PChat; }
    }

    [SerializeField]
    private Button _mCreateGroupChat;
    public Button CreateGroupChat {
        get { return _mCreateGroupChat; }
    }

    [SerializeField]
    private Button _mCreateGroup;
    public Button CreateGroup
    {
        get { return _mCreateGroup; }
    }

    [SerializeField]
    private Button _mSerachButton;
    public Button SerachButton
    {
        get { return _mSerachButton; }
    }
    [SerializeField]
    private Button _mDeleteSessionBut;

    public Button DeleteSessionBut
    {
        get { return _mDeleteSessionBut; }
    }

    [SerializeField]
    private Button _mCancelDeSessionBut;

    public Button CancelDeSessionBut
    {
        get { return _mCancelDeSessionBut; }
    }

    [SerializeField]
    private RectTransform _mDeleteSessionTransform;

    public RectTransform DeleteSessionTransform
    {
        get { return _mDeleteSessionTransform; }
    }


    /// <summary>
    /// 删除会话记录背景是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsVisibleDeleteSessionBg(bool flag)
    {
        if (flag)
        {
            DeleteSessionTransform.DOAnchorPosY(0f, AnimationTime());
        }
        else
        {
            DeleteSessionTransform.DOAnchorPosY(-1280f, AnimationTime());
        }
    }
    /// <summary>
    /// Action背景是否可见
    /// </summary>
    /// <param name="flag"></param>
    public void IsVisibleAction(bool flag)
    {
        if (flag)
        {
            ActionT.DOAnchorPosY(0f, AnimationTime());
        }
        else
        {
            ActionT.DOAnchorPosY(-1280f, AnimationTime());
        }
    }

    public void IsVisibleScrollRect(bool flag)
    {
        if (flag)
        {
            Filler.ScrollView.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f);
        }
        else
        {
            Filler.ScrollView.GetComponent<RectTransform>().DOAnchorPosX(720f, 0f);
        }
    }
    public override int GetUiId()
    {
        return (int) UiId.ChatSession;
    }
    public override int GetLayer()
    {
        return (int) UiLayer.Default;
    }
}
