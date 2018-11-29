using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdEvent
{
    public enum Command
    {
        #region Boot、Register、Login About
        /// <summary>
        /// 启动应用后，检查更新结束。
        /// </summary>
        UpdateFinish,
        /// <summary>
        /// 底部栏点击
        /// </summary>
        BottomBarClick,
        /// <summary>
        /// 请求发送验证码
        /// </summary>
        ReqVerifyCode,
        /// <summary>
        /// 检查用户昵称是否重复
        /// </summary>
        ReqCheckNameExist,
        /// <summary>
        /// 登录
        /// </summary>
        ReqLogin,
        /// <summary>
        /// 请求绑定手机号码
        /// </summary>
        ReqBindPhone,
        /// <summary>
        /// 请求关注和粉丝列表
        /// </summary>
        ReqFriends,
        /// <summary>
        /// 请求设置昵称
        /// </summary>
       ReqSetDyName,
       /// <summary>
       /// 请求登录账户头像
       /// </summary>
       LoadAccountHeadT2D,
        #endregion

        #region ChatAbout
        /// <summary>
        /// 读取会话记录
        /// </summary>
        LoadSession,
        /// <summary>
        /// 读取聊天记录
        /// </summary>
        LoadChatRecord,
        /// <summary>
        /// 根据消息ID查询一条记录
        /// </summary>
        LoadSingleChatRecord,
        /// <summary>
        /// 读取所有我加入的群信息
        /// </summary>
        LoadMyAllGroups,
        /// <summary>
        /// 读取群信息
        /// </summary>
        LoadGroupOption,
        /// <summary>
        /// 根据群id查询群信息
        /// </summary>
        LoadGroupInfoById,
        /// <summary>
        /// 下载丢失的资源
        /// </summary>
        DownloadLostResChat,
        /// <summary>
        /// 发送即时消息
        /// </summary>
        SendImMsg,
        /// <summary>
        /// 语音操作
        /// </summary>
        AudioOption,
         /// <summary>
        /// 群组操作
        /// </summary>
        GroupOption,
        /// <summary>
        /// 加载群组管理员列表
        /// </summary>
        LoadGroupManagers,
        /// <summary>
        /// 加载群组成员信息
        /// </summary>
        LoadGroupMembers,
        /// <summary>
        /// 加载转让备选群组成员信息
        /// </summary>
        LoadGroupTransferMemers,
        /// <summary>
        /// 读取系统消息记录
        /// </summary>
        LoadSysMs,
        /// <summary>
        /// 加载添加群成员好友信息
        /// </summary>
        LoadGroupSelectMm,
        /// <summary>
        /// 请求删除会话记录
        /// </summary>
        ReqDeleteSession,
        /// <summary>
        /// 请求查询用户信息
        /// </summary>
        ReqQueryUserInfo,
        /// <summary>
        /// 请求取消或添加关注
        /// </summary>
        FocusOptions,
        /// <summary>
        /// 设置消息状态
        /// </summary>
        SetMessageStatus,
        /// <summary>
        /// 聊天界面跳转到个人页面
        /// </summary>
        ChatMainTurnPersonal,
        /// <summary>
        /// 登录IM
        /// </summary>
        LoginIm,
        /// <summary>
        /// 加载点对点透传消息
        /// </summary>
        LoadSysCustomMs,
        #endregion

        #region PreachAbout

        /// <summary>
        /// 请求创建帖子
        /// </summary>
        ReqCreatePreach,
        /// <summary>
        /// 请求帖子
        /// </summary>
        ReqPreach,
        /// <summary>
        /// 请求发布帖子
        /// </summary>
        ReqPublishPreach,
        /// <summary>
        /// 请求个人数据
        /// </summary>
        ReqPersonalData,
        /// <summary>
        /// 论道帖子交互(点赞，转发，评论，回复评论)
        /// </summary>
        PostInteraction,
        /// <summary>
        /// 加载名人栏
        /// </summary>
        LoadFamousUsers,
        /// <summary>
        /// 加载推荐栏数据
        /// </summary>
        LoadRecommendation,
        /// <summary>
        /// 屏蔽帖子的相关操作
        /// </summary>
        BlockPostOptions,

        #endregion

        #region Me About
        /// <summary>
        /// 退出登录
        /// </summary>
        LoginOut,
        /// <summary>
        /// 修改用户资料
        /// </summary>
        EditorAccountDataOption,
        /// <summary>
        /// 加载地区数据
        /// </summary>
        LoadLocalData,
        /// <summary>
        /// 加载粉丝和被关注者
        /// </summary>
        LoadFansAndFocus,
        /// <summary>
        /// 加载用户点赞过的帖子
        /// </summary>
        LoadLikesPostsByUser,
        /// <summary>
        /// 查询用户的阅读记录
        /// </summary>
        QueryUserReadRecord,
        /// <summary>
        /// 请求查询Or取消收藏
        /// </summary>
        ReqCollectOption,
        /// <summary>
        /// 查询群组成员读经状态
        /// </summary>
        LoadGroupMemberReadRecord,
        /// <summary>
        /// 请求创建举报
        /// </summary>
        ReqCreateReport,
        /// <summary>
        /// 请求创建反馈
        /// </summary>
        ReqCreateFeedBack,
        /// <summary>
        /// 查询所有群组成员读经记录
        /// </summary>
        LoadAllGroupReadRecord,
        /// <summary>
        /// 加载个人阅读排名
        /// </summary>
        LoadPersonalRank,
        /// <summary>
        /// 加载群组阅读排名
        /// </summary>
        LoadGroupsRank,
        /// <summary>
        /// 记载推送的数据
        /// </summary>
        LoadJPushNocition,
        /// <summary>
        /// 请求黑名单数据
        /// </summary>
        ReqBlackData,
        /// <summary>
        /// 请求设置黑名单
        /// </summary>
        ReqSetBlack,
        /// <summary>
        /// 请求查询最后一次阅读记录
        /// </summary>
        ReqQueryLastReadRecord,

        #endregion
    }
    
    public enum ViewEvent
    {
        #region Boot、Register、Login About
        /// <summary>
        /// 请求登陆验证码完毕
        /// </summary>
        ReqLoginVerifyCodeFinish,
        /// <summary>
        /// 请求绑定验证码完毕
        /// </summary>
        ReqBindVerifyCodeFinish,
        /// <summary>
        /// 登录失败
        /// </summary>
        ReqLoginFail,
        /// <summary>
        /// 登陆成功
        /// </summary>
        ReqLoginFinish,
        /// <summary>
        /// 绑定成功
        /// </summary>
        ReqBindPhoneFinish,
        /// <summary>
        /// 检查昵称结果
        /// </summary>
        ReqCheckNameExistFinish,
        /// <summary>
        /// 检查验证码失败
        /// </summary>
        ReqCheckVerfiyCodeFail,
        /// <summary>
        /// 好友列表失败
        /// </summary>
        ReqFriendsFail,
        /// <summary>
        /// 首次登陆设置头像完成
        /// </summary>
        ReqFirstSetHeadFinish,
        /// <summary>
        /// 首次登陆设置昵称完成
        /// </summary>
        ReqFirstSetDyNameFinish,
        /// <summary>
        /// 首次登陆设置失败
        /// </summary>
        ReqFirstSetFail,
        /// <summary>
        /// 登录IM失败
        /// </summary>
        ReqLoginImFail,
        #endregion

        #region ChatAbout
        /// <summary>
        /// 未读消息
        /// </summary>
        UnReadCount,
        /// <summary>
        /// 读取会话记录结束
        /// </summary>
        LoadSessionFinish,
        /// <summary>
        /// 读取聊天记录结束
        /// </summary>
        LoadChatRecordFinish,
        /// <summary>
        /// 查询一条消息记录结束
        /// </summary>
        LoadSingleChatRecordFinish,
        /// <summary>
        /// 读取所有我加入的群结束
        /// </summary>
        LoadMyAllGroupsFinish,
        /// <summary>
        /// 读取所有群结束
        /// </summary>
        LoadAllGroupsFinish,
        /// <summary>
        /// 根据Id查询群信息结束
        /// </summary>
        LoadGroupInfoByIdFinish,
        /// <summary>
        /// 搜索单个群组信息完成
        /// </summary>
        LoadSingleGroupFinish,
        /// <summary>
        /// 创建群组结束
        /// </summary>
        CreateGroupFinish,
        /// <summary>
        /// 邀请入群成功
        /// </summary>
        AddMemberSucc,
        /// <summary>
        /// 邀请入群失败
        /// </summary>
        AddMemberFail,
        /// <summary>
        /// 踢出成员成功
        /// </summary>
        KickMemberSucc,
        /// <summary>
        /// 踢出成员失败
        /// </summary>
        KickMemberFail,
        /// <summary>
        /// 有私新来了
        /// </summary>
        ReceiveImMsg,
        /// <summary>
        /// 系统消息来了
        /// </summary>
        ReceiveSysImMsg,
        /// <summary>
        /// 点对点透传系统点赞消息
        /// </summary>
        ReceiveSysCustomLikeImMsg,
        /// <summary>
        /// 点对点透传系统评论消息
        /// </summary>
        ReceiveSysCustomCommentImMsg,
        /// <summary>
        /// 发送即时消息成功
        /// </summary>
        SendImMsgSucc,
        /// <summary>
        /// 发送及时消息失败
        /// </summary>
        SendImMsgFail,
        /// <summary>
        /// 自动下载IM资源成功
        /// </summary>
        AutoDownloadImResSucc,
        /// <summary>
        /// 自动下载IM资源失败
        /// </summary>
        AutoDonwloadImResFail,
        /// <summary>
        /// 手动下载IM资源成功
        /// </summary>
        DownloadImMediaResSucc,
        /// <summary>
        /// 手动下载IM资源失败
        /// </summary>
        DownloadImMediaResFail,
        /// <summary>
        /// 打开群公告界面
        /// </summary>
        OpenAnnouncementView,
        /// <summary>
        /// 打开编辑群公告界面
        /// </summary>
        OpenAnnouncementEditorView,
        /// <summary>
        /// 查询单个群组信息结束
        /// </summary>
        LoadSingleTeamFinish,
        /// <summary>
        /// 群组公告更新完成
        /// </summary>
        UpdateAnnouncementFinish,
        /// <summary>
        /// 加载群组管理员列表完成
        /// </summary>
        LoadGroupManagersFinish,
        /// <summary>
        /// 打开群组管理员设置界面
        /// </summary>
        OpenSetGroupManagerView,
        /// <summary>
        /// 打开添加管理员界面
        /// </summary>
        OpenAddManagerView,
        /// <summary>
        /// 添加管理员完成
        /// </summary>
        AddGroupManagerFinish,
        /// <summary>
        /// 删除管理员完成
        /// </summary>
        RemoveManagerFinish,
        /// <summary>
        /// 打开群组资料修改界面
        /// </summary>
        OpenEditorGroupView,
        /// <summary>
        /// 修改群组资料完成
        /// </summary>
        UpdateTeamInfoFinish,
        /// <summary>
        /// 加载群组成员信息完成
        /// </summary>
        LoadGroupMembersFinish,
        /// <summary>
        /// 退出群组完成
        /// </summary>
        LeaveGroupFinish,
        /// <summary>
        /// 解散群组完成
        /// </summary>
        DismissGroupFinish,
        /// <summary>
        /// 转让群主完成
        /// </summary>
        TransferGroupFinish,
        /// <summary>
        /// 加载转让备选群组成员信息完成
        /// </summary>
        LoadGroupTransferMemersFinish,
        /// <summary>
        /// 读取系统消息结束
        /// </summary>
        LoadSystemMsFinish,
        /// <summary>
        /// 加载添加群成员好友信息完成
        /// </summary>
        LoadGroupSelectMmFinish,
        /// <summary>
        /// 删除会话记录完成
        /// </summary>
        ReqDeleteSessionFinish,
        /// <summary>
        /// 删除会话记录失败
        /// </summary>
        ReqDeleteSessionFail,
        /// <summary>
        /// 清空聊天记录完成
        /// </summary>
        ReqCleanRecordFinish,
        /// <summary>
        /// 清空聊天记录失败
        /// </summary>
        ReqCleanRecordFail,
        /// <summary>
        /// 请求查询用户信息完成
        /// </summary>
        ReqQueryUserInfoFinish,
        /// <summary>
        /// 请求查询用户信息失败
        /// </summary>
        ReqQueryUserInfoFail,
        /// <summary>
        /// 上传群组头像成功
        /// </summary>
        UpLoadGroupHeadImageFinish,
        /// <summary>
        /// 上传群组头像失败
        /// </summary>
        UpLoadGroupHeadImageFail,
        /// <summary>
        /// IM登录成功
        /// </summary>
        LoginImSucc,
        /// <summary>
        /// IM登录失败
        /// </summary>
        LoginImFail,
        /// <summary>
        /// 加载系统评论消息完成
        /// </summary>
        LoadSysCommentFinish,
        /// <summary>
        /// 加载系统点赞消息完成
        /// </summary>
        LoadSysLikeFinish,
        #endregion

        #region PreachAbout
        /// <summary>
        /// 请求创建帖子成功
        /// </summary>
        ReqCreatePreachSucc,
        /// <summary>
        /// 请求帖子成功
        /// </summary>
        ReqPreachSucc,
        /// <summary>
        /// 请求搜索的帖子成功
        /// </summary>
        ReqPreachSearchSucc,
        /// <summary>
        /// 请求帖子失败
        /// </summary>
        ReqPreachFail,
        /// <summary>
        /// 请求关注的帖子成功
        /// </summary>
        ReqFocusPreachSucc,
        /// <summary>
        /// 发布帖子成功
        /// </summary>
        ReqPublishPreachSucc,
        /// <summary>
        /// 转发帖子成功
        /// </summary>
        ReqForwardPreachSucc,
        /// <summary>
        /// 创建评论成功
        /// </summary>
        ReqCommentPreachSucc,
        /// <summary>
        /// 创建评论失败
        /// </summary>
        ReqCommentPreachFail,
        /// <summary>
        /// 创建点赞成功
        /// </summary>
        ReqLikePreachSucc,
        /// <summary>
        /// 请求回复评论成功
        /// </summary>
        ReqReplyCommentSucc,
        /// <summary>
        /// 请求回复评论失败
        /// </summary>
        ReqReplyCommentFail,
        /// <summary>
        /// 请求举报帖子成功
        /// </summary>
        ReqReportPostSucc,
        /// <summary>
        /// 请求指定帖子所有评论成功
        /// </summary>
        ReqAllCommentByPostSucc,
        /// <summary>
        /// 请求指定帖子的所有点赞成功
        /// </summary>
        ReqAllLikeByPostSucc,
        /// <summary>
        /// 请求指定用户的个人数据成功
        /// </summary>
        ReqDataByPosterSucc,
        /// <summary>
        /// 请求指定用户的个人数据失败
        /// </summary>
        ReqDataByPosterFail,
        /// <summary>
        /// 打开论道正文界面
        /// </summary>
        OpenPreachPostView,
        /// <summary>
        /// 打开论道评论编辑界面
        /// </summary>
        OpenPreachCommentView,
        /// <summary>
        /// 打开编辑回复评论界面
        /// </summary>
        OpenPreachReplyCommentView,
        /// <summary>
        /// 删除帖子成功
        /// </summary>
        DeletePostFinish,
        /// <summary>
        /// 删除帖子失败
        /// </summary>
        DeletePostFail,
        /// <summary>
        /// 加载名人栏完成
        /// </summary>
        LoadFamousUsersSucc,
        /// <summary>
        /// 加载推荐栏数据完成
        /// </summary>
        LoadRecommendationFinish,
        /// <summary>
        /// 屏蔽帖子完成
        /// </summary>
        BlockPostFinish,
        /// <summary>
        /// 取消屏蔽帖子完成
        /// </summary>
        DeleteBlockPostFinish,
        /// <summary>
        /// 加载被屏蔽帖子完成
        /// </summary>
        LoadBlockPostsFinish,
        #endregion

        #region BibleAbout
        /// <summary>
        /// 展示经文具体章节
        /// </summary>
        BibleShow,
        /// <summary>
        /// 圣经下一章
        /// </summary>
        NextChapter,
        /// <summary>
        /// 圣经上一章
        /// </summary>
        LastChapter,
        /// <summary>
        /// 读经结束记录
        /// </summary>
        UpDateBibleRecord,
        /// <summary>
        /// 跳转到书本详情页
        /// </summary>
        TurnToBookDetailView,
        /// <summary>
        /// 查询用户阅读详情完成
        /// </summary>
        QueryUserReadRecordDetailFinish,

        #endregion

        #region Me About
        /// <summary>
        /// 退出登录成功
        /// </summary>
        LoginOutSucc,
        /// <summary>
        /// 退出登录失败
        /// </summary>
        LoginOutFail,
        /// <summary>
        /// 用户资料修改完成
        /// </summary>
        EditorAccountDataOptionFinish,
        /// <summary>
        /// 修改用户资料的类型
        /// </summary>
        EditorAccountDataType,
        /// <summary>
        /// 加载地区数据完成
        /// </summary>
        LoadLocalDataFinish,
        /// <summary>
        /// 加载粉丝和被关注者完成
        /// </summary>
        LoadFansAndFocusFinish,
        /// <summary>
        /// 加载用户点赞过的帖子完成
        /// </summary>
        LoadLikesPostsByUserFinish,
        /// <summary>
        /// 查询用户阅读数量完成
        /// </summary>
        QueryUserReadRecordCoutFinish,
        /// <summary>
        /// 查询收藏数据完成
        /// </summary>
        LoadCollectsFinish,
        /// <summary>
        /// 取消收藏完成
        /// </summary>
        DeleteCollectFinish,
        /// <summary>
        /// 跳转到收藏界面
        /// </summary>
        TurnToCollect,
        /// <summary>
        /// 跳转到编辑出生日期界面
        /// </summary>
        TurnToBirthday,
        /// <summary>
        /// 查询群组成员读经状态完成
        /// </summary>
        LoadGroupMemberReadRecordFinish,
        /// <summary>
        /// 查询所有群组人员已经读经状态完成
        /// </summary>
        LoadAllGroupReadRecordSucc,
        /// <summary>
        /// 加载个人阅读排名完成
        /// </summary>
        LoadPersonalRankSucc,
        /// <summary>
        /// 查询单个用户阅读排名数据
        /// </summary>
        LoadSinglePersonalRankSucc,
        /// <summary>
        /// 加载群组阅读排名完成
        /// </summary>
        LoadGroupsRankSucc,
        /// <summary>
        /// 加载已加入的群组阅读排名完成
        /// </summary>
        LoadAllOwnGroupsRankSucc,
        /// <summary>
        /// 请求黑名单完成
        /// </summary>
        ReqBlackListSucc,
        /// <summary>
        /// 请求黑名单失败
        /// </summary>
        ReqBlackListFail,
        /// <summary>
        /// 请求设置黑名单完成
        /// </summary>
        ReqSetBlackSucc,
        /// <summary>
        /// 请求设置黑名单失败
        /// </summary>
        ReqSetBlackFail,
        /// <summary>
        /// 请求查询最后一次阅读记录完成
        /// </summary>
        ReqQueryLastReadRecordSucc,
        /// <summary>
        /// 请求查询最后一次阅读记录失败
        /// </summary>
        ReqQueryLastReadRecordFail,
        #endregion
    }
}
