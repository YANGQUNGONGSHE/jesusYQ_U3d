using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

namespace WongJJ.Game.Core.SimpleUIManager
{
    /**
     * @Summary:所有面板管理器
     * @Author：WongJJ
     * @Date: 2017-07-22 16:34:52
     * @Remark: 重构 2017年07月22日16:34:42
     */
    public static class UIManager
    {
        #region Config
        private static eLoadAssetType _loadAssetType = eLoadAssetType.File;
        public static string UiPanelInfoJsonPath = "UIPanelInfo";
        public static string RootCanvasName = "Canvas";
        #endregion

        #region Event
        public static Action OnSystemOpenUi;
        public static Action<UIPanelType> OnSystemOpenUiWithType; //no test
        public static Action<UIPanelType> OnSystemCloseUi; // no test
        #endregion

        private static readonly Dictionary<UIPanelType, string> MPanelPaths;
        private static readonly Dictionary<UIPanelType, BasePanel> MPanelDict;

        /// <summary>
        /// 上次显示的窗口
        /// </summary>
        public static BasePanel PrevVisiblePanel { get { return MPrevVisiblePanel; } }
        internal static BasePanel MPrevVisiblePanel;

        /// <summary>
        /// 当前显示的窗口
        /// </summary>
        public static BasePanel CurrentVisiblePanel { get { return MCurrentVisiblePanel; } }
        internal static BasePanel MCurrentVisiblePanel;

        /// <summary>
        /// Canvas组件->UIRoot
        /// </summary>
        /// <value>The canvascomponent.</value>
        public static Transform Canvascomponent
        {
            get
            {
                if (_mCanvascomponent == null)
                {
                    _mCanvascomponent = GameObject.Find(RootCanvasName).transform;
                }
                return _mCanvascomponent;
            }
        }
        private static Transform _mCanvascomponent;

        /// <summary>
        /// Initializes the <see cref="T:WongJJ.Game.SimpleUIManager.UIManagerNew"/> class.
        /// </summary>
        static UIManager()
        {
            MPanelPaths = new Dictionary<UIPanelType, string>();
            MPanelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        /// <summary>
        /// 根据资源的加载方式初始化
        /// </summary>
        /// <returns>The init.</returns>
        /// <param name="loadAssetType">eLoadAssetType</param>
        public static void Init(eLoadAssetType loadAssetType)
        {
            _loadAssetType = loadAssetType;
            InitPanelPath();
        }

        /// <summary>
        /// 初始化路径配置
        /// </summary>
        internal static void InitPanelPath()
        {
            TextAsset asset = Resources.Load<TextAsset>(UiPanelInfoJsonPath);
            UIPanelPathJsonList pathJsonList = JsonUtility.FromJson<UIPanelPathJsonList>(asset.text);
            foreach (UIPanelInfo info in pathJsonList.InfoList)
            {
                string path = string.Empty;
                switch (_loadAssetType)
                {
                    case eLoadAssetType.Resource:
                        path = info.ResPath;
                        break;
                    case eLoadAssetType.AssetBundle:
                        path = info.AbPath;
                        break;
                    case eLoadAssetType.File:
                        path = info.NabPath;
                        break;
                }
                MPanelPaths.Add(info.Type, path);
            }
        }

        /// <summary>
        /// 生成一个指定窗口
        /// </summary>
        /// <returns>The open.</returns>
        /// <param name="type">窗口类型.</param>
        public static BasePanel Open(UIPanelType type)
        {
            return Open(type, new Vector2(Screen.width, 0));
        }

        /// <summary>
        /// 生成一个指定窗口
        /// </summary>
        /// <returns>The open.</returns>
        /// <param name="type">Type.</param>
        /// <param name="startPos">Start position.</param>
        public static BasePanel Open(UIPanelType type, Vector2 startPos)
        {
            return Open(type, startPos, true);
        }

        /// <summary>
        /// Open the specified type and allowAutoShow.
        /// </summary>
        /// <returns>The open.</returns>
        /// <param name="type">Type.</param>
        /// <param name="allowAutoShow">If set to <c>true</c> allow auto show.</param>
        public static BasePanel Open(UIPanelType type, bool allowAutoShow)
        {
            return Open(type, new Vector2(Screen.width, 0), allowAutoShow);
        }

        /// <summary>
        /// Open the specified type, startPos and allowAutoShow.
        /// </summary>
        /// <returns>The open.</returns>
        /// <param name="type">Type.</param>
        /// <param name="startPos">Start position.</param>
        /// <param name="allowAutoShow">If set to <c>true</c> allow auto show.</param>
        public static BasePanel Open(UIPanelType type, Vector2 startPos, bool allowAutoShow)
        {
            BasePanel panel;
            MPanelDict.TryGetValue(type, out panel);
            if (panel == null)
            {
                string resPath;
                MPanelPaths.TryGetValue(type, out resPath);
                if (!string.IsNullOrEmpty(resPath))
                {
                    GameObject go = null;
                    switch (_loadAssetType)
                    {
                        //case eLoadAssetType.Resource:
                        //go = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(resPath));
                        //break;
#if ASSETBUNDLE_MODEL
                        case eLoadAssetType.AssetBundle:
                            go = AssetBundleLoaderManager.Instance.LoadInstanceSync(resPath + ".assetbundle", type.ToString());
                            break;
#else
                        case eLoadAssetType.File:
                            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", resPath + ".prefab"));
                            go = UnityEngine.Object.Instantiate(prefab);
                            break;
#endif
                    }
                    if (go != null)
                    {
                        go.transform.SetParent(Canvascomponent, false);
                        go.SetActive(false);
                        go.GetComponent<RectTransform>().offsetMin = startPos;
                        go.GetComponent<RectTransform>().offsetMax = startPos;
                        panel = go.GetComponent<BasePanel>();
                    }
                    MPanelDict.Add(type, panel);
                    if (allowAutoShow)
                        Show(type);
                    if (OnSystemOpenUi != null)
                        OnSystemOpenUi();
                    return panel;
                }
                else
                {
                    UnityEngine.Debug.LogError(string.Format("没有找到类型为{0}的路径，请检查是否配置路径", type.ToString()));
                    return null;
                }
            }
            panel.gameObject.SetActive(false);
            panel.gameObject.GetComponent<RectTransform>().offsetMin = startPos;
            panel.gameObject.GetComponent<RectTransform>().offsetMax = startPos;
            panel.gameObject.SetActive(true);
            if (allowAutoShow)
                Show(type);
            if (OnSystemOpenUi != null)
                OnSystemOpenUi();
            return panel;
        }

        /// <summary>
        /// 使窗体可见（显示）
        /// 如果缓存中未有该类型窗口，系统会尝试新建并显示
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="type">Type.</param>
        public static BasePanel Show(UIPanelType type)
        {
            BasePanel panel;
            MPanelDict.TryGetValue(type, out panel);
            if (panel == null)
            {
                UnityEngine.Debug.LogWarning(string.Format("没有找到{0}窗口，系统自动创建了一个", type.ToString()));
                return Open(type, true);
            }
            panel.BeginShow();
            panel.Show();
            BasePanel tmp = MCurrentVisiblePanel;
            MCurrentVisiblePanel = panel;
            MPrevVisiblePanel = tmp;
            return panel;
        }

        /// <summary>
        /// 使窗体不可见（隐藏）
        /// </summary>
        /// <returns>The hide.</returns>
        /// <param name="type">Type.</param>
        public static void Hide(UIPanelType type)
        {
            BasePanel panel;
            MPanelDict.TryGetValue(type, out panel);
            if (panel == null)
                return;
            panel.Hide();
        }

        /// <summary>
        /// 销毁一个窗体
        /// </summary>
        /// <returns>The close.</returns>
        /// <param name="type">Type.</param>
        public static void Close(UIPanelType type)
        {
            Close(type, false);
        }

        /// <summary>
        /// 销毁一个窗体
        /// </summary>
        /// <returns>The close.</returns>
        /// <param name="type">Type.</param>
        /// <param name="isImmediatelyDestory">是否不经过隐藏动画立即释放.</param>
        public static void Close(UIPanelType type, bool isImmediatelyDestory)
        {
            BasePanel panel;
            MPanelDict.TryGetValue(type, out panel);
            if (panel == null)
                return;
            if (isImmediatelyDestory)
            {
                UnityEngine.Object.Destroy(panel.gameObject);
            }
            else
            {
                panel.Hide();
                UnityEngine.Object.Destroy(panel.gameObject, panel.AnimationTime());
            }
            MPanelDict.Remove(type);
        }

        /// <summary>
        /// 查询一个窗体是否在缓存中
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasInCache(UIPanelType type)
        {
            BasePanel panel;
            MPanelDict.TryGetValue(type, out panel);
            return panel != null;
        }

        #region 废弃

        //        #region Config
        //        private static eLoadAssetType LoadAssetType = eLoadAssetType.Resource;
        //        public static string UIPanelInfoJsonPath = "UIPanelInfo";
        //        public static string RootCanvasName = "Canvas";
        //        #endregion

        //        public static Action OnSystemOpenUI;
        //        public static Action<UIPanelType> OnSystemOpenUIWithType; //no test
        //        public static Action<UIPanelType> OnSystemCloseUI; // no test

        //        private static Dictionary<UIPanelType, string> m_panelPaths;
        //        private static Dictionary<UIPanelType, BasePanel> m_panelDict;

        //        private static Stack<BasePanel> m_stack;

        //        private static Transform m_Canvascomponent;
        //        public static Transform Canvascomponent
        //        {
        //            get
        //            {
        //                if (m_Canvascomponent == null)
        //                {
        //                    m_Canvascomponent = GameObject.Find(RootCanvasName).transform;
        //                }
        //                return m_Canvascomponent;
        //            }
        //        }

        //        static UIManager()
        //        {
        //            m_panelPaths = new Dictionary<UIPanelType, string>();
        //            m_panelDict = new Dictionary<UIPanelType, BasePanel>();
        //            m_stack = new Stack<BasePanel>();
        //        }

        //        static public void Init(eLoadAssetType loadAssetType)
        //        {
        //            LoadAssetType = loadAssetType;
        //            InitPanelPath();
        //        }

        //        static private void InitPanelPath()
        //        {
        //            TextAsset asset = Resources.Load<TextAsset>(UIPanelInfoJsonPath);
        //            UIPanelPathJsonList pathJsonList = JsonUtility.FromJson<UIPanelPathJsonList>(asset.text);
        //            foreach (UIPanelInfo info in pathJsonList.infoList)
        //            {
        //                string path = string.Empty;
        //                switch (LoadAssetType)
        //                {
        //                    case eLoadAssetType.Resource:
        //                        path = info.ResPath;
        //                        break;
        //                    case eLoadAssetType.AssetBundle:
        //                        path = info.ABPath;
        //                        break;
        //                    case eLoadAssetType.File:
        //                        path = info.NABPath;
        //                        break;
        //                }
        //                m_panelPaths.Add(info.type, path);
        //            }
        //        }

        //        static public BasePanel Open(UIPanelType type)
        //        {
        //            BasePanel panel = GetPanel(type);
        //            if (m_stack.Count > 0)
        //            {
        //                foreach (BasePanel panelinStack in m_stack)
        //                {
        //                    if (panelinStack.gameObject == null)
        //                    {
        //                        m_stack.Pop();
        //                    }
        //                    else
        //                    {
        //                        BasePanel prePanel = m_stack.Peek();
        //                        prePanel.OnPause();
        //                        break;
        //                    }
        //                }
        //            }
        //            panel.OnOpen();
        //            m_stack.Push(panel);
        //            if (OnSystemOpenUI != null) OnSystemOpenUI();
        //            if (OnSystemOpenUIWithType != null) OnSystemOpenUIWithType(type);
        //            return panel;
        //        }

        //        static public void Close(UIPanelType type, bool isKeepMm = true)
        //        {
        //            BasePanel panel;
        //            if (m_panelDict.ContainsKey(type))
        //            {
        //                m_panelDict.TryGetValue(type, out panel);
        //                if (!isKeepMm)
        //                {
        //                    panel.StartCoroutine(DestroyObj(panel.AnimationTime(), panel));
        //                    m_panelDict.Remove(type);
        //                }
        //                panel.OnClose();
        //                if (OnSystemCloseUI != null) OnSystemCloseUI(type);

        //                BasePanel Checkpanel = m_stack.Pop();

        //                if (m_stack.Count <= 0)
        //                    return;
        //                else
        //                    m_stack.Peek().OnResume();
        //            }
        //            else
        //            {
        //                UnityEngine.Debug.LogWarning("找不到指定窗口进行关闭");
        //                return;
        //            }
        //        }

        //        static public void Close(bool isKeepMm = true)//, eUIAnimationType useAmType = eUIAnimationType.UseSystem_ConsistentDirection)
        //        {
        //            if (m_stack.Count <= 0)
        //                return;

        //            BasePanel panel = m_stack.Pop();
        //            if (!isKeepMm)
        //            {
        //                panel.StartCoroutine(DestroyObj(panel.AnimationTime(), panel));
        //            }
        //            panel.OnClose();
        //            if (OnSystemCloseUI != null) OnSystemCloseUI(panel.GetUIType());

        //            if (m_stack.Count <= 0)
        //                return;
        //            else
        //                m_stack.Peek().OnResume();
        //        }

        //        static private BasePanel GetPanel(UIPanelType type)
        //        {
        //            BasePanel panel;
        //            m_panelDict.TryGetValue(type, out panel);
        //            if (panel == null)
        //            {
        //                string resPath = string.Empty;
        //                m_panelPaths.TryGetValue(type, out resPath);
        //                if (!string.IsNullOrEmpty(resPath))
        //                {
        //                    GameObject go = null;
        //                    switch (LoadAssetType)
        //                    {
        //                        case eLoadAssetType.Resource:
        //                            go = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(resPath));
        //                            break;
        //#if ASSETBUNDLE_MODEL
        //                        case eLoadAssetType.AssetBundle:
        //                            go = AssetBundleLoaderManager.Instance.LoadInstanceSync(resPath + ".assetbundle", type.ToString());
        //                            break;
        //#else
        //                        case eLoadAssetType.File:
        //                            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", resPath + ".prefab"));
        //                            go = UnityEngine.Object.Instantiate(prefab);
        //                            break;
        //#endif
        //                }
        //                go.transform.SetParent(Canvascomponent, false);
        //                panel = go.GetComponent<BasePanel>();
        //                m_panelDict.Add(type, panel);
        //                return panel;
        //            }
        //            else
        //            {
        //                UnityEngine.Debug.LogError("没有找到指定UI的路径，请检查是否配置路径");
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return panel;
        //        }
        //    }

        //    static public BasePanel GetPanelByType(UIPanelType type)
        //    {
        //        BasePanel panel;
        //        m_panelDict.TryGetValue(type, out panel);
        //        if (panel == null)
        //        {
        //            UnityEngine.Debug.LogError("没有找到该窗口");
        //            return null;
        //        }
        //        return panel;
        //    }

        //    static private IEnumerator DestroyObj(float time, BasePanel panel)
        //    {
        //        yield return new WaitForSeconds(time);
        //        m_panelDict.Remove(panel.GetUIType());
        //        UnityEngine.Object.Destroy(panel.gameObject);
        //    }
        //}

        //[Serializable]
        //class UIPanelPathJsonList
        //{
        //    public List<UIPanelInfo> infoList = null;
        //}

        //public enum eLoadAssetType
        //{
        //    Resource,
        //    AssetBundle,
        //    File,
        //}

        //public enum eUIAnimationType
        //{
        //    None,
        //    UseSystem_ConsistentDirection,
        //    UseSystem_AdverseDirection,
        //    UseCustom,
        //}
        #endregion
    }

    [Serializable]
    class UIPanelPathJsonList
    {
        public List<UIPanelInfo> InfoList = null;
    }

    public enum eLoadAssetType
    {
        Resource,
        AssetBundle,
        File,
    }

    public enum eUIAnimationType
    {
        None,
        UseSystemConsistentDirection,
        UseSystemAdverseDirection,
        UseCustom,
    }
}