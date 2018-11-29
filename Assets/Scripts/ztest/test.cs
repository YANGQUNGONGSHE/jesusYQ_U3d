using NIM;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    ClientAPI.Init(AppDataPath);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private static string _appDataPath;
    public string AppDataPath
    {
        get
        {
            if (string.IsNullOrEmpty(_appDataPath))
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    _appDataPath = Application.persistentDataPath + "/NimUnityDemo";
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    string androidPathName = "com.netease.nim_unity_android_demo";
                    if (System.IO.Directory.Exists("/sdcard"))
                        _appDataPath = Path.Combine("/sdcard", androidPathName);
                    else
                        _appDataPath = Path.Combine(Application.persistentDataPath, androidPathName);
                }
                else if (Application.platform == RuntimePlatform.WindowsEditor ||
                         Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    _appDataPath = "NimUnityDemo";
                }
                Debug.Log("AppDataPath:" + _appDataPath);
            }
            return _appDataPath;
        }
    }
}
