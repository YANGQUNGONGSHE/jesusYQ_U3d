using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core;

public static class LocalDataObjKey
{
    public const string USER = "USER.bat";
    public const string BibleRecord = "BibleRecord.bat";
    public const string SysLastTimeTag = "SysLastTimeTag.bat";
    public const string SysLastLikeMsg = "SysLastLikeMsg.bat";
    public const string SysLastCommentMsg = "SysLastCommentMsg.bat";
    public const string LastReadRecord = "BookRecord.bat";
    public static string Ssid = Util.Md5("WJjXsSiDyQgs");
}

public static class Const
{
    public const int ChatWidthImageMax = 280;
    public const int ChatHeightImageMax = 180;
}

public static class NotifiyName
{
    public const string OnStopCaptureCb = "StopCaptureCb";
    public const string PlayAuido = "PlayAudio";
    public const string StopPlayAudio = "StopPlayAudio";
    public const string SelectedMember = "SelectedMember";
    public const string SelectedManager = "SelectedManager";
    public const string AddManager = "AddManager";
    public const string GroupMember = "GroupMember";
    public const string AddGroup = "AddGroup";
    public const string ApplyJoinTeamDeal = "ApplyJoinTeamDeal";
    public const string RejectJoinTeam = "RejectJoinTeam";
    public const string AddMembers = "AddMembers";
    public const string AddFocus = "AddFocus";
    public const string SysAddFocus = "SysAddFocus";
    public const string FansAddFocus = "FansAddFocus";
    public const string PublishTextImage = "PublishTextImage";
    public const string PublishVideo = "PublishVideo";
    public const string PublishPreachSucc = "PublishPreachSucc";
    public const string PublishComment = "PublishComment";
    public const string PublishReply = "PublishReply";
    public const string SetCurrenWebIndexIsContent = "SetCurrenWebIndexIsContent";
    public const string OpenClassiceBook = "OpenClassiceBook";
    public const string DeleteCollect = "DeleteCollect";
    public const string ReloadIndexandScroll = "ReloadIndexandScroll";
    public const string ChatMainHeadClick = "ChatMainHeadClick";
    public const string DeletePreachSucc = "DeletePreachSucc";
    public const string DeletePersonalPreachSucc = "DeletePersonalPreachSucc";
    public const string RefreshPostData = "RefreshPostData";
}

public enum UiId
{
    None = -1,
    Login = 1,
    SetDyName = 2,
    BottomBar = 3,
    Preach = 4,
    Me = 5,
    Bible = 8,
    Personal = 9,
    BibleShow = 10,
    ChatSession = 106,
    ChatMain = 107,
    ChatFriend = 108,
    ChatEditorGroupView =109,
    ChatGroup = 110,
    ChatSerach = 111,
    ChatGroupSetting = 112,
    ChatGroupSelectMemberAdd = 113,
    ChatGroupAnnouncement =114,
    ChatGrouAnnEditor = 115,
    ChatGroupManage = 116,
    ChatGroupSetManager = 117,
    ChatAddManager = 118,
    ChatGroupMember = 119,
    ChatGroupTransMembers = 120,
    ChatSysTemMs,
    PreachPost = 201,
    PreachEditorOption = 202,
    Setting = 203,
    EditorUserData = 204,
    EditorNameOrSignature = 205,
    Local = 206,
    FocusAndFans=207,
    AccountSafe = 208,
    ChangeBindPhone = 209,
    MyLikePosts = 210,
    PreachEditor = 211,
    PreachEditorVideo = 212,
    PreachSearch = 213,
    PreachEditorCommnet = 214,
    PreachReplyComment = 215,
    Classics = 216,
    BookDetail = 217,
    Collect = 218,
    EditorBirthday = 219,
    Report = 220,
    FeedBack = 221,
    SysCustomLike = 222,
    SysCustomComment = 223,
    Terms = 224,
    Black = 225,
}

public enum UiLayer
{
    Default = 0,
    BottomBar = 1,
    Post = 2,
    Tip = 5,
}

public class Web
{
    public const string Url = "http://test.yangqungongshe.com";
}


public class NEWURLPATH
{
    //private const string BaseUrl = "http://apiv2.yangqungongshe.com";
    private const string BaseUrl = "https://api.yangqungongshe.com";

    public static string PhoneLoginVerfiycodeSend = string.Format(BaseUrl + "{0}", "/securitytokens");
    public static string PhoneLoginVerfiycodeCheck = string.Format(BaseUrl + "{0}", "/securitytokens/verify");
    public static string PhoneLogin = string.Format(BaseUrl + "{0}", "/account/login/mobile");
    public static string QuitOut = string.Format(BaseUrl + "{0}", "/account/login");
    public static string BindPhone = string.Format(BaseUrl + "{0}", "/account/bindings/mobile");
    public static string CancelBindPhone = string.Format(BaseUrl + "{0}", "/account/bindings/mobile");

    public static string UpdateAccountDisPlayName = string.Format(BaseUrl + "{0}", "/account/displayname");
    public static string UpdateAccountAvatar = string.Format(BaseUrl + "{0}", "/account/avatar");
    public static string UpdateAccountCover = string.Format(BaseUrl + "{0}", "/account/coverphoto");
    public static string UpdateAccountSignature = string.Format(BaseUrl + "{0}", "/account/signature");
    public static string UpdateAccountLocal = string.Format(BaseUrl + "{0}", "/account/location");
    public static string UpdateAccountGender = string.Format(BaseUrl + "{0}", "/account/gender");
    public static string UpdateAccountBirthday = string.Format(BaseUrl + "{0}", "/account/birthdate");
    public static string UpdateGroupAvatar = string.Format(BaseUrl + "{0}", "/groups/{GroupId}/icon");
    public static string QueryReadRecord = string.Format(BaseUrl + "{0}", "/views/query/byuser");
    public static string QueryReadRecordCount = string.Format(BaseUrl + "{0}", "/views/count/byuser");
    public static string QueryReadRecordCountByUsers = string.Format(BaseUrl + "{0}", "/views/count/byusers");
    public static string QueryCollectsByUser = string.Format(BaseUrl + "{0}", "/bookmarks/query/byuser");
    public static string ReqDeleteCollect = string.Format(BaseUrl + "{0}", "/bookmarks");

    public static string QueryUserById = string.Format(BaseUrl + "{0}", "/users/{UserId}");
    public static string QueryBaseUserById = string.Format(BaseUrl + "{0}", "/users/basic/{UserId}");
    public static string QueryUsers = string.Format(BaseUrl + "{0}", "/users/query");
    public static string QueryUsersByIds = string.Format(BaseUrl + "{0}", "/users/query/byids");
    public static string QueryUserByUNameOrEmail = string.Format(BaseUrl + "{0}", "/users/show/{UserNameOrEmail}");
    public static string QueryUserByDisplayName = string.Format(BaseUrl + "{0}", "/users/showname/{DisplayName}");
    public static string QueryAccountInfo = string.Format(BaseUrl + "{0}", "/account");
    public static string MyFollowList = string.Format(BaseUrl + "{0}", "/follows/owners");

    public static string 
        
        MyFansList = string.Format(BaseUrl + "{0}", "/follows/followers");
    public static string AddFollow = string.Format(BaseUrl + "{0}", "/follows");
    public static string DeleteFollow = string.Format(BaseUrl + "{0}", "/follows");


        
    public static string CreatePost = string.Format(BaseUrl + "{0}", "/posts");
    public static string DeletePost = string.Format(BaseUrl + "{0}", "/posts/{PostId}");
    public static string QuerySinglePost = string.Format(BaseUrl + "{0}", "/posts/{PostId}");
    public static string UpdateSinglePost = string.Format(BaseUrl + "{0}", "/posts/{PostId}");
    public static string QueryPosts = string.Format(BaseUrl + "{0}", "/posts/query");
    public static string QueryBasePosts = string.Format(BaseUrl + "{0}", "/posts/basic/query");
    public static string QueryBasePostsbyfollowingowners = string.Format(BaseUrl + "{0}", "/posts/basic/query/byfollowingowners");
    public static string QueryBasePostsbyauthor = string.Format(BaseUrl + "{0}", "/posts/basic/query/byauthor");
    public static string CreateComment = string.Format(BaseUrl + "{0}", "/comments");
    public static string CreateReply = string.Format(BaseUrl + "{0}", "/replies");
    public static string DeleteComment = string.Format(BaseUrl + "{0}", "/comments/{CommentId}");
    public static string DeleteReply = string.Format(BaseUrl + "{0}", "/replies/{ReplyId}");

    public static string QueryCountries = string.Format(BaseUrl + "{0}", "/countries/query");
    public static string QueryStates = string.Format(BaseUrl + "{0}", "/states/query");
    public static string QueryCities = string.Format(BaseUrl + "{0}", "/cities/query");

    public static string ReqCreateLike = string.Format(BaseUrl + "{0}", "/likes");
    public static string ReqCancelLike = string.Format(BaseUrl + "{0}", "/likes"); 
    public static string QuerylLikesByUser = string.Format(BaseUrl + "{0}", "/likes/query/byuser");
    public static string CreateReport = string.Format(BaseUrl + "{0}", "/abusereports");
    public static string QueryLikeInfoForSelf = string.Format(BaseUrl + "{0}", "/likes/query/bypostsofauthor");
    public static string QueryCommentInfoForSelf = string.Format(BaseUrl + "{0}", "/comments/query/bypostsofauthor");
    public static string CreateFeedBack = string.Format(BaseUrl + "{0}", "/feedbacks");
    public static string QueryRecommendation = string.Format(BaseUrl + "{0}", "/recommendations/query");

    public static string QueryPersonalRank = string.Format(BaseUrl + "{0}", "/users/ranks/query");
    public static string QueryGroupsRank = string.Format(BaseUrl + "{0}", "/groups/ranks/query");
    public static string QueryPersonalRankById = string.Format(BaseUrl +"{0}", "/users/ranks/{UserId}");
    public static string QueryGroupRankByIds = string.Format(BaseUrl + "{0}", "/groups/ranks/query/byids");

    public static string ReqQueryLastReadRecord = string.Format(BaseUrl + "{0}", "/chapterreads/last");

    public static string ReqCreateBlack = string.Format(BaseUrl + "{0}", "/blocks");
    public static string ReqDeleteBlack = string.Format(BaseUrl + "{0}", "/blocks");
    /// <summary>
    /// 请求被屏蔽者信息
    /// </summary>
    public static string ReqBlackedInfo = string.Format(BaseUrl + "{0}", "/blocks/blockees");

    public static string ReqCreatePostBlack = string.Format(BaseUrl + "{0}", "/postblocks");
    public static string ReqDeletePostBlack = string.Format(BaseUrl + "{0}", "/postblocks");
    public static string ReqBlackedPostsInfo = string.Format(BaseUrl + "{0}", "/postblocks/query/byblocker");

}

public class DefaultImage
{
    private static Texture2D _mHead;
    private static Texture2D _mCover;
    public static Texture2D Head {
        get
        {
            if (_mHead == null)
            {
                _mHead = Resources.Load<Texture2D>("defaultHeadIcon");
            }
            return _mHead;
        }
    }

    public static Texture2D Cover
    {
        get
        {
            if (_mCover == null)
            {
                _mCover = Resources.Load<Texture2D>("Cover");
            }

            return _mCover;
        }
    }

    public static Texture2D ImHeadTexture2D { get; set; } 

}

public class DBPATH
{
    public const string BIBLE = "DataBase/Bible";
    public const string USERRECORD = "DataBase/UserRecord";
}

public class SQL
{
    public const string QUERT_BIOGRAPHY = "SELECT * FROM Biography";
    public const string QUERY_BIOGRAPHY_OLD = "SELECT * FROM Biography WHERE Id < 40";
    public const string QUERY_BIOGRAPHY_NEW = "SELECT * FROM Biography WHERE Id >= 40";
    public const string QUERY_BIOGRAPHY_BYID = "SELECT * FROM Biography WHERE Id = ?";
    public const string QUERY_CHAPTER_BYCHAPTERID = "SELECT * FROM Chapter WHERE Id = ?";
    public const string QUERY_CHAPTER_BYBIOGRAPHYID = "SELECT * FROM Chapter WHERE BiographyId = ?";
    public const string QUERY_SEGMENT_BYCHAPTERID = "SELECT * FROM Segment WHERE ChapterId = ?";

    public const string QUERY_BIBLECOLLECT = "SELECT * FROM BibleCollect";
    public const string QUERY_BIBLECOLLECT_BYSEGMENTID = "SELECT * FROM BibleCollect WHERE SegmentId = ?";
}

public class CommDefine
{
    public const int FontSize = 32;
}

public class LoadPicStyle
{
    public const string Thumbnail = "?x-oss-process=style/thumbnail";
    public const string Standard = "?x-oss-process=style/standard";
    public const string Cell = "?x-oss-process=style/thumbnailpostcell";
    public const string ThumbnailHead = "?x-oss-process=style/thumbnailhead";
    public const string ThumbnailRankHead = "?x-oss-process=style/rankhead";
}

public enum PostSearchType
{
    /// <summary>
    /// 论道浏览界面
    /// </summary>
    PearchBrowse,
    /// <summary>
    /// 个人界面
    /// </summary>
    Personal
}

public enum SearchUserType
{
    /// <summary>
    /// 会话记录界面
    /// </summary>
    ChatSession,
    /// <summary>
    /// 粉丝和被关注界面
    /// </summary>
    FansAndFollow,
    /// <summary>
    /// 群组成员列表界面
    /// </summary>
    GroupMember,
    /// <summary>
    /// 好友列表界面
    /// </summary>
    Friendlist,
    /// <summary>
    /// 已加入群组列表界面
    /// </summary>
    GroupList
}

public enum EditorUserDataType
{
    /// <summary>
    /// 账户中心界面
    /// </summary>
    AccountCenter,
    /// <summary>
    /// 个人界面
    /// </summary>
    Personal,
}

public enum EditorGroupType
{
    /// <summary>
    /// 创建群组类型
    /// </summary>
    CreateGroup,
    /// <summary>
    /// 更新群组数据类型
    /// </summary>
    UpdateGroup,
}

public enum FromChatMainType
{
    ChatSession,
    Personal
}
/// <summary>
/// 举报界面
/// </summary>
public enum FromReportViewType
{
    /// <summary>
    /// 个人页面
    /// </summary>
    Personal,
    /// <summary>
    /// 主聊天界面
    /// </summary>
    ChatMain,
    /// <summary>
    /// 帖子界面
    /// </summary>
    PreachPost,
    /// <summary>
    /// 首页列表界面
    /// </summary>
    Preach,
}
