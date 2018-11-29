using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class AccountBookRecordFiler : BaseFiller<BookModel>
{

    public ListScrollRect ScrollRect;

    protected override ListScrollRect GetListScrollRect()
    {
        return ScrollRect;
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);

        if(DataSource==null||DataSource.Count<1)return;
        obj.GetComponent<AccountBookRecordCell>().InitUi(index,DataSource[index], (clickIndex, model) =>
        {
            if (OnCellClick != null)
                OnCellClick(clickIndex, model);
        } );
    }
}
