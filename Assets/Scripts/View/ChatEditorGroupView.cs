using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatEditorGroupView : iocView
{
	[SerializeField]
	private CircleRawImage _mGroupImage;
	public CircleRawImage GroupImage
	{
		get{return _mGroupImage;}
	}

	[SerializeField]
	private InputField _mGroupName;
	public InputField GroupName
	{
		get{return _mGroupName;}
	}

	[SerializeField]
	private InputField _mGroupBrief;
	public InputField GroupBrief
	{
		get{return _mGroupBrief;}
	}
	
	[SerializeField]
	private Button _mCommitButton;
	public Button CommitButton
	{
		get
		{
			return _mCommitButton;
		}
	}

	[SerializeField]
	private Button _mBackButton;
	public Button BackButton
	{
		get{return _mBackButton;}
	}

    [SerializeField]
    private Button _mChioseHeadImageButton;

    public Button ChioseHeadImageButton
    {
        get { return _mChioseHeadImageButton; }
    }

    public override int GetLayer()
    {
        return (int)UiLayer.Post;
    }

    public override int GetUiId()
    {
        return (int)UiId.ChatEditorGroupView;
    }

	public override float AnimationTime()
    {
        return .25f;
    }

    public override void OnRender()
    {
        base.OnRender();
        RectTransform.DOAnchorPosX(0, AnimationTime());
    }

    public override void OnNoRender()
    {
        base.OnNoRender();
        RectTransform.DOAnchorPosX(Screen.width, AnimationTime());
    }

}
