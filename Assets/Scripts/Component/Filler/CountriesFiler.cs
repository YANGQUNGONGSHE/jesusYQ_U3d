using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class CountriesFiler : BaseFiller<LocalModel>{



    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("LocalScrollList").GetComponent<ListScrollRect>();
    }


    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);

        if(DataSource==null||DataSource.Count<1)return;

        obj.GetComponent<LocalCell>().InitUi(index,DataSource[index], (i, info) =>
        {
            OnCellClick(i, info);
        } );
    }
}
