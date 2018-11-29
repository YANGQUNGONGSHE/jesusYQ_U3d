using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class SysLikeFiler : BaseFiller<SysCustomLikeModel> {


    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("SysLikeRect").GetComponent<ListScrollRect>();
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);
        if(DataSource==null||DataSource.Count<1)return;

        obj.GetComponent<SysLikeCell>().InitUi(index,DataSource[index], (clickIndex, model) =>
        {
            if (OnCellClick != null)
                OnCellClick(clickIndex, model);
        } );
    }
}
