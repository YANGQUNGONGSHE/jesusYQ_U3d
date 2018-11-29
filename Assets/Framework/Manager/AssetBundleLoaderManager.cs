using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

namespace WongJJ.Game.Core
{
    /**
     * @Summary:AssetBundle加载管理器
     * @Author：WongJJ
     * @Date: 2017-05-15 23:52:54
     * @Remark: 2017-05-16 14:26:53 修改先加载依赖项
     */
    public class AssetBundleLoaderManager : Singletion<AssetBundleLoaderManager>
    {
        //依赖配置文件
        private AssetBundleManifest _mManifest;
#if ASSETBUNDLE_MODEL
        //镜像字典<路径，镜像>
        private Dictionary<string, object> m_AssetDic = new Dictionary<string, object>();
        //loader字典，释放
        private Dictionary<string, AssetBundleLoader> m_loaderDic = new Dictionary<string, AssetBundleLoader>();
#endif

        /// <summary>
        /// 加载依赖配置
        /// </summary>
        private void LoadAssetBundleManifest()
        {
            if (_mManifest != null)
                return;

            string assetName = string.Empty;

#if UNITY_STANDALONE_OSX
            assetName = "Mac";
#elif UNITY_STANDALONE_WIN 
            assetName = "Win";
#elif UNITY_IPHONE
            assetName = "iOS";
#elif UNITY_ANDROID
            assetName = "Android";
#endif
            using (AssetBundleLoader loader = new AssetBundleLoader(assetName))
            {
                _mManifest = loader.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }

        /// <summary>
        /// 加载镜像
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject Load(string path, string name)
        {
            using (AssetBundleLoader loader = new AssetBundleLoader(path))
            {
                return loader.LoadAsset<GameObject>(name);
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public AssetBundleLoaderAsync LoadAsync(string path, string name)
        {
            GameObject obj = new GameObject("AssetBundleLoadAsync");
            AssetBundleLoaderAsync async = Util.CreateOrGetComponent<AssetBundleLoaderAsync>(obj);
            async.Init(path, name);
            return async;
        }

        /// <summary>
        /// 加载克隆
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject LoadInstance(string path, string name)
        {
            Log.I("LoadInstance支线");
            using (AssetBundleLoader loader = new AssetBundleLoader(path))
            {
                GameObject obj = loader.LoadAsset<GameObject>(name);
                return UnityEngine.Object.Instantiate(obj);
            }
        }

#if ASSETBUNDLE_MODEL
        /// <summary>
        /// *******
        /// 注意 警告：
        /// 使用该方法必须要求AB资源必须已下载到本地，
        /// 和LoadOrDownload接口不同。此功能不支持边下载边读取，如果本地没有成功下载资源，则会报错崩溃，请慎用。
        /// *******
        /// </summary>
        /// <returns>The instance sync.</returns>
        /// <param name="path">Path.</param>
        /// <param name="name">Name.</param>
        public GameObject LoadInstanceSync(string _path, string _name)
        {

            string path = _path.ToLower();
            string name = _name.ToLower();
            //1.加载依赖配置
            LoadAssetBundleManifest();
            string[] arrDps = _mManifest.GetAllDependencies(path);
            string fullPath = LocalByteFileManager.Instance.LocalFilePath + path;
            if (!File.Exists(fullPath))
            {
                Log.I(fullPath + "不存在！");
                return null;
            }
            else
            {
                if (m_AssetDic.ContainsKey(fullPath))
                {
                    return UnityEngine.Object.Instantiate(m_AssetDic[fullPath] as GameObject);
                }
                else
                {
                    for (int i = 0; i < arrDps.Length; i++)
                    {
                        if (!m_AssetDic.ContainsKey(LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()))
                        {
                            AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                            UnityEngine.Object obj = loader.LoadAsset<UnityEngine.Object>(arrDps[i]);
                            //Loader加入字典
                            m_loaderDic[LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()] = loader;
                            //依赖项加入字典
                            m_AssetDic[LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()] = obj;
                        }
                    }

                    using (AssetBundleLoader loader = new AssetBundleLoader(fullPath, true))
                    {
                        return UnityEngine.Object.Instantiate(loader.LoadAsset<GameObject>(name));
                    }
                }
            }
        }
#endif

        public void LoadOrDownload(string path, string name, Action<GameObject> onComplete, bool isInstantiate = false)
        {
            LoadOrDownload<GameObject>(path.ToLower(), name, onComplete, 0, isInstantiate);
        }

        /// <summary>
        /// 加载镜像或者实实例化，如果没有则下载资源
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="name">Name.</param>
        /// <param name="onComplete">On complete.</param>
        /// <param name="type">0=prefab 1=png</param>
        /// <param name="isInstantiate">If set to <c>true</c> is instantiate.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void LoadOrDownload<T>(string path, string name, Action<T> onComplete, byte type, bool isInstantiate = false) where T : UnityEngine.Object
        {
#if ASSETBUNDLE_MODEL
            //1.加载依赖配置
            LoadAssetBundleManifest();
            //2.加载依赖项
            string[] arrDps = _mManifest.GetAllDependencies(path);
            //3.检查依赖项是否已经下载，如果没有下载，则下载
            CheckDps(0, arrDps, () =>
                {
                    //4.有了依赖项，加载主资源
                    string fullPath = LocalByteFileManager.Instance.LocalFilePath + path;

                    //不存在
                    if (!File.Exists(fullPath))
                    {
                        AssetBundleDownloadData data = DownloadManager.Instance.GetServerAssetData(path);
                        if (data != null)
                        {
                            CoroutineController.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadSingleAsset(data, (bool succ) =>
                                    {
                                        if (succ)
                                        {
                                            if (m_AssetDic.ContainsKey(fullPath))
                                            {
                                                if (onComplete != null)
                                                {
                                                    if (isInstantiate)
                                                        onComplete(UnityEngine.Object.Instantiate(m_AssetDic[fullPath] as T));
                                                    else
                                                        onComplete(m_AssetDic[fullPath] as T);
                                                }
                                                return;
                                            }

                                            //先加载依赖项
                                            for (int i = 0; i < arrDps.Length; i++)
                                            {
                                                if (!m_AssetDic.ContainsKey(LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()))
                                                {
                                                    AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                                                    UnityEngine.Object obj = loader.LoadAsset<UnityEngine.Object>(arrDps[i]);
                                                    //Loader加入字典
                                                    m_loaderDic[LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()] = loader;
                                                    //依赖项加入字典
                                                    m_AssetDic[LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()] = obj;
                                                }
                                            }

                                            //直接加载
                                            using (AssetBundleLoader loader = new AssetBundleLoader(fullPath, true))
                                            {
                                                if (onComplete != null)
                                                {
                                                    if (isInstantiate)
                                                        onComplete(UnityEngine.Object.Instantiate(loader.LoadAsset<T>(name)));
                                                    else
                                                        onComplete(loader.LoadAsset<T>(name));
                                                }
                                            }
                                        }
                                    }));
                        }
                    }
                    else //存在
                    {
                        Log.I("依赖项路径:" + fullPath + "存在.");
                        if (m_AssetDic.ContainsKey(fullPath))
                        {
                            if (onComplete != null)
                            {
                                if (isInstantiate)
                                    onComplete(UnityEngine.Object.Instantiate(m_AssetDic[fullPath] as T));
                                else
                                    onComplete(m_AssetDic[fullPath] as T);
                            }
                            return;
                        }

                        //先加载依赖项
                        for (int i = 0; i < arrDps.Length; i++)
                        {
                            if (!m_AssetDic.ContainsKey(LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()))
                            {
                                AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                                UnityEngine.Object obj = loader.LoadAsset<UnityEngine.Object>(arrDps[i]);
                                //Loader加入字典
                                m_loaderDic[LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()] = loader;
                                //依赖项加入字典
                                m_AssetDic[LocalByteFileManager.Instance.LocalFilePath + arrDps[i].ToLower()] = obj;
                            }
                        }

                        //直接加载
                        using (AssetBundleLoader loader = new AssetBundleLoader(fullPath, true))
                        {
                            if (onComplete != null)
                            {
                                if (isInstantiate)
                                    onComplete(UnityEngine.Object.Instantiate(loader.LoadAsset<T>(name)));
                                else
                                    onComplete(loader.LoadAsset<T>(name));
                            }
                        }
                    }
                });
#else
            if (onComplete != null)
            {
                string newPath = string.Empty;
                switch (type)
                {
                    case 0:
                        newPath = string.Format("Assets/{0}", path.Replace("assetbundle", "prefab"));
                        break;
                    case 1:
                        newPath = string.Format("Assets/{0}", path.Replace("assetbundle", "png"));
                        break;
                }

                if (isInstantiate)
                    onComplete(UnityEngine.Object.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(newPath)));
                else
                    onComplete(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(newPath));
            }
#endif
        }

        /// <summary>
        /// 检查依赖项是否已经下载，如果没有下载，则下载
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="dps">Dps.</param>
        /// <param name="onComplete">On complete.</param>
        private void CheckDps(int index, string[] dps, Action onComplete)
        {
            if (dps == null || dps.Length == 0)
            {
                if (onComplete != null)
                {
                    onComplete();
                }
                return;
            }

            string fullPath = LocalByteFileManager.Instance.LocalFilePath + dps[index];
            if (!File.Exists(fullPath))
            {
                AssetBundleDownloadData data = DownloadManager.Instance.GetServerAssetData(fullPath);
                if (data != null)
                {
                    CoroutineController.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadSingleAsset(data, (bool isSucc) =>
                            {
                                index++;
                                if (index == dps.Length)
                                {
                                    if (onComplete != null)
                                    {
                                        onComplete();
                                    }
                                    return;
                                }
                                CheckDps(index, dps, onComplete);
                            }));
                }
            }
            else
            {
                index++;
                if (index == dps.Length)
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
                    return;
                }
                CheckDps(index, dps, onComplete);
            }
        }
    }
}