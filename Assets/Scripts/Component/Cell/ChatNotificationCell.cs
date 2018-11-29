using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatNotificationCell : BaseCell<ChatNotificationModel> {

    [SerializeField]
    private Text _mMessageText;

    public override void InitUi(int index, ChatNotificationModel t, Action<int, ChatNotificationModel> onCellClickCallback = null,
        Action<int, ChatNotificationModel> onCellLongPressCallback = null, bool isSelected = false)
    {
        base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

       _mMessageText.text = t.SenderId +"  "+t.NotificationType+ "  " + t.ReceiverId;  
    }
}
