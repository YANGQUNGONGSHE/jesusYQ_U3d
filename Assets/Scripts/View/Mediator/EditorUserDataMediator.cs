using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core.StrangeExtensions;
using ImageAndVideoPicker;

public class EditorUserDataMediator : EventMediator {

    [Inject]
    public EditorUserDataView EditorUserDataView { get; set; }

    [Inject]
    public UserModel UserModel { get; set; }


    public override void OnRegister()
    {
        EditorUserDataView.BackBut.onClick.AddListener(BackClick);
        EditorUserDataView.DisplayNameBut.onClick.AddListener(DisplyNameClick);
        EditorUserDataView.SignatureBut.onClick.AddListener(SignatureClick);
        EditorUserDataView.LocalBut.onClick.AddListener(LocalClick);
        EditorUserDataView.GenderBut.onClick.AddListener(GenderClick);
        EditorUserDataView.BirthdayBut.onClick.AddListener(BirthdayClick);
        EditorUserDataView.HeadBut.onClick.AddListener(HeadClick);
        EditorUserDataView.MenToggle.onValueChanged.AddListener(MenToggleListener);
        EditorUserDataView.WenToggle.onValueChanged.AddListener(WenToggleListener);
        dispatcher.AddListener(CmdEvent.ViewEvent.EditorAccountDataOptionFinish,UpdateAccountFinish);

        PickerEventListener.onImageSelect += OnPickImageSelect;
        PickerEventListener.onImageLoad += OnPickImageLoad;
        PickerEventListener.onError += OnPickImagePickerError;
        PickerEventListener.onCancel += OnPickImagePickerCancel;

        EditorUserDataView.SetUi(UserModel.User);
    }

    #region Click Event

    private void HeadClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
                        AndroidPicker.BrowseImage(false);
#elif UNITY_IPHONE && !UNITY_EDITOR
                	    IOSPicker.BrowseImage(false);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        int random = UnityEngine.Random.Range(1, 7);
        string sendImagePath = @"D:\pic\" + random + ".jpg";
        UploadHead(sendImagePath,400,400);
        Log.I("头像地址："+sendImagePath);
#endif
    }
    private byte[] LoadImageFromLocalPath(string path)
    {
        if (System.IO.File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            return bytes;
        }
        return null;
    }
    private void BirthdayClick()
    {
       iocViewManager.DestroyAndOpenNew(EditorUserDataView.GetUiId(),(int)UiId.EditorBirthday);
    }
    private void GenderClick()
    {
       EditorUserDataView.IsvisibleGenderBg(true);
    }
    private void LocalClick()
    {
        iocViewManager.DestroyAndOpenNew(EditorUserDataView.GetUiId(),(int)UiId.Local);
    }
    private void SignatureClick()
    {
        UserModel.EditorOptionType = EditorOption.EditorSignature;
        iocViewManager.DestroyAndOpenNew(EditorUserDataView.GetUiId(),(int)UiId.EditorNameOrSignature);
    }
    private void DisplyNameClick()
    {
        UserModel.EditorOptionType = EditorOption.EditorDisplayName;
        iocViewManager.DestroyAndOpenNew(EditorUserDataView.GetUiId(),(int)UiId.EditorNameOrSignature);
    }
    private void BackClick()
    {
        switch (UserModel.EditorUserDataType)
        {
            case EditorUserDataType.AccountCenter:
                iocViewManager.DestroyAndOpenNew(EditorUserDataView.GetUiId(),(int)UiId.Me);
                break;
            case EditorUserDataType.Personal:
                iocViewManager.DestroyAndOpenNew(EditorUserDataView.GetUiId(),(int)UiId.Personal);
                break;
        }
    }
    private void WenToggleListener(bool arg0)
    {
        if (arg0)
        {
            dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
            {
                Option = EditorOption.EditorGender,
                Content = "女"
            });
            EditorUserDataView.IsvisibleGenderBg(false);
        }
        
    }

    private void MenToggleListener(bool arg0)
    {

        if (arg0)
        {
            dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
            {
                Option = EditorOption.EditorGender,
                Content = "男"
            });
            EditorUserDataView.IsvisibleGenderBg(false);
        }
    }

    private void UpdateAccountFinish()
    {
        EditorUserDataView.SetUi(UserModel.User);
    }
    #endregion
    private void UploadHead(string path,int width,int height)
    {

        var hedadTexture2D = new Texture2D(width,height);
        hedadTexture2D.LoadImage(LoadImageFromLocalPath(path));

        dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
        {
            Option = EditorOption.EditorHead,
            Texture = hedadTexture2D
        });
    }

    #region 图片选择器回调

    private void OnPickImageSelect(string imgPath, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("Image Location : " + imgPath + " Image Orientation" + imgOrientation);
    }

    private void OnPickImageLoad(string imgPath, Texture2D tex, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("OnImageLoad ： Image imgPath : " + imgPath+"   "+tex);

        UploadHead(imgPath,tex.width,tex.height);

    }

    private void OnPickImagePickerError(string errorMsg)
    {
        Log.I("Error : " + errorMsg);
    }

    private void OnPickImagePickerCancel()
    {
        Log.I("Cancel by user");
    }

#endregion

    public override void OnRemove()
    {
        EditorUserDataView.BackBut.onClick.RemoveListener(BackClick);
        EditorUserDataView.DisplayNameBut.onClick.RemoveListener(DisplyNameClick);
        EditorUserDataView.SignatureBut.onClick.RemoveListener(SignatureClick);
        EditorUserDataView.LocalBut.onClick.RemoveListener(LocalClick);
        EditorUserDataView.GenderBut.onClick.RemoveListener(GenderClick);
        EditorUserDataView.BirthdayBut.onClick.RemoveListener(BirthdayClick);
        EditorUserDataView.HeadBut.onClick.RemoveListener(HeadClick);
        EditorUserDataView.MenToggle.onValueChanged.RemoveListener(MenToggleListener);
        EditorUserDataView.WenToggle.onValueChanged.RemoveListener(WenToggleListener);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.EditorAccountDataOptionFinish, UpdateAccountFinish);

        PickerEventListener.onImageSelect -= OnPickImageSelect;
        PickerEventListener.onImageLoad -= OnPickImageLoad;
        PickerEventListener.onError -= OnPickImagePickerError;
        PickerEventListener.onCancel -= OnPickImagePickerCancel;


    }

    private void OnDestroy()
    {
        OnRemove();
    }


}
