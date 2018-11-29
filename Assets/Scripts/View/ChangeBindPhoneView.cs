using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class ChangeBindPhoneView : iocView
{



    [SerializeField]
    private Sprite[] _mFlagImages;

    [HideInInspector] public Button BackBut;
    [HideInInspector] public InputField PhoneInputField;
    [HideInInspector] public InputField VeryCodeInputField;
    [HideInInspector] public Button SendCodeBut;
    [HideInInspector] public Button BindBut;
    [HideInInspector] public Text VerifyTimeText;
    [HideInInspector] public Image ErrImage;


    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        PhoneInputField = transform.Find("InputPhoneNum").GetComponent<InputField>();
        VeryCodeInputField = transform.Find("InputVeryCode").GetComponent<InputField>();
        SendCodeBut = transform.Find("SendVeryButton").GetComponent<Button>();
        BindBut = transform.Find("BindBut").GetComponent<Button>();
        VerifyTimeText = transform.Find("SendVeryButton/Text").GetComponent<Text>();
        ErrImage = transform.Find("InputPhoneNum/ErrIcon").GetComponent<Image>();
    }

    public IEnumerator BeginTiming()
    {
        SendCodeBut.interactable = false;
        var timer = 60;
        while (timer > 1)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            VerifyTimeText.text = timer + "秒";
        }
        VerifyTimeText.text = "获取验证码";
        SendCodeBut.interactable = true;
    }


    public void ShowCheckPhoneFlag(bool passed)
    {
        ErrImage.sprite = _mFlagImages[passed ? 1 : 0];
        SendCodeBut.interactable = passed;
    }

    public void ShowBindBtnEnable(bool flag)
    {
        BindBut.interactable = flag;
    }

    public override int GetUiId()
    {
        return (int) UiId.ChangeBindPhone;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
