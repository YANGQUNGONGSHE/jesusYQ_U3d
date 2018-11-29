using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WongJJ.Game.Core.SimpleUIManager
{
    /**
     * @Summary:所有Panel基类
     * @Author：WongJJ
     * @Date: 2017-07-22 16:32:54
     * @Remark: 重构 2017-07-22 16:32:54
     */
    public abstract class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 本面板对应的类型
        /// </summary>
        /// <returns>The UIT ype.</returns>
        public abstract UIPanelType GetUiType();

        /// <summary>
        /// 本面板显示或隐藏所用动画的时间
        /// </summary>
        /// <returns>The time.</returns>
        public abstract float AnimationTime();

        /// <summary>
        /// 面板的CanvasGroup
        /// </summary>
        /// <value>The cg.</value>
        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (_mCanvasGroup == null)
                    _mCanvasGroup = gameObject.GetComponent<CanvasGroup>();
                return _mCanvasGroup;
            }
        }
        private CanvasGroup _mCanvasGroup;

        /// <summary>
        /// 面板的RectTransform
        /// </summary>
        /// <value>The rect transform.</value>
        protected RectTransform RectTransform
        {
            get
            {
                if (_mRectTransform == null)
                    _mRectTransform = gameObject.GetComponent<RectTransform>();
                return _mRectTransform;
            }
        }
        private RectTransform _mRectTransform;

        void Awake()
        {
            OnAwake();
            AddListener();
        }

        void Start()
        {
            OnStart();
        }

        void Update()
        {
            OnUpdate(Time.deltaTime);
        }

        public virtual void OnAwake() { }

        public virtual void AddListener() { }

        public virtual void OnUpdate(float deltaTime) { }

        public virtual void OnStart() { }

        /// <summary>
        /// 本面板窗体如何显示出来？
        /// </summary>
        public abstract void Show();

        /// <summary>
        /// 本面板窗体如何隐藏掉？
        /// </summary>
        public abstract void Hide();

        /// <summary>
        /// 面板创建之前
        /// </summary>
        public virtual void BeginShow() { gameObject.SetActive(true); }

        /// <summary>
        /// 销毁自身
        /// </summary>
        public void CloseSelf()
        {
            UIManager.Close(GetUiType());
        }

        #region 废弃
        //public virtual void OnOpen()
        //{
        //    gameObject.SetActive(true);
        //    cg.alpha = 1f;
        //}

        //public virtual void OnClose()
        //{
        //    StartCoroutine(Divisible());
        //}

        //public virtual void OnPause()
        //{
        //    ChangeInteraction(false);
        //}

        //public virtual void OnResume()
        //{
        //    ChangeInteraction(true);
        //}

        //private IEnumerator Divisible()
        //{
        //    yield return new WaitForSeconds(DelayDestroyTime());
        //    cg.alpha = 0f;
        //    gameObject.SetActive(false); //App模式特性,无需栈式显示。
        //}

        //private void ChangeInteraction(bool flag)
        //{
        //    if (cg != null)
        //        cg.blocksRaycasts = flag;
        //    else
        //        return;
        //}
        #endregion
    }
}