using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChatGroupSettingView : iocView
{
    [SerializeField]
    private Text _mGroupId;
    public Text GroupId
    {
        get
        {
            return _mGroupId;
        }
    }

    [SerializeField]
    private Text _mAnnouncement;
    public Text Announcement
    {
        get
        {
            return _mAnnouncement;
        }
    }

    [SerializeField]
    private Transform _mMemberContanier;
    public Transform MemberContanier
    {
        get
        {
            return _mMemberContanier;
        }
    }

    [SerializeField]
    private Button _mMoreMemberButton;
    public Button MoreMemberButton
    {
        get
        {
            return _mMoreMemberButton;
        }
    }

    [SerializeField]
    private Button _mQuitGroupButton;
    public Button QuitGroupButton
    {
        get
        {
            return _mQuitGroupButton;
        }
    }

    [SerializeField]
    private Button _mBackButton;
    public Button BackButton
    {
        get
        {
            return _mBackButton;
        }
    }

    [HideInInspector] public Button AnnounceMentBut;
    [HideInInspector] public Button MgrBut;
    [HideInInspector] public Text GroupName;
    [HideInInspector] public Button MoreMemberBut;

    [HideInInspector] public RectTransform ToolBarRectTransform;
    [HideInInspector] public RectTransform CreaterTool;
    [HideInInspector] public Button LeaveBut;
    [HideInInspector] public Button Cancelbut;
    [HideInInspector] public Button TransferBut;
    [HideInInspector] public Button DismissBut;
    [HideInInspector] public Button CreateCancelBut;
    [HideInInspector] public Button CleanRecordBut;

    /// <summary>
    /// 成员item预设
    /// </summary>
    public GameObject MemberItemPrefab;
    
    /// <summary>
    /// 添加成员按钮
    /// </summary>
    public GameObject AddMemberItemButton;

    /// <summary>
    /// 成员列表父容器
    /// </summary>
    public Transform MemberItemParent;
   
    protected override void Awake()
    {
        base.Awake();
        AnnounceMentBut = transform.Find("ScrollView/Content/Cell-Announcement").GetComponent<Button>();
        MgrBut = transform.Find("ScrollView/Content/Cell-Mgr").GetComponent<Button>();
        GroupName = transform.Find("NavigationBar_3/Ttile").GetComponent<Text>();
        MoreMemberBut = transform.Find("ScrollView/Content/Cell-Member/MoreButton").GetComponent<Button>();

        ToolBarRectTransform = transform.Find("ToolBar").GetComponent<RectTransform>();
        CreaterTool = transform.Find("ToolBar/CreaterTool").GetComponent<RectTransform>();
        LeaveBut = transform.Find("ToolBar/NormalTool/QuitBut").GetComponent<Button>();
        Cancelbut = transform.Find("ToolBar/NormalTool/CancelBut").GetComponent<Button>();
        TransferBut = transform.Find("ToolBar/CreaterTool/TransferBut").GetComponent<Button>();
        DismissBut = transform.Find("ToolBar/CreaterTool/DismissBut").GetComponent<Button>();
        CreateCancelBut = transform.Find("ToolBar/CreaterTool/CancelBut").GetComponent<Button>();
        CleanRecordBut = transform.Find("ScrollView/Content/Cell-Clear").GetComponent<Button>();
    }

    public override int GetLayer()
    {
        return (int)UiLayer.Post;
    }

    public override int GetUiId()
    {
        return (int)UiId.ChatGroupSetting;
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

    public void SetMember(List<GroupMeberInfoModel> memberList)
    {
        int count = MemberItemParent.transform.childCount;
        if(count > 0)
        {
            for(var i = 0; i < count; i++)
            {
                var obj = MemberItemParent.transform.GetChild(i);
                GameObject.DestroyObject(obj.gameObject);
            }
        }
        
        if (memberList != null && memberList.Count > 0)
		{
            memberList.Sort((x, y) => y.Uid.CompareTo(x.Uid));
			int index = memberList.Count >= 9 ? 9 : memberList.Count;
			for (var i = 0; i < index; i++)
			{
                var obj = GameObject.Instantiate<GameObject>(MemberItemPrefab);
                obj.transform.SetParent(MemberItemParent,false);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                var memberItem = obj.transform.GetComponent<MemberItem>();
			    memberItem.SetUi(memberList[i].HeadIconTexture2D, memberList[i].Displayname);
			}
		    var btn = GameObject.Instantiate(AddMemberItemButton);
		    btn.transform.SetParent(MemberItemParent,false);
		    btn.transform.localPosition = Vector3.zero;
		    btn.transform.localScale = Vector3.one;
		}
    }
}
