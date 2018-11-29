using System;
using UnityEngine;

namespace WongJJ.Game.Core
{
    public abstract class KeepSingletion<T> : MonoBehaviour where T: MonoBehaviour
    {
        protected static T _Instance = null;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject go = GameObject.Find("KeepLifeSingletion");
                    if (go == null)
                    {
                        go = new GameObject("KeepLifeSingletion");
                        DontDestroyOnLoad(go);
                    }
                    _Instance = go.AddComponent<T>();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// Raises the application quit event.
        /// </summary>
        private void OnApplicationQuit()
        {
            _Instance = null;
        }
    }
}

