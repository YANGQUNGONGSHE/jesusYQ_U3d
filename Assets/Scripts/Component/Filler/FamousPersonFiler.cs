using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class FamousPersonFiler : BaseFiller<FamousPersonModel>
{

    public ListScrollRect ScrollRect;
    protected override ListScrollRect GetListScrollRect()
    {
        return ScrollRect;
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);

        if(DataSource==null||DataSource.Count<=0) return;

        obj.GetComponent<FamousPersonCell>().InitUi(index,DataSource[index],OnCellClickCallback);
    }
    private void OnCellClickCallback(int i, FamousPersonModel famousPersonModel)
    {
        if (OnCellClick != null)
            OnCellClick(i, famousPersonModel);
    }
}
