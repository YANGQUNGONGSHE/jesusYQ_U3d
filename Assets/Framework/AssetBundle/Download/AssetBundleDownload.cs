using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.IO;

namespace WongJJ.Game.Core
{
    /**
     * @Summary:AssetBundle资源下载主线程
     * @Author：WongJJ
     * @Date: 2017-05-09 17:15:54
     */
    public class AssetBundleDownload : KeepSingletion<AssetBundleDownload>
    {
        //版本文件路径
        internal string MVerPathUrl;
        //下载版本文件成功回调
        public Action<List<AssetBundleDownloadData>> MOnDownloadVersion;
        //下载器
        private readonly AssetBundleDownloadRoutine[] _mAssetBundleDownloadRoutines = new AssetBundleDownloadRoutine[DownloadManager.DOWNLOADROUTINENUM];
        //下载器索引
        private int _mAssetBundleDownloadRoutinesIndex;
        //是否已经下载完成
        private bool _mIsDownloadComplete = false;

        //需要下载的文件总大小
        private int _mTotalSize;

        public int TotalSize
        {
            get { return _mTotalSize; }
        }
        //需要下载的文件总数
        private int _mTotalCount;

        public int TotalCount
        {
            get { return _mTotalCount; }
        }

        //当前已经下载的总大小
        public int CurrentCompleteTotalSize()
        {
            int completeSize = 0;
            for (int i = 0; i < _mAssetBundleDownloadRoutines.Length; i++)
            {
                if (_mAssetBundleDownloadRoutines[i] == null)
                    continue;
                completeSize += _mAssetBundleDownloadRoutines[i].CompleteDownloadSize;
            }
            return completeSize;
        }

        //当前已经下载的总数量
        public int CurrentCompleteTotalCount()
        {
            int completeCount = 0;
            for (int i = 0; i < _mAssetBundleDownloadRoutines.Length; i++)
            {
                if (_mAssetBundleDownloadRoutines[i] == null)
                    continue;
                completeCount += _mAssetBundleDownloadRoutines[i].CompleteDownloadCount;
            }
            return completeCount;
        }

        void Start()
        {
            StartCoroutine(DownloadServerVersion(MVerPathUrl));
        }

        void Update()
        {
            if (TotalCount > 0 && !_mIsDownloadComplete)
            {
                int totalCompleteCount = CurrentCompleteTotalCount();
                totalCompleteCount = totalCompleteCount == 0 ? 1 : totalCompleteCount;

                int totalCompleteSize = CurrentCompleteTotalSize();

                string str = string.Format("正在下载{0}/{1}", totalCompleteCount, TotalCount);
                string strProgress = string.Format("下载进度={0}", totalCompleteSize / (float)TotalSize);
                //Debug.Log(str);
                //Debug.Log(strProgress);

                if (DownloadManager.Instance.OnProgress != null)
                    DownloadManager.Instance.OnProgress(totalCompleteCount, TotalCount);

                if (totalCompleteCount == TotalCount)
                {
                    _mIsDownloadComplete = true;
                    print("下载完毕!");
                    if (DownloadManager.Instance.OnInitComplete != null)
                        DownloadManager.Instance.OnInitComplete();
                }
            }
        }

        /// <summary>
        /// 配置下载服务器版本文件的所需要地址等
        /// </summary>
        /// <param name="verPathUrl">Ver path URL.</param>
        public void ConfigDownloadServerVersion(string verPathUrl, Action<List<AssetBundleDownloadData>> onDownloadVersion)
        {
            MVerPathUrl = verPathUrl;
            MOnDownloadVersion = onDownloadVersion;
        }

        /// <summary>
        /// 下载服务器的版本文件
        /// </summary>
        /// <returns>The server version.</returns>
        /// <param name="url">URL.</param>
        private IEnumerator DownloadServerVersion(string url)
        {
            WWW www = new WWW(url);
            float timeout = Time.time;
            float progress = www.progress;

            while (!www.isDone)
            {
                if (progress < www.progress)
                {
                    timeout = Time.time;
                    progress = www.progress;
                }
                if (Time.time - timeout > DownloadManager.DOWNLOADTIMEOUT)
                {
                    Debug.LogError("Download time out!");
                    yield break;
                }
            }
            yield return www;

            if (www.error == null)
            {
                string content = www.text;
                if (MOnDownloadVersion != null)
                    MOnDownloadVersion(DownloadManager.Instance.ParseVersionInfo(content)); //解析
            }
            else
            {
                Debug.LogError("Download fail :" + www.error);
                //UIUtil.Instance.ShowTextToast(www.error);
                //Application.Quit();
            }
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="downloadList">Download list.</param>
        public void DownloadAsset(List<AssetBundleDownloadData> downloadList)
        {
            _mTotalSize = 0;
            _mTotalCount = 0;
            for (int i = 0; i < _mAssetBundleDownloadRoutines.Length; i++)
            {
                if (_mAssetBundleDownloadRoutines[i] == null)
                {
                    _mAssetBundleDownloadRoutines[i] = gameObject.AddComponent<AssetBundleDownloadRoutine>();
                }
            }

            for (int i = 0; i < downloadList.Count; i++)
            {
                //始终0-4
                _mAssetBundleDownloadRoutinesIndex = _mAssetBundleDownloadRoutinesIndex % _mAssetBundleDownloadRoutines.Length;
                //分配
                _mAssetBundleDownloadRoutines[_mAssetBundleDownloadRoutinesIndex].AddAssetDownload(downloadList[i]);
                _mTotalSize += downloadList[i].Size;
                _mTotalCount++;
                _mAssetBundleDownloadRoutinesIndex++;
            }

            //开始下载
            for (int i = 0; i < _mAssetBundleDownloadRoutines.Length; i++)
            {
                if (_mAssetBundleDownloadRoutines[i] == null)
                    continue;
                _mAssetBundleDownloadRoutines[i].StartDownload();
            }
        }

        /// <summary>
        /// 下载单独一个资源（一般用于边下边加载）
        /// </summary>
        /// <returns>The single asset.</returns>
        /// <param name="data">Data.</param>
        /// <param name="onComplete">On complete.</param>
        public IEnumerator DownloadSingleAsset(AssetBundleDownloadData data, Action<bool> onComplete)
        {
            string url = DownloadManager.DOWNLOADURL + data.FullName;
            //短路径->创建文件夹
            int lastIndex = data.FullName.LastIndexOf("/", StringComparison.Ordinal);
            if (lastIndex > -1)
            {
                string path = data.FullName.Substring(0, lastIndex);
                string localFilePath = DownloadManager.Instance.LOCALFILEPATH + path;
                if (!Directory.Exists(localFilePath))
                {
                    Directory.CreateDirectory(localFilePath);
                }
            }

            WWW www = new WWW(url);
            float timeout = Time.time;
            float progress = www.progress;
            while (!www.isDone)
            {
                if (progress > www.progress)
                {
                    timeout = Time.time;
                    progress = www.progress;
                }
                if (Time.time - timeout > DownloadManager.DOWNLOADTIMEOUT)
                {
                    if (onComplete != null)
                        onComplete(false);
                    yield break;
                }
                yield return null;
            }
            yield return www;

            if (www.error == null)
            {
                using (FileStream fs = new FileStream(DownloadManager.Instance.LOCALFILEPATH + data.FullName, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(www.bytes, 0, www.bytes.Length);
                }
            }
            else
            {
                if (onComplete != null)
                    onComplete(false);
                Debug.LogError("Download" + data.FullName + "fail:" + www.error);
            }
            //修改替换本地资源文件
            DownloadManager.Instance.ModifyLocalData(data);
            if (onComplete != null)
                onComplete(true);
        }
    }
}

public class AssetBundleDownloadData
{
    public string FullName;
    public string Md5;
    public int Size;
    public bool IsFirstData;
}
