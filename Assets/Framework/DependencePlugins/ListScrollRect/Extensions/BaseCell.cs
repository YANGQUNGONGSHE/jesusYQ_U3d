using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WongJJ.Game.Core.ListRectExtensions
{
    public class BaseCell<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
        where T : class
    {
        #region Cell属性
        /// <summary>
        /// Cell索引
        /// </summary>
        protected int MIndex;

        /// <summary>
        /// 当前模型
        /// </summary>
        protected T t;
        #endregion

        #region 长按判定属性
        [Tooltip("多少时间算长按")]
        private float _mDurationThreshold = 0.80f;
        private bool _mIsPointerDown = false;
        private bool _mLongPressTriggered = false;
        private float _mTimePressStarted;
        #endregion

        #region 回调
        protected Action<int,T> MOnCellLongPress;
        protected Action<int,T> MOnCellClick;
        #endregion

        #region 长按和点击判定
        public void OnPointerDown(PointerEventData eventData)
        {
            _mTimePressStarted = Time.time;
            _mIsPointerDown = true;
            _mLongPressTriggered = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _mIsPointerDown = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mIsPointerDown = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_mLongPressTriggered)
            {
                if (MOnCellClick != null) MOnCellClick(MIndex,t);
            }
        }
        #endregion

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            if (_mIsPointerDown && !_mLongPressTriggered)
            {
                if (Time.time - _mTimePressStarted > _mDurationThreshold)
                {
                    _mLongPressTriggered = true;
                    if (MOnCellLongPress != null) MOnCellLongPress(MIndex,t);
                }
            }
            OnUpdate();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnStart() { }

        protected virtual void OnUpdate() { }

        public virtual void InitUi(int index, T t, Action<int,T> onCellClickCallback = null, Action<int,T> onCellLongPressCallback = null, bool isSelected = false)
        {
            MIndex = index;
            this.t = t;
            MOnCellClick = onCellClickCallback;
            MOnCellLongPress = onCellLongPressCallback;
        }
    }
}
