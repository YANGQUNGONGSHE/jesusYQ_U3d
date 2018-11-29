using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;
using UnityEngine;
using UnityEngine.UI;

public class ShareView : MonoBehaviour
{

    private ShareSDK ssdk;
    
	void Start ()
	{
	    ssdk = GameObject.Find("ShareSdk").GetComponent<ShareSDK>();
        ssdk.authHandler = OnAuthResultHandler;
        ssdk.shareHandler = OnShareResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;
        ssdk.getFriendsHandler = OnGetFriendsResultHandler;
        ssdk.followFriendHandler = OnFollowFriendResultHandler;
        gameObject.GetComponent<Button>().onClick.AddListener(ShareClick);
	}

    private void ShareClick()
    {
        ShareContent content = new ShareContent();

        content.SetTitle("羊群公社");
        content.SetTitleUrl("http://a.app.qq.com/o/simple.jsp?pkgname=com.sheep.jesus&from=groupmessage&isappinstalled=0");
        content.SetSite("羊群公社");
        content.SetSiteUrl("http://a.app.qq.com/o/simple.jsp?pkgname=com.sheep.jesus&from=groupmessage&isappinstalled=0");
        content.SetUrl("http://a.app.qq.com/o/simple.jsp?pkgname=com.sheep.jesus&from=groupmessage&isappinstalled=0");
		content.SetText("数千万弟兄姊妹期待已久的资讯应用，基督徒值得信赖的阅读软件!");
        content.SetImageUrl("https://sheepcommunity.oss-cn-shanghai.aliyuncs.com/files/LOGO.png");
        content.SetShareType(ContentType.Webpage);
        //优先客户端分享
        content.SetEnableClientShare(true);
        //使用微博API接口应用内分享 iOS only
        // content.SetEnableSinaWeiboAPIShare(true);
        //通过分享菜单分享
        var plats = new PlatformType[5];

        plats[0] = PlatformType.WeChat;
        plats[1] = PlatformType.WeChatMoments;
        plats[2] = PlatformType.WeChatFavorites;
        plats[3] = PlatformType.QQ;
        plats[4] = PlatformType.QZone;
        ssdk.ShowPlatformList(plats, content, 100, 100);
    }

    private void OnFollowFriendResultHandler(int reqId, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("Follow friend successfully !");
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
    private void OnGetFriendsResultHandler(int reqId, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("get friend list result :");
            print(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    private void OnGetUserInfoResultHandler(int reqId, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("get user info result :");
            print(MiniJSON.jsonEncode(result));
            print("AuthInfo:" + MiniJSON.jsonEncode(ssdk.GetAuthInfo(PlatformType.QQ)));
            print("Get userInfo success !Platform :" + type);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    private void OnShareResultHandler(int reqId, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("share successfully - share result :");
            print(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    private void OnAuthResultHandler(int reqId, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            if (result != null && result.Count > 0)
            {
                print("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
            }
            else
            {
                print("authorize success !" + "Platform :" + type);
            }
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    
}
