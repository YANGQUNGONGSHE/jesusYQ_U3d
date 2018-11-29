using System.Collections.Generic;
using UnityEngine;

namespace WongJJ.Game.Core.SimpleObjectPool
{
    /// <summary>
    /// 对象池
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        /// <summary>
        /// 对象预设
        /// </summary>
        [SerializeField]
        private GameObject ObjectPrefab;

        /// <summary>
        /// 对象池名称
        /// </summary>
        public string ObjectName;

        /// <summary>
        /// 对象池最大数量
        /// </summary>
        [SerializeField]
        private int PoolMaxCount;

        /// <summary>
        /// 对象集合
        /// </summary>
        [System.NonSerialized]
        private List<GameObject> _mPrefabsList = new List<GameObject>();

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool Contain(GameObject go)
        {
            return _mPrefabsList.Contains(go);
        }

        /// <summary>
        /// 从对象池取出一个对象
        /// </summary>
        /// <returns>The object.</returns>
        public GameObject GetObject()
        {
            GameObject go = null;
            for (int i = 0; i < _mPrefabsList.Count; i++)
            {
                if (!_mPrefabsList[i].activeSelf)
                {
                    go = _mPrefabsList[i];
                    go.SetActive(true);
                    break;
                }
            }
            if (go == null)
            {
                if (_mPrefabsList.Count >= PoolMaxCount)
                {
                    Object.Destroy(_mPrefabsList[0]);
                    _mPrefabsList.RemoveAt(0);
                }
                go = Object.Instantiate<GameObject>(ObjectPrefab);
                _mPrefabsList.Add(go);
            }
            go.SendMessage("BeforGetObject", SendMessageOptions.DontRequireReceiver);
            return go;
        }

        /// <summary>
        /// 回收一个对象
        /// </summary>
        /// <param name="go">Go.</param>
        public void DeleteObject(GameObject go)
        {
            if (_mPrefabsList.Contains(go))
            {
                go.SendMessage("BeforDeleteObject", SendMessageOptions.DontRequireReceiver);
                go.SetActive(false);
            }
        }

        /// <summary>
        /// 回收所有对象
        /// </summary>
        public void DeleteAllObject()
        {
            for (int i = 0; i < _mPrefabsList.Count; i++)
            {
                if (_mPrefabsList[i].activeSelf)
                {
                    DeleteObject(_mPrefabsList[i]);
                }
            }

        }
    }
}