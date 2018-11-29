using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.ListRectExtensions;

public class ChatSelectMemberCell : BaseCell<GroupSelectMemberModel>
{
	[SerializeField]
    private Text _mDisplayNameText;

    [SerializeField]
    private Text _mBriefText;

    [SerializeField]
    private CircleRawImage _mHeadIcon;

	[SerializeField]
	private Button _mSelectButon;

	[SerializeField]
	private Transform _mIcon_Selected;

    [SerializeField]
    private Transform _mSelecTransform;

	private bool _mIsSelected = false;
	
    protected override void OnAwake() 
    {
        _mSelectButon.onClick.AddListener(OnSelectBtnClick);
    }

	public override void InitUi(int index, GroupSelectMemberModel t, Action<int, GroupSelectMemberModel> onCellClickCallback = null, Action<int, GroupSelectMemberModel> onCellLongPressCallback = null,
        bool isSelected = false)
	{
		base.InitUi(index, t, onCellClickCallback, onCellLongPressCallback, isSelected);

		_mDisplayNameText.text = t.DisplayName;
        _mBriefText.text = t.Brief;

	    _mSelecTransform.gameObject.SetActive(!t.IsGm);
	    if (t.HeadIconTexture2D == null)
	    {
	        if (!string.IsNullOrEmpty(t.HeadIconUrl))
	        {
	            HttpManager.RequestImage(t.HeadIconUrl + LoadPicStyle.ThumbnailHead, (texture2D) =>
	            {
	                if (texture2D)
	                {
	                    _mHeadIcon.texture = texture2D;
	                    t.HeadIconTexture2D = texture2D;
	                }
	                else
	                {
	                    _mHeadIcon.texture = DefaultImage.Head;
                    }
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

	private void OnSelectBtnClick()
	{
		_mIsSelected = !_mIsSelected;
		_mIcon_Selected.gameObject.SetActive(_mIsSelected);
		NotificationCenter.DefaultCenter().PostNotification(NotifiyName.SelectedMember, this, new ArgSelectedMember(){
			IsSelected = _mIsSelected,
			MemberUid = t.Uid.ToString()
		});
	}
}
