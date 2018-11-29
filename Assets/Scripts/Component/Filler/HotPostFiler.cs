﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class HotPostFiler : BaseFiller<PostModel>
{

    public Action<ClickType,int, PostModel> ClickTypeCallBack;

    protected override ListScrollRect GetListScrollRect()
    {
        return GameObject.Find("PostListScrollRect").GetComponent<ListScrollRect>();
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
       if(DataSource==null||DataSource.Count<=0)return;

       obj.GetComponent<HotPostCell>().InitUi(index,DataSource[index], 
           (clickIndex, model) =>{ OnCellClick(clickIndex, model);},
           (longPressIndex, model) =>{ OnCellLongPress(longPressIndex, model);});
        obj.GetComponent<HotPostCell>().TypeCallBack = TypeCallBack;
    }

    private void TypeCallBack(ClickType clickType,int index, PostModel postModel)
    {
        ClickTypeCallBack(clickType,index, postModel);
    }
}
