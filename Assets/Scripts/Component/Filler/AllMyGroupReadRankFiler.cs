using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class AllMyGroupReadRankFiler : BaseFiller<RankGroupModel> {


    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("AllMyGroupScrollRect").GetComponent<ListScrollRect>();
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        if (DataSource == null || DataSource.Count < 1)return;

        obj.GetComponent<AllMyRankGroupCell>().InitUi(index,DataSource[index], (clickIndex, model) =>
        {
            if (OnCellClick != null)
            {
                OnCellClick(clickIndex, model);
            }
        } );
        
    }
}
