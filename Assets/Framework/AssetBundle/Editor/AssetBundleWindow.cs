using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace WongJJ.Game.Core
{
    //===================================================
    //作    者：王家俊  http://www.unity3d.com  QQ：394916173
    //创建时间：2016-08-30 19:09:31
    //备    注：AssetBundle窗口
    //===================================================
    public class AssetBundleWindow : EditorWindow
    {
        private AssetBundleDAL _mDal;
        private readonly List<AssetBundleEntity> _mBundleList;
        private readonly Dictionary<string, bool> _mSelectedDict;

        private readonly string[] _mTags = new[]
        {
            "All",
            "Scene",
            "Role",
            "Effect",
            "Audio",
            "UI",
            "None"
        };

        private int _mTagIndex = 0;
        //标记的索引
        private int _mSelectTagIndex = -1;

        //选择的标记
        private readonly string[] _mBuildPlatForm = { "Mac", "Win", "iOS", "Android" };
        private int _mSelectBuildPlatFormIndex = -1;
#if UNITY_STANDALONE_OSX
        private int _mBuildPlatFormIndex = 0;
        private BuildTarget _mBuildtarget = BuildTarget.StandaloneOSXIntel64;
    
#elif UNITY_STANDALONE_WIN
	    private int _mBuildPlatFormIndex = 1;
        private BuildTarget _mBuildtarget = BuildTarget.StandaloneWindows64;

#elif UNITY_IPHONE
        private int _mBuildPlatFormIndex = 2;
        private BuildTarget _mBuildtarget = BuildTarget.iOS;

#elif UNITY_ANDROID
        private int _mBuildPlatFormIndex = 3;
        private BuildTarget _mBuildtarget = BuildTarget.Android;
#endif

        public AssetBundleWindow()
        {
            string xmlPath = "Assets/Framework/AssetBundle/Editor/AssetBundleConfig.xml";
            AssetBundleDAL dal = new AssetBundleDAL(xmlPath);
            Debug.Log(dal.GetBundleList().Count);
            _mBundleList = dal.GetBundleList();
            _mSelectedDict = new Dictionary<string, bool>();

            foreach (AssetBundleEntity t in _mBundleList)
            {
                _mSelectedDict[t.Key] = true;
            }
        }

        readonly Vector2 _pos = Vector2.zero;

        void OnGUI()
        {
            #region 选择栏
            if (_mBundleList == null)
                return;
            GUILayout.BeginHorizontal("box");

            //绘制下拉窗口 选择要配置文件里要打包的tag
            _mSelectTagIndex = EditorGUILayout.Popup(_mTagIndex, _mTags, GUILayout.Width(100));
            //选定按钮
            //			if (GUILayout.Button ("选定资源类型", GUILayout.Width (100))) {
            //				EditorApplication.delayCall = OnSelectedTag;
            //			}

            if (_mSelectTagIndex != _mTagIndex)
            {
                _mTagIndex = _mSelectTagIndex;
                EditorApplication.delayCall = OnSelectedTag;
            }

            //绘制下拉窗口 选择打包平台
            _mSelectBuildPlatFormIndex = EditorGUILayout.Popup(_mBuildPlatFormIndex, _mBuildPlatForm, GUILayout.Width(100));
            //			if (GUILayout.Button ("选定打包平台", GUILayout.Width (100))) {
            //				EditorApplication.delayCall = OnSelectedPlatform;
            //			}

            if (_mSelectBuildPlatFormIndex != _mBuildPlatFormIndex)
            {
                _mBuildPlatFormIndex = _mSelectBuildPlatFormIndex;
                EditorApplication.delayCall = OnSelectedPlatform;
            }

            if (GUILayout.Button("保存设置", GUILayout.Width(200)))
            {
                EditorApplication.delayCall = OnSaveAssetBundle;
            }

            if (GUILayout.Button("打包AssetBundle", GUILayout.Width(200)))
            {
                EditorApplication.delayCall = OnAssetBundle;
            }

            if (GUILayout.Button("清空AssetBundle", GUILayout.Width(200)))
            {
                EditorApplication.delayCall = OnClearAssetBundle;
            }

            if (GUILayout.Button("拷贝数据文件", GUILayout.Width(200)))
            {
                EditorApplication.delayCall = OnCopyDataTable;
            }

            if (GUILayout.Button("生成版本文件", GUILayout.Width(200)))
            {
                EditorApplication.delayCall = OnCreateVersionFile;
            }

            EditorGUILayout.Space();

            GUILayout.EndHorizontal();
            #endregion

            #region 内容栏
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("包名");
            GUILayout.Label("标记", GUILayout.Width(100));
            //			GUILayout.Label ("保存路径", GUILayout.Width (200));
            GUILayout.Label("文件夹", GUILayout.Width(200));
            //			GUILayout.Label ("大小", GUILayout.Width (100));
            GUILayout.Label("初始资源", GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            EditorGUILayout.BeginScrollView(_pos);
            for (int i = 0; i < _mBundleList.Count; i++)
            {
                AssetBundleEntity entity = _mBundleList[i];

                GUILayout.BeginHorizontal("box");
                _mSelectedDict[entity.Key] = GUILayout.Toggle(_mSelectedDict[entity.Key], "", GUILayout.Width(20));
                GUILayout.Label(entity.Name);
                GUILayout.Label(entity.Tag, GUILayout.Width(100));
                GUILayout.Label(entity.IsFolder.ToString(), GUILayout.Width(200));
                GUILayout.Label(entity.IsFirstData.ToString(), GUILayout.Width(200));
                //				GUILayout.Label (entity.ToWritePath, GUILayout.Width (200));
                //				GUILayout.Label (entity.Version.ToString (), GUILayout.Width (100));
                //				GUILayout.Label (entity.Size.ToString () + " KB", GUILayout.Width (100));
                GUILayout.EndHorizontal();

                foreach (string path in entity.PathList)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Space(40);
                    GUILayout.Label(path);
                    GUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
            #endregion
        }

        /// <summary>
        /// 清空
        /// </summary>
        private void OnClearAssetBundle()
        {
            string path = Application.dataPath + "/../AssetBundles/" + _mBuildPlatForm[_mBuildPlatFormIndex];
            if (System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path, true);
            }
            Debug.Log("清空完毕!!");
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        private void OnSaveAssetBundle()
        {
            List<AssetBundleEntity> bundles = new List<AssetBundleEntity>();

            foreach (AssetBundleEntity entity in _mBundleList)
            {
                if (_mSelectedDict[entity.Key])
                {
                    entity.IsChecked = true;
                    bundles.Add(entity);
                }
                else
                {
                    entity.IsChecked = false;
                    bundles.Add(entity);
                }
            }

            for (int i = 0; i < bundles.Count; i++)
            {
                AssetBundleEntity entity = bundles[i];
                if (entity.IsFolder)
                {
                    string[] folderArr = new string[entity.PathList.Count];
                    for (int j = 0; j < entity.PathList.Count; j++)
                    {
                        folderArr[j] = Application.dataPath + "/" + entity.PathList[j];
                    }
                    SaveFolderSettings(folderArr, !entity.IsChecked);
                }
                else
                {
                    string[] folderArr = new string[entity.PathList.Count];
                    for (int j = 0; j < entity.PathList.Count; j++)
                    {
                        folderArr[j] = Application.dataPath + "/" + entity.PathList[j];
                        SaveFolderSetting(folderArr[j], !entity.IsChecked);
                    }
                }
            }
            Debug.Log("对已找到的资源保存完毕,可以开始打包!!");
        }

        private void SaveFolderSettings(string[] folderArr, bool isSetNull)
        {
            //1.查看这个文件夹下的文件
            foreach (string folderPath in folderArr)
            {
                string[] arrFile = Directory.GetFiles(folderPath);
                //2.对文件进行设置
                foreach (string filePath in arrFile)
                {
                    //进行设置
                    SaveFolderSetting(filePath, isSetNull);
                }
                //3.查看这个文件夹下的子文件夹
                string[] arrFolder = Directory.GetDirectories(folderPath);
                SaveFolderSettings(arrFolder, isSetNull);
            }
        }

        private void SaveFolderSetting(string filePath, bool isSetNull)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Extension.Equals(".meta", StringComparison.CurrentCultureIgnoreCase))
            {
                int index = filePath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);
                //路径
                string newPath = filePath.Substring(index);
                //文件名
                string fileName = newPath.Replace("Assets/", "").Replace(file.Extension, "");
                //后缀
                string variant = file.Extension.Equals(".unity", StringComparison.CurrentCultureIgnoreCase) ? "unity3d" : "assetbundle";

                AssetImporter import = AssetImporter.GetAtPath(newPath);

                import.SetAssetBundleNameAndVariant(fileName, variant);

                if (isSetNull)
                    import.SetAssetBundleNameAndVariant(null, null);

                import.SaveAndReimport();
            }
        }

        /// <summary>
        /// 打包
        /// </summary>
        private void OnAssetBundle()
        {
            string toPath = Application.dataPath + "/../AssetBundles/" + _mBuildPlatForm[_mBuildPlatFormIndex];
            if (!Directory.Exists(toPath))
            {
                Directory.CreateDirectory(toPath);
            }
            BuildPipeline.BuildAssetBundles(toPath, BuildAssetBundleOptions.None, _mBuildtarget);

            //			List<AssetBundleEntity> needBundles = new List<AssetBundleEntity> ();
            //
            //			foreach (AssetBundleEntity entity in m_bundleList) {
            //				if (m_selectedDict [entity.Key]) {
            //					needBundles.Add (entity);
            //				}
            //			}
            //
            //			for (int i = 0; i < needBundles.Count; i++) {
            //				BuildAssetBundle (needBundles [i]);
            //			}
            //
            //			Debug.Log ("打包完毕!!");
            Debug.Log(string.Format("AssetBundle打包完毕!!目录:{0}", toPath));
        }

        //		private void BuildAssetBundle (AssetBundleEntity entity)
        //		{
        //			AssetBundleBuild[] builds = new AssetBundleBuild[1];
        //			AssetBundleBuild build = new AssetBundleBuild ();
        //			//包名
        //			build.assetBundleName = string.Format (entity.Name + "{0}", entity.Tag.Equals ("Scene", StringComparison.CurrentCultureIgnoreCase) ? ".unity3d" : ".assetbundle");
        //			//资源路径
        //			build.assetNames = entity.PathList.ToArray ();
        //
        //			builds [0] = build;
        //
        //			//输出目标路径
        //			string writePath = Application.dataPath + "/../AssetBundles/" + buildPlatForm [buildPlatFormIndex] + entity.ToWritePath;
        //			if (!System.IO.Directory.Exists (writePath)) {
        //				System.IO.Directory.CreateDirectory (writePath);
        //			}
        //			BuildPipeline.BuildAssetBundles (writePath, builds, BuildAssetBundleOptions.None, buildtarget);
        //		}

        /// <summary>
        /// 选定平台
        /// </summary>
        private void OnSelectedPlatform()
        {
            switch (_mBuildPlatFormIndex)
            {
                case 0:
                    _mBuildtarget = BuildTarget.StandaloneOSXIntel64;
                    break;
                case 1:
                    _mBuildtarget = BuildTarget.StandaloneWindows64;
                    break;
                case 2:
                    _mBuildtarget = BuildTarget.iOS;
                    break;
                case 3:
                    _mBuildtarget = BuildTarget.Android;
                    break;
            }
        }

        /// <summary>
        /// 拷贝数据文件到指定目录
        /// </summary>
        private void OnCopyDataTable()
        {
            string fromPath = Application.dataPath + "/Downloads/Datas/";
            string toPath = Application.dataPath + "/../AssetBundles/" + _mBuildPlatForm[_mBuildPlatFormIndex] + "/Downloads/Datas/";
            if (!Directory.Exists(fromPath))
            {
                Debug.Log(string.Format("{0}不存在，请先将数据文件目录拖进改目标目录", fromPath));
                return;
            }
            else
            {
                Util.CopyDirectory(fromPath, toPath);
                Debug.Log("Datas目录拷贝完成!!");
            }
        }

        /// <summary>
        /// 生成版本文件
        /// </summary>
        private void OnCreateVersionFile()
        {
            string path = Application.dataPath + "/../AssetBundles/" + _mBuildPlatForm[_mBuildPlatFormIndex];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string versionFile = path + "/Version.txt";
            //存在则删除
            Util.DeleteFile(versionFile);

            StringBuilder strBuilder = new StringBuilder();

            DirectoryInfo directory = new DirectoryInfo(path);

            FileInfo[] fileInfos = directory.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < fileInfos.Length; i++)
            {
                FileInfo fileinfo = fileInfos[i];

                //全民 包含路径扩展命
                string fullName = fileinfo.FullName;
                //相对路径
                string name = fullName.Substring(fullName.IndexOf(_mBuildPlatForm[_mBuildPlatFormIndex], StringComparison.Ordinal) + _mBuildPlatForm[_mBuildPlatFormIndex].Length + 1);

                //文件的MD5
                string md5 = Util.GetFileMD5(fullName);

                if (md5 == null)
                    continue;

                //文件大小
                string size = Math.Ceiling(fileinfo.Length / 1024f).ToString();

                //是否初始
                bool isFirstData = false;
                bool isBreak = false;

                for (int j = 0; j < _mBundleList.Count; j++)
                {
                    foreach (string xmlPath in _mBundleList[j].PathList)
                    {
                        string tempPath = xmlPath;
                        if (xmlPath.IndexOf(".", StringComparison.Ordinal) != -1)
                        {
                            tempPath = xmlPath.Substring(0, xmlPath.IndexOf(".", StringComparison.Ordinal));
                        }
                        if (name.IndexOf(tempPath, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            isFirstData = _mBundleList[j].IsFirstData;
                            isBreak = true;
                            break;
                        }
                    }
                    if (isBreak)
                        break;
                }

                //Datas文件夹下的肯定都是需要初始化的
                if (name.IndexOf("Datas", StringComparison.Ordinal) != -1)
                    isFirstData = true;

                string strLine = string.Format("{0} {1} {2} {3}", name, md5, size, isFirstData ? "1" : "0");
                //                string strLine = $"{name} {md5} {size} {(isFirstData ? 1 : 0)}";
                strBuilder.AppendLine(strLine);
            }
            Util.CreateTextFile(versionFile, strBuilder.ToString());
            Debug.Log("版本文件生成成功!!");
        }

        /// <summary>
        /// 选定Tag
        /// </summary>
        private void OnSelectedTag()
        {
            switch (_mTagIndex)
            {
                case 0:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = true;
                    }
                    break;
                case 1:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = entity.Tag.Equals("Scene", StringComparison.CurrentCultureIgnoreCase);
                    }
                    break;
                case 2:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = entity.Tag.Equals("Role", StringComparison.CurrentCultureIgnoreCase);
                    }
                    break;
                case 3:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = entity.Tag.Equals("Effect", StringComparison.CurrentCultureIgnoreCase);
                    }
                    break;
                case 4:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = entity.Tag.Equals("Audio", StringComparison.CurrentCultureIgnoreCase);
                    }
                    break;
                case 5:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = entity.Tag.Equals("UI", StringComparison.CurrentCultureIgnoreCase);
                    }
                    break;
                case 6:
                    foreach (AssetBundleEntity entity in _mBundleList)
                    {
                        _mSelectedDict[entity.Key] = false;
                    }
                    break;
            }
        }
    }
}