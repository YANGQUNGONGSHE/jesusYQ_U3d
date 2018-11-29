using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class BookDetailMediator : EventMediator {

    [Inject]public BookDetailView BookDetailView { get; set; }
    [Inject]public UserModel UserModel { get; set; }

    private string _webKey = "book";
    
    private string _selectedVolumeString;

	private string _endIdStr;

    private readonly List<string> _paths = new List<string>()
    {
        "guoxueDetail.html",
        "texts.html",
        "myNotes.html",
        "addNote.html",
        "textsFilter.html"
    };
    
    private enum WebPage
    {
        Main,
        Detail,
        MyNote,
        AddNote,
        Search
    }
    
    private WebPage _mCurrentPage = WebPage.Main;

    private bool _mIsCheckedWebViewClose = false;

    void Update()
    {
        if (!_mIsCheckedWebViewClose)
        {
            if (!WebController.Instance.hasExist(_webKey))
            {
                _mIsCheckedWebViewClose = true;
                BackClick();
            }
        }

		if(Input.GetKeyDown(KeyCode.Escape)) 
		{
			if (_mCurrentPage == WebPage.Detail) 
			{
				SendLastRead ();
			}
		}   
    }

	private void SendLastRead()
	{
		Dictionary<string,string> param = new Dictionary<string, string> ();
		param.Add("ChapterId", _endIdStr);
		HttpManager.RequestPost("http://apiv2.yangqungongshe.com/chapterreads", param, (request, response) =>
		{
		    Log.E(response.StatusCode);
		});
	}

    public override void OnRegister()
    {
        dispatcher.Dispatch(CmdEvent.Command.ReqQueryLastReadRecord, UserModel.BookModel.Id);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqQueryLastReadRecordSucc,LoadLastReadRecordFinish);
        dispatcher.AddListener(CmdEvent.ViewEvent.ReqQueryLastReadRecordFail,LoadLastReadRecordFail);
        NotificationCenter.DefaultCenter().AddObserver(NotifiyName.OpenClassiceBook, OnOpenClassiceBook);
        BookDetailView.BackBut.onClick.AddListener(BackClick);
        BookDetailView.OptionBut.onClick.AddListener(OptionButClick);
        BookDetailView.SetTitleUi(UserModel.BookModel.BookName);
        WebController.Instance.OnMessageReceived += OnWebMessageReceived;
       // CreateWebView();
    }

    private void OptionButClick()
    {
        WebController.Instance.RunJavaScript(_webKey, "send();", null);
    }

    private void OnOpenClassiceBook(Notification notification)
    {
        //CreateWebView(); 
    }

    private void CreateWebView(string volumeNumber,string chapterNumber ,string chapterLength)
    {
        //var oUrl = UniWebViewHelper.StreamingAssetURLForPath("www/guoxueDetail.html?VolumeNumber="+volumeNumber+"&ChapterNumber="+chapterNumber+ "&ChapterLength="+chapterLength);
        var oUrl = Web.Url + "/guoxueDetail.html?VolumeNumber="+volumeNumber+"&ChapterNumber="+chapterNumber+ "&ChapterLength="+chapterLength;
        WebController.Instance.CreateWeb(_webKey, BookDetailView.WebRect, oUrl, false, (web, code, url) =>
        {
            if (!string.IsNullOrEmpty(url))
            {
                int index1 = url.LastIndexOf('/');
                string page;
                if (url.Contains("?"))
                {
                    var index2 = url.IndexOf('?') - 1;
                    var length = index2 - index1;
                    page = url.Substring(index1 + 1, length);
                }
                else
                {
                    page = url.Substring(index1 + 1);
                }
                int pageIndex = _paths.IndexOf(page);
                _mCurrentPage = (WebPage)pageIndex;

                if (_mCurrentPage == WebPage.Main)
                {
                    BookDetailView.SetTitleUi(UserModel.BookModel.BookName);
                    _selectedVolumeString = string.Empty;
                }
                
                if (_mCurrentPage == WebPage.AddNote)
                    BookDetailView.OptionBut.gameObject.SetActive(true);
                else
                    BookDetailView.OptionBut.gameObject.SetActive(false);
                
                if (_mCurrentPage == WebPage.Main || _mCurrentPage == WebPage.MyNote || _mCurrentPage == WebPage.AddNote || _mCurrentPage == WebPage.Search || _mCurrentPage == WebPage.Detail)
                {
                    BookDetailView.BackBut.gameObject.SetActive(true);
                }
                else
                {
                    BookDetailView.BackBut.gameObject.SetActive(false);
                }
            }
            
            if (code == 200)
            {
                var ssid = UserModel.SessionId.Ssid;
                switch (_mCurrentPage)
                {
                    case WebPage.Main:
                        
                        string jsCodeBid = String.Format("getUnityData('{0}', '{1}');", ssid, UserModel.BookModel.Id);
                        WebController.Instance.RunJavaScript(_webKey, jsCodeBid, delegate(UniWebViewNativeResultPayload payload)
                        {
                            Log.I(payload.resultCode);
                        });
                        break;
                        
                    case WebPage.MyNote:
                    case WebPage.AddNote:
                    case WebPage.Detail:
                        string jsCodeSid = String.Format("getUnityData('{0}');", ssid);
                        WebController.Instance.RunJavaScript(_webKey, jsCodeSid, delegate(UniWebViewNativeResultPayload payload)
                        {
                            Log.I(payload.resultCode);
                        });
                        break;
                }
                //WebController.Instance.Show(_webKey);
            }
        });
    }

    private void OnWebMessageReceived(UniWebView view, UniWebViewMessage msg)
    {
        if (msg.Path.Equals("loadComplete"))
        {
            var chapterString = msg.Args["chapterTitle"];
			_endIdStr = msg.Args["endIdStr"];
            BookDetailView.SetTitleUi(_selectedVolumeString + chapterString);
        }
        
        if (msg.Path.Equals("selectedVolume"))
        {
            _selectedVolumeString = msg.Args["msg"];
        }

		if (msg.Path.Equals("selectedLastRead"))
		{
			_selectedVolumeString = msg.Args["msg"];
		}
        
        if (msg.Path.Equals("selectedChapter"))
        {
            var chapterString = msg.Args["msg"];
            BookDetailView.SetTitleUi(_selectedVolumeString + chapterString);
        }
        
        if (msg.Path.Equals("publish_node"))
        {
            var isSucc = msg.Args["succ"].ToInt();
            if (isSucc == 0)
            {
                string errMsg = msg.Args["msg"];
                UIUtil.Instance.ShowFailToast(errMsg);
            }
            else
            {
                UIUtil.Instance.ShowSuccToast("笔记添加成功");
                WebController.Instance.GoBack(_webKey);
            }
        }

		if (msg.Path.Equals("endIdStr"))
		{
			_endIdStr = msg.Args["msg"];
		}

    }

    private void BackClick()
    {
        switch (_mCurrentPage)
        {
            case WebPage.AddNote:
            case WebPage.MyNote:
            case WebPage.Search:
                WebController.Instance.GoBack(_webKey);
                break;
                
            case WebPage.Detail:
                WebController.Instance.RunJavaScript(_webKey, "goBack();", delegate(UniWebViewNativeResultPayload payload)
                {
                    Log.I(payload.resultCode);
                });
                //WebController.Instance.GoBack(_webKey);
                break;
            
            case WebPage.Main:
                WebController.Instance.CloseWeb(_webKey);
                iocViewManager.DestroyAndOpenNew(BookDetailView.GetUiId(),(int)UiId.Classics);
                break;
        }
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqQueryLastReadRecordSucc, LoadLastReadRecordFinish);
        dispatcher.RemoveListener(CmdEvent.ViewEvent.ReqQueryLastReadRecordFail, LoadLastReadRecordFail);
        NotificationCenter.DefaultCenter().RemoveObserver(NotifiyName.OpenClassiceBook, OnOpenClassiceBook);
        BookDetailView.BackBut.onClick.RemoveListener(BackClick);
        BookDetailView.OptionBut.onClick.RemoveListener(OptionButClick);
        WebController.Instance.OnMessageReceived -= OnWebMessageReceived;
    }

    private void LoadLastReadRecordFinish(IEvent eEvent)
    {
       
        var info = eEvent.data as ChapterRead;
        if (info != null)
        {
            CreateWebView(info.Chapter.VolumeNumber.ToString(), info.Chapter.Number.ToString(),info.Volume.ChaptersCount.ToString());
        }
        else
        {
            CreateWebView(string.Empty,string.Empty,string.Empty);
        }
        
    }

    private void LoadLastReadRecordFail()
    {
        CreateWebView(string.Empty, string.Empty,string.Empty);
    }

    private void OnDestroy()
    {
        OnRemove();
    }
}
