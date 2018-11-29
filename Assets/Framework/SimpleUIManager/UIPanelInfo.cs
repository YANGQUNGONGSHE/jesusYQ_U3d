using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WongJJ.Game.Core.SimpleUIManager
{
    [Serializable]
    public class UIPanelInfo : ISerializationCallbackReceiver
	{
        [NonSerialized]
        public UIPanelType Type;
        public string PanelType;
        public string ResPath;
        public string AbPath;
        public string NabPath;

        #region ISerializationCallbackReceiver implementation
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            Type = (UIPanelType)Enum.Parse(typeof(UIPanelType),PanelType);
        }
        #endregion
	}
}
