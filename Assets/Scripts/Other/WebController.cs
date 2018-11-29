using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WongJJ.Game.Core;
using System;

public class WebController :  KeepSingletion<WebController>
{
	private UniWebView _mWebView;
	private GameObject _mWebViewGameObject;
	
	private readonly Dictionary<string, UniWebView> _allWebViews = new Dictionary<string, UniWebView>();

    public bool hasExist(string key)
    {
        var webView = _allWebViews[key];
        if (webView != null)
        {
            return true;
        }
        return false;
    }

    public UniWebView GetWebViewByKey(string key)
    {
        if (_allWebViews.ContainsKey(key))
        {
            return _allWebViews[key];
        }
        return null;
    }

	public Action<UniWebView, UniWebViewMessage> OnMessageReceived;
	
	public void CreateWeb(string key, RectTransform rect, string openUrl, bool startShow, Action<UniWebView, int, string> onPageFinishCb)
	{
		//UIUtil.Instance.ShowWaiting();
		var mWebViewGameObject = new GameObject("UniWebView");
		var mWebView = mWebViewGameObject.AddComponent<UniWebView>();
		mWebView.ReferenceRectTransform = rect;
		mWebView.SetBouncesEnabled(false);
	    mWebView.SetZoomEnabled(false);
	    mWebView.SetSpinnerText("正在拼命加载...");
        mWebView.SetShowSpinnerWhileLoading(true);
        mWebView.SetImmersiveModeEnabled(false);
	    mWebView.SetBackButtonEnabled(false);
        mWebView.Load(openUrl);
	    mWebView.Show();
		//Hide();
		mWebView.OnPageFinished += (view, statusCode, url) => 
		{
			/*if (startShow)
				Show();*/
		    if (onPageFinishCb != null)
		    {
		        onPageFinishCb(view, statusCode, url);
                //UIUtil.Instance.CloseWaiting();
		        mWebView.SetShowSpinnerWhileLoading(false);
            }
		};
		mWebView.OnShouldClose += (view) => 
		{
			mWebView = null;
			return true;
		};
		mWebView.OnMessageReceived += (view, message) => 
		{
			if(OnMessageReceived != null)
				OnMessageReceived(view, message);
		};
		
		_allWebViews.Add(key, mWebView);
	}

	public void CreateWeb(Rect rect, string openUrl, bool startShow, Action<UniWebView, int, string> onPageFinishCb)
	{
		//UIUtil.Instance.ShowWaiting();
		_mWebViewGameObject = new GameObject("UniWebView");
		_mWebView = _mWebViewGameObject.AddComponent<UniWebView>();
		_mWebView.Frame = rect;
		_mWebView.SetBouncesEnabled(false);
    	if(_mWebView != null)
			_mWebView.Load(openUrl);
		Hide();
		_mWebView.OnPageFinished += (view, statusCode, url) => 
		{
//			if (startShow)
//				Show();
			UIUtil.Instance.CloseWaiting();
			if(onPageFinishCb != null)
				onPageFinishCb(view, statusCode, url);
    	};
		_mWebView.OnShouldClose += (view) => 
		{
        	_mWebView = null;
        	return true;
    	};
		_mWebView.OnMessageReceived += (view, message) => 
		{
			if(OnMessageReceived != null)
				OnMessageReceived(view, message);
        };
	}

	// In a UIBehavior script:
	// Called when associated `rectTransform` is changed.
	void OnRectTransformDimensionsChange() 
	{
		// This will update web view's frame to match the reference rect transform if set.
		_mWebView.UpdateFrame();
		foreach (UniWebView mWebView in _allWebViews.Values)
		{
			mWebView.UpdateFrame();
		}
	}

	public void CloseWeb()
	{
		Destroy(_mWebView);
		Destroy(_mWebViewGameObject);
        _mWebView = null;
		_mWebViewGameObject = null;
	}

	public void CloseWeb(string key)
	{
        Log.I("_allWebViews" + _allWebViews);
		if (_allWebViews.ContainsKey(key))
		{
			var webView = _allWebViews[key];
		    if (webView != null)
		    {
		        Destroy(webView);
		        if (webView.gameObject != null)
		        {
		            Destroy(webView.gameObject);
		        }
		    }
		    _allWebViews.Remove(key);
		}
    }

	private void OnDestroy() 
	{
        CloseWeb();
		foreach (string key in _allWebViews.Keys)
		{
			CloseWeb(key);
		}
    }

	public void Reload()
	{
		if(_mWebView != null)
			_mWebView.Reload();
	}
	
	public void Reload(string key)
	{
		if (_allWebViews.ContainsKey(key))
		{
			var webView = _allWebViews[key];
			webView.Reload();
		}
	}

	public void Show()
	{
		if (_mWebView != null)
			_mWebView.Show();
	}
	
	public void Show(string key)
	{
		if (_allWebViews.ContainsKey(key))
		{
			var webView = _allWebViews[key];
			webView.Show();
		}
	}

	public void Hide()
	{
		if (_mWebView != null)
			_mWebView.Hide();
	}
	
	public void Hide(string key)
	{
		if (_allWebViews.ContainsKey(key))
		{
			var webView = _allWebViews[key];
			webView.Hide();
		}
	}

	public void GoBack()
	{
		if(_mWebView.CanGoBack)
			_mWebView.GoBack();
	}
	
	public void GoBack(string key)
	{
		if (_allWebViews.ContainsKey(key))
		{
			var webView = _allWebViews[key];
			if(webView.CanGoBack)
				webView.GoBack();
		}
	}

	public void GoForward()
	{
		if(_mWebView.CanGoForward)
			_mWebView.GoForward();
	}

	public void AddJavaScript(string code, Action<UniWebViewNativeResultPayload> cb)
	{
		_mWebView.AddJavaScript(code, (payload) => 
		{
			if(cb != null)
				cb(payload);
		});
	}

	public void RunJavaScript(string code, Action<UniWebViewNativeResultPayload> cb)
	{
		if (!code.EndsWith(";"))
		{
			Log.I("请添加分号");
			return;
		}
		_mWebView.EvaluateJavaScript(code, (payload) => 
		{
			if (cb != null)
				cb(payload);
		});
	}
	
	public void RunJavaScript(string key, string code, Action<UniWebViewNativeResultPayload> cb)
	{
		if (_allWebViews.ContainsKey(key))
		{
			var webView = _allWebViews[key];
			if (!code.EndsWith(";"))
			{
				Log.I("请添加分号");
				return;
			}
			webView.EvaluateJavaScript(code, (payload) => 
			{
				if (cb != null)
					cb(payload);
			});
		}
	}
}
