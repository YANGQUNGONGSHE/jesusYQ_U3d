using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatMainFiller : MonoBehaviour, IContentFiller
{
    [HideInInspector]
    public ListScrollRect MScrollRect;

    public GameObject[] AllChatCellPrefabs;

    [HideInInspector]
    public List<ChatBaseModel> DataSource;

    private void OnDestroy()
    {
        Log.I("ChatMainFiller  销毁:" + DataSource.Count);
    }

    void Awake()
    {
        MScrollRect = GameObject.Find("ChatList").GetComponent<ListScrollRect>();
    }

    public int GetItemCount()
    {
        if (DataSource == null || DataSource.Count < 1)
            return 0;
        return DataSource.Count;
    }

    public int GetItemType(int index)
    {
        return (int)DataSource[index].ChatMsgType;
    }

    public GameObject GetListItem(int index, int itemType, GameObject obj)
    {
        if (obj == null)
        {
            obj = Instantiate(AllChatCellPrefabs[itemType]);
        }

        bool isShow = true;
        if (DataSource != null && DataSource.Count > 1)
        {
            if(index > 0)
                isShow = CommUtil.Instance.NeedToShowSendTime(DataSource[index].OriginalTime, DataSource[index - 1].OriginalTime);
        }

        switch (itemType)
        {
            case (int)ChatMsgType.Text:
                var textCell = obj.GetComponent<ChatTextCell>();
                if (DataSource != null)
                    textCell.InitUi(index, DataSource[index] as ChatTextModel, null, null, isShow);
                break;

            case (int)ChatMsgType.Image:
                var imageCell = obj.GetComponent<ChatImageCell>();
                if (DataSource != null)
                    imageCell.InitUi(index, DataSource[index] as ChatImageModel, null, null, isShow);
                break;

            case (int)ChatMsgType.Audio:
                var audioCell = obj.GetComponent<ChatAudioCell>();
                if (DataSource != null)
                    audioCell.InitUi(index, DataSource[index] as ChatAudioModel, null, null, isShow);
                break;

            case (int)ChatMsgType.Notification:
                var notifyCell = obj.GetComponent<ChatNotificationCell>();
                if (DataSource != null)
                    notifyCell.InitUi(index, DataSource[index] as ChatNotificationModel, null, null, isShow);
                break;
        }
        return obj;
    }

    public void Refresh()
    {
        MScrollRect.RefreshContent();
    }

    public void GoToBottom()
    {
        MScrollRect.GoToListItem(DataSource.Count - 1);
    }

    public void ScrollToBottom()
    {
        MScrollRect.ScrollToListItem(DataSource.Count - 1);
    }

    public void GoToTop(int index)
    {
        MScrollRect.GoToListItem(index);
    }
}
