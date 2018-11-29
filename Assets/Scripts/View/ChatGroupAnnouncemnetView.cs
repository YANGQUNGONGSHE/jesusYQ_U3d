using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupAnnouncemnetView : iocView
{


    [HideInInspector] public CircleRawImage HeadImage;
    [HideInInspector] public Text DisPlayName;
    [HideInInspector] public Text PushTime;
    [HideInInspector] public Text AnnouncemenText;
    [HideInInspector] public Button UpdateBut;
    [HideInInspector] public GameObject UpdatGameobject;
    [HideInInspector] public Button BackBut;

    protected override void Awake()
    {
        base.Awake();
        HeadImage = transform.Find("AnnPublisherInfo/HeadImage").GetComponent<CircleRawImage>();
        DisPlayName = transform.Find("AnnPublisherInfo/DisplayName").GetComponent<Text>();
        PushTime = transform.Find("AnnPublisherInfo/Text").GetComponent<Text>();
        AnnouncemenText = transform.Find("AnnouncemnetContent/Text").GetComponent<Text>();
        UpdateBut = transform.Find("BottomBar").GetComponent<Button>();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        UpdatGameobject = transform.Find("BottomBar").gameObject;
    }






    public override int GetUiId()
    {
        return (int) UiId.ChatGroupAnnouncement;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
