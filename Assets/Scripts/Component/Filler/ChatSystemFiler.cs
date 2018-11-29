using System.Collections;
using System.Collections.Generic;
using NIM.SysMessage;
using UnityEngine;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSystemFiler : MonoBehaviour, IContentFiller
{


    public GameObject[]  AllCellPrefabs;

    private ListScrollRect _mScrollRect;

    [HideInInspector]
    public List<SysTemModel> DataSource;

    private void Awake()
    {
        _mScrollRect = GameObject.Find("SysMsListScrollRect").GetComponent<ListScrollRect>();
    }


    public GameObject GetListItem(int index, int itemType, GameObject obj)
    {
        if (obj == null)
        {
            obj = Instantiate(AllCellPrefabs[itemType]);
        }
        bool isShow = true;

        if (DataSource != null && DataSource.Count > 1)
        {
            if (index > 0)
                isShow = CommUtil.Instance.NeedToShowSendTime(DataSource[index].Time, DataSource[index - 1].Time);
        }

        switch (itemType)
        {
            case (int)SystemType.ApplyGpOption:

                var applyGpCell = obj.GetComponent<ChatSysApplyGpCell>();
                if(DataSource!=null)
                    applyGpCell.InitUi(index,DataSource[index],null,null,isShow);

                    break;
            case (int)SystemType.FocusOption:

                var focusCell = obj.GetComponent<ChatSysAddFocusCell>();
                if (DataSource != null)
                    focusCell.InitUi(index,DataSource[index],null,null,isShow);

                    break;

        }
        return obj;
    }

    public int GetItemCount()
    {
        if (DataSource == null || DataSource.Count < 1)
            return 0;
        return DataSource.Count;
    }

    public int GetItemType(int index)
    {
        return (int)DataSource[index].Type;
    }


    public void Refresh()
    {
        _mScrollRect.RefreshContent();
    }


}
