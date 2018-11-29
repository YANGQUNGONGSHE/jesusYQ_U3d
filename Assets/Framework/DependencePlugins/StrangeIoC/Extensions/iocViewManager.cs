using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WongJJ.Game.Core.StrangeExtensions
{
    public static class iocViewManager
    {
        public static LoadAssetType LoadAssetType = LoadAssetType.File;
        public static string IocViewInfoJsonPath = "UIViewInfo";
        public static string RootCanvasName = "RootCanvas";

        private static readonly Dictionary<int, string> MViewPathDict;
        private static readonly Dictionary<int, iocView> MViewMapDict;

        private static Transform _mCanvascomponent;
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

        private static iocView _mCurrentView;
        public static iocView CurrentView {
            get { return _mCurrentView; }
        }

        static iocViewManager()
        {
            MViewPathDict = new Dictionary<int, string>();
            MViewMapDict = new Dictionary<int, iocView>();
            InitPath();
        }

        private static void InitPath()
        {
            TextAsset asset = Resources.Load<TextAsset>(IocViewInfoJsonPath);
            iocViewPathJsonList pathJsonList = JsonUtility.FromJson<iocViewPathJsonList>(asset.text);
            foreach (iocViewInfo info in pathJsonList.InfoList)
            {
                string path = string.Empty;
                switch (LoadAssetType)
                {
                    case LoadAssetType.Resource:
                        path = info.ResPath;
                        break;
                    case LoadAssetType.AssetBundle:
                        path = info.AbPath;
                        break;
                    case LoadAssetType.File:
                        path = info.NabPath;
                        break;
                }
                MViewPathDict.Add(info.UiId, path);
            }
        }

        public static iocView OpenView(int uiId)
        {
            return OpenView(uiId, Vector2.zero);
        }

        public static iocView OpenView(int uiId, Vector2 createPos)
        {
            return OpenView(uiId, createPos, true);
        }

        public static iocView OpenView(int uiId, Vector2 createPos, bool allowAutoShow)
        {
            iocView view;
            MViewMapDict.TryGetValue(uiId, out view);
            if (view == null)
            {
                string resPath;
                MViewPathDict.TryGetValue(uiId, out resPath);
                if (!string.IsNullOrEmpty(resPath))
                {
                    GameObject go = null;
                    switch (LoadAssetType)
                    {
                        //case eLoadAssetType.Resource:
                        //go = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(resPath));
                        //break;
#if ASSETBUNDLE_MODEL
					case LoadAssetType.AssetBundle:
						int index = resPath.LastIndexOf ("/");
						string name = resPath.Substring (index + 1).ToLower ();
						go = AssetBundleLoaderManager.Instance.LoadInstanceSync(resPath + ".assetbundle", name);
                        break;
#else
                        case LoadAssetType.File:
                            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", resPath + ".prefab"));
                            go = UnityEngine.Object.Instantiate(prefab);
                            Log.I(go.name);
                            break;
#endif
                    }
                    if (go != null)
                    {
                        go.transform.SetParent(Canvascomponent, false);
                        go.GetComponent<RectTransform>().offsetMin = createPos;
                        go.GetComponent<RectTransform>().offsetMax = createPos;
                        view = go.GetComponent<iocView>();
                        view.gameObject.layer = LayerMask.NameToLayer("UI");
                        view.GraphicRaycaster.enabled = true;
                        view.Canvas.sortingOrder = view.GetLayer();
                        view.OnRender();
                    }
                    MViewMapDict.Add(uiId, view);
                    _mCurrentView = view;
                    return view;
                }
                else
                {
                    UnityEngine.Debug.LogError(string.Format("没有找到类型为{0}的路径，请检查是否配置路径", uiId.ToString()));
                    return null;
                }
            }
            view.gameObject.GetComponent<RectTransform>().offsetMin = createPos;
            view.gameObject.GetComponent<RectTransform>().offsetMax = createPos;
            view.gameObject.layer = LayerMask.NameToLayer("UI");
            view.GraphicRaycaster.enabled = true;
            view.Canvas.sortingOrder = view.GetLayer();
            view.OnRender();
            _mCurrentView = view;
            return view;
        }

        public static void CloseView(int uiId)
        {
            iocView view;
            MViewMapDict.TryGetValue(uiId, out view);
            if (view == null)
                return;
            view.GraphicRaycaster.enabled = false;
            view.OnNoRender();
            CoroutineController.Instance.StartCoroutine(WaitAnimationToNoRender(view));
        }

        public static void DestroyAndOpenNew(int closeUid, int openUid)
        {
            iocView view;
            MViewMapDict.TryGetValue(closeUid, out view);
            if (view == null)
                return;
            GameObject.Destroy(view.gameObject);
            MViewMapDict.Remove(closeUid);
            GC.Collect();
            Resources.UnloadUnusedAssets();
            OpenView(openUid);
        }

        public static void DestoryView(int closeUid)
        {
            iocView view;
            MViewMapDict.TryGetValue(closeUid, out view);
            if (view == null)
                return;
            GameObject.Destroy(view.gameObject);
            MViewMapDict.Remove(closeUid);
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        public static void DestoryCurrentOpenNew(int newUiId, Vector2 pos = default(Vector2))
        {
            if(_mCurrentView.GetUiId()== newUiId)return;
            iocView view;
            MViewMapDict.TryGetValue(_mCurrentView.GetUiId(), out view);
            if (view == null)
                return;
            GameObject.Destroy(view.gameObject);
            MViewMapDict.Remove(_mCurrentView.GetUiId());
            GC.Collect();
            Resources.UnloadUnusedAssets();
            OpenView(newUiId, pos);
        }
        static IEnumerator WaitAnimationToNoRender(iocView view)
        {
            yield return new WaitForSeconds(view.AnimationTime());
            view.gameObject.layer = LayerMask.NameToLayer("DontRender");
            view.Canvas.sortingOrder = -1;
        }

        public static void CloseCurrentOpenNew(int newUiId,Vector2 pos = default(Vector2))
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
            if (_mCurrentView.GetUiId() == newUiId) return;
            CloseView(_mCurrentView.GetUiId());
            OpenView(newUiId, pos);
        }

        public static void CloseAndOpenView(int closeUiId, int openUiId)
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
            CloseView(closeUiId);
            OpenView(openUiId);
        }
    }

    public enum LoadAssetType
    {
        Resource,
        AssetBundle,
        File,
    }

    [Serializable]
    public class iocViewInfo : ISerializationCallbackReceiver
    {
        public int UiId;
        public string PanelType;
        public string ResPath;
        public string AbPath;
        public string NabPath;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }

    [Serializable]
    class iocViewPathJsonList
    {
        public List<iocViewInfo> InfoList = null;
    }
}