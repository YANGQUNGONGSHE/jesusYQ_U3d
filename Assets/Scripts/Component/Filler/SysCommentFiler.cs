using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class SysCommentFiler : BaseFiller<SysCustomCommentModel> {

    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("SysCommentRect").GetComponent<ListScrollRect>();
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);
        if(DataSource==null||DataSource.Count<1)return;
        obj.GetComponent<SysCommentCell>().InitUi(index,DataSource[index], (clickIndex, model) =>
        {
            if (OnCellClick != null)
                OnCellClick(clickIndex, model);
        });
    }
}
