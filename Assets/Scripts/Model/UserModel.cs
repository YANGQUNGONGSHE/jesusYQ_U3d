using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using NIM.Session;
using UnityEngine;

public class UserModel
{
    private Dictionary<string, FollowInfo> _mFollows;
    private Dictionary<string, FollowFnInfo> _mFans;
    private Dictionary<string, FollowInfo> _mFriends;

    public Action<PropertyChagedEventArgs> PropertyChanging;
    public Action<PropertyChagedEventArgs> PropertyChanged;

    public const string PropertyNameFollows = "Follow";
    public const string PropertyNameFans = "Fans";
    public const string PropertyNameFriends = "Friends";
    public const string PropertyNameUser = "User";

    public string TempCommentId;
    public string TempCommentName;

    public ReportedUserModel ReportedUserModel;

    private NIMSessionType _mUserSelectedChatModel = NIMSessionType.kNIMSessionTypeP2P;
    public NIMSessionType UserSelectedChatModel 
    { 
        get 
        {
            return _mUserSelectedChatModel;
        }
        set
        {
            _mUserSelectedChatModel = value;
        }
    }

    private string _mSelectedGroupId;
    public string UserSelectedGroupId
    {
        get 
        {
            return _mSelectedGroupId;
        }
        set
        {
            _mSelectedGroupId = value;
        }
    }

    public SearchUserType SearchUserType { get; set; }

    public PostSearchType PostSearchType { get; set; }

    public EditorUserDataType EditorUserDataType { get; set; }

    public EditorGroupType EditorGroupType { get; set; }

    public FromChatMainType FromChatMainType { get; set; }

    public EditorOption EditorOptionType { get; set; }
    /// <summary>
    /// 加载会话记录所需的条件
    /// </summary>
    public ArgLoadChatRecord ArgLoadChatRecord { get; set; }
    public BookModel BookModel { get; set; }
    /// <summary>
    /// 跳转到个人界面的数据
    /// </summary>
    public PostModel PostModel { get; set; }
    /// <summary>
    /// 跳转到帖子详情界面数据
    /// </summary>
    public PostModel PostDetailModel { get; set; }
    /// <summary>
    /// 自己的读经排行数据
    /// </summary>
    public RankPersonalModel SingleRankModel { get; set; }
    /// <summary>
    /// 所有加入的群组排行数据
    /// </summary>
    public List<RankGroupModel> AllJoinRankGroupModels { get; set; }
    /// <summary>
    /// 黑名单数据
    /// </summary>
    public List<BlackModel> BlackModels { get; set; }

    public int NowUid { get; set; }

    public int AboutEditorUid { get; set; }

    public string TermPath { get; set; }

    /// <summary>
    /// 当前用户
    /// </summary>
    private User _mUser;
    public User User
    {
        set { _mUser = value; }
        get
        {
            if (_mUser == null)
            {
                Log.I("LocalDataManager!!!_mUser");
                var data = (string) LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.USER);
                if (!string.IsNullOrEmpty(data))
                {
                    _mUser = JsonMapper.ToObject<User>(data);
                }
            }
            return _mUser;
        }
    }

    private SessionId _sessionId;

    public SessionId SessionId
    {
        set { _sessionId = value; }
        get {
            if (_sessionId == null)
            {
                Log.I("LocalDataManager!!!_sessionId");
                var ssid = (string)LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.Ssid);
                if (!string.IsNullOrEmpty(ssid))
                {
                    _sessionId = JsonMapper.ToObject<SessionId>(ssid);
                }
            }
            return _sessionId;
           }
    }

    private LastSysTime _lastSysTime;

    public LastSysTime LastSysTime
    {
        set { _lastSysTime = value; }
        get
        {
            if (_lastSysTime == null)
            {
                var time = (string)LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.SysLastTimeTag);
                if (!string.IsNullOrEmpty(time))
                {
                    _lastSysTime = JsonMapper.ToObject<LastSysTime>(time);
                }
            }
            return _lastSysTime;
        }
    }

    public LastSysCustomLikeMsg _LastSysCustomLikeMsg;

    public LastSysCustomLikeMsg LastSysCustomLikeMsg
    {
        set { _LastSysCustomLikeMsg = value; }
        get
        {
            if (_LastSysCustomLikeMsg == null)
            {
                var time = (string)LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.SysLastLikeMsg);
                if (!string.IsNullOrEmpty(time))
                {
                    _LastSysCustomLikeMsg = JsonMapper.ToObject<LastSysCustomLikeMsg>(time);
                }
            }
            return _LastSysCustomLikeMsg;
        }
    }

    public LastSysCustomCommentMsg _LastSysCustomCommentMsg;

    public LastSysCustomCommentMsg LastSysCustomCommentMsg
    {
        set { _LastSysCustomCommentMsg = value; }
        get
        {
            if (_LastSysCustomCommentMsg == null)
            {
                var time = (string)LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.SysLastCommentMsg);
                if (!string.IsNullOrEmpty(time))
                {
                    _LastSysCustomCommentMsg = JsonMapper.ToObject<LastSysCustomCommentMsg>(time);
                }
            }
            return _LastSysCustomCommentMsg;
        }
    }

    public UserModel()
    {
        _mFollows = new Dictionary<string, FollowInfo>();
        _mFans = new Dictionary<string, FollowFnInfo>();
        _mFriends = new Dictionary<string, FollowInfo>();
    }

    /// <summary>
    /// 用户的关注列表
    /// </summary>
    public Dictionary<string, FollowInfo> Follows
    {
        get { return _mFollows; }
        set
        {
            if (value != _mFollows)
            {
                PropertyChagedEventArgs e = new PropertyChagedEventArgs(PropertyNameFollows, _mFollows, value);
                if (PropertyChanging != null)
                {
                    PropertyChanging(e);
                    if (e.Cancel) return;
                }
                _mFollows = (Dictionary<string, FollowInfo>) e.NewValue;
                //TODO：以下等待服务器修复
                //foreach (var follow in _mFollows)
                //{
                //    if (follow.Value.IsFriend)
                //    {
                //        _mFriends.Add(follow.Key,follow.Value);
                //    }
                //}
                
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(e);
                }
            }
        }
    }

    /// <summary>
    /// 用户的粉丝列表
    /// </summary>
    public Dictionary<string, FollowFnInfo> Fans
    {
        get { return _mFans; }
        set
        {
            if (value != _mFans)
            {
                PropertyChagedEventArgs e = new PropertyChagedEventArgs(PropertyNameFans, _mFans, value);
                if (PropertyChanging != null)
                {
                    PropertyChanging(e);
                    if (e.Cancel) return;
                }
                _mFans = (Dictionary<string, FollowFnInfo>)e.NewValue;
                _mFriends.Clear();
                //TODO：等关注列表问题解决后 使用关注列表判断好友
                foreach (var follow in _mFans)
                {
                    if (follow.Value.IsBidirectional)
                    {
                        var info = new FollowInfo()
                        {
                            Owner = follow.Value.Follower,
                            IsBidirectional = true,
                            CreatedDate = follow.Value.CreatedDate,
                            ModifiedDate = follow.Value.ModifiedDate
                        };
                        _mFriends.Add(follow.Key, info);
                    }
                }
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(e);
                }
            }
        }
    }

    /// <summary>
    /// 好友列表（相互关注）
    /// </summary>
    public Dictionary<string, FollowInfo> Friends
    {
        get { return _mFriends; }
        set { _mFriends = value; }
    }
}