using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WongJJ.Game.Core
{
    public class TouchListener : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IUpdateSelectedHandler,
    IDeselectHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IScrollHandler,
    IMoveHandler
    {
        public TouchHandle onClick;
        public TouchHandle onDoubleClick;
        public TouchHandle onDown;
        public TouchHandle onEnter;
        public TouchHandle onExit;
        public TouchHandle onUp;
        public TouchHandle onSelect;
        public TouchHandle onUpdateSelect;
        public TouchHandle onDeSelect;
        public TouchHandle onDrag;
        public TouchHandle onDragEnd;
        public TouchHandle onDrop;
        public TouchHandle onScroll;
        public TouchHandle onMove;

        /// <summary>
        /// Get the specified go.
        /// </summary>
        /// <param name="go">Go.</param>
        static public TouchListener Get(GameObject go)
        {
            TouchListener listener = go.GetComponent<TouchListener>();
            if (listener == null)
                listener = go.AddComponent<TouchListener>();
            return listener;
        }

        /// <summary>
        /// Removes the handle.
        /// </summary>
        /// <param name="_handle">Handle.</param>
        private void RemoveHandle(TouchHandle _handle)
        {
            if (null != _handle)
            {
                _handle.DestoryHandle();
                _handle = null;
            }
        }

        /// <summary>
        /// Removes all handle.
        /// </summary>
        private void RemoveAllHandle()
        {
            this.RemoveHandle(onClick);
            this.RemoveHandle(onDoubleClick);
            this.RemoveHandle(onDown);
            this.RemoveHandle(onEnter);
            this.RemoveHandle(onExit);
            this.RemoveHandle(onUp);
            this.RemoveHandle(onDrop);
            this.RemoveHandle(onDrag);
            this.RemoveHandle(onDragEnd);
            this.RemoveHandle(onScroll);
            this.RemoveHandle(onMove);
            this.RemoveHandle(onUpdateSelect);
            this.RemoveHandle(onSelect);
            this.RemoveHandle(onDeSelect);
        }

        void OnDestory()
        {
            this.RemoveAllHandle();
        }

        #region IDragHandler implementation

        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
                onDrag.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IEndDragHandler implementation

        public void OnEndDrag(PointerEventData eventData)
        {
            if (onDragEnd != null)
                onDragEnd.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IDropHandler implementation
        public void OnDrop(PointerEventData eventData)
        {
            if (onDrop != null)
                onDrop.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerClickHandler implementation
        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
                onClick.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerDownHandler implementation
        public void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
                onDown.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerUpHandler implementation
        public void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null)
                onUp.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerEnterHandler implementation
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
                onEnter.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerExitHandler implementation
        public void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null)
                onExit.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region ISelectHandler implementation
        public void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null)
                onSelect.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IUpdateSelectedHandler implementation
        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null)
                onUpdateSelect.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IDeselectHandler implementation

        public void OnDeselect(BaseEventData eventData)
        {
            if (onDeSelect != null)
                onDeSelect.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IScrollHandler implementation

        public void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null)
                onScroll.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IMoveHandler implementation

        public void OnMove(AxisEventData eventData)
        {
            if (onMove != null)
                onMove.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        /// <summary>
        /// Adds the observer.
        /// </summary>
        /// <param name="_type">Type.</param>
        /// <param name="_handle">Handle.</param>
        /// <param name="_params">Parameters.</param>
        public void AddObserver(TouchEventType _type, OnTouchEvent _handle, params object[] _params)
        {
            switch (_type)
            {
                case TouchEventType.OnClick:
                    if (null == onClick)
                    {
                        onClick = new TouchHandle();
                    }
                    onClick.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnDoubleClick:
                    if (null == onDoubleClick)
                    {
                        onDoubleClick = new TouchHandle();
                    }
                    onDoubleClick.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnDown:
                    if (onDown == null)
                    {
                        onDown = new TouchHandle();
                    }
                    onDown.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnUp:
                    if (onUp == null)
                    {
                        onUp = new TouchHandle();
                    }
                    onUp.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnEnter:
                    if (onEnter == null)
                    {
                        onEnter = new TouchHandle();
                    }
                    onEnter.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnExit:
                    if (onExit == null)
                    {
                        onExit = new TouchHandle();
                    }
                    onExit.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnDrag:
                    if (onDrag == null)
                    {
                        onDrag = new TouchHandle();
                    }
                    onDrag.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnDrop:
                    if (onDrop == null)
                    {
                        onDrop = new TouchHandle();
                    }
                    onDrop.SetHandle(_handle, _params);
                    break;

                case TouchEventType.OnDragEnd:
                    if (onDragEnd == null)
                    {
                        onDragEnd = new TouchHandle();
                    }
                    onDragEnd.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnSelect:
                    if (onSelect == null)
                    {
                        onSelect = new TouchHandle();
                    }
                    onSelect.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnUpdateSelect:
                    if (onUpdateSelect == null)
                    {
                        onUpdateSelect = new TouchHandle();
                    }
                    onUpdateSelect.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnDeSelect:
                    if (onDeSelect == null)
                    {
                        onDeSelect = new TouchHandle();
                    }
                    onDeSelect.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnScroll:
                    if (onScroll == null)
                    {
                        onScroll = new TouchHandle();
                    }
                    onScroll.SetHandle(_handle, _params);
                    break;
                case TouchEventType.OnMove:
                    if (onMove == null)
                    {
                        onMove = new TouchHandle();
                    }
                    onMove.SetHandle(_handle, _params);
                    break;
            }
        }
    }

    /// <summary>
    /// Touch handle.
    /// </summary>
    public class TouchHandle
    {
        private event OnTouchEvent eventHandle = null;
        private object[] handleParams;

        public TouchHandle(OnTouchEvent _handle, params object[] _params)
        {
            SetHandle(_handle, _params);
        }

        public TouchHandle() { }

        public void SetHandle(OnTouchEvent _handle, params object[] _params)
        {
            DestoryHandle();
            eventHandle += _handle;
            handleParams = _params;
        }

        public void CallEventHandle(GameObject _listener, object _args)
        {
            if (null != eventHandle)
            {
                eventHandle(_listener, _args, handleParams);
            }
        }

        public void DestoryHandle()
        {
            if (null != eventHandle)
            {
                eventHandle -= eventHandle;
                eventHandle = null;
            }
        }
    }

    public delegate void OnTouchEvent(GameObject listener, object eventData, params object[] param);
    public enum TouchEventType
    {
        OnClick,
        OnDoubleClick,
        OnDown,
        OnUp,
        OnEnter,
        OnExit,
        OnSelect,
        OnUpdateSelect,
        OnDeSelect,
        OnDrag,
        OnDragEnd,
        OnDrop,
        OnScroll,
        OnMove,
    }
}

