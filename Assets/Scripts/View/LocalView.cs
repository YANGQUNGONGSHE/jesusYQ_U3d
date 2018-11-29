using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WongJJ.Game.Core.StrangeExtensions;

public class LocalView : iocView
{


    [HideInInspector] public Button BackBut;
    [HideInInspector] public Text SelfLocal;
    [HideInInspector] public CountriesFiler CountriesFiler;

    protected override void Awake()
    {
        base.Awake();
        BackBut = transform.Find("NavigationBar/BackBut").GetComponent<Button>();
        SelfLocal = transform.Find("SelfLocal/LocalText").GetComponent<Text>();
        CountriesFiler = transform.Find("CountryFiler").GetComponent<CountriesFiler>();
    }


    public void SetUi(User user)
    {
        SelfLocal.text = user.Country + " " + user.State + " " + user.City;
    }
    public override int GetUiId()
    {
        return (int) UiId.Local;
    }

    public override int GetLayer()
    {
        return (int) UiLayer.Post;
    }
}
