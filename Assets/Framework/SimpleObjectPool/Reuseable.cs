using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WongJJ.Game.Core.SimpleObjectPool
{
    public abstract class Reuseable : MonoBehaviour
    {
        public abstract void BeforGetObject();

        public abstract void BeforDeleteObject();
    }
}