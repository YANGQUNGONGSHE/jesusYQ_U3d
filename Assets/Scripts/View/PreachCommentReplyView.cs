using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class PreachCommentReplyView : iocView {



    [HideInInspector] public Button BackBut;
    [HideInInspector] public Button SendBut;
    [HideInInspector] public InputField InputField;
    [HideInInspector] public Text SendBuText;
    [HideInInspector] public RectTransform WebRect;
    protected override void Awake()
    {
        base.Awake();

        BackBut = transform.Find("NavigationBar/BackButton").GetComponent<Button>();
        SendBut = transform.Find("NavigationBar/OptionButton").GetComponent<Button>();
        InputField = transform.Find("InputField").GetComponent<InputField>();
        SendBuText = transform.Find("NavigationBar/OptionButton/Text").GetComponent<Text>();
        WebRect = transform.Find("WebRect").GetComponent<RectTransform>();
    }


    public void IsSendinteractable(bool flag)
    {
        if (flag)
        {
            SendBut.interactable = true;
            SendBuText.color = new Color(225f / 255f, 110f / 255f, 107f / 255f, 255f / 255f);
        }
        else
        {
            SendBut.interactable = false;
            SendBuText.color = new Color(220f / 255f, 220f / 255f, 220f / 255f, 255f / 255f);
        }
    }
    public override int GetUiId()
    {

        return (int) UiId.PreachReplyComment;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
