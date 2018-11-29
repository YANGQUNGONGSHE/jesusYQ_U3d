using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace WongJJ.Game.Core.StrangeExtensions
{
    public abstract class iocView : EventView
    {
        protected override void Awake()
        {
            base.Awake();
            ApplicationChrome.statusBarState = ApplicationChrome.States.TranslucentOverContent;
        }

        public abstract int GetUiId();
        public abstract int GetLayer();

        private Canvas _mCanvas;
        public Canvas Canvas
        {
            get
            {
                if (_mCanvas == null)
                {
                    _mCanvas = transform.GetComponent<Canvas>();
                }
                return _mCanvas;
            }
        }

        private CanvasGroup _mCanvasGroup;
        protected virtual CanvasGroup CanvasGroup
        {
            set { _mCanvasGroup = value; }
            get
            {
                if (_mCanvasGroup == null)
                    _mCanvasGroup = transform.GetComponent<CanvasGroup>();
                return _mCanvasGroup;
            }
        }

        private RectTransform _mRectTransform;
        protected RectTransform RectTransform
        {
            get
            {
                if (_mRectTransform == null)
                    _mRectTransform = transform.GetComponent<RectTransform>();
                return _mRectTransform;
            }
        }

        private GraphicRaycaster _mGraphicRaycaster;
        public GraphicRaycaster GraphicRaycaster
        {
            get
            {
                if (_mGraphicRaycaster == null)
                {
                    _mGraphicRaycaster = transform.GetComponent<GraphicRaycaster>();
                }
                return _mGraphicRaycaster;
            }
        }

        public virtual float AnimationTime(){return 0f;}

        public virtual void OnRender(){ }

        public virtual void OnNoRender() { }
    }
}
