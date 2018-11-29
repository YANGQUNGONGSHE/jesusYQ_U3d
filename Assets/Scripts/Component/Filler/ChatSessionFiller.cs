using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSessionFiller : BaseFiller<ChatSessionCellModel>
{
    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("SessionList").GetComponent<ListScrollRect>();
    }
    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        if (DataSource == null || DataSource.Count <= 0)
        {
            return;
        }
        else
        {
            obj.GetComponent<ChatSessionCell>().InitUi(index, DataSource[index], (clickIndex,model) =>
                {
                    if (OnCellClick != null)
                        OnCellClick(clickIndex,model);

                },
                (longPressIndex, model) =>
                {
                    if (OnCellLongPress != null)
                        OnCellLongPress(longPressIndex,model);
                });
        }
    }
}
