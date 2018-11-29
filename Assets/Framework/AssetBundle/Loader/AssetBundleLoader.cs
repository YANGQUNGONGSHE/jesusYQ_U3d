using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 同步加载资源包
/// </summary>
namespace WongJJ.Game.Core
{
    public class AssetBundleLoader : IDisposable
    {
        private AssetBundle bundle;

        public AssetBundleLoader(string assetBundlePath, bool isFullPath = false)
        {
            string fullPath = isFullPath ? assetBundlePath : LocalByteFileManager.Instance.LocalFilePath + assetBundlePath;
            bundle = AssetBundle.LoadFromMemory(LocalByteFileManager.Instance.GetBuffer(fullPath));
        }

        public T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
            if (bundle == null)
                return default(T);
            return bundle.LoadAsset(name) as T;
        }

        public T LoadAsset2<T>(string name) where T : UnityEngine.Object
        {
            if (bundle == null)
                return default(T);
            return bundle.LoadAsset<T>(name);
        }

        public void Dispose()
        {
            if (bundle != null)
                bundle.Unload(false);
        }
    }
}