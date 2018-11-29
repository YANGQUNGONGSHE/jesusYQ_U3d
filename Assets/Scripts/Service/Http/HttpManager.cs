using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using BestHTTP;
using LitJson;
using Org.BouncyCastle.Asn1.Utilities;

public enum ContentTypes
{
    Json,
    Protobuf,
    Text,
}

public class HttpManager
{
    static HttpManager()
    {
        
    }

    public static void Download(string url, Action<HTTPRequest, HTTPResponse> requestCallback)
    {
        //TODO:
    }

    public static void UploadHead(string url, Texture2D texture2D, Dictionary<string,string> param,Action<HTTPRequest, HTTPResponse> requestCallback)
    {
        var ssid = JsonMapper.ToObject<SessionId>((string)LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.Ssid));
        var request = new HTTPRequest(new Uri(url),HTTPMethods.Put, (originalRequest, response) =>
        {
            if (requestCallback != null)
                requestCallback(originalRequest, response);
        });

        request.AddHeader("X-ss-pid", ssid.Ssid);
        request.AddHeader("X-ss-opt", "perm");
        request.AddHeader("Accept", "application/json");
        request.AddBinaryData("image", texture2D.EncodeToJPG(), "image.jpg");
        if (param != null)
        {
            foreach (var keyValue in param)
            {
                var k = keyValue.Key;
                var v = keyValue.Value;
                request.AddField(k, v);
            }
        }
        request.Send();
    }

    public static void RequestImage(string url, Action<Texture2D> requestCallback)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), (originalRequest, response) =>
        {
             if (requestCallback != null)
                    requestCallback(response.DataAsTexture2D);
        });
        request.Send();
    }

    public static void RequestImage(string url, Action<string,Texture2D> requestCallback)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), (originalRequest, response) =>
        {
                if (requestCallback != null)
                    requestCallback(originalRequest.Uri.OriginalString, response.DataAsTexture2D);
        });
        request.Send();
    }

    public static void RequestGet(string url, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {
        Request(url, HTTPMethods.Get, false, isUseXsspid, caChe,null, null, ContentTypes.Json, requestCallback);
    }

    public static void RequestGet(string url, Dictionary<string, string> param ,Action<HTTPRequest,HTTPResponse> requestCallBack,bool isUseXsspid =true, bool caChe = false)
    {
        Request(url, HTTPMethods.Get, false, isUseXsspid, caChe,param, null, ContentTypes.Json, requestCallBack);
    }

    public static void RequestGetWithProtobuf(string url, byte[] serialize, Action<HTTPRequest, HTTPResponse> requestCallBack, bool isUseXsspid = true, bool caChe = false)
    {

        Request(url, HTTPMethods.Get, true, isUseXsspid, caChe, null, serialize, ContentTypes.Protobuf, requestCallBack);
    }


    public static void RequestPost(string url, Dictionary<string, string> param, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {
        Request(url, HTTPMethods.Post, false, isUseXsspid, caChe, param, null,ContentTypes.Json, requestCallback);
    }

    public static void ReuqestPostWithProtobuf(string url, byte[] serialize, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {
        //  Request(url, true, true, true, null, serialize, requestCallback);
        Request(url, HTTPMethods.Post, true, isUseXsspid, caChe, null, serialize,ContentTypes.Protobuf, requestCallback);
    }


    public static void RequestPut(string url, Dictionary<string, string> param, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {
        Request(url, HTTPMethods.Put, false, isUseXsspid, caChe, param, null,ContentTypes.Json, requestCallback);
    }

    public static void RequestPutWithProtobuf(string url, byte[] serialize, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {

        Request(url, HTTPMethods.Put, true, isUseXsspid,caChe, null, serialize,ContentTypes.Protobuf, requestCallback);
    }


    public static void RequestDelete(string url, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {
        Request(url, HTTPMethods.Delete, false, isUseXsspid,caChe, null, null, ContentTypes.Json,requestCallback);
    }

    public static void RequestDelete(string url, Dictionary<string, string> param, Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true, bool caChe = false)
    {
        Request(url, HTTPMethods.Delete, false, isUseXsspid,caChe, param, null, ContentTypes.Json,requestCallback);
    }

    public static void RequestDeleteWithProtobuf(string url, byte[] serialize,Action<HTTPRequest, HTTPResponse> requestCallback, bool isUseXsspid = true)
    {
       // Request(url, HTTPMethods.Delete, true, isUseXsspid, null, serialize,ContentType.Protobuf, requestCallback);
    }

    public static void Request(string url, HTTPMethods httpMethods, bool isPb = false, bool isUseXsspid = true, bool caChe = false,Dictionary<string, string> param = null, byte[] serialize = null,  ContentTypes contentType = ContentTypes.Protobuf, Action<HTTPRequest, HTTPResponse> requestCallback = null)
    {
                var httpRequest = new HTTPRequest(new Uri(url), httpMethods, (originalRequest, response) =>
                {
                    if (originalRequest.State == HTTPRequestStates.Finished)
                    {
                        if (requestCallback != null)
                        {
                            requestCallback(originalRequest, response);
                        }
                    }
                    else
                    {
                        UIUtil.Instance.ShowTextToast(originalRequest.State.ToString());
                    }
                });

                if (isUseXsspid)
                {
                    var ssid = JsonMapper.ToObject<SessionId>(
                        (string) LocalDataManager.Instance.LoadJsonObj(LocalDataObjKey.Ssid));
                    httpRequest.AddHeader("X-ss-pid", ssid.Ssid);
                    httpRequest.AddHeader("X-ss-opt", "perm");
                }

                switch (contentType)
                {
                    case ContentTypes.Protobuf:
                        httpRequest.AddHeader("Content-Type", "application/x-protobuf");
                        httpRequest.AddHeader("Accept", "application/x-protobuf");
                        break;
                    case ContentTypes.Json:
                        httpRequest.AddHeader("Content-Type", "application/json");
                        httpRequest.AddHeader("Accept", "application/json");
                        break;
                }

                if (isPb)
                {
                    if(serialize!=null)
                    httpRequest.RawData = serialize;
                }
                else
                {
                    if(param!=null)
                    foreach (var keyValue in param)
                    {
                        var k = keyValue.Key;
                        var v = keyValue.Value;
                        httpRequest.AddField(k, v);
                    }
                }
               httpRequest.DisableCache = caChe;
               httpRequest.Send();
    }
}