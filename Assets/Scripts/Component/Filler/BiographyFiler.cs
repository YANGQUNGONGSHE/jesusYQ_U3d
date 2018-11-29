using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class BiographyFiler : BaseFiller<Biography>
{

    public int IsSelected = -1;

    protected override void OnAwake()
    {
        DataSource = LocalDataManager.LoadSQLTable<Biography>(SQL.QUERT_BIOGRAPHY);
    }

    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("BiographyScrollRect").GetComponent<ListScrollRect>();
    }


    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        if(DataSource==null||DataSource.Count<1)return;

       
        obj.GetComponent<BiographyCell>().InitUi(index,DataSource[index], (clickIndex, biography) =>
        {
           
              OnCellClick(clickIndex, biography);
              IsSelected = clickIndex;
              Refresh();
        },null,  index == IsSelected);
    }
}
