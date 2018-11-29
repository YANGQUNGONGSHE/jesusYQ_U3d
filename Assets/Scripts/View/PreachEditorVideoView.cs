using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachEditorVideoView : iocView {

	[HideInInspector] public Button BackBut;
	[HideInInspector] public Button PublishBut;
	[HideInInspector] public RectTransform WebRect;

	protected override void Awake()
	{
		base.Awake();
		BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
		PublishBut = transform.Find("NavigationBar/PublishBut").GetComponent<Button>();
		WebRect = transform.Find("WebRect").GetComponent<RectTransform>();
	}

	public override int GetUiId()
	{
		return (int) UiId.PreachEditorVideo;
	}

	public override int GetLayer()
	{
		return (int) UiLayer.Post;
	}
}
