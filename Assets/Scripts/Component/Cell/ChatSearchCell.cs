using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSearchCell :BaseCell<ChatSearchModel>
{
	[SerializeField]
    private CircleRawImage _mHeadIcon;

	[SerializeField]
    private Text _mDisplayNameText;

    [SerializeField]
    private Text _mBriefText;

	[SerializeField]
	private Text _ExtText;

    [SerializeField]
    private Button _mAddButton;

    [SerializeField]
    private Transform _mFollowedTransform;

    [SerializeField] private Text _mFollowedText;
    private bool _mIsSelected = false;


    public override void InitUi(int index, ChatSearchModel t, System.Action<int, ChatSearchModel> onCellClickCallback = null, System.Action<int, ChatSearchModel> onCellLongPressCallback = null, bool isSelected = false)
	{
		base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);
	    _mDisplayNameText.text = !string.IsNullOrEmpty(t.DisplayName) ? t.DisplayName : t.UserName;
        _mBriefText.text = t.Brief;
		_ExtText.text = t.Type == 0 ? "粉丝数: " + t.Ext : "成员数: " + t.Ext;
        _mAddButton.GetComponentInChildren<Text>().text = t.Type == 0 ? "添加关注" : "申请入群";
       
	    _mAddButton.gameObject.SetActive(!t.IsOwn);
        if (t.IsOwn)
	    {
	        _mFollowedTransform.gameObject.SetActive(true);
	        _mFollowedText.text = t.Type == 0 ? "已关注" : "已加入";
	    }
	    else
	    {
	        _mFollowedTransform.gameObject.SetActive(false);
        }
	    _mAddButton.onClick.AddListener(OnAddButtonClick);
	    if (t.HeadIconTexture2D == null)
	    {
	        if (!string.IsNullOrEmpty(t.HeadIconUrl))
	        {
	            HttpManager.RequestImage(t.HeadIconUrl + LoadPicStyle.ThumbnailHead, (texture2D) =>
	            {
	                if (texture2D != null)
	                {
	                    _mHeadIcon.texture = texture2D;
	                    t.HeadIconTexture2D = texture2D;
	                }
	                else
	                    _mHeadIcon.texture = DefaultImage.Head;
	            });
	        }
	        else
	        {
	            _mHeadIcon.texture = DefaultImage.Head;
	        }
        }
	    else
	    {
	        _mHeadIcon.texture = t.HeadIconTexture2D;
	    }
	}

    private void OnAddButtonClick()
    {
		if(t.Type == 0)
		{
		    //用户
		    _mIsSelected = !_mIsSelected;

		    NotificationCenter.DefaultCenter().PostNotification(NotifiyName.AddFocus, this, new ArgSelectedMember()
		    {
		        IsSelected = _mIsSelected,
		        MemberUid = t.Id
		    });
		    _mFollowedText.text = "已关注";
        }
		else
		{
            //组群
		    _mIsSelected = !_mIsSelected;
		   
		    NotificationCenter.DefaultCenter().PostNotification(NotifiyName.AddGroup, this, new ArgSelectedMember()
		    {
		        IsSelected = _mIsSelected,
		        MemberUid = t.Id
		    });
		    _mFollowedText.text = "已申请";
        }
        _mAddButton.gameObject.SetActive(false);
        _mFollowedTransform.gameObject.SetActive(true);
    }
}
