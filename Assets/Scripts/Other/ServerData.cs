using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Account About



/// <summary>
/// 请求发送登录验证码
/// </summary>

public class Js_RequestSendLGVerifyCode
{
    /// <summary>
    ///     手机号码。
    /// </summary>

    public string PhoneNumber { get; set; }

    /// <summary>
    ///     验证码用途。
    /// </summary>

    public string Purpose { get; set; }
}

/// <summary>
/// 响应请求登录验证码
/// </summary>

public class Js_ResponseSendLGVerifyCode
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 请求校验登录验证码
/// </summary>

public class Js_RequestAuthenticationLGVerifyCode
{
    /// <summary>
    ///     手机号码。
    /// </summary>

    public string PhoneNumber { get; set; }

    /// <summary>
    ///     验证码用途。
    /// </summary>

    public string Purpose { get; set; }

    /// <summary>
    ///     动态验证码。
    /// </summary>

    public string Token { get; set; }
}

/// <summary>
/// 响应校验登录验证码
/// </summary>

public class Js_ResponseAuthenticationLGVerifyCode
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 请求手机验证码登录
/// </summary>

public class Js_RequestPhoneLogins
{

    public string PhoneNumber { get; set; }


    public string Token { get; set; }
}

/// <summary>
/// 返回手机登录响应
/// </summary>

public class Js_ResponseLogins
{
    /// <summary>
    ///     服务端用户身份的会话唯一凭证。
    /// </summary>

    public string SessionId { get; set; }

    /// <summary>
    ///     用户Id。
    /// </summary>

    public int UserId { get; set; }
    /// <summary>
    /// 是否是第一次登录
    /// </summary>

    public Boolean NewlyCreated { get; set; }
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 更改账户显示名称
/// </summary>

public class Js_RequestUpdateAccountDisplayName
{

    public string DisplayName { get; set; }
}

/// <summary>
/// 响应更改账户显示名称
/// </summary>

public class Js_ResponeUpdateAccountDisplayName
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

public class Js_ResponeUpdateAccountCover
{
    /// <summary>
    /// 账户头像图片地址
    /// </summary>
    public string AvatarUrl { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应修改用户资料
/// </summary>
public class Js_ResponeUpdateAccount
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应请求国家数据
/// </summary>
public class Js_ResponeCountries
{
    /// <summary>
    /// 国家名数据
    /// </summary>
    public List<LocalInfo> Countries { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求省份数据
/// </summary>
public class Js_ResponeStates
{
    /// <summary>
    /// 省份名数据
    /// </summary>
    public List<LocalInfo> States { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求城市数据
/// </summary>
public class Js_ResponeCities
{
    /// <summary>
    /// 城市名数据
    /// </summary>
    public List<LocalInfo> Cities { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应群组头像的更新
/// </summary>
public class Js_ResponeUpdateGroupAvatar
{
    /// <summary>
    /// 群组头像地址
    /// </summary>
    public string IconUrl { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}


/// <summary>
/// 请求单个用户信息
/// </summary>

public class Js_ReqSingleUserInfo
{

    public string UserId { get; set; }
}
/// <summary>
/// 响应请求单个用户信息
/// </summary>

public class Js_ResponeSingleUserInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>

    public User User { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应请求用户列表信息
/// </summary>
public class Js_ResponeUsersInfo
{
    /// <summary>
    /// 一组用户数据信息
    /// </summary>
    public List<User> Users { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应登陆成功后请求账户信息
/// </summary>

public class Js_ResponeAccountInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>

    public User Account { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应登出
/// </summary>
public class Js_ResponeLoginOut
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应请求用户阅读记录
/// </summary>
public class Js_ResponeReadRecord
{
    /// <summary>
    /// 一组阅读记录
    /// </summary>
    public List<ReadRecordInfo> Views { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应请求用户阅读数量
/// </summary>
public class Js_ResponeReadRecordCount
{
    /// <summary>
    /// 阅读数量
    /// </summary>
    public ReadRecordCount Counts { get; set; }
    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应请求用户列表的阅读数据数量
/// </summary>
public class Js_ResponeReadRecordCountByUsers
{
    /// <summary>
    /// 用户读经列表数目
    /// </summary>
    public Dictionary<string, ReadRecordCount> UsersCounts { get; set; }
    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 响应收藏信息请求
/// </summary>
public class Js_ResponeCollectInfos
{
    /// <summary>
    /// 收藏信息组
    /// </summary>
    public List<CollectInfo> Bookmarks { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 用户信息
/// </summary>

public class User
{
    /// <summary>
    ///     编号。
    /// </summary>

    public int Id { get; set; }

    /// <summary>
    ///     类型。
    /// </summary>

    public string Type { get; set; }

    /// <summary>
    ///     用户名称。
    /// </summary>

    public string UserName { get; set; }

    /// <summary>
    ///     电子邮件地址。
    /// </summary>

    public string Email { get; set; }

    /// <summary>
    ///     显示名称。
    /// </summary>

    public string DisplayName { get; set; }

    /// <summary>
    ///     真实全称姓名。
    /// </summary>

    public string FullName { get; set; }

    /// <summary>
    ///     真实全称姓名是否已通过认证。
    /// </summary>

    public bool FullNameVerified { get; set; }

    /// <summary>
    ///     签名。
    /// </summary>

    public string Signature { get; set; }

    /// <summary>
    ///     简介。
    /// </summary>

    public string Description { get; set; }

    /// <summary>
    ///     头像地址。
    /// </summary>

    public string AvatarUrl { get; set; }

    /// <summary>
    ///     封面图像地址。
    /// </summary>

    public string CoverPhotoUrl { get; set; }

    /// <summary>
    ///     出生日期。
    /// </summary>

    public string BirthDate { get; set; }

    /// <summary>
    ///     性别。
    /// </summary>

    public string Gender { get; set; }

    /// <summary>
    ///     手机号码。
    /// </summary>

    public string PhoneNumber { get; set; }

    /// <summary>
    ///     居住国家。
    /// </summary>

    public string Country { get; set; }

    /// <summary>
    ///     居住省份/州。
    /// </summary>

    public string State { get; set; }

    /// <summary>
    ///     居住城市。
    /// </summary>

    public string City { get; set; }

    /// <summary>
    ///     所属教会。
    /// </summary>

    public string Guild { get; set; }

    /// <summary>
    ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
    /// </summary>

    public string Status { get; set; }

    /// <summary>
    ///     禁止的原因。（可选值：垃圾营销, 不实信息, 违法信息, 有害信息, 淫秽色情, 欺诈骗局, 冒充他人, 抄袭内容, 人身攻击, 泄露隐私）
    /// </summary>

    public string BanReason { get; set; }

    ///// <summary>
    /////     禁止的取消日期。
    ///// </summary>
    //[ProtoMember(20)]
    //public long? BannedUntilDate { get; set; }

    ///// <summary>
    /////     创建日期。
    ///// </summary>
  
    //public long CreatedDate { get; set; }

    ///// <summary>
    /////     更新日期。
    ///// </summary>
   
    //public long ModifiedDate { get; set; }

    ///// <summary>
    /////     锁定日期。
    ///// </summary>
    //[ProtoMember(23)]
    //public long? LockedDate { get; set; }

    /// <summary>
    ///     积分。
    /// </summary>
    public int Points { get; set; }

}
/// <summary>
/// 账户信息
/// </summary>

public class Account
{
    /// <summary>
    ///     编号。
    /// </summary>

    public int Id { get; set; }

    /// <summary>
    ///     类型。
    /// </summary>

    public string Type { get; set; }

    /// <summary>
    ///     用户名称。
    /// </summary>

    public string UserName { get; set; }

    /// <summary>
    ///     电子邮件地址。
    /// </summary>

    public string Email { get; set; }

    /// <summary>
    ///     显示名称。
    /// </summary>

    public string DisplayName { get; set; }

    /// <summary>
    ///     真实全称姓名。
    /// </summary>

    public string FullName { get; set; }

    /// <summary>
    ///     真实全称姓名是否已通过认证。
    /// </summary>

    public bool FullNameVerified { get; set; }

    /// <summary>
    ///     身份证图片地址。
    /// </summary>

    public string IdImageUrl { get; set; }

    /// <summary>
    ///     签名。
    /// </summary>

    public string Signature { get; set; }

    /// <summary>
    ///     简介。
    /// </summary>

    public string Description { get; set; }

    /// <summary>
    ///     头像地址。
    /// </summary>

    public string AvatarUrl { get; set; }

    /// <summary>
    ///     封面图像地址。
    /// </summary>

    public string CoverPhotoUrl { get; set; }

    /// <summary>
    ///     出生日期。
    /// </summary>

    public string BirthDate { get; set; }

    /// <summary>
    ///     性别。
    /// </summary>

    public string Gender { get; set; }

    /// <summary>
    ///     站点联系用户的电子邮件。
    /// </summary>

    public string PrimaryEmail { get; set; }

    /// <summary>
    ///     手机号码。
    /// </summary>

    public string PhoneNumber { get; set; }

    /// <summary>
    ///     居住国家。
    /// </summary>

    public string Country { get; set; }

    /// <summary>
    ///     居住省份/州。
    /// </summary>

    public string State { get; set; }

    /// <summary>
    ///     居住城市。
    /// </summary>

    public string City { get; set; }

    /// <summary>
    ///     所属教会。
    /// </summary>

    public string Guild { get; set; }

    /// <summary>
    ///     所在公司。
    /// </summary>

    public string Company { get; set; }

    /// <summary>
    ///     居住地址。
    /// </summary>

    public string Address { get; set; }

    /// <summary>
    ///     居住地址二。
    /// </summary>

    public string Address2 { get; set; }

    /// <summary>
    ///     寄件地址。
    /// </summary>

    public string MailAddress { get; set; }

    /// <summary>
    ///     邮政编码。
    /// </summary>

    public string PostalCode { get; set; }

    /// <summary>
    ///     所在时区。
    /// </summary>

    public string TimeZone { get; set; }

    /// <summary>
    ///     显示语言。
    /// </summary>

    public string Language { get; set; }

    /// <summary>
    ///     私信消息的来源。（可选值：None, Friends, Everyone）
    /// </summary>

    public string PrivateMessagesSource { get; set; }

    /// <summary>
    ///     是否接收电子邮件。
    /// </summary>

    public bool? ReceiveEmails { get; set; }

    /// <summary>
    ///     是否接收手机短消息。
    /// </summary>

    public bool? ReceiveSms { get; set; }

    /// <summary>
    ///     是否接收评论通知。
    /// </summary>

    public bool? ReceiveCommentNotifications { get; set; }

    /// <summary>
    ///     是否接收对话通知。
    /// </summary>

    public bool? ReceiveConversationNotifications { get; set; }

    /// <summary>
    ///     是否允许其他用户看到在线状态。
    /// </summary>

    public bool? TrackPresence { get; set; }

    /// <summary>
    ///     是否共享书签。
    /// </summary>

    public bool? ShareBookmarks { get; set; }

    /// <summary>
    ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
    /// </summary>

    public string Status { get; set; }

    /// <summary>
    ///     禁止的原因。（可选值：垃圾营销, 不实信息, 违法信息, 有害信息, 淫秽色情, 欺诈骗局, 冒充他人, 抄袭内容, 人身攻击, 泄露隐私）
    /// </summary>

    public string BanReason { get; set; }

    /// <summary>
    ///     禁止的取消日期。
    /// </summary>

    public long? BannedUntilDate { get; set; }

    /// <summary>
    ///     撰写的内容是否需要管理员审核。
    /// </summary>

    public bool? RequireModeration { get; set; }

    /// <summary>
    ///     创建日期。
    /// </summary>

    public long CreatedDate { get; set; }

    /// <summary>
    ///     更新日期。
    /// </summary>

    public long ModifiedDate { get; set; }

    /// <summary>
    ///     锁定日期。
    /// </summary>

    public long? LockedDate { get; set; }

    /// <summary>
    ///     积分。
    /// </summary>

    public int Points { get; set; }
}
/// <summary>
/// 基本用户信息
/// </summary>

public class BaseUser
{
    /// <summary>
    ///     编号。
    /// </summary>

    public int Id { get; set; }

    /// <summary>
    ///     用户名称。
    /// </summary>

    public string UserName { get; set; }

    /// <summary>
    ///     显示名称。
    /// </summary>

    public string DisplayName { get; set; }

    /// <summary>
    ///     头像地址。
    /// </summary>

    public string AvatarUrl { get; set; }

    /// <summary>
    ///     性别。
    /// </summary>

    public string Gender { get; set; }
}

/// <summary>
/// 地区信息
/// </summary>
public class LocalInfo
{
    /// <summary>
    /// 地区编号
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 地区名称
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// 阅读记录信息
/// </summary>
public class ReadRecordInfo
{

    /// <summary>
    /// 上级类型（可选值：帖子, 章, 节）
    /// </summary>
    public string ParentType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string ParentTitle { get; set; }
    /// <summary>
    /// 用户信息
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public int CreatedDate { get; set; }
}
/// <summary>
/// 阅读记录的数目
/// </summary>
public class ReadRecordCount
{
    /// <summary>
    /// 阅读的次数
    /// </summary>
    public int ViewsCount { get; set; }
    /// <summary>
    /// 阅读的节数
    /// </summary>
    public int ParentsCount { get; set; }
    /// <summary>
    /// 阅读的天数
    /// </summary>
    public int DaysCount { get; set; }
}
/// <summary>
/// 收藏信息
/// </summary>
public class CollectInfo
{
    /// <summary>
    /// 类型
    /// </summary>
    public string ParentType { get; set; }
    /// <summary>
    /// Id
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 标题内容
    /// </summary>
    public string ParentTitle { get; set; }
    /// <summary>
    /// 用户信息
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public int CreatedDate { get; set; }
}
/// <summary>
/// 举报信息
/// </summary>
public class AbuseReport
{
    /// <summary>
    /// 举报Id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 上级类型
    /// </summary>
    public string ParentType { get; set; }
    /// <summary>
    /// 上级编号
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 举报理由
    /// </summary>
    public string Reason { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public int CreatedDate { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public int ModifiedDate { get; set; }
    /// <summary>
    /// 举报者信息
    /// </summary>
    public User User { get; set; }
}
/// <summary>
/// 响应举报请求
/// </summary>
public class Js_ResponeReportInfo
{
    /// <summary>
    /// 举报信息
    /// </summary>
    public AbuseReport AbuseReport { get; set; }
    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 个人阅读排名信息
/// </summary>
public class PersonalRankInfo
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// 上次帖子浏览次数
    /// </summary>
    public int LastPostViewsCount { get; set; }
    /// <summary>
    /// 上次帖子排名
    /// </summary>
    public int LastPostViewsRank { get; set; }
    /// <summary>
    /// 当前帖子浏览次数
    /// </summary>
    public int PostViewsCount { get; set; }
    /// <summary>
    /// 当前帖子浏览排名
    /// </summary>
    public int PostViewsRank { get; set; }
    /// <summary>
    /// 上次阅读总节数
    /// </summary>
    public int LastParagraphViewsCount { get; set; }
    /// <summary>
    /// 上次阅读排名
    /// </summary>
    public int LastParagraphViewsRank { get; set; }
    /// <summary>
    /// 当前阅读总节数
    /// </summary>
    public int ParagraphViewsCount { get; set; }
    /// <summary>
    /// 当前阅读排名
    /// </summary>
    public int ParagraphViewsRank { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public int CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ModifiedDate { get; set; }
}
/// <summary>
/// 响应请求一组个人阅读排名数据
/// </summary>
public class Js_ResponePersonalRankInfo
{
    /// <summary>
    /// 一组用户排名信息
    /// </summary>
    public List<PersonalRankInfo> UserRanks { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 群组排名信息
/// </summary>
public class GroupRankInfo
{
    /// <summary>
    /// 群组信息
    /// </summary>
    public GroupInfo Group { get; set; }
    /// <summary>
    /// 上次帖子浏览总次数
    /// </summary>
    public int LastPostViewsCount { get; set; }
    /// <summary>
    /// 上次帖子排名
    /// </summary>
    public int LastPostViewsRank { get; set; }
    /// <summary>
    /// 当前帖子浏览次数
    /// </summary>
    public int PostViewsCount { get; set; }
    /// <summary>
    /// 当前帖子浏览排名
    /// </summary>
    public int PostViewsRank { get; set; }
    /// <summary>
    /// 上次阅读总节数
    /// </summary>
    public int LastParagraphViewsCount { get; set; }
    /// <summary>
    /// 上次阅读排名
    /// </summary>
    public int LastParagraphViewsRank { get; set; }
    /// <summary>
    /// 当前阅读总节数
    /// </summary>
    public int ParagraphViewsCount { get; set; }
    /// <summary>
    /// 当前阅读排名
    /// </summary>
    public int ParagraphViewsRank { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public int CreatedDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ModifiedDate { get; set; }

}
/// <summary>
/// 群组信息
/// </summary>
public class GroupInfo
{
    /// <summary>
    /// 群组ID
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 群组昵称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 群组头像地址
    /// </summary>
    public string IconUrl { get; set; }
}

/// <summary>
/// 响应请求一组群组排名数据
/// </summary>
public class Js_ResponeGroupRankInfo
{
    /// <summary>
    /// 一组群组排行信息
    /// </summary>
    public List<GroupRankInfo> GroupRanks { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求单个用户阅读排名数据
/// </summary>
public class Js_ResponePersonalRankInfoById
{
    /// <summary>
    /// 单个用户阅读排名信息
    /// </summary>
    public PersonalRankInfo UserRank { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 通用服务器响应
/// </summary>

public class Js_ResponseComm
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

public class ResponseStatus
{
    /// <summary>
    /// Holds the custom ErrorCode enum if provided in ValidationException
    /// otherwise will hold the name of the Exception type, e.g. typeof(Exception).Name
    /// 
    /// A value of non-null means the service encountered an error while processing the request.
    /// </summary>

    public string ErrorCode { get; set; }

    /// <summary>A human friendly error message</summary>

    public string Message { get; set; }

    /// <summary>The Server StackTrace when DebugMode is enabled</summary>

    public string StackTrace { get; set; }

    /// <summary>
    /// For multiple detailed validation errors.
    /// Can hold a specific error message for each named field.
    /// </summary>

    public List<ResponseError> Errors { get; set; }

    /// <summary>For additional custom metadata about the error</summary>

    public Dictionary<string, string> Meta { get; set; }

    public ResponseStatus()
    {
    }

    public ResponseStatus(string errorCode)
    {
        this.ErrorCode = errorCode;
    }

    public ResponseStatus(string errorCode, string message)
        : this(errorCode)
    {
        this.Message = message;
    }
}

public class ResponseError
{

    public string ErrorCode { get; set; }


    public string FieldName { get; set; }


    public string Message { get; set; }


    public Dictionary<string, string> Meta { get; set; }
}
/// <summary>
/// 身份证明Id
/// </summary>
public class SessionId
{
    public string Ssid { get; set; }
}

public class LastSysTime
{
    public string SystimeTag { get; set; }
}

public class LastSysCustomLikeMsg
{
    public string IsLike { get; set; }
}

public class LastSysCustomCommentMsg
{
    public string IsComment { get; set; }
}
/// <summary>
/// 响应反馈结果
/// </summary>
public class ResponeFeedBack
{
    public Feedback Feedback { get; set; }
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }

}
/// <summary>
/// 反馈数据
/// </summary>
public class Feedback
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public int CreatedDate { get; set; }
    public int ModifiedDate { get; set; }
    public User User { get; set; }
}

#endregion


#region 好友关系相关


public class Js_RequestMyFollow
{
    /// <summary>
    ///     关注者编号
    /// </summary>

    public string followerid { get; set; }

    /// <summary>
    ///     创建日期在指定的时间之后
    /// </summary>

    public string createdsince { get; set; }

    /// <summary>
    ///     修改日期在指定的时间之后
    /// </summary>

    public string modifiedsince { get; set; }

    /// <summary>
    ///     排序的字段（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）
    /// </summary>

    public string orderby { get; set; }

    /// <summary>
    ///     是否按降序排序
    /// </summary>

    public string descending { get; set; }

    /// <summary>
    ///     忽略的行数
    /// </summary>

    public string skip { get; set; }

    /// <summary>
    ///     获取的行数
    /// </summary>

    public string limit { get; set; }
}


public class Js_ResponseMyFollow
{
    /// <summary>
    ///     一组关注信息。
    /// </summary>

    public List<FollowInfo> Follows { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}


public class Js_RequestMyFans
{
    /// <summary>
    ///     被关注者编号
    /// </summary>

    public string ownerid { get; set; }
    /// <summary>
    ///     创建日期在指定的时间之后
    /// </summary>

    public string createdsince { get; set; }
    /// <summary>
    ///     修改日期在指定的时间之后
    /// </summary>

    public string modifiedsince { get; set; }
    /// <summary>
    ///     排序的字段（可选值：IsBidirectional, CreatedDate, ModifiedDate 默认为 CreatedDate）
    /// </summary>

    public string orderby { get; set; }

    /// <summary>
    ///     是否按降序排序
    /// </summary>

    public string descending { get; set; }

    /// <summary>
    ///     忽略的行数
    /// </summary>

    public string skip { get; set; }

    /// <summary>
    ///     获取的行数
    /// </summary>

    public string limit { get; set; }
}


public class Js_ResponseMyFans
{
    /// <summary>
    ///     一组关注信息。
    /// </summary>

    public List<FollowFnInfo> Follows { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}


public class Js_RequestAddFollow
{
    /// <summary>
    ///     被关注者编号
    /// </summary>

    public string OwnerId { get; set; }
}


public class Js_ResponseAddFollow
{
    /// <summary>
    ///     关注信息。
    /// </summary>

    public Follow Follow { get; set; }
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}


public class Js_RequestDeleteFollow
{
    /// <summary>
    ///     被关注者编号。
    /// </summary>

    public string OwnerId { get; set; }
}


public class Js_ResponseDeleteFollow
{
    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}


public class FollowInfo
{
    /// <summary>
    ///     被关注者。
    /// </summary>

    public User Owner { get; set; }

    /// <summary>
    ///     是否已双向关注。
    /// </summary>

    public bool IsBidirectional { get; set; }

    /// <summary>
    ///     创建日期。
    /// </summary>

    public int CreatedDate { get; set; }

    /// <summary>
    ///     更新日期。
    /// </summary>

    public int ModifiedDate { get; set; }

}


public class FollowFnInfo
{
    /// <summary>
    ///     关注者。
    /// </summary>

    public User Follower { get; set; }

    /// <summary>
    ///     是否已双向关注。
    /// </summary>

    public bool IsBidirectional { get; set; }

    /// <summary>
    ///     创建日期。
    /// </summary>

    public int CreatedDate { get; set; }

    /// <summary>
    ///     更新日期。
    /// </summary>

    public int ModifiedDate { get; set; }
}


public class Follow
{
    /// <summary>
    ///     被关注者信息。
    /// </summary>

    public User Owner { get; set; }
    /// <summary>
    ///     关注者信息。
    /// </summary>

    public User Follower { get; set; }
    /// <summary>
    /// 是否是双向关注
    /// </summary>

    public bool IsBidirectional { get; set; }
    /// <summary>
    ///     创建日期。
    /// </summary>

    public int CreatedDate { get; set; }
    /// <summary>
    ///     更新日期。
    /// </summary>

    public int ModifiedDate { get; set; }
}

#endregion

#region 论道相关

/// <summary>
/// 请求热门论道数据
/// </summary>
public class Js_RequestHotPosts
{
    /// <summary>
    /// 是否是已发布
    /// </summary>
    public string ispublished { get; set; }
    /// <summary>
    /// 忽略的行数
    /// </summary>
    public string skip { get; set; }
    /// <summary>
    /// 获取的行数
    /// </summary>
    public string limit { get; set; }

}

/// <summary>
/// 响应请求热门论道数据
/// </summary>
public class Js_ResponePostsData
{
    /// <summary>
    /// 一组帖子数据
    /// </summary>
    public List<BasePostInfo> Posts { get; set; }

    /// <summary>
    ///     处理响应的状态。
    /// </summary>

    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求单个帖子的数据
/// </summary>
public class Js_ResponeSinglePostData
{
    /// <summary>
    /// 帖子数据
    /// </summary>
    public BasePostInfo Post { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求用户的点赞信息
/// </summary>
public class Js_ResponeLikesData
{
    /// <summary>
    /// 一组点赞信息
    /// </summary>
    public List<LikeInfo> Likes { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }

}

/// <summary>
/// 帖子信息
/// </summary>
public class  PostInfo
{
    /// <summary>
    ///     编号。
    /// </summary>
    
    public string Id { get; set; }

    /// <summary>
    ///     类型。
    /// </summary>
    
    public string Type { get; set; }

    /// <summary>
    ///     标题。
    /// </summary>
   
    public string Title { get; set; }

    /// <summary>
    ///     概要。
    /// </summary>
    
    public string Summary { get; set; }

    /// <summary>
    ///     图片的地址。
    /// </summary>
  
    public string PictureUrl { get; set; }

    /// <summary>
    ///     内容的类型。（可选值：图文, 音频, 视频）
    /// </summary>
    
    public string ContentType { get; set; }

    /// <summary>
    ///     正文内容。
    /// </summary>
   
    public string Content { get; set; }

    /// <summary>
    ///     内容的地址。（当类型为音频或视频时，填写音频或视频的地址）
    /// </summary>
   
    public string ContentUrl { get; set; }

    /// <summary>
    ///     分类的标签列表。
    /// </summary>
   
    public List<string> Tags { get; set; }

    /// <summary>
    ///     状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
    /// </summary>
   
    public string Status { get; set; }

    ///// <summary>
    /////     禁止的原因。（可选值：垃圾营销, 不实信息, 违法信息, 有害信息, 淫秽色情, 欺诈骗局, 冒充他人, 抄袭内容, 人身攻击, 泄露隐私）
    ///// </summary>
  
    //public string BanReason { get; set; }

    ///// <summary>
    /////     禁止的取消日期。
    ///// </summary>
  
    //public long? BannedUntilDate { get; set; }

    /// <summary>
    ///     创建日期。
    /// </summary>
   
    public int CreatedDate { get; set; }

    /// <summary>
    ///     更新日期。
    /// </summary>
    
    public int ModifiedDate { get; set; }

    /// <summary>
    ///     是否已发布。
    /// </summary>
    
    public bool IsPublished { get; set; }

    /// <summary>
    ///     发布日期。
    /// </summary>
   
    public int? PublishedDate { get; set; }

    /// <summary>
    ///     是否为精选。
    /// </summary>
    
    public bool IsFeatured { get; set; }

    /// <summary>
    ///     作者的用户。
    /// </summary>
    
    public User Author { get; set; }

    /// <summary>
    ///     查看的次数。
    /// </summary>
    
    public int ViewsCount { get; set; }

    /// <summary>
    ///     收藏的次数。
    /// </summary>
   
    public int BookmarksCount { get; set; }

    /// <summary>
    ///     评论的次数。
    /// </summary>
    
    public int CommentsCount { get; set; }

    /// <summary>
    ///     点赞的次数。
    /// </summary>
    
    public int LikesCount { get; set; }

    /// <summary>
    ///     评分的次数。
    /// </summary>
   
    public int RatingsCount { get; set; }

    /// <summary>
    ///     评分的平均值。
    /// </summary>
    
    public float RatingsAverageValue { get; set; }

    /// <summary>
    ///     分享的次数。
    /// </summary>
    
    public int SharesCount { get; set; }

    /// <summary>
    ///     滥用举报的次数。
    /// </summary>
    
    public int AbuseReportsCount { get; set; }

    ///// <summary>
    /////     内容质量的评分。
    ///// </summary>
    
    //public float ContentQuality { get; set; }
}

/// <summary>
/// 基本帖子信息
/// </summary>
public class BasePostInfo
{
   
        /// <summary>
        ///     编号。
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>

        public string Title { get; set; }

        /// <summary>
        ///     概要。
        /// </summary>

        public string Summary { get; set; }

        /// <summary>
        ///     图片的地址。
        /// </summary>

        public string PictureUrl { get; set; }

        /// <summary>
        ///     内容的类型。（可选值：图文, 音频, 视频）
        /// </summary>

        public string ContentType { get; set; }

    /// <summary>
    ///     发布日期。
    /// </summary>

    public int? PublishedDate { get; set; }

        /// <summary>
        ///     是否为精选。
        /// </summary>

        public bool IsFeatured { get; set; }

        /// <summary>
        ///     作者的用户。
        /// </summary>

        public User Author { get; set; }

        /// <summary>
        ///     评论的次数。
        /// </summary>

        public int CommentsCount { get; set; }

        /// <summary>
        ///     点赞的次数。
        /// </summary>

        public int LikesCount { get; set; }


        /// <summary>
        ///     分享的次数。
        /// </summary>

        public int SharesCount { get; set; }

      
    
}

/// <summary>
/// 点赞信息
/// </summary>
public class LikeInfo
{
    /// <summary>
    /// 上级类型
    /// </summary>
    public string ParentType { get; set; }
    /// <summary>
    /// 上级编号
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 用户信息
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// 创建日期
    /// </summary>
    public int CreatedDate { get; set; }
}

/// <summary>
/// 响应点赞请求
/// </summary>
public class Js_ResponeCreateLike
{
    /// <summary>
    /// 点赞信息
    /// </summary>
    public LikeInfo Like { get; set; }
    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 评论信息
/// </summary>
public class CommentInfo
{
    /// <summary>
    /// 评论Id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 上级类型（可选值：帖子, 章, 节）
    /// </summary>
    public string ParentType { get; set; }
    /// <summary>
    /// 上级编号（如帖子编号）
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 正文内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    ///  状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 创建日期。
    /// </summary>
    public int CreatedDate { get; set; }
    /// <summary>
    ///  更新日期。
    /// </summary>
    public int ModifiedDate { get; set; }
    /// <summary>
    /// 是否为精选。
    /// </summary>
    public bool IsFeatured { get; set; }
    /// <summary>
    /// 评论User信息
    /// </summary>
    public User User { get; set; }
    /// <summary>
    /// 回复评论的Count
    /// </summary>
    public int RepliesCount { get; set; }
    public int VotesCount { get; set; }
    public int YesVotesCount { get; set; }
    public int NoVotesCount { get; set; }
    public bool YesVoted { get; set; }
    public bool NoVoted { get; set; }

}

/// <summary>
/// 响应创建评论请求
/// </summary>
public class Js_ResponeCreateComment
{
    /// <summary>
    /// 评论信息
    /// </summary>
    public CommentInfo Comment { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 评论回复信息
/// </summary>
public class ReplyInfo
{
    /// <summary>
    /// 回复Id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 上级类型（可选值：评论）
    /// </summary>
    public string ParentType { get; set; }
    /// <summary>
    /// 上级编号（如评论编号）
    /// </summary>
    public string ParentId { get; set; }
    /// <summary>
    /// 正文内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    ///  状态。（可选值：待审核, 审核通过, 已禁止, 审核失败, 等待删除）
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 创建日期。
    /// </summary>
    public int CreatedDate { get; set; }
    /// <summary>
    ///  更新日期。
    /// </summary>
    public int ModifiedDate { get; set; }

    /// <summary>
    /// 回复User信息
    /// </summary>
    public User User { get; set; }

    public int VotesCount { get; set; }
    public int YesVotesCount { get; set; }
    public int NoVotesCount { get; set; }
    public bool YesVoted { get; set; }
    public bool NoVoted { get; set; }


}

/// <summary>
/// 响应回复评论的请求
/// </summary>
public class Js_ResponeReplyComment
{
    /// <summary>
    /// 回复评论的信息
    /// </summary>
    public ReplyInfo Reply { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求点赞消息
/// </summary>
public class Js_ResponeSysLikeInfo
{
    public List<SysLikeInfo> Likes { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

public class SysLikeInfo
{
   public string ParentType { get; set; }

    public string ParentContentType { get; set; }

    public string ParentId { get; set; }

   public string ParentTitle { get; set; }

   public string ParentPictureUrl { get; set; }

   public User User { get; set; }

   public int CreatedDate { get; set; }


}

public class Js_ResoneSysCommentInfo
{
    public List<SysCommentInfo> Comments { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}
public class SysCommentInfo
{
    public string Id { get; set; }
    public string ParentType { get; set; }
    public string ParentContentType { get; set; }
    public string ParentPictureUrl { get; set; }
    public string ParentId { get; set; }
    public string ParentTitle { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public int CreatedDate { get; set; }
    public int ModifiedDate { get; set; }
    public bool IsFeatured { get; set; }
    public User User { get; set; }
    public int RepliesCount { get; set; }
    public int VotesCount { get; set; }
    public int YesVotesCount { get; set; }
    public int NoVotesCount { get; set; }
    public bool YesVoted { get; set; }
    public bool NoVoted { get; set; }

}
/// <summary>
/// 响应推荐信息
/// </summary>
public class Js_ResponeRecommendationInfo
{
    public List<RecommendationInfo> Recommendations { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 论道轮播推荐信息
/// </summary>
public class RecommendationInfo
{
    public string Id { get; set; }
    public string ContentType { get; set; }
    public string ContentId { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string PictureUrl { get; set; }
    public int Position { get; set; }
    public int CreatedDate { get; set; }
}
/// <summary>
/// 响应推送点击状态栏的数据
/// </summary>
public class Js_ResponeJpushNotification
{
    public string title { get; set; }

    public string content { get; set; }

    public JpushExtras extras { get; set; }

    public JpushAps aps { get; set; }

    public string PostId { get; set; }

    public string _j_uid { get; set; }

    public string _j_msgid { get; set; }

    public string _j_business { get; set; }

}
/// <summary>
/// android推送附加信息
/// </summary>
public class JpushExtras
{
    /// <summary>
    /// 帖子Id
    /// </summary>
    public string PostId { get; set; }
}
/// <summary>
/// ios推送附加信息
/// </summary>
public class JpushAps
{
    public string alert { get; set; }

    public string badge { get; set; }

    public string sound { get; set; }
}


/// <summary>
/// 响应请求被屏蔽用户信息
/// </summary>
public class Js_ResponeBlocksInfo
{
    /// <summary>
    /// 被屏蔽者数据
    /// </summary>
    public List<BlockerInfo> Blocks { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }

}

/// <summary>
/// 响应请求创建屏蔽用户
/// </summary>
public class Js_ResponeCreateBlockInfo
{   
    /// <summary>
    /// 屏蔽信息
    /// </summary>
    public BlockInfo Block { get; set; }
    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 被屏蔽者信息
/// </summary>
public class BlockerInfo
{  
    /// <summary>
    /// 被屏蔽用户信息
    /// </summary>
    public User Blockee { get; set; }
    public int CreatedDate { get; set; }
    public int ModifiedDate { get; set; }
}

/// <summary>
/// 屏蔽信息
/// </summary>
public class BlockInfo
{
    public User Blockee { get; set; }
    public User Blocker { get; set; }
    public int CreatedDate { get; set; }
    public int ModifiedDate { get; set; }
}


/// <summary>
/// 响应请求被屏蔽的帖子的数据
/// </summary>
public class Js_ResponePostsBlockInfo
{
    /// <summary>
    /// 被屏蔽帖子集合数据
    /// </summary>
    public List<PostBlockInfo> PostBlocks { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

/// <summary>
/// 响应请求创建屏蔽帖子
/// </summary>
public class Js_ResponeCreatePostBlockInfo
{
    public PostBlockInfo PostBlock { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}
/// <summary>
/// 被屏蔽帖子信息
/// </summary>
public class PostBlockInfo
{
    public BasePostInfo Post { get; set; }

    public User Blocker { get; set; }

    public string Reason { get; set; }

    public int CreatedDate { get; set; }

    public int ModifiedDate { get; set; }
}

/// <summary>
/// 响应最后一次阅读记录请求
/// </summary>
public class Js_ResponeLastReadRecordInfo
{
    public ChapterRead ChapterRead { get; set; }

    /// <summary>
    /// 处理响应的状态。
    /// </summary>
    public ResponseStatus ResponseStatus { get; set; }
}

public class ChapterRead
{
    public Book Book { get; set; }

    public Volume Volume { get; set; }

    public Chapters Chapter { get; set; }

    public User User { get; set; }

    public int CreatedDate { get; set; }
}

public class Book
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int VolumesCount { get; set; }
}

public class Volume
{
    public string Id { get; set; }
    public int Number { get; set; }
    public string Title { get; set; }
    public int ChaptersCount { get; set; }
}

public class Chapters
{
    public string Id { get; set; }
    public int VolumeNumber { get; set; }
    public string VolumeTitle { get; set; }
    public int Number { get; set; }
    public string Title { get; set; }
    public int ParagraphsCount { get; set; }
}

#endregion




