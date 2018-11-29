using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSearchFiller : BaseFiller<ChatSearchModel>
{
    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("SearchList").GetComponent<ListScrollRect>();
    }

	protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        if (DataSource == null || DataSource.Count <= 0)
        {
            return;
        }
        else
        {
            obj.GetComponent<ChatSearchCell>().InitUi(index, DataSource[index], (clickIndex, model) =>
            {
                if (OnCellClick != null)
                    OnCellClick(clickIndex, model);
            },
            (longPressIndex, model) =>
            {
                if (OnCellLongPress != null)
                    OnCellLongPress(longPressIndex, model);
            });
        }
    }
}
