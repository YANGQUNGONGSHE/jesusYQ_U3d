using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class LoadGroupSelectMemberCommand : EventCommand
{

    [Inject]
    public IImService ImService { get; set; }
    [Inject]
    public UserModel UserModel { get; set; }

    private ArgLoadGroupInfo _mGroupInfo;
    private List<GroupSelectMemberModel> _list;
    private bool _mIsGm = false;
    public override void Execute()
    {
        _mGroupInfo = evt.data as ArgLoadGroupInfo;
        if(_mGroupInfo==null)return;
        _list = new List<GroupSelectMemberModel>();
        var dict = UserModel.Friends;

        foreach (var followInfo in dict)
        {
            _mIsGm = false;
            foreach (var groupMeberInfo in _mGroupInfo.GroupMeberInfoModels)
            {
                if (followInfo.Value.Owner.Id == groupMeberInfo.Uid)
                    _mIsGm = true;
            }

            var model = new GroupSelectMemberModel()
            {
                IsGm = _mIsGm,
                Uid = followInfo.Value.Owner.Id,
                DisplayName = followInfo.Value.Owner.DisplayName,
                HeadIconUrl = followInfo.Value.Owner.AvatarUrl,
                Brief = followInfo.Value.Owner.Signature,
            };
            _list.Add(model);
        }

        dispatcher.Dispatch(CmdEvent.ViewEvent.LoadGroupSelectMmFinish,_list);

    }
}

public class GroupSelectMemberModel
{ 
    
    /// <summary>
    /// 是否已是群成员
    /// </summary>
    public bool IsGm { get; set; }
    /// <summary>
    /// 用户Id
    /// </summary>
    public int Uid { get; set; }
    /// <summary>
    /// 昵称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 个性签名
    /// </summary>
    public string Brief { get; set; }
    /// <summary>
    /// 头像地址
    /// </summary>
    public string HeadIconUrl { get; set; }
    /// <summary>
    /// 头像Texture2D
    /// </summary>
    public Texture2D HeadIconTexture2D { get; set; }
}
