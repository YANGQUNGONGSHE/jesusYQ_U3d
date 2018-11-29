using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class PreachSliderFiler : BaseFiller<RecommendationModel>
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
        obj.GetComponent<SliderCell>().InitUi(index,DataSource[index], (i, model) =>
        {
            if (OnCellClick != null)
                OnCellClick(i, model);
        });
    }
}
