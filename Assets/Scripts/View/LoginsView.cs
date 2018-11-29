using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class LoginsView : iocView
{
    [HideInInspector] public InputField PhoneInputField;
    [HideInInspector] public InputField VerifyInputField;
    [HideInInspector] public Button SendVerifyCodeBut;
    [HideInInspector] public Button LoginBut;
    [HideInInspector] public Text VerifyTimeText;
    [HideInInspector] public Image SendVerifyBg;
    [HideInInspector] public RectTransform ErrorActionRectTransform;
    [HideInInspector] public Text ErrorTips;
    [HideInInspector] public Text ErrorText;
    [HideInInspector] public Button ReLoginBut;
    [HideInInspector] public Button TermServiceBut;
    [HideInInspector] public Button TermPricacyBut;



    protected override void Awake()
    {
        base.Awake();
        PhoneInputField = transform.Find("PhoneInput").GetComponent<InputField>();
        VerifyInputField = transform.Find("VerfyInput").GetComponent<InputField>();
        SendVerifyCodeBut = transform.Find("SendVerfyCodeBut").GetComponent<Button>();
        LoginBut = transform.Find("LoginBut").GetComponent<Button>();
        VerifyTimeText = transform.Find("SendVerfyCodeBut/Text").GetComponent<Text>();
        SendVerifyBg = transform.Find("SendVerfyCodeBut").GetComponent<Image>();
        ErrorActionRectTransform = transform.Find("LoginErrorAction").GetComponent<RectTransform>();
        ErrorTips = transform.Find("LoginErrorAction/Image/ErrTips").GetComponent<Text>();
        ErrorText = transform.Find("LoginErrorAction/Image/ErrorText").GetComponent<Text>();
        ReLoginBut = transform.Find("LoginErrorAction/Image/ReLoginBut").GetComponent<Button>();
        TermServiceBut = transform.Find("Terms/termServiceBut").GetComponent<Button>();
        TermPricacyBut = transform.Find("Terms/termPrivacyBut").GetComponent<Button>();
    }


    public IEnumerator BeginTiming()
    {
        SendVerifyCodeBut.interactable = false;
        VerifyTimeText.color = new Color(255/255f,110/255f,107/255f,255/255f);
        SendVerifyBg.color = new Color(255 / 255f, 110 / 255f, 107 / 255f, 255 / 255f);
        var timer = 60;
        while (timer > 1)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            VerifyTimeText.text = timer + "秒";
        }
        if (VerifyTimeText != null) VerifyTimeText.text = "获取验证码";
        if (VerifyTimeText != null) VerifyTimeText.color = new Color(180 / 255f, 180 / 255f, 180 / 255f, 255 / 255f);
        SendVerifyBg.color = new Color(180 / 255f, 180 / 255f, 180 / 255f, 255 / 255f);
        SendVerifyCodeBut.interactable = true;
    }
    public void IsVisibleErrorAction(bool flag)
    {
        ErrorActionRectTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
    public void SetErrorText(string errorTips,string errorText)
    {
        ErrorTips.text = errorTips;
        ErrorText.text = errorText;
    }
    public override int GetUiId()
    {
        return (int) UiId.Login;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
