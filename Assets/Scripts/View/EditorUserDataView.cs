using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class EditorUserDataView : iocView
{

    #region 界面控件属性
    /// <summary>
    /// 返回按钮
    /// </summary>
    [HideInInspector] public Button BackBut;
    /// <summary>
    /// 头像
    /// </summary>
    [HideInInspector] public CircleRawImage HeadImage;
    /// <summary>
    /// 用户昵称
    /// </summary>
    [HideInInspector] public Text DisplayName;
    /// <summary>
    /// 个性签名
    /// </summary>
    [HideInInspector] public Text Signature;
    /// <summary>
    /// 所在地
    /// </summary>
    [HideInInspector] public Text Local;
    /// <summary>
    /// 性别
    /// </summary>
    [HideInInspector] public Text Gender;
    /// <summary>
    /// 生日
    /// </summary>
    [HideInInspector] public Text Birthday;
    /// <summary>
    /// 换头像按钮
    /// </summary>
    [HideInInspector] public Button HeadBut;
    /// <summary>
    /// 修改昵称按钮
    /// </summary>
    [HideInInspector] public Button DisplayNameBut;
    /// <summary>
    /// 修改个性签名按钮
    /// </summary>
    [HideInInspector] public Button SignatureBut;
    /// <summary>
    /// 修改所在地按钮
    /// </summary>
    [HideInInspector] public Button LocalBut;
    /// <summary>
    /// 修改性别按钮
    /// </summary>
    [HideInInspector] public Button GenderBut;
    /// <summary>
    /// 修改出生日期按钮
    /// </summary>
    [HideInInspector] public Button BirthdayBut;
    /// <summary>
    /// 
    /// </summary>
    [HideInInspector] public RectTransform GenderTransform;

    [HideInInspector] public Toggle MenToggle;

    [HideInInspector] public Toggle WenToggle;

   

    

    #endregion

    protected override void Awake()
    {
        base.Awake();

        BackBut = transform.Find("NavigationBar/BackButton").GetComponent<Button>();
        HeadBut = transform.Find("HeadBg/HeadImage").GetComponent<Button>();
        DisplayNameBut = transform.Find("DisplayNameBg").GetComponent<Button>();
        SignatureBut = transform.Find("SignatureBg").GetComponent<Button>();
        LocalBut = transform.Find("LocalBg").GetComponent<Button>();
        GenderBut = transform.Find("GenderBg").GetComponent<Button>();
        BirthdayBut = transform.Find("BirthdyBg").GetComponent<Button>();

        DisplayName = transform.Find("DisplayNameBg/Name").GetComponent<Text>();
        Signature = transform.Find("SignatureBg/Signature").GetComponent<Text>();
        Local = transform.Find("LocalBg/LocalName").GetComponent<Text>();
        Gender = transform.Find("GenderBg/Gender").GetComponent<Text>();
        Birthday = transform.Find("BirthdyBg/BirthdyDate").GetComponent<Text>();
        HeadImage = transform.Find("HeadBg/HeadImage").GetComponent<CircleRawImage>();

        GenderTransform = transform.Find("GenderAction").GetComponent<RectTransform>();
        MenToggle = transform.Find("GenderAction/Bg/ManToggle").GetComponent<Toggle>();
        WenToggle = transform.Find("GenderAction/Bg/WenToggle").GetComponent<Toggle>();
    }


    /// <summary>
    /// 设置属性
    /// </summary>
    public void SetUi(User user)
    {
        if (user != null)
        {
            DisplayName.text = !string.IsNullOrEmpty(user.DisplayName) ? user.DisplayName : user.UserName;
            Signature.text = user.Signature;
            Local.text = user.Country +" "+user.State+" "+ user.City;
            Gender.text = user.Gender;
            Birthday.text = user.BirthDate;
            if (DefaultImage.ImHeadTexture2D == null)
            {
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                {
                    HttpManager.RequestImage(user.AvatarUrl + LoadPicStyle.ThumbnailHead, text2D =>
                    {
                        if (text2D != null)
                            HeadImage.texture = text2D;
                    });
                }
            }
            else
            {
                HeadImage.texture = DefaultImage.ImHeadTexture2D;
            }
        }

    }

    public void IsvisibleGenderBg(bool flag)
    {
        if (flag)
        {
            GenderTransform.DOAnchorPosY(0f, AnimationTime());
        }
        else
        {
            GenderTransform.DOAnchorPosY(-1280f, AnimationTime());
        }
    }
    public override int GetUiId()
    {
        return (int) UiId.EditorUserData;
    }
    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
