using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace WongJJ.Game.Core
{
    /**
     * @Summary:AssetBundle资源下载器
     * @Author：WongJJ
     * @Date: 2017-05-09 20:14:54
     */
    public class AssetBundleDownloadRoutine : MonoBehaviour
    {
        private readonly List<AssetBundleDownloadData> _mDownloadList = new List<AssetBundleDownloadData>();
        private AssetBundleDownloadData _mCurrentDownloadData; //正在下载的资源；
        /// <summary>
        /// 当前下载器需要下载的数量；
        /// </summary>
        private int _mNeedDownloadCount;
        public int NeedDownloadCount
        {
            get { return _mNeedDownloadCount; }
        }
        /// <summary>
        /// 当前下载器下载完成的数量
        /// </summary>
        /// <value>The complete download count.</value>
        private int _mCompleteDownloadCount;
        public int CompleteDownloadCount
        {
            get { return _mCompleteDownloadCount; }
        }
        /// <summary>
        /// 当前正在下载资源的大小
        /// </summary>
        private int _mCurrentDownloadSize;
        public int CurrentDownloadSize
        {
            get { return _mCurrentDownloadSize; }
        }
        /// <summary>
        /// 已下载资源完成的大小
        /// </summary>
        private int _mCompleteDownloadSize;
        public int CompleteDownloadSize
        {
            get { return _mCompleteDownloadSize + _mCurrentDownloadSize; }
        }
        /// <summary>
        /// 是否开始下载
        /// </summary>
        private bool _mIsStartDownload;
        public bool IsStartDownload
        {
            get { return _mIsStartDownload; }
        }

        /// <summary>
        /// 添加一个下载资源
        /// </summary>
        /// <param name="data">Data.</param>
        public void AddAssetDownload(AssetBundleDownloadData data)
        {
            if (data != null)
                _mDownloadList.Add(data);
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void StartDownload()
        {
            _mIsStartDownload = true;
            _mNeedDownloadCount = _mDownloadList.Count;
        }

        void Update()
        {
            if (_mIsStartDownload)
            {
                _mIsStartDownload = false;
                StartCoroutine(Download());
            }
        }

        IEnumerator Download()
        {
            if (_mNeedDownloadCount == 0)
                yield break;
            _mCurrentDownloadData = _mDownloadList[0];
            string url = DownloadManager.DOWNLOADURL + _mCurrentDownloadData.FullName;
            //短路径->创建文件夹
            int lastIndexOf = _mCurrentDownloadData.FullName.LastIndexOf("/", StringComparison.Ordinal);
            if (lastIndexOf > -1)
            {
                string path = _mCurrentDownloadData.FullName.Substring(0, lastIndexOf);
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
                    _mCurrentDownloadSize = (int)(_mCurrentDownloadData.Size * progress);
                }
                if (Time.time - timeout > DownloadManager.DOWNLOADTIMEOUT)
                {
                    Debug.LogError("Download" + _mCurrentDownloadData.FullName + "time out!");
                    yield break;
                }
                yield return null;
            }
            yield return www;

            if (www.error == null)
            {
                using (FileStream fs = new FileStream(DownloadManager.Instance.LOCALFILEPATH + _mCurrentDownloadData.FullName, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(www.bytes, 0, www.bytes.Length);
                }
            }
            else
            {
                Debug.LogError("Download" + _mCurrentDownloadData.FullName + "fail:" + www.error);
            }

            _mCurrentDownloadSize = 0;
            _mCompleteDownloadSize += _mCurrentDownloadData.Size;
            _mCompleteDownloadCount++;
            print("Download Complete!->:" + _mCurrentDownloadData.FullName);
            //修改替换本地资源文件
            DownloadManager.Instance.ModifyLocalData(_mCurrentDownloadData);

            _mDownloadList.RemoveAt(0);
            if (_mDownloadList.Count == 0)
                _mDownloadList.Clear();
            else
                _mIsStartDownload = true;
        }
    }
}