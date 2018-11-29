using System.Collections;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupSelectMemberAddView : iocView 
{
    [SerializeField]
    private ChatSelectMemberFiller _mFiller;
    public ChatSelectMemberFiller Filler
    {
        get { return _mFiller; }
    }

    [SerializeField]
    private Button _mAddButton;
    public Button AddButton
    {
        get { return _mAddButton; }
    }

    [SerializeField]
    private Button _mBackButton;
    public Button BackButton
    {
        get { return _mBackButton; }
    }

	public override int GetLayer()
    {
        return (int)UiLayer.Post;
    }

    public override int GetUiId()
    {
        return (int)UiId.ChatGroupSelectMemberAdd;
    }

	public override float AnimationTime()
    {
        return .25f;
    }

}
