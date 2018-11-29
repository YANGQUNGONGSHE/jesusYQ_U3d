
using System.Collections.Generic;
using UnityEngine;

namespace WongJJ.Game.Core.SimpleObjectPool
{
    public class PoolManager : Singletion<PoolManager>
    {
        /** 所有对象池集合 */
        private Dictionary<string, Pool> _mPoolDict = new Dictionary<string, Pool>();

        public override void Init()
        {
            _mPoolDict.Clear();
            ObjectPoolConfig config = Resources.Load<ObjectPoolConfig>("ObjectPoolConfig");
            foreach(var pool in config.PoolList)
            {
                _mPoolDict.Add(pool.ObjectName,pool);
            }
        }

        /// <summary>
        /// 从对象池中获取一个对象
        /// </summary>
        /// <param name="poolName">对象池</param>
        /// <returns></returns>
        public GameObject GetObject(string poolName)
        {
            if(!_mPoolDict.ContainsKey(poolName))
            {
                Debug.LogError("对象池没有这个物体");
                return null;
            }
            Pool pool =  _mPoolDict[poolName];
            return pool.GetObject();
        }

        /// <summary>
        /// 隐藏一个指定对象
        /// </summary>
        /// <param name="go">对象</param>
        public void HideObject(GameObject go)
        {
            Pool pool = null;
            foreach(var p in _mPoolDict.Values)
            {
                if(p.Contain(go))
                {
                    pool = p;
                    break;
                }
            }
            if(pool != null)
            {
                pool.DeleteObject(go);
            }
        }

        /// <summary>
        /// 隐藏所有对象
        /// </summary>
        /// <param name="poolName">对象池</param>
        public void HideAllObject(string poolName)
        {
            if(!_mPoolDict.ContainsKey(poolName))
            {
                Debug.LogError("没有这个对象池");
                return;
            }
            Pool p = _mPoolDict[poolName];
            p.DeleteAllObject();
        }
    }
}
