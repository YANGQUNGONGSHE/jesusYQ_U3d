using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Session;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSessionCell : BaseCell<ChatSessionCellModel>
{
    #region 属性
    /// <summary>
    /// 发送者昵称（标题）
    /// </summary>
    [SerializeField]
    private Text _mTitle;

    /// <summary>
    /// 正文内容
    /// </summary>
    [SerializeField]
    private Text _mContent;

    /// <summary>
    /// 头像
    /// </summary>
    [SerializeField]
    private CircleRawImage _mHeadIcon;

    /// <summary>
    /// 类型图标
    /// </summary>
    [SerializeField]
    private Image _mFlagIcon;
    public Sprite[] FlagIcons;

    /// <summary>
    /// 发送时间文字
    /// </summary>
    [SerializeField]
    private Text _mTime;

    /// <summary>
    /// 未读数组件
    /// </summary>
    [SerializeField]
    private Image _mUnreadCount;

    /// <summary>
    /// 系统级图片
    /// </summary>
    [SerializeField]
    public Texture2D[] SystemTexture2D;

    #endregion

    #region 系统重写
    public override void InitUi(int index, ChatSessionCellModel t, Action<int, ChatSessionCellModel> onCellClickCallback = null,
        Action<int, ChatSessionCellModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        if (t.ChatSessionType == ChatSessionType.Default)
        {
            SetDefaultUi();
        }
        else if (t.ChatSessionType == ChatSessionType.Comment || t.ChatSessionType == ChatSessionType.Like)
        {
            SetOneLineUi();
        }
        else if (t.ChatSessionType == ChatSessionType.ReadBible || t.ChatSessionType == ChatSessionType.SystemMsg)
        {
            SetSystemUi();
        }
    }

    #endregion

    private void SetDefaultUi()
    {
        _mContent.gameObject.SetActive(true);
        _mTime.gameObject.SetActive(true);
        //_mFlagIcon.gameObject.SetActive(false);
        _mTitle.text = !string.IsNullOrEmpty(t.DisplayName) ? t.DisplayName : t.UserName;
        
        _mTitle.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(160f, 23.5f);

        _mContent.text = t.Content;
        //_mHeadIcon.texture = DefaultImage.Head; //强制设置下，以免缓存
        _mTime.text = "| "+t.FormatTime;
        _mTime.transform.localPosition = _mContent.preferredWidth >= 340 ? new Vector3(340f + 10f, 0, 0) : new Vector3(_mContent.preferredWidth + 10f, 0, 0);

        if (t.SessionInfo != null)
        {
             Log.I("未读条数：：："+ t.SessionInfo.UnreadCount);
            _mUnreadCount.gameObject.SetActive(t.SessionInfo.UnreadCount > 0);
            if (_mUnreadCount.gameObject.activeSelf)
            {
               _mUnreadCount.GetComponentInChildren<Text>().text = t.SessionInfo.UnreadCount.ToString();
            }
           // _mUnreadCount.GetComponentInChildren<Text>().text = t.SessionInfo.UnreadCount.ToString();
        }
        if (t.HeadIconTexture2D == null)
        {
            if (!string.IsNullOrEmpty(t.HeadIconUrl))
            {
                HttpManager.RequestImage(t.HeadIconUrl + LoadPicStyle.ThumbnailHead, (texture2D) =>
                {
                    if (texture2D)
                    {
                        t.HeadIconTexture2D = texture2D;
                        _mHeadIcon.texture = texture2D;
                    }
                    else
                    {
                        _mHeadIcon.texture = DefaultImage.Head;
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
        if (t.SessionInfo != null && t.SessionInfo.SessionType == NIMSessionType.kNIMSessionTypeTeam)
        {
            _mFlagIcon.gameObject.SetActive(true);
            _mFlagIcon.sprite = FlagIcons[2];
            _mFlagIcon.transform.localPosition = new Vector3(_mTitle.preferredWidth + 50f, -1, 0);
        }
        else
        {
            _mFlagIcon.gameObject.SetActive(false);
        }
    }

    private void SetOneLineUi()
    {
        _mHeadIcon.texture = null;
        _mContent.gameObject.SetActive(false);
        _mTime.gameObject.SetActive(false);
        _mFlagIcon.gameObject.SetActive(false);
        _mHeadIcon.texture = SystemTexture2D[(int)t.ChatSessionType];
        Log.I("类型：：："+ (int)t.ChatSessionType);
        //  _mUnreadCount.GetComponentInChildren<Text>().gameObject.SetActive(false);
        _mUnreadCount.gameObject.SetActive(t.IsSysCustomMsg);
        if (_mUnreadCount.gameObject.activeSelf)
        {
            _mUnreadCount.GetComponentInChildren<Text>().text = "✔";
        }
        _mTitle.text = t.DisplayName;
        _mTitle.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(160f,0);
    }

    private void SetSystemUi()
    {
        _mContent.gameObject.SetActive(true);
        _mTime.gameObject.SetActive(true);
        _mFlagIcon.gameObject.SetActive(true);

        _mTitle.text = t.DisplayName;

        _mUnreadCount.gameObject.SetActive(t.SystemImUnReadCount > 0);
        _mUnreadCount.GetComponentInChildren<Text>().text = t.SystemImUnReadCount.ToString();
        _mTitle.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(160f, 23.5f);

        _mContent.text = t.Content;
        _mHeadIcon.texture = SystemTexture2D[(int)t.ChatSessionType];

        _mTime.text = "| "+t.FormatTime;
        _mTime.transform.localPosition = _mContent.preferredWidth >= 340 ? new Vector3(340f + 10f, 0, 0) : new Vector3(_mContent.preferredWidth + 10f, 0, 0);

        _mFlagIcon.sprite = t.ChatSessionType == ChatSessionType.ReadBible ? FlagIcons[0] : FlagIcons[1];
        _mFlagIcon.transform.localPosition = new Vector3(_mTitle.preferredWidth + 50f, -1, 0);
    }
}