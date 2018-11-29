using System;
using UnityEngine;

namespace WongJJ.Game.Core
{
    public class SingletionException : Exception
    {
        public SingletionException(string errorMsg) : base(errorMsg)
        {
            Debug.LogError(errorMsg);
        }
    }
}

