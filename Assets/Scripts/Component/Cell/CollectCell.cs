using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class CollectCell : BaseCell<CollectModel>
{

     public Text Title;
     public Text Content;
     public Button CancelCollectBut;

    protected override void OnAwake()
    {
        CancelCollectBut.onClick.AddListener(CancelCollectClick);
    }

    public override void InitUi(int index, CollectModel t, Action<int, CollectModel> onCellClickCallback = null, Action<int, CollectModel> onCellLongPressCallback = null,
        bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
        Title.text = t.ParentId;
        Content.text = t.ParentTitle;
    }
    private void CancelCollectClick()
    {
        NotificationCenter.DefaultCenter().PostNotification(NotifiyName.DeleteCollect, this,new ArgSelectedMember()
        {
            MemberUid = t.ParentId
        });
    }
}
