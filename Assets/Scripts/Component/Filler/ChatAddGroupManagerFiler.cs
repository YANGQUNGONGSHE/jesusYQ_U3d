using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatAddGroupManagerFiler : BaseFiller<GroupMeberInfoModel> {




    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("AddManagerListScrollRect").GetComponent<ListScrollRect>();
    }


    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        if (DataSource == null || DataSource.Count <= 0) return;

        obj.GetComponent<ChatAddGroupManagerCell>().InitUi(index, DataSource[index], (clickIndex, model) =>
        {
            if (OnCellClick != null)
                OnCellClick(clickIndex, model);

        }, (longPressIndex, model) =>
        {
            if (OnCellLongPress != null)
                OnCellLongPress(longPressIndex, model);
        });
    }
}
