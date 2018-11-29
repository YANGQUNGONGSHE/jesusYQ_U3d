using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;

public class UpdateView : View 
{
    private Text _mProgressText;
    public Text ProgressText{get{ return _mProgressText;}}

    private Slider _mProgress;
    public Slider Progress{get{ return _mProgress;}}

    private Image _mLoadingImage;
    public Image LoadingImage {get { return _mLoadingImage; }}

    private RectTransform _mErroRectTransform;
    public RectTransform ErroRectTransform { get { return _mErroRectTransform; } }

    private Button _mQuitAppBut;
    public Button QuitAppBut { get { return _mQuitAppBut;} }


    protected override void Awake()
    {
        base.Awake();
        _mProgress = transform.Find("LoadProgressbar").GetComponent<Slider>();
        _mProgressText = transform.Find("LoadProgressbar/Text").GetComponent<Text>();
        _mLoadingImage = transform.Find("Loading_1").GetComponent<Image>();
        _mErroRectTransform = transform.Find("LoginErrorAction").GetComponent<RectTransform>();
        _mQuitAppBut = transform.Find("LoginErrorAction/Image/ReLoginBut").GetComponent<Button>();
    }

    public void SetProgress(float value)
    {
        _mLoadingImage.gameObject.SetActive(true);
        _mProgress.gameObject.SetActive(true);
        _mProgress.value = Mathf.Clamp(value, 0, 1);
        _mProgressText.text = string.Format("更新中...{0}%", (int)(value * 100));
    }

    public void IsVisibleErrorAction(bool flag)
    {
        _mErroRectTransform.DOAnchorPosX(flag ? 0f : 720f, 0f);
    }
}
