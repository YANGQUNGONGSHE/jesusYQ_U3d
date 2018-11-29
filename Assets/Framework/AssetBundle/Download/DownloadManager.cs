using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace WongJJ.Game.Core
{ /**
     * @Summary:AssetBundle资源下载管理器
     * @Author：WongJJ
     * @Date: 2017-05-09 16:59:54
     */
    public class DownloadManager : Singletion<DownloadManager>
    {
        //1.0版本配置地址
        //public const string DOWNLOADBASEURL = "http://test8922.oss-cn-shanghai.aliyuncs.com/";
        //1.1版本配置地址
        //public const string DOWNLOADBASEURL = "http://testsheep2018.oss-cn-shanghai.aliyuncs.com/";
        //1.2版本配置地址
        //public const string DOWNLOADBASEURL = "http://sheep2.oss-cn-shanghai.aliyuncs.com/";
        //1.3版本配置地址
         public const string DOWNLOADBASEURL = "http://sheep4.oss-cn-shanghai.aliyuncs.com/";
        //版本文件名
        public const string VERSIONTEXTNAME = "Version.txt";
        //下载器数量
        public const int DOWNLOADROUTINENUM = 5;
        //超时
        public const int DOWNLOADTIMEOUT = 60;
        //本地资源路径
        public string LOCALFILEPATH = Application.persistentDataPath + "/";
        //StreamingAsset文件夹中的AssetBundle路径
        private string m_StreamingAsesstPath;
        //本地版本文件的路径
        private string m_LocalVersionPath;
        //下载进度，<已下载，总数>
        public Action<int, int> OnProgress;
        //监听AssetBundle检查更新下载一系列全部操作完成
        public Action OnInitComplete;
        //监听是否没有资源需要更新
        public Action<bool> OnNoAssetUpdate;

#if UNITY_STANDALONE_WIN
        public const string DOWNLOADURL = DOWNLOADBASEURL + "Win/";
#elif UNITY_STANDALONE_OSX
        public const string DOWNLOADURL = DOWNLOADBASEURL + "Mac/";
#elif UNITY_ANDROID
        public const string DOWNLOADURL = DOWNLOADBASEURL + "Android/";
#elif UNITY_IPHONE
        public const string DOWNLOADURL = DOWNLOADBASEURL + "iOS/";
#endif

        //需要下载的数据列表
        private List<AssetBundleDownloadData> m_NeedDownloadList = new List<AssetBundleDownloadData>();
        //本地数据列表
        private List<AssetBundleDownloadData> m_LocalDataList = new List<AssetBundleDownloadData>();
        //服务器数据列表
        private List<AssetBundleDownloadData> m_ServerDataList = new List<AssetBundleDownloadData>();

        /// <summary>
        /// AssetBundle初始化
        /// </summary>
        public void InitAssetBundle(Action _OnInitComplete)
        {
            OnInitComplete = _OnInitComplete;
            m_LocalVersionPath = LOCALFILEPATH + VERSIONTEXTNAME; //本地版本文件的路径
            //本地文件是否存在，存在说明已经有资源
            if (File.Exists(m_LocalVersionPath))
            {
                //如果有资源，检查更新
                CheckVersion();
            }
            else
            {
                //如果没有资源，先初始化StreamingAsset资源，然后再检查更新
                m_StreamingAsesstPath = "file:///" + Application.streamingAssetsPath + "/AssetBundle/";
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
                m_StreamingAsesstPath = Application.streamingAssetsPath + "/AssetBundle/";
#endif
                string versionFileUrl = m_StreamingAsesstPath + VERSIONTEXTNAME;
                CoroutineController.Instance.StartCoroutine(ReadStreamingAssetVersionFile(versionFileUrl, OnReadVersionFileFinishCallback));
            }
        }

        /// <summary>
        /// 初始化StreamingAsset文件夹中的资源
        /// </summary>
        /// <returns>The streaming asset.</returns>
        /// <param name="content">Content.</param>
        private IEnumerator InitStreamingAsset(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                CheckVersion();
                yield break;
            }

            string[] lines = content.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string[] datas = lines[i].Split(' ');
                string fileUrl = datas[0]; //短路径；
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    yield return CoroutineController.Instance.StartCoroutine(AssetLoadToLocal(m_StreamingAsesstPath + fileUrl, LOCALFILEPATH + fileUrl));
                }
                Debug.Log(string.Format("初始化资源{0}/{1}", i + 1, lines.Length));
            }

            yield return CoroutineController.Instance.StartCoroutine(AssetLoadToLocal(m_StreamingAsesstPath + VERSIONTEXTNAME, LOCALFILEPATH + VERSIONTEXTNAME));
            CheckVersion();
        }

        /// <summary>
        /// 将初始资源从StreamingAsesstPath写入到PersistentDataPath
        /// </summary>
        /// <returns>The load to local.</returns>
        /// <param name="fileUrl">File URL.</param>
        /// <param name="toPath">To path.</param>
        private IEnumerator AssetLoadToLocal(string fileUrl, string toPath)
        {
            using (WWW www = new WWW(fileUrl))
            {
                yield return www;
                if (www.error == null)
                {
                    int lastIndexof = toPath.LastIndexOf('/');
                    if (lastIndexof != -1)
                    {
                        string localPath = toPath.Substring(0, lastIndexof);
                        if (!Directory.Exists(localPath))
                        {
                            Directory.CreateDirectory(localPath);
                        }
                    }
                    using (FileStream fs = File.Create(toPath, www.bytes.Length))
                    {
                        fs.Write(www.bytes, 0, www.bytes.Length);
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 读取StreamingAsset文件夹中的版本信息文件
        /// </summary>
        /// <returns>The streaming asset version file.</returns>
        /// <param name="viersionFilePath">Viersion file path.</param>
        /// <param name="OnReadVersionFileFinish">On read version file finish.</param>
        private IEnumerator ReadStreamingAssetVersionFile(string viersionFilePathUrl, Action<string> OnReadVersionFileFinish)
        {
            using (WWW www = new WWW(viersionFilePathUrl))
            {
                yield return www;
                if (www.error == null)
                {
                    if (OnReadVersionFileFinish != null)
                    {
                        OnReadVersionFileFinish(Encoding.UTF8.GetString(www.bytes));
                    }
                }
                else
                {
                    OnReadVersionFileFinish("");
                }
            }
        }

        /// <summary>
        /// 读取StreamingAsset文件夹中的版本信息文件完毕
        /// </summary>
        /// <param name="versionContent">Version content.</param>
        private void OnReadVersionFileFinishCallback(string versionContent)
        {
            CoroutineController.Instance.StartCoroutine(InitStreamingAsset(versionContent));
        }

        /// <summary>
        /// 检查版本文件
        /// </summary>
        public void CheckVersion()
        {
            string serverVersionPath = DOWNLOADURL + VERSIONTEXTNAME;
            AssetBundleDownload.Instance.ConfigDownloadServerVersion(serverVersionPath, OnDownloadVersionCallback);
        }

        /// <summary>
        /// 下载服务器版本信息完成回调
        /// </summary>
        /// <param name="datas">Datas.</param>
        private void OnDownloadVersionCallback(List<AssetBundleDownloadData> datas)
        {
            m_ServerDataList = datas;
            if (File.Exists(m_LocalVersionPath))
            {
                //存在，和服务器的进行对比
                Dictionary<string, string> serverDic = ParseVersionInfoToDic(datas); //服务端<fullname,md5>
                string content = System.IO.File.ReadAllText(m_LocalVersionPath);
                m_LocalDataList = ParseVersionInfo(content);
                Dictionary<string, string> clientDic = ParseVersionInfoToDic(content);//客户端<fullname,md5>

                //1.检查有新加的初始资源
                for (int i = 0; i < datas.Count; i++)
                {
                    if (datas[i].IsFirstData && !clientDic.ContainsKey(datas[i].FullName))
                    {
                        m_NeedDownloadList.Add(datas[i]);
                    }
                }

                //2.检查已经下载过的，但是有更新的资源
                foreach (var item in clientDic)
                {
                    if (serverDic.ContainsKey(item.Key) && serverDic[item.Key] != item.Value)  //MD5不一致
                    {
                        AssetBundleDownloadData data = GetDownloadData(item.Key, datas);
                        if (data != null)
                        {
                            m_NeedDownloadList.Add(data);
                        }
                    }
                }
            }
            else
            {
                //不存在，那么下载配置好的需要初始化的资源
                for (int i = 0; i < datas.Count; i++)
                {
                    if (datas[i].IsFirstData)
                    {
                        m_NeedDownloadList.Add(datas[i]);
                    }
                }
            }

            //没有需要下载的；
            if (m_NeedDownloadList.Count == 0)
            {
                if (OnNoAssetUpdate != null)
                    OnNoAssetUpdate(true);
                return;
            }

            if (OnNoAssetUpdate != null)
                OnNoAssetUpdate(false);
            //开始下载
            AssetBundleDownload.Instance.DownloadAsset(m_NeedDownloadList);
        }

        /// <summary>
        /// 解析版本文件
        /// </summary>
        /// <param name="versionContent">Version content.</param>
        public List<AssetBundleDownloadData> ParseVersionInfo(string versionContent)
        {
            List<AssetBundleDownloadData> list = new List<AssetBundleDownloadData>();
            string[] lines = versionContent.Split('\n');
            if (lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(' ');
                    if (data.Length == 4)
                    {
                        AssetBundleDownloadData assetData = new AssetBundleDownloadData();
                        assetData.FullName = data[0];
                        assetData.Md5 = data[1];
                        assetData.Size = data[2].ToInt();
                        assetData.IsFirstData = data[3].ToInt() == 1;
                        list.Add(assetData);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 解析版本文件
        /// </summary>
        /// <returns>The version info.<FullName,MD5></returns>
        /// <param name="datas">Datas.</param>
        public Dictionary<string, string> ParseVersionInfoToDic(List<AssetBundleDownloadData> datas)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < datas.Count; i++)
            {
                dic[datas[i].FullName] = datas[i].Md5;
            }
            return dic;
        }

        /// <summary>
        /// 解析版本文件
        /// </summary>
        /// <returns>The version info.<FullName,MD5></returns>
        /// <param name="versionContent">Version content.</param>
        public Dictionary<string, string> ParseVersionInfoToDic(string versionContent)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] lines = versionContent.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string[] datas = lines[i].Split(' ');
                if (datas.Length == 4)
                {
                    dic[datas[0]] = datas[1];
                }
            }
            return dic;
        }

        /// <summary>
        /// 根据资源名称获取实体
        /// </summary>
        /// <returns>The download data.</returns>
        /// <param name="fullname">Fullname.</param>
        /// <param name="datas">Datas.</param>
        public AssetBundleDownloadData GetDownloadData(string fullname, List<AssetBundleDownloadData> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].FullName.Equals(fullname))
                {
                    return datas[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 从服务器资源列表查找一个资源
        /// </summary>
        /// <returns>The server asset data.</returns>
        /// <param name="path">Path.</param>
        public AssetBundleDownloadData GetServerAssetData(string path)
        {
            if (m_ServerDataList == null)
                return null;

            for (int i = 0; i < m_ServerDataList.Count; i++)
            {
                if (path.Equals(m_ServerDataList[i].FullName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return m_ServerDataList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 修改本地文件
        /// </summary>
        /// <param name="data">Data.</param>
        public void ModifyLocalData(AssetBundleDownloadData data)
        {
            if (m_LocalDataList == null)
                return;
            bool isExists = false;
            for (int i = 0; i < m_LocalDataList.Count; i++)
            {
                if (m_LocalDataList[i].FullName.Equals(data.FullName, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    m_LocalDataList[i].Md5 = data.Md5;
                    m_LocalDataList[i].Size = data.Size;
                    m_LocalDataList[i].IsFirstData = data.IsFirstData;
                    isExists = true;
                    break;
                }
            }
            if (!isExists)
            {
                m_LocalDataList.Add(data);
            }

            SaveLocalVersion();
        }

        /// <summary>
        /// 保存本地版本文件
        /// </summary>
        public void SaveLocalVersion()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_LocalDataList.Count; i++)
            {
                sb.AppendLine(string.Format("{0} {1} {2} {3}", m_LocalDataList[i].FullName, m_LocalDataList[i].Md5, m_LocalDataList[i].Size, m_LocalDataList[i].IsFirstData));
            }
            Util.CreateTextFile(m_LocalVersionPath, sb.ToString());
        }
    }
}
