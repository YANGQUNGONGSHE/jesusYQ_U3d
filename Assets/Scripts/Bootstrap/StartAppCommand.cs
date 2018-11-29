using System.Collections;
using UnityEngine;
using strange.extensions.command.impl;
using UnityEngine.SceneManagement;
using WongJJ.Game.Core.StrangeExtensions;

public class StartAppCommand : Command
{
    public override void Execute()
    {
        Object.DontDestroyOnLoad(GameObject.Find("BootStrap"));
#if REALMACHINE
        iocViewManager.LoadAssetType = LoadAssetType.AssetBundle;
        UniWebView.SetWebContentsDebuggingEnabled(false);
#else
        iocViewManager.LoadAssetType = LoadAssetType.AssetBundle;
        UniWebView.SetWebContentsDebuggingEnabled(false);
#endif
        InitUtil();
#if UNITY_ANDROID || UNITY_IPHONE
        Application.targetFrameRate = 60;
#endif
        //CoroutineController.Instance.StartCoroutine(WaitForStart());
        SceneManager.LoadScene(1);
    }
    
    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }

    void InitUtil()
    {
        var uiUtil = UIUtil.Instance;
        var commUtil = CommUtil.Instance;
    }
}
