using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressAudio : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image Bg;
    private Vector2 _mDownPoint;
    private int _mPressTime;
    private bool _mIsCancel;

    public Action OnPressAudio;
    public Action OnEndPressAudio;
    public Action OnCancelAudio;
    public Action OnTimeNotEnough;
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(_mDownPoint.y - eventData.position.y) >= 60)
        {
            _mIsCancel = true;
            if (OnCancelAudio != null)
                OnCancelAudio();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
         if (Mathf.Abs(_mDownPoint.y - eventData.position.y) >= 60)
        {
            _mIsCancel = true;
            if (OnCancelAudio != null)
                OnCancelAudio();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Log.I("按下...");
        _mDownPoint = eventData.pressPosition;
        Log.I(_mDownPoint);
        _mPressTime = 0;
        if (OnPressAudio != null)
            OnPressAudio();
        StartCoroutine(BeginTiming());
        Bg.color = new Color(210/255f, 210/255f, 210/255f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Log.I("松开...");
        Log.I(eventData.position);
        Bg.color = Color.white;
        if (_mPressTime < 1)
        {
            Log.I("时间过短");
            if (OnTimeNotEnough  != null)
            {
                OnTimeNotEnough();
                return;
            }
        }
        else
        {
            if(_mIsCancel)
            {
                _mIsCancel = false;
                return;
            }
            else
            {
                if (OnEndPressAudio != null)
                    OnEndPressAudio();
            }
        }
    }

    public IEnumerator BeginTiming()
    {
        while (_mPressTime < 1f)
        {
            yield return new WaitForSeconds(1f);
            _mPressTime++;
        }
    }


}
