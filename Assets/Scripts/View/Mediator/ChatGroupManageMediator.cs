using System;
using System.Collections;
using System.Collections.Generic;
using NIM.Team;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupManageMediator : EventMediator {

    [Inject]
    public ChatGroupManageView ChatGroupManageView { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }

    private GroupAndManagers _mGroupAndManagers;

    public override void OnRegister()
    {
        dispatcher.AddListener(CmdEvent.ViewEvent.LoadGroupManagersFinish, GroupDataCallBack);
        ChatGroupManageView.BackBut.onClick.AddListener(BackClick);
        ChatGroupManageView.SetManagerBut.onClick.AddListener(SetManagerClick);
        ChatGroupManageView.EditorDataBut.onClick.AddListener(EditorGroupDataClick);


    }

    private void EditorGroupDataClick()
    {
        Log.I("编辑群组资料事件！！！");
        UserModel.EditorGroupType = EditorGroupType.UpdateGroup;
        iocViewManager.CloseCurrentOpenNew((int)UiId.ChatEditorGroupView);
        StartCoroutine(GroupDataToEditorView());
    }

    IEnumerator GroupDataToEditorView()
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(CmdEvent.ViewEvent.OpenEditorGroupView, _mGroupAndManagers);

    }

    private void SetManagerClick()
    {
        Log.I("设置管理员事件！！！");
        if (UserModel.User.Id == _mGroupAndManagers.CreaterId)
        {
            iocViewManager.CloseCurrentOpenNew((int)UiId.ChatGroupSetManager);
            _mGroupAndManagers.ArgLoadGroupInfo.IsUpdateGroup = false;
            StartCoroutine(SendInfo());
        }
        else
        {
            UIUtil.Instance.ShowTextToast("没有该权限");
        }
    }

    IEnumerator SendInfo()
    {
        yield return new WaitForSeconds(0.1f);
        dispatcher.Dispatch(CmdEvent.ViewEvent.OpenSetGroupManagerView, _mGroupAndManagers);
    }

    private void BackClick()
    {
        iocViewManager.DestroyAndOpenNew(ChatGroupManageView.GetUiId(),(int)UiId.ChatGroupSetting);
    }

    private void GroupDataCallBack(IEvent eEvent)
    {
        _mGroupAndManagers = eEvent.data as GroupAndManagers;
        if(_mGroupAndManagers==null) return;
        SetUi();
        if (_mGroupAndManagers.ArgLoadGroupInfo.IsUpdateGroup == true)
        {
            dispatcher.Dispatch(CmdEvent.ViewEvent.OpenSetGroupManagerView, _mGroupAndManagers);
        }
    }

    private void SetUi()
    {
       
        ChatGroupManageView.GroupMembersText.text = _mGroupAndManagers.ManagersList.Count+"/5";
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.LoadGroupManagersFinish, GroupDataCallBack);
        ChatGroupManageView.SetManagerBut.onClick.RemoveListener(SetManagerClick);
        ChatGroupManageView.BackBut.onClick.RemoveListener(BackClick);
    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
