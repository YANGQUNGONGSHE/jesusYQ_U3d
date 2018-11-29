using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatGroupMemberFiler : BaseFiller<GroupMeberInfoModel>
{


    [HideInInspector] public bool IsShow = false;
     private bool _mIsManager = false;

    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("MemberListScrollRect").GetComponent<ListScrollRect>();
    }

    public void IsManage(bool isManager)
    {
        _mIsManager = isManager;
    }


    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        if (DataSource == null || DataSource.Count <= 0) return;

        obj.GetComponent<ChatGroupMemberCell>().InitUi(index, DataSource[index], (clickIndex, model) =>
        {
            if (OnCellClick != null)
                OnCellClick(clickIndex, model);

        }, (longPressIndex, model) =>
        {
            if (OnCellLongPress != null)
                OnCellLongPress(longPressIndex, model);
        }, IsShow);

        obj.GetComponent<ChatGroupMemberCell>().IsManager(_mIsManager);
    }
}
