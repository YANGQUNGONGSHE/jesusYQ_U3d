using System;
using System.Diagnostics;

namespace WongJJ.Game.Core
{
    public abstract class Singletion<T> where T : class,new()
    {
        protected static T _Instance = null;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new T();
                }
                return _Instance;
            }
        }

        protected Singletion()
        {
            if (_Instance != null)
                throw new SingletionException("this" + typeof(T).ToString() + "Singletion is exits , can not new one !");
            Init();
        }

        public virtual void Init(){}
    }
}

