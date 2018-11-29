using System;
using JPush;
using LitJson;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;
using WongJJ.Game.Core;

public class BottomBarView : iocView
{
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    [Inject]
    public IImService ImService { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    [SerializeField]
    private Toggle _mPreach;

    [SerializeField]
    private Text _mPreachText;

    [SerializeField]
    private Toggle _mPray;

    [SerializeField]
    private Text _mPrayText;

    [SerializeField]
    private Toggle _mChat;

    [SerializeField]
    private Text _mChatText;

    [SerializeField]
    private Toggle _mMe;

    [SerializeField]
    private Text _mMeText;

    [SerializeField]
    private Transform _mUnreadCountGo;

    [SerializeField]
    private Button _mPublishBut;
    private int _mUnreadCount;

    protected override void Start()
    {
        base.Start();
        dispatcher.AddListener(CmdEvent.ViewEvent.ReceiveImMsg, OnReceiveImMsg);
        dispatcher.AddListener(CmdEvent.ViewEvent.UnReadCount,UnReadCountListener);
        JPushBinding.SetDebug(false);
        JPushBinding.Init(gameObject.name);
#if !EDITOR_MAC
        OnReceiveImMsg(null);
#endif

#if UNITY_ANDROID

        if (JPushBinding.IsPushStopped())
        {
            JPushBinding.ResumePush();
        }
#endif
    }

    private void UnReadCountListener(IEvent eEvent)
    {
        var count = (int)eEvent.data;
        _mUnreadCount = count;
        Dispatcher.InvokeAsync(ShowUnreadCount);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReceiveImMsg, OnReceiveImMsg);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.UnReadCount, UnReadCountListener);
        _mPublishBut.onClick.RemoveListener(PublishClick);
    }

    public override int GetUiId()
    {
        return (int)UiId.BottomBar;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.BottomBar;
    }

    protected override void Awake()
    {
        base.Awake();
        _mPreach.onValueChanged.AddListener((isOn) =>
        {
            _mPreachText.color = isOn ? Color.red : Color.black;
            if (isOn)
            {
                if (iocViewManager.CurrentView.GetUiId() == (int)UiId.Preach)
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)
                    {
                        UIUtil.Instance.ShowTextToast("当前网络不可用");
                    }
                    else
                    {
                        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.RefreshPostData, this);
                    }
                }
                dispatcher.Dispatch(CmdEvent.Command.BottomBarClick,UiId.Preach);
            }
        });
        _mPray.onValueChanged.AddListener((isOn) =>
        {
            _mPrayText.color = isOn ? Color.red : Color.black;
            if (isOn)
            {
                dispatcher.Dispatch(CmdEvent.Command.BottomBarClick, UiId.Classics);
            }
            else
            {
                iocViewManager.DestoryView((int)UiId.Classics);
            }
        });
        _mChat.onValueChanged.AddListener((isOn) =>
        {
            _mChatText.color = isOn ? Color.red : Color.black;
            if (isOn)
            {
                dispatcher.Dispatch(CmdEvent.Command.BottomBarClick, UiId.ChatSession);
            }
            else
            {
                iocViewManager.DestoryView((int)UiId.ChatSession);
            }
        });
        _mMe.onValueChanged.AddListener((isOn) =>
        {
            _mMeText.color = isOn ? Color.red : Color.black;
            if (isOn)
            {
                dispatcher.Dispatch(CmdEvent.Command.BottomBarClick, UiId.Me);
            }
            else
            {
                iocViewManager.DestoryView((int)UiId.Me);
            }
        });
        _mPublishBut.onClick.AddListener(PublishClick);
    }

    private void PublishClick()
    {
        UserModel.AboutEditorUid = iocViewManager.CurrentView.GetUiId();
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.PublishTextImage, this);
        iocViewManager.CloseCurrentOpenNew((int) UiId.PreachEditor);
    }

    private void OnReceiveImMsg(IEvent evt)
    {
        ImService.QuerySessionRecord((unreadCount, list) =>
        {
            _mUnreadCount = unreadCount;
            Dispatcher.InvokeAsync(ShowUnreadCount);
        });
    }

    private void ShowUnreadCount()
    {
        if (_mUnreadCount <= 0)
        {
            _mUnreadCountGo.gameObject.SetActive(false);
        }
        else
        {
            _mUnreadCountGo.gameObject.SetActive(true);
            _mUnreadCountGo.GetComponentInChildren<Text>().text = _mUnreadCount.ToString();
        }
    }

    /* data format
{
   "message": "hhh",
   "extras": {
       "f": "fff",
       "q": "qqq",
       "a": "aaa"
   }
}
*/
    // 开发者自己处理由 JPush 推送下来的消息。
    void OnReceiveMessage(string jsonStr)
    {
        Debug.Log("开发者自己处理由 JPush 推送下来的消息recv----message-----" + jsonStr);
    }

    /**
     * {
     *	 "title": "notiTitle",
     *   "content": "content",
     *   "extras": {
     *		"key1": "value1",
     *       "key2": "value2"
     * 	}
     * }
     */
    // 获取的是 json 格式数据，开发者根据自己的需要进行处理。
    void OnReceiveNotification(string jsonStr)
    {
        Debug.Log("获取的是 json 格式数据，开发者根据自己的需要进行处理。recv---notification---" + jsonStr);
    }

    //开发者自己处理点击通知栏中的通知
    void OnOpenNotification(string jsonStr)
    {
        Debug.Log("开发者自己处理点击通知栏中的通知recv---openNotification---" + jsonStr);
        //str_unity = jsonStr;

        var rs = JsonMapper.ToObject<Js_ResponeJpushNotification>(jsonStr);

        if (rs != null)
        {

#if UNITY_IPHONE
			dispatcher.Dispatch(CmdEvent.Command.LoadJPushNocition,rs.PostId);
#elif UNITY_ANDROID
            dispatcher.Dispatch(CmdEvent.Command.LoadJPushNocition, rs.extras.PostId);
#endif
        }
    }

    /// <summary>
    /// JPush 的 tag 操作回调。
    /// </summary>
    /// <param name="result">操作结果，为 json 字符串。</param>
    void OnJPushTagOperateResult(string result)
    {
        Debug.Log("JPush 的 tag 操作回调。JPush tag operate result: " + result);

    }

    /// <summary>
    /// JPush 的 alias 操作回调。
    /// </summary>
    /// <param name="result">操作结果，为 json 字符串。</param>
    void OnJPushAliasOperateResult(string result)
    {
        Debug.Log("JPush 的 alias 操作回调JPush alias operate result: " + result);

    }

    void OnGetRegistrationId(string result)
    {
        Debug.Log("JPush on get registration Id: " + result);
    }
}
