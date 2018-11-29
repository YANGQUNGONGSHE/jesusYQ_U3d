using strange.extensions.context.api;
using UnityEngine;
using strange.extensions.context.impl;

public class MainMVCS : MVCSContext
{
    public MainMVCS(MonoBehaviour view, bool autoMapping) : base(view,autoMapping) { }

    protected override void mapBindings()
    {
        AccountModule();
        ChatModule();
        MeModule();
        PreachModule();
        BibleModle();
        commandBinder.Bind(CmdEvent.Command.BottomBarClick).To<BottomBarCommand>();
        commandBinder.Bind(ContextEvent.START).To<StartAppCommand>();
    }

    private void AccountModule()
    {
        //************************************ Injection绑定 *************************************************
        injectionBinder.Bind<UserModel>().To<UserModel>().ToSingleton();
        injectionBinder.Bind<IAccountService>().To<AccountService>().ToSingleton();
        injectionBinder.Bind<IFriendService>().To<FriendService>().ToSingleton();

        //************************************ mediation绑定 *************************************************
        mediationBinder.Bind<UpdateView>().To<UpdateMediator>();
        mediationBinder.Bind<LoginsView>().To<LoginsMediator>();
        mediationBinder.Bind<SetDyNameView>().To<SetDyNameMediator>();
        mediationBinder.Bind<TermsView>().To<TermsMediator>();
        mediationBinder.Bind<BlackView>().To<BlackMediator>();

        //************************************ Command绑定 *************************************************
        commandBinder.Bind(CmdEvent.Command.UpdateFinish).To<UpdateFinishCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqFriends).To<ReqFriendListCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqVerifyCode).To<ReqVerfiyCodeCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqLogin).To<ReqLoginsCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadAccountHeadT2D).To<LoadAccountHeadT2DCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqCreateFeedBack).To<ReqCreateFeedBackCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadPersonalRank).To<LoadPersonalRankCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupsRank).To<LoadGroupsRankCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqBlackData).To<ReqBlackListCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqSetBlack).To<ReqSetBlackCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqQueryLastReadRecord).To<ReqQueryLastReadRecordCommand>();
    }

    private void ChatModule()
    {
        //************************************ Injection绑定 *************************************************
        injectionBinder.Bind<IImService>().To<ImService>().ToSingleton();
        injectionBinder.Bind<IAudioService>().To<AudioService>().ToSingleton();
        injectionBinder.Bind<IGroupService>().To<GroupService>().ToSingleton();
        
        //************************************ mediation绑定 *************************************************
        mediationBinder.Bind<ChatSessionView>().To<ChatSessionMediator>();
        mediationBinder.Bind<ChatMainView>().To<ChatMainMediator>();
        mediationBinder.Bind<ChatFriendView>().To<ChatFriendMediator>();
        mediationBinder.Bind<ChatGroupView>().To<ChatGroupMediator>();
        mediationBinder.Bind<ChatEditorGroupView>().To<ChatEditorGroupMediator>();
        mediationBinder.Bind<ChatGroupSettingView>().To<ChatGroupSettingMediator>();
        mediationBinder.Bind<ChatGroupSelectMemberAddView>().To<ChatGroupSelectMemberMediator>();
        mediationBinder.Bind<ChatSearchView>().To<ChatSearchMediator>();
        mediationBinder.Bind<ChatGroupAnnouncemnetView>().To<ChatGroupAnnouncementMeidator>();
        mediationBinder.Bind<ChatGroupAnnEditorView>().To<ChatGroupAnnEditorMediator>();
        mediationBinder.Bind<ChatGroupManageView>().To<ChatGroupManageMediator>();
        mediationBinder.Bind<ChatSetManagerView>().To<ChatSetManagerMediator>();
        mediationBinder.Bind<ChatAddManagerView>().To<ChatAddManagerMeidator>();
        mediationBinder.Bind<ChatGroupMemberView>().To<ChatGroupMemberMediator>();
        mediationBinder.Bind<ChatGroupTransferMembersView>().To<ChatGroupTransferMembersMediator>();
        mediationBinder.Bind<ChatSystemMsView>().To<ChatSystemMsMediator>();
        mediationBinder.Bind<SysCutomLikeView>().To<SysCustomLikeMediator>();
        mediationBinder.Bind<SysCustomCommentView>().To<SysCustomCommentMediator>();

        //************************************ Command绑定 *************************************************
        commandBinder.Bind(CmdEvent.Command.LoadSession).To<LoadSessionCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadChatRecord).To<LoadChatRecordBySessionCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadSingleChatRecord).To<LoadChatRecordByMsgIdCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadMyAllGroups).To<LoadMyAllGroupsCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupOption).To<LoadGroupOptionCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupInfoById).To<LoadGroupInfoByTidCommand>();
        commandBinder.Bind(CmdEvent.Command.DownloadLostResChat).To<DownloadLostResChatCommand>();
        commandBinder.Bind(CmdEvent.Command.SendImMsg).To<SendImMsgCommand>();
        commandBinder.Bind(CmdEvent.Command.AudioOption).To<AudioOptionCommand>();
        commandBinder.Bind(CmdEvent.Command.GroupOption).To<GroupOptionCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupManagers).To<LoadGroupManagersCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupMembers).To<LoadGroupMembersCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupTransferMemers).To<LoadGroupMembersCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadSysMs).To<LoadSystemRecordCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupSelectMm).To<LoadGroupSelectMemberCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqDeleteSession).To<ReqDeleteSessionCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqQueryUserInfo).To<ReqQueryUserCommand>();
        commandBinder.Bind(CmdEvent.Command.FocusOptions).To<ReqFocusOptionCommand>();
        commandBinder.Bind(CmdEvent.Command.SetMessageStatus).To<MarkMessagesStatusCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadGroupMemberReadRecord).To<LoadGroupMemberReadRecordCommand>();
        commandBinder.Bind(CmdEvent.Command.ChatMainTurnPersonal).To<ReqQuerySingleUserCommand>();
        commandBinder.Bind(CmdEvent.Command.LoginIm).To<ReqLoginImCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadSysCustomMs).To<LoadSysCustomRecordCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadAllGroupReadRecord).To<LoadAllGroupReadRecordCommand>();
    }

    private void MeModule()
    {
        //************************************ Injection绑定 *************************************************
        injectionBinder.Bind<FriendsShipModel>().To<FriendsShipModel>().ToSingleton();

        //************************************ mediation绑定 *************************************************
        mediationBinder.Bind<MeView>().To<MeMediator>();
        mediationBinder.Bind<SetView>().To<SetMediator>();
        mediationBinder.Bind<EditorUserDataView>().To<EditorUserDataMediator>();
        mediationBinder.Bind<EditorNameOrSignatureView>().To<EditorNameOrSignatureMedaitor>();
        mediationBinder.Bind<LocalView>().To<LocalMediator>();
        mediationBinder.Bind<FansAndFocusView>().To<FansAndFocusMediator>();
        mediationBinder.Bind<AccountSafeView>().To<AccountSafeMediator>();
        mediationBinder.Bind<ChangeBindPhoneView>().To<ChangeBindPhoneMediator>();
        mediationBinder.Bind<MyLikePostsView>().To<MyLikePostsMediator>();
        mediationBinder.Bind<CollectView>().To<CollectMediator>();
        mediationBinder.Bind<EditorBirthdayView>().To<EditorBirthdayMediator>();
        mediationBinder.Bind<ReportView>().To<ReportViewMediator>();
        mediationBinder.Bind<FeedBackView>().To<FeedBackMediator>();

        //************************************ Command绑定 *************************************************

        commandBinder.Bind(CmdEvent.Command.LoginOut).To<ReqLoginOutCommand>();
        commandBinder.Bind(CmdEvent.Command.EditorAccountDataOption).To<EditorUserDataOptionCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadLocalData).To<ReqLocalDataCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadFansAndFocus).To<ReqFansAndFocusCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqBindPhone).To<ReqChangeBindPhoneCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadLikesPostsByUser).To<ReqLikesPostsByUserCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqCollectOption).To<ReqCollectOptionCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqCreateReport).To<ReqCreateReportCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadJPushNocition).To<LoadPushNocitionCommand>();
    }

    private void PreachModule()
    {
        //************************************ Injection绑定 ************************************
        injectionBinder.Bind<IPreachService>().To<PreachService>().ToSingleton();
        //************************************ mediation绑定 ******************************************
        mediationBinder.Bind<PreachView>().To<PreachMeidator>();
        mediationBinder.Bind<PreachPostView>().To<PreachPostMediator>();
        mediationBinder.Bind<PersonalView>().To<PersonalMediator>();
        mediationBinder.Bind<PreachOptionView>().To<PreachOptionMeidator>();
        mediationBinder.Bind<PreachEditorView>().To<PreachEditorMediator>();
        mediationBinder.Bind<PreachEditorVideoView>().To<PreachEditorVideoMediator>();
        mediationBinder.Bind<PreachSearchView>().To<PreachSearchMediator>();
        mediationBinder.Bind<PreachEditorCommentView>().To<PreachEditorCommantMediator>();
        mediationBinder.Bind<PreachCommentReplyView>().To<PreachCommentReplyMediator>();
        //************************************ Command绑定 *******************************************
        commandBinder.Bind(CmdEvent.Command.ReqPreach).To<ReqHotPreachCommand>();
        commandBinder.Bind(CmdEvent.Command.PostInteraction).To<ReqPostInteractionCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqPersonalData).To<ReqPersonalCommand>();
        commandBinder.Bind(CmdEvent.Command.ReqCreatePreach).To<ReqEditorPostCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadFamousUsers).To<ReqFamousUserCommand>();
        commandBinder.Bind(CmdEvent.Command.LoadRecommendation).To<ReqRecommendationCommand>();
        commandBinder.Bind(CmdEvent.Command.BlockPostOptions).To<ReqBlockPostOptionsCommand>();
    }

    private void BibleModle()
    {
        //************************************ mediation绑定 ******************************************
        mediationBinder.Bind<BibleView>().To<BibleMediator>();
        mediationBinder.Bind<BibleShowView>().To<BibleShowMediator>();
        mediationBinder.Bind<ClassicsView>().To<ClassicsMediator>();
        mediationBinder.Bind<BookDetailView>().To<BookDetailMediator>();
        //************************************ Command绑定 *******************************************
        commandBinder.Bind(CmdEvent.Command.QueryUserReadRecord).To<ReqReadRecordCommand>();
    }
}

