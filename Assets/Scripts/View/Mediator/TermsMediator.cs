using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;

public class TermsMediator : EventMediator {

    [Inject]public TermsView TermsView { get; set; }
    [Inject]public UserModel UserModel { get; set; }
    private string _mWebKey = "terms";
    private bool _mIsCheckedWebViewClose = false;


    private void Update()
    {
        if (!_mIsCheckedWebViewClose)
        {
            if (!WebController.Instance.hasExist(_mWebKey))
            {
                _mIsCheckedWebViewClose = true;
                Back();
            }
        }
    }

    public override void OnRegister()
    {
       TermsView.TermBackBut.onClick.AddListener(Back);
       CreateView();
       TermsView.SetTermName(UserModel.TermPath.Equals("www/termservice.html") ? "服务条款" : "隐私条款");
    }

    private void CreateView()
    {
        var oUrl = UniWebViewHelper.StreamingAssetURLForPath(UserModel.TermPath);
        WebController.Instance.CreateWeb(_mWebKey,TermsView.WebRect,oUrl,false, (view, code, url) =>
        {
        });
    }

    private void Back()
    {
        WebController.Instance.CloseWeb(_mWebKey);
        iocViewManager.DestroyAndOpenNew(TermsView.GetUiId(),(int)UiId.Login);
    }

    public override void OnRemove()
    {
        TermsView.TermBackBut.onClick.RemoveListener(Back);
        _mWebKey = null;
        UserModel.TermPath = null;
    }
    private void OnDestroy()
    {
        OnRemove();
    }


}
