using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace WongJJ.Game.Core
{
    /// <summary>
    /// Class of Assetinfo with Load.
    /// </summary>
    public class AssetInfo
    {
        /// <summary>
        /// The private Assetobject.
        /// </summary>
        private UnityEngine.Object _object;

        /// <summary>
        /// Gets or sets the type oyou want to load the resource.
        /// </summary>
        /// <value>The type of the asset.</value>
        public Type AssetType { get; set;}

        /// <summary>
        /// Gets or sets The path you want to load the resource.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set;}

        /// <summary>
        /// Record how many times the resource is referenced
        /// </summary>
        /// <value>The reference count.</value>
        public int RefCount{ get; set;}

        /// <summary>
        /// Whether the resource is loaded
        /// </summary>
        /// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
        public bool IsLoaded
        {
            get
            {
                return null != _object;
            }
        }

        /// <summary>
        /// Gets the AssetObject.
        /// </summary>
        /// <value>The asset object.</value>
        public UnityEngine.Object AssetObject
        {
            get
            {
                if (_object == null)
                {
                    _ResourceLoad();
                }
                return _object;
            }
        }

        /// <summary>
        /// Using UnityEngine.Resources.Load() return _object.
        /// </summary>
        private void _ResourceLoad()
        {
            try
            {
                _object = Resources.Load(Path);
                if (_object == null)
                    Debug.LogError("Resources.Load(Path) fail;");
            }
            catch (Exception ex)
            {
                Debug.LogError("Resources.Load(Path) fail " + ex.ToString());
            }
        }

        /// <summary>
        /// Use the coroutine load resource object.
        /// </summary>
        /// <returns>The coroutine object.</returns>
        /// <param name="loadedCallback">Loaded callback.</param>
        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> loadedCallback)
        {
            while (true)
            {
                yield return null;
                if (_object == null)
                {
                    _ResourceLoad();
                    yield return null;
                }
                if (loadedCallback != null)
                    loadedCallback(_object);
                yield break;
            }
        }

        /// <summary>
        /// Use the Async load resource object.
        /// </summary>
        /// <returns>The async object.</returns>
        /// <param name="loadedCallback">Loaded callback.</param>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> loadedCallback)
        {
            return GetAsyncObject(loadedCallback, null);
        }

        /// <summary>
        /// Use the Async load resource object.
        /// </summary>
        /// <returns>The async object.</returns>
        /// <param name="loadedCallback">Loaded callback.</param>
        /// <param name="progressCallback">Progress callback.</param>
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> loadedCallback, Action<float> progressCallback)
        {
            if (_object != null)
            {
                if (loadedCallback != null)
                {
                    loadedCallback(_object);
                    yield break;
                }
            }

            ResourceRequest resourceRequest = Resources.LoadAsync(Path);

            while (resourceRequest.progress > 0.9f)
            {
                if (progressCallback != null)
                    progressCallback(resourceRequest.progress);
                yield return null;
            }

            while (!resourceRequest.isDone)
            {
                if (progressCallback != null)
                    progressCallback(resourceRequest.progress);
                yield return null;
            }

            _object = resourceRequest.asset;
            if (_object != null)
            {
                if (loadedCallback != null)
                {
                    loadedCallback(_object);
                }
            }
        }
    }

    /// <summary>
    /// Resource manager.
    /// </summary>
    public class ResourceManager :Singletion<ResourceManager>
    {
        /// <summary>
        /// Keep all loaded resources.
        /// </summary>
        private Dictionary<string, AssetInfo> _dicAssetInfo = null;

        /// <summary>
        /// Init this instance.
        /// </summary>
        public override void Init()
        {
            _dicAssetInfo = new Dictionary<string, AssetInfo>();
        }

        /// <summary>
        /// Get the resource information based on the path.
        /// </summary>
        /// <returns>The asset info.</returns>
        /// <param name="path">Path.</param>
        private AssetInfo GetAssetInfo(string path)
        {
            return GetAssetInfo(path,null);
        }

        /// <summary>
        /// Get the resource information based on the path.
        /// </summary>
        /// <returns>The asset info.</returns>
        /// <param name="path">Path.</param>
        /// <param name="loadedCallback">Loaded callback.</param>
        private AssetInfo GetAssetInfo(string path, Action<UnityEngine.Object> loadedCallback)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("ResourceManager.Load().GetAssetInfo(); path can not be null");
                if (loadedCallback != null)
                    loadedCallback(null);
            }

            AssetInfo assetInfo = null;
            if (!_dicAssetInfo.TryGetValue(path, out assetInfo))
            {
                assetInfo = new AssetInfo();
                assetInfo.Path = path;
                _dicAssetInfo.Add(path, assetInfo);
            }
            assetInfo.RefCount++;
            return assetInfo;
        }

        /// <summary>
        /// General load resources based on path.
        /// </summary>
        /// <param name="path">Path.</param>
        public UnityEngine.Object Load(string path)
        {
            AssetInfo assetInfo = GetAssetInfo(path);
            if (assetInfo.AssetObject != null)
            {
                return assetInfo.AssetObject;
            }
            else
            {
                Debug.LogError("ResourceManager Load(); _assetInfo.AssetObject == null !");
                return null;
            }
        }

        /// <summary>
        /// Coroutine load resources based on path with Callback.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="loadedCallback">Loaded callback.</param>
        public void LoadCoroutine(string path, Action<UnityEngine.Object> loadedCallback)
        {
            AssetInfo assetInfo = GetAssetInfo(path, loadedCallback);
            if (assetInfo != null)
                CoroutineController.Instance.StartCoroutine(assetInfo.GetCoroutineObject(loadedCallback));
        }

        /// <summary>
        /// Async load resources based on path with Callback.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="loadedCallback">Loaded callback.</param>
        public void LoadAsync(string path, Action<UnityEngine.Object> loadedCallback)
        {
            LoadAsync(path, loadedCallback, null);
        }

        /// <summary>
        /// Async load resources based on path with Callback and Progress
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="loadedCallback">Loaded callback.</param>
        /// <param name="progressCallback">Progress callback.</param>
        public void LoadAsync(string path, Action<UnityEngine.Object> loadedCallback, Action<float> progressCallback)
        {
            AssetInfo assetInfo = GetAssetInfo(path, loadedCallback);
            if (assetInfo != null)
                CoroutineController.Instance.StartCoroutine(assetInfo.GetAsyncObject(loadedCallback, progressCallback));
        }

        /// <summary>
        /// Instantiate the specified _object.
        /// </summary>
        /// <param name="_object">Object.</param>
        public UnityEngine.Object Instantiate(UnityEngine.Object _object)
        {
            return Instantiate(_object, null);
        }

        /// <summary>
        /// Instantiate the specified _obj and _loadedCallback.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="loadedCallback">Loaded callback.</param>
        public UnityEngine.Object Instantiate(UnityEngine.Object obj, Action<UnityEngine.Object> loadedCallback)
        {
            UnityEngine.Object retObj = null;
            if (obj != null)
            {
                retObj = MonoBehaviour.Instantiate(obj);
                if (retObj != null)
                {
                    if (loadedCallback != null)
                    {
                        loadedCallback(retObj);
                        return null;
                    }
                    return retObj;
                }
                else
                {
                    Debug.LogError("ResourceManager.Instantiate(); MonoBehaviour.Instantiate is fail.");
                }
            }
            else
            {
                Debug.LogError("ResourceManager.Instantiate(); can not load path");
            }
            return null;
        }

        /// <summary>
        /// Loads and instance Object base on Path.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="path">Path.</param>
        public UnityEngine.Object LoadInstance(string path)
        {
            UnityEngine.Object obj = Load(path);
            return Instantiate(obj);
        }

        /// <summary>
        /// Use Coroutine Loads and Instance Object base on Path with Callback.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="loadedcallback">Loadedcallback.</param>
        public void LoadCoroutineInstance(string path, Action<UnityEngine.Object> loadedcallback)
        {
            LoadCoroutine(path, (obj) => {
                Instantiate(obj, loadedcallback);
            });
        }

        /// <summary>
        /// Use Async Loads and Instance Object base on Path with Callback.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="loadedcallback">Loadedcallback.</param>
        public void LoadAsyncInstance(string path, Action<UnityEngine.Object> loadedcallback)
        {
            LoadAsync(path,(obj) => {
                Instantiate(obj, loadedcallback);
            });
        }

        /// <summary>
        /// Use Async Loads and Instance Object base on Path with Callback.
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="loadedcallback">Loadedcallback.</param>
        /// <param name="progressCallback">Progress callback.</param>
        public void LoadAsyncInstance(string path, Action<UnityEngine.Object> loadedcallback, Action<float> progressCallback)
        {
            LoadAsync(path, (obj)=> { 
                Instantiate(obj, loadedcallback); 
            } , progressCallback);
        }

    }
}

