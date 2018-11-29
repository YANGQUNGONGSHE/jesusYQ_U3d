using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using CameraShot;
using ImageAndVideoPicker;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class SetDyNameMediator : EventMediator {


    [Inject]
    public SetDyNameView SetDyNameView { get; set; }

    private bool _isNameLenthPassed;

    private bool _isSetHead;
    public override void OnRegister()
    {
        Dispatcher.InvokeAsync(UIUtil.Instance.CloseWaiting);
        SetDyNameView.DyNameInputField.onValueChanged.AddListener(DyNameInputFieldChanged);
        SetDyNameView.SummbitBut.onClick.AddListener(SummbitButClick);
        SetDyNameView.CancelTakeBut.onClick.AddListener(CancelTakePhotoClick);
        SetDyNameView.DefalutPhotoBut.onClick.AddListener(DefalutPhotoClick);
        SetDyNameView.PickPhotoBut.onClick.AddListener(PickPhotoClick);
        SetDyNameView.TakePhotoBut.onClick.AddListener(TakePhotoClick);
        SetDyNameView.HeadChioseBut.onClick.AddListener(HeadChioseClick);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqFirstSetHeadFinish,ReqFirstSetHeadFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqFirstSetDyNameFinish,ReqFirstSetDyNameFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqFirstSetFail,ReqFirstFail);

        PickerEventListener.onImageSelect += OnPickImageSelect;
        PickerEventListener.onImageLoad += OnPickImageLoad;
        PickerEventListener.onError += OnPickImagePickerError;
        PickerEventListener.onCancel += OnPickImagePickerCancel;

        CameraShotEventListener.onImageSaved += OnCameraImageSaved;
        CameraShotEventListener.onImageLoad += OnCameraImageLoad;
        CameraShotEventListener.onVideoSaved += OnCameraVideoSaved;
        CameraShotEventListener.onError += OnCameraError;
        CameraShotEventListener.onCancel += OnCameraCancel;
    }

    #region Click Event

    private void HeadChioseClick()
    {
        SetDyNameView.IsVisiblePhotoBg(true);
    }

    private void TakePhotoClick()
    {
        SetDyNameView.IsVisiblePhotoBg(false);
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidCameraShot.GetTexture2DFromCamera(); 
#elif UNITY_IPHONE && !UNITY_EDITOR
		IOSCameraShot.GetTexture2DFromCamera(false);// capture and crop
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
       return;
#endif
    }

    private void PickPhotoClick()
    {
        SetDyNameView.IsVisiblePhotoBg(false);
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidPicker.BrowseImage(false);
#elif UNITY_IPHONE && !UNITY_EDITOR
	    IOSPicker.BrowseImage(false);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        return;
#endif
    }

    private void DefalutPhotoClick()
    {
        _isSetHead = true;
        SetDyNameView.HeadImage.texture = DefaultImage.Head;
        SetDyNameView.IsVisiblePhotoBg(false);
    }

    private void CancelTakePhotoClick()
    {
        SetDyNameView.IsVisiblePhotoBg(false);
    }

    private void SummbitButClick()
    {
        if (_isSetHead)
        {
            dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
            {
                Option = EditorOption.EditorDisplayName,
                Content = SetDyNameView.DyNameInputField.text,
                IsNewCreateed = true
            });
            UIUtil.Instance.ShowWaiting();
        }
        else
        {
            UIUtil.Instance.ShowFailToast("请设置头像");
        }
    }

    #endregion

    #region dispatcher Event

    private void ReqFirstFail(IEvent eEvent)
    {
        Log.I("！！！！"+(string)eEvent.data);
        UIUtil.Instance.CloseWaiting();
        UIUtil.Instance.ShowFailToast((string)eEvent.data);
    }

    private void ReqFirstSetHeadFinish()
    {
        _isSetHead = true;
        UIUtil.Instance.CloseWaiting();
    }
    private void ReqFirstSetDyNameFinish()
    {
        iocViewManager.DestoryView(SetDyNameView.GetUiId());
        SceneUtil.Instance.LoadScene(3);
    }
      
    #endregion

    #region 拍照回调

    void OnCameraImageSaved(string path, CameraShot.ImageOrientation orientation)
    {
        Log.I("Image Saved to gallery, path : " + path + ", orientation : " + orientation);
    }

    void OnCameraImageLoad(string path, Texture2D tex, CameraShot.ImageOrientation orientation)
    {
        Log.I("Image Saved to gallery, loaded: " + path + ", orientation: " + orientation);

        UploadHead(path, tex.width, tex.height);
    }

    void OnCameraVideoSaved(string path)
    {
        Log.I("Video Saved at path : " + path);
    }

    void OnCameraError(string errorMsg)
    {
        Log.I("Error : " + errorMsg);
    }

    void OnCameraCancel()
    {
        Log.I("OnCancel");
    }


    #endregion

    #region 图片选择器回调

    private void OnPickImageSelect(string imgPath, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("Image Location : " + imgPath + " Image Orientation" + imgOrientation);
    }

    private void OnPickImageLoad(string imgPath, Texture2D tex, ImageAndVideoPicker.ImageOrientation imgOrientation)
    {
        Log.I("准备发送OnImageLoad ： Image Location : " + imgPath);
        UploadHead(imgPath, tex.width, tex.height);
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

    private void DyNameInputFieldChanged(string arg0)
    {
        _isNameLenthPassed = SetDyNameView.DyNameInputField.text.Length < 9;
        SetDyNameView.ShowCheckNameFlag(_isNameLenthPassed);
        CheckBut();
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

    private void UploadHead(string path, int width, int height)
    {
        var hedadTexture2D = new Texture2D(width, height);
        hedadTexture2D.LoadImage(LoadImageFromLocalPath(path));

        dispatcher.Dispatch(CmdEvent.Command.EditorAccountDataOption, new EditorUserData()
        {
            Option = EditorOption.EditorHead,
            Texture = hedadTexture2D,
            IsNewCreateed = true
        });
        SetDyNameView.HeadImage.texture = hedadTexture2D;
        UIUtil.Instance.ShowWaiting();
    }

    private void CheckBut()
    {
        if (!string.IsNullOrEmpty(SetDyNameView.DyNameInputField.text) && _isNameLenthPassed)
        {
            SetDyNameView.ShowSummbitButEnable(true);
        }
        else
        {
            SetDyNameView.ShowSummbitButEnable(false);
        }
    }

    public override void OnRemove()
    {
        SetDyNameView.DyNameInputField.onValueChanged.RemoveListener(DyNameInputFieldChanged);
        SetDyNameView.SummbitBut.onClick.RemoveListener(SummbitButClick);
        SetDyNameView.CancelTakeBut.onClick.RemoveListener(CancelTakePhotoClick);
        SetDyNameView.DefalutPhotoBut.onClick.RemoveListener(DefalutPhotoClick);
        SetDyNameView.PickPhotoBut.onClick.RemoveListener(PickPhotoClick);
        SetDyNameView.TakePhotoBut.onClick.RemoveListener(TakePhotoClick);
        SetDyNameView.HeadChioseBut.onClick.RemoveListener(HeadChioseClick);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqFirstSetHeadFinish, ReqFirstSetHeadFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqFirstSetDyNameFinish, ReqFirstSetDyNameFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqFirstSetFail, ReqFirstFail);


        PickerEventListener.onImageSelect -= OnPickImageSelect;
        PickerEventListener.onImageLoad -= OnPickImageLoad;
        PickerEventListener.onError -= OnPickImagePickerError;
        PickerEventListener.onCancel -= OnPickImagePickerCancel;

        CameraShotEventListener.onImageSaved -= OnCameraImageSaved;
        CameraShotEventListener.onImageLoad -= OnCameraImageLoad;
        CameraShotEventListener.onVideoSaved -= OnCameraVideoSaved;
        CameraShotEventListener.onError -= OnCameraError;
        CameraShotEventListener.onCancel -= OnCameraCancel;
    }

    private void OnDestroy()
    {
        OnRemove();
    }

}
