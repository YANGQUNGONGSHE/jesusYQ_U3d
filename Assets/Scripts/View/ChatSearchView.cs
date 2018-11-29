using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatSearchView : iocView
{
	[SerializeField]
    private ChatSearchFiller _mFiller;
    public ChatSearchFiller Filler
    {
        get { return _mFiller; }
    }

	[SerializeField]
    private Button _mBackButton;
    public Button BackButton
    {
        get { return _mBackButton; }
    }

	[SerializeField]
	private InputField _mSerachInput;
	public InputField SerachInput
	{
		get
		{
			return _mSerachInput;
		}
	}

	[SerializeField]
	private Toggle _mSearchGroup;
	public Toggle SearchGroup
	{
		get
		{
			return _mSearchGroup;
		}
	}

	[SerializeField]
	private Toggle _mSearchP2P;
	public Toggle SearchP2P
	{
		get
		{
			return _mSearchP2P;
		}
	}

	[SerializeField]
	private Text _mSearchGroupText;
	public Text SearchGroupText
	{
		get
		{
			return _mSearchGroupText;
		}
	}

	[SerializeField]
	private Text _mSearchP2PText;
	public Text SearchP2PText
	{
		get
		{
			return _mSearchP2PText;
		}
	}

	[SerializeField]
	private Transform _mBoxApplyToGroup;
	public Transform BoxApplyToGroup
	{
		get
		{
			return _mBoxApplyToGroup;
		}
	}

	[SerializeField]
	private RectTransform _mNoResultGo;
	public RectTransform NoResultGo
	{
		get
		{
			return _mNoResultGo;
		}
	}

	[SerializeField]
    private Button _mCancelAppyToGroupButton;
    public Button CancelAppyToGroupButton
    {
        get { return _mCancelAppyToGroupButton; }
    }

	[SerializeField]
    private Button _mSendAppyToGroupButton;
    public Button SendAppyToGroupButton
    {
        get { return _mSendAppyToGroupButton; }
    }

	[SerializeField]
	private InputField _mAppyToGroupInput;
	public InputField AppyToGroupInput
	{
		get
		{
			return _mAppyToGroupInput;
		}
	}



    public void IsVisibleNoResultGo(bool flag)
    {
        if (flag)
        {
            NoResultGo.DOAnchorPosX(0f, 0f);
        }
        else
        {
            NoResultGo.DOAnchorPosX(720f, 0f);
        }
    }

    public void IsVisibleScrollRect(bool flag)
    {
        if (flag)
        {
            Filler.ScrollView.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f);
        }
        else
        {
            Filler.ScrollView.GetComponent<RectTransform>().DOAnchorPosX(720f, 0f);
        }
    }
    public override int GetLayer()
    {
        return (int)UiLayer.Post;
    }

    public override int GetUiId()
    {
        return (int)UiId.ChatSerach;
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

	public void ChangeToggleTextColor(int toggle)
	{
		if(toggle == 0)
		{
			_mSearchGroupText.color = new Color(225/255f,225/255f,225/255f);
			_mSearchP2PText.color = new Color(253/255f,79/255f,78/255f);
		}
		else
		{
			_mSearchP2PText.color = new Color(225/255f,225/255f,225/255f);
			_mSearchGroupText.color = new Color(253/255f,79/255f,78/255f);
		}
	}

	public void AlertBoxApplyToGroupBox(bool flag)
	{
		if(flag)
		{
			_mBoxApplyToGroup.DOScale(Vector3.one, .25f);
		}
		else
		{
			_mBoxApplyToGroup.DOScale(Vector3.zero, .25f);
		}
	}
}
