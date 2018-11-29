using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class UIUtil : KeepSingletion<UIUtil>
{
    private static GameObject _mWaitingView;

    public void ShowSuccToast(string text, ToastPos pos = ToastPos.Lower)
    {
        Dispatcher.InvokeAsync(ShowToast, "icon_ok", text, pos, 1f);
    }

    public void ShowFailToast(string text, ToastPos pos = ToastPos.Lower)
    {
        Dispatcher.InvokeAsync(ShowToast, "icon_no", text, pos, 1f);
    }

    public void ShowTextToast(string text)
    {
        Dispatcher.InvokeAsync(ShowToast,"",text,  ToastPos.Mid,2f);
    }

    public void ShowToast(string icon, string text, ToastPos pos, float stayTime = 4f)
    {
        var toastView = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIPrefabs/ToastView"));
        toastView.transform.SetParent(iocViewManager.Canvascomponent, false);
        toastView.GetComponent<ToastView>().SetToast(icon, text, pos, stayTime);
    }

    public void ShowWaiting()
    {
        if (_mWaitingView == null)
        {
            _mWaitingView = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIPrefabs/WaitingView"));
            _mWaitingView.transform.SetParent(iocViewManager.Canvascomponent, false);
        }
    }

    public void CloseWaiting()
    {
        if (_mWaitingView != null)
        {
            UnityEngine.Object.Destroy(_mWaitingView);
        }
    }
}
