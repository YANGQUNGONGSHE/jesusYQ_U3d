using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class FansAndFocusFiler : BaseFiller<FansAndFocusModel> {



    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("FansAndFocusScrollRect").GetComponent<ListScrollRect>();
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);

        if (DataSource != null && DataSource.Count > 0)
        {
            obj.transform.GetComponent<FansAndFocusCell>().InitUi(index,DataSource[index], (clickIndex, model) =>
            {
                if (OnCellClick != null)
                    OnCellClick(clickIndex, model);
            } );
        }
    }
}
