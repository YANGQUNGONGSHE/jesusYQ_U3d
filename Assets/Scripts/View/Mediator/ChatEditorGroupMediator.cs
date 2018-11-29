using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NIM.Session;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;
using ImageAndVideoPicker;

public class ChatEditorGroupMediator : EventMediator
{
    [Inject]
    public ChatEditorGroupView ChatEditorGroupView{get;set;}
    [Inject]
    public UserModel UserModel { get; set; }

    private NIMTeamInfo _mTeamInfo;

    private GroupAndManagers _groupAndManagers;
    private string _mHeadImagePath;
    private string _mTeamId;
    private string _mPickImagePath;
    private int _mWidth;
    private int _mHeight;

    public override void OnRegister()
    {
        ChatEditorGroupView.CommitButton.onClick.AddListener(OnCommitBtnClick);
        ChatEditorGroupView.BackButton.onClick.AddListener(OnBackBtnClick);
        ChatEditorGroupView.ChioseHeadImageButton.onClick.AddListener(ChioseHeadClick);
        ChatEditorGroupView.GroupBrief.onValueChanged.AddListener(GroupBriefListener);
        ChatEditorGroupView.GroupName.onValueChanged.AddListener(GroupNameListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.OpenEditorGroupView,GroupDataCallBack);
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadSingleTeamFinish,QuerySingleTeamInfoListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpLoadGroupHeadImageFinish,UpLoadGroupHeadImageFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.UpLoadGroupHeadImageFail,UpLoadGroupHeadImageFail);
        dispatcher.AddListener(CmdEvent.ViewEvent.CreateGroupFinish,CreateGroupFinish);


        PickerEventListener.onImageSelect += OnPickImageSelect;
        PickerEventListener.onImageLoad += OnPickImageLoad;
        PickerEventListener.onError += OnPickImagePickerError;
        PickerEventListener.onCancel += OnPickImagePickerCancel;
    }

    private void GroupNameListener(string arg0)
    {
        if (arg0.Length > 10)
        {
            UIUtil.Instance.ShowFailToast("名称最多十个字");
        }
    }

    private void GroupBriefListener(string arg0)
    {
        if (arg0.Length > 40)
        {
            UIUtil.Instance.ShowFailToast("简介最多四十个字");
        }
    }

    private void CreateGroupFinish(IEvent eEvent)
    {
        _mTeamId = (string)eEvent.data;
        Dispatcher.InvokeAsync(UploadHead, _mTeamId, _mPickImagePath, _mWidth, _mHeight);
    }
    private void UpLoadGroupHeadImageFail(IEvent eEvent)
    {
        UIUtil.Instance.ShowFailToast((string)eEvent.data);
        UIUtil.Instance.CloseWaiting();
        if (UserModel.EditorGroupType == EditorGroupType.CreateGroup)
        {
            iocViewManager.DestroyAndOpenNew(ChatEditorGroupView.GetUiId(),(int)UiId.ChatGroup);
        }
    }
    private void UpLoadGroupHeadImageFinish(IEvent eEvent)
    {
        _mHeadImagePath = (string)eEvent.data;
        UIUtil.Instance.CloseWaiting();
        if (UserModel.EditorGroupType == EditorGroupType.CreateGroup)
        {
            dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
            {
                Option = EGroupOption.UpdateTeamData,
                GroupId = _mTeamId,
                TeamInfo = new NIMTeamInfo() { TeamIcon = _mHeadImagePath }
            });
            iocViewManager.DestroyAndOpenNew(ChatEditorGroupView.GetUiId(),(int)UiId.ChatGroup);
        }
    }

    private void ChioseHeadClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
                        AndroidPicker.BrowseImage(false);
#elif UNITY_IPHONE && !UNITY_EDITOR
                	    IOSPicker.BrowseImage(false);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
       return;
#endif
    }

    private void QuerySingleTeamInfoListener(IEvent eEvent)
    {
        _mTeamInfo = eEvent.data as NIMTeamInfo;
        if (_mTeamInfo != null)
        Dispatcher.InvokeAsync(SetUi);
    }

    private void GroupDataCallBack(IEvent eEvent)
    {
        _groupAndManagers = eEvent.data as GroupAndManagers;
        if(_groupAndManagers==null) return;
        dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
        {
            Option = EGroupOption.QuerySingleTeam,
            GroupId = _groupAndManagers.ArgLoadGroupInfo.Tid,
        });
    }

    private void SetUi()
    {
        ChatEditorGroupView.GroupName.text = _mTeamInfo.Name;
        ChatEditorGroupView.GroupBrief.text = _mTeamInfo.Introduce;
        if(_mTeamInfo.TeamIcon!=null)
        HttpManager.RequestImage(_mTeamInfo.TeamIcon+LoadPicStyle.Thumbnail, texture2D =>
        {
            if (texture2D != null)
            {
                ChatEditorGroupView.GroupImage.texture = texture2D;
            }
        } );
    }

    public override void OnRemove()
    {
        ChatEditorGroupView.CommitButton.onClick.RemoveListener(OnCommitBtnClick);
        ChatEditorGroupView.ChioseHeadImageButton.onClick.RemoveListener(ChioseHeadClick);
        ChatEditorGroupView.GroupBrief.onValueChanged.RemoveListener(GroupBriefListener);
        ChatEditorGroupView.GroupName.onValueChanged.RemoveListener(GroupNameListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.OpenEditorGroupView, GroupDataCallBack);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadSingleTeamFinish, QuerySingleTeamInfoListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.UpLoadGroupHeadImageFinish, UpLoadGroupHeadImageFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.UpLoadGroupHeadImageFail, UpLoadGroupHeadImageFail);


        PickerEventListener.onImageSelect -= OnPickImageSelect;
        PickerEventListener.onImageLoad -= OnPickImageLoad;
        PickerEventListener.onError -= OnPickImagePickerError;
        PickerEventListener.onCancel -= OnPickImagePickerCancel;
    }

    private void OnDestroy()
    {
        OnRemove();
    }

    private void OnCommitBtnClick()
    {
        switch (UserModel.EditorGroupType)
        {
            case EditorGroupType.UpdateGroup:

                if (string.IsNullOrEmpty(ChatEditorGroupView.GroupName.text) ||
                    string.IsNullOrEmpty(ChatEditorGroupView.GroupBrief.text))
                {
                    UIUtil.Instance.ShowFailToast("请输入修改内容");
                    return;
                }

                _mTeamInfo.Name = ChatEditorGroupView.GroupName.text;
                _mTeamInfo.Introduce = ChatEditorGroupView.GroupBrief.text;
                if (!string.IsNullOrEmpty(_mHeadImagePath))
                {
                    _mTeamInfo.TeamIcon = _mHeadImagePath;
                }
                StartCoroutine(UpdateGroupData());
                break;

            case EditorGroupType.CreateGroup:

                if (string.IsNullOrEmpty(ChatEditorGroupView.GroupName.text) || string.IsNullOrEmpty(ChatEditorGroupView.GroupBrief.text)|| string.IsNullOrEmpty(_mPickImagePath))
                {
                    UIUtil.Instance.ShowFailToast("请填写完善!");
                    return;
                }
                string name = ChatEditorGroupView.GroupName.text;
                string introduce = ChatEditorGroupView.GroupBrief.text;

                dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam() { Option = EGroupOption.Create, GroupName = name, GroupIntroduce = introduce });
                break;
        }
    }

    IEnumerator UpdateGroupData()
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(CmdEvent.Command.GroupOption, new ArgGroupOptionParam()
        {
            Option = EGroupOption.UpdateTeamData,
            GroupId = _mTeamInfo.TeamId,
            TeamInfo = _mTeamInfo
        });
        _groupAndManagers = null;
        iocViewManager.DestroyAndOpenNew(ChatEditorGroupView.GetUiId(),(int)UiId.ChatGroupManage);
    }
    private void OnBackBtnClick()
    {
        if (_groupAndManagers != null)
        {
            iocViewManager.DestroyAndOpenNew(ChatEditorGroupView.GetUiId(),(int)UiId.ChatGroupManage);
            _groupAndManagers = null;
            
        }
        else
        {
            iocViewManager.DestroyAndOpenNew(ChatEditorGroupView.GetUiId(),(int)UiId.ChatGroup);
        }
    }
    #region 图片选择器回调

    private void OnPickImageSelect(string imgPath, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("Image Location : " + imgPath + " Image Orientation" + imgOrientation);
    }

    private void OnPickImageLoad(string imgPath, Texture2D tex, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("准备发送OnImageLoad ： Image Location : " + imgPath);
        if (UserModel.EditorGroupType == EditorGroupType.UpdateGroup)
        {
            UploadHead(_mTeamInfo.TeamId, imgPath, tex.width, tex.height);
        }
        _mPickImagePath = imgPath;
        _mWidth = tex.width;
        _mHeight = tex.height;
        ChatEditorGroupView.GroupImage.texture = tex;
    }

    private void OnPickImagePickerError(string errorMsg)
    {
        Log.I("Error : " + errorMsg);
    }

    private void OnPickImagePickerCancel()
    {
        Log.I("Cancel by user");
    }


    #endregion

    private byte[] LoadImageFromLocalPath(string path)
    {
        if (System.IO.File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            return bytes;
        }
        return null;
    }

    private void UploadHead(string teamId,string path, int width, int height)
    {
        var hedadTexture2D = new Texture2D(width, height);
        hedadTexture2D.LoadImage(LoadImageFromLocalPath(path));
        dispatcher.Dispatch(CmdEvent.Command.GroupOption,new ArgGroupOptionParam()
        {
            Option = EGroupOption.UploadGroupHeadImage,
            GroupId = teamId,
            GroupTexture2D = hedadTexture2D
        });
        UIUtil.Instance.ShowWaiting();
    }
}