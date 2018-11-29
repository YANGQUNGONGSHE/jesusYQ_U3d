﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class PreachSearchFiler : BaseFiller<PostModel>
{
    public ListScrollRect ScrollRect;
    public Action<ClickType,int, PostModel> ClickTypeCallBack;

    protected override ListScrollRect GetListScrollRect()
    {
        return ScrollRect;
    }

    protected override void OnGetListItem(int index, int itemType, GameObject obj)
    {
        base.OnGetListItem(index, itemType, obj);
        if (DataSource == null || DataSource.Count <= 0) return;

        obj.GetComponent<PrivatePostCell>().InitUi(index, DataSource[index],
            (clickIndex, model) => { OnCellClick(clickIndex, model); });
        obj.GetComponent<PrivatePostCell>().TypeCallBack = TypeCallBack;
    }

    private void TypeCallBack(ClickType clickType,int index, PostModel postModel)
    {
        ClickTypeCallBack(clickType, index, postModel);
    }
}
